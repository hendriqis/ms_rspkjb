using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentGenerateListDiagnosticSafe : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            var param = Page.Request.QueryString["id"];
            switch (param)
            {
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.MCU_APPOINTMENT_REQUEST;
                default: return Constant.MenuCode.MedicalCheckup.MCU_APPOINTMENT_REQUEST;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];

                List<StandardCode> lst = new List<StandardCode>();
                lst.Insert(0, new StandardCode { StandardCodeName = "SEMUA", StandardCodeID = "0" });
                lst.Insert(1, new StandardCode { StandardCodeName = "Belum Perjanjian, Belum Registrasi", StandardCodeID = "1" });
                lst.Insert(2, new StandardCode { StandardCodeName = "Sudah Perjanjian, Belum Registrasi", StandardCodeID = "2" });
                lst.Insert(3, new StandardCode { StandardCodeName = "Sudah Perjanjian, Sudah Registrasi", StandardCodeID = "3" });
                Methods.SetComboBoxField<StandardCode>(cboFilterType, lst, "StandardCodeName", "StandardCodeID");
                cboFilterType.SelectedIndex = 0;

                GetSettingParameter();
                InitializeFilterParameter();
                hdnTodayDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                BindGridView(1, true, ref PageCount);

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            }
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", 
                AppSession.UserLogin.HealthcareID, //0
                Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //1
                Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //2
                Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS, //3
                Constant.SettingParameter.MEDICAL_CHECKUP_CLASS, //4
                Constant.SettingParameter.FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ, //5
                Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI, //6
                Constant.SettingParameter.FN_KONTROL_BIAYA_ADM_PASIEN_RJ, //7
                Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI, //8
                Constant.SettingParameter.FN_KONTROL_BIAYA_KARTU, //9
                Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU //10
            ));
            hdnHealthcareServiceUnitImagingID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitLaboratoryID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            hdnIsBridgingToMedinfrasMobileApps.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
            hdnMCUClassID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.MEDICAL_CHECKUP_CLASS).FirstOrDefault().ParameterValue;
            //
            hdnIsControlAdministrationCharges.Value = setvar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ).ParameterValue;
            hdnChargeCodeAdministrationForInstansi.Value = setvar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI).ParameterValue;
            hdnIsControlAdmCost.Value = setvar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_BIAYA_ADM_PASIEN_RJ).ParameterValue;
            hdnAdminID.Value = setvar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI).ParameterValue;
            hdnIsControlPatientCardPayment.Value = setvar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_BIAYA_KARTU).ParameterValue;
            hdnItemCardFee.Value = setvar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU).ParameterValue;
        }

        private void InitializeFilterParameter()
        {
            txtFromAppointmentRequestDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToAppointmentRequestDate.Text = DateTime.Today.AddDays(6).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected string GetFilterHealthcareServiceUnit()
        {
            string result = "";
            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                result = string.Format("DepartmentID = '{0}' AND IsUsingRegistration = 1 AND IsDeleted = 0", hdnDepartmentID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.IMAGING)
            {
                result = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}' AND IsUsingRegistration = 1 AND IsDeleted = 0", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitImagingID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.LABORATORY)
            {
                result = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}' AND IsUsingRegistration = 1 AND IsDeleted = 0", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitLaboratoryID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                result = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ('{1}','{2}') AND IsUsingRegistration = 1 AND IsDeleted = 0", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitImagingID.Value, hdnHealthcareServiceUnitLaboratoryID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
            {
                result = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnit WHERE DepartmentID = '{0}') AND IsUsingRegistration = 1 AND IsDeleted = 0", hdnDepartmentID.Value);
            }
            return result;
        }

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string apmFromDateYear = txtFromAppointmentRequestDate.Text.Substring(6, 4);
            string apmFromDateMonth = txtFromAppointmentRequestDate.Text.Substring(3, 2);
            string apmFromDateDay = txtFromAppointmentRequestDate.Text.Substring(0, 2);
            string apmFromDateFormat = string.Format("{0}-{1}-{2}", apmFromDateYear, apmFromDateMonth, apmFromDateDay);
            DateTime apmFromDate = DateTime.ParseExact(apmFromDateFormat, Constant.FormatString.DATE_PICKER_FORMAT2, null);

            string apmToDateYear = txtToAppointmentRequestDate.Text.Substring(6, 4);
            string apmToDateMonth = txtToAppointmentRequestDate.Text.Substring(3, 2);
            string apmToDateDay = txtToAppointmentRequestDate.Text.Substring(0, 2);
            string apmToDateFormat = string.Format("{0}-{1}-{2}", apmToDateYear, apmToDateMonth, apmToDateDay);
            DateTime apmToDate = DateTime.ParseExact(apmToDateFormat, Constant.FormatString.DATE_PICKER_FORMAT2, null);

            string filterExpression = "IsDeleted = 0";

            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", hdnDepartmentID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.IMAGING)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}'", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitImagingID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.LABORATORY)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID = '{1}'", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitLaboratoryID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ('{1}','{2}')", Constant.Facility.DIAGNOSTIC, hdnHealthcareServiceUnitImagingID.Value, hdnHealthcareServiceUnitLaboratoryID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
            {
                filterExpression += string.Format("AND DepartmentID = '{0}' AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnit WHERE DepartmentID = '{0}')", hdnDepartmentID.Value);
            }

            if (!String.IsNullOrEmpty(hdnHealthcareServiceUnitID.Value))
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value);
            }

            //if (!String.IsNullOrEmpty(txtFromAppointmentRequestDate.Text) && !String.IsNullOrEmpty(txtToAppointmentRequestDate.Text))
            //{
            //    //filterExpression += string.Format(" AND AppointmentDate BETWEEN '{0}' AND '{1}'", apmFromDate.ToString(Constant.FormatString.DATE_FORMAT_112), apmToDate.ToString(Constant.FormatString.DATE_FORMAT_112));
            //    filterExpression += string.Format(" AND AppointmentDate = '{0}'", apmFromDate.ToString(Constant.FormatString.DATE_FORMAT_112));
            //}

            if (cboFilterType.Value.ToString() == "0")
            {
                filterExpression += string.Format(" AND (AppointmentRequestDate = '{0}' OR AppointmentDate = '{0}' OR RegistrationDate = '{0}')", apmFromDate.ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            else if (cboFilterType.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND (AppointmentRequestDate = '{0}' AND (AppointmentID IS NULL AND RegistrationID IS NULL))", apmFromDate.ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            else if (cboFilterType.Value.ToString() == "2")
            {
                filterExpression += string.Format(" AND ((AppointmentRequestDate = '{0}' OR AppointmentDate = '{0}') AND (AppointmentID IS NOT NULL AND RegistrationID IS NULL))", apmFromDate.ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            else if (cboFilterType.Value.ToString() == "3")
            {
                filterExpression += string.Format(" AND ((AppointmentRequestDate = '{0}' OR AppointmentDate = '{0}' OR RegistrationDate = '{0}') AND (AppointmentID IS NOT NULL AND RegistrationID IS NOT NULL))", apmFromDate.ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            //if (chkShowAllAppointmentRequest.Checked)
            //{
            //    filterExpression += string.Format(" AND ((AppointmentID IS NULL) OR (AppointmentID IS NOT NULL))");
            //}
            //else
            //{
            //    filterExpression += string.Format(" AND (AppointmentID IS NULL)");
            //}

            //if (isCountPageCount)
            //{
            //    int rowCount = BusinessLayer.GetvAppointmentRequestRowCount(filterExpression);
            //    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            //}

            List<vAppointmentRequest> lstEntity = BusinessLayer.GetvAppointmentRequestList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AppointmentRequestID");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vAppointmentRequest entity = e.Item.DataItem as vAppointmentRequest;
                HtmlInputHidden hdnKey = (HtmlInputHidden)e.Item.FindControl("hdnKey");
                HtmlInputHidden hdnParamedicID = (HtmlInputHidden)e.Item.FindControl("hdnParamedicID");
                HtmlInputHidden hdnCorporateAccountNo = (HtmlInputHidden)e.Item.FindControl("hdnCorporateAccountNo");
                HtmlInputHidden hdnCorporateAccountName = (HtmlInputHidden)e.Item.FindControl("hdnCorporateAccountName");
                HtmlInputHidden hdnAppointmentID = (HtmlInputHidden)e.Item.FindControl("hdnAppointmentID");
                HtmlInputHidden hdnRegistrationID = (HtmlInputHidden)e.Item.FindControl("hdnRegistrationID");
                HtmlInputHidden hdnHealthcareServiceUnitPerRowID = (HtmlInputHidden)e.Item.FindControl("hdnHealthcareServiceUnitPerRowID");
                HtmlInputHidden hdnGCCustomerType = (HtmlInputHidden)e.Item.FindControl("hdnGCCustomerType");
                HtmlGenericControl lblParamedicName = (HtmlGenericControl)e.Item.FindControl("lblParamedicName");
                HtmlGenericControl lblCustomerType = (HtmlGenericControl)e.Item.FindControl("lblCustomerType");
                TextBox txtAppointmentDate = e.Item.FindControl("txtAppointmentDate") as TextBox;
                txtAppointmentDate.Text = entity.AppointmentRequestDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                hdnKey.Value = entity.AppointmentRequestID.ToString();
                if (!string.IsNullOrEmpty(entity.ParamedicName))
                {
                    lblParamedicName.InnerText = entity.ParamedicName;
                }
                else
                {
                    lblParamedicName.InnerText = "Pilih Dokter";
                }
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                hdnHealthcareServiceUnitPerRowID.Value = entity.HealthcareServiceUnitID.ToString();
                lblCustomerType.InnerText = entity.CustomerType;
                if (!string.IsNullOrEmpty(entity.GCCustomerType))
                {
                    lblCustomerType.InnerText = entity.CustomerType;
                }
                else
                {
                    lblCustomerType.InnerText = "Ubah Penjamin";
                }
                hdnGCCustomerType.Value = entity.GCCustomerType;
                hdnCorporateAccountNo.Value = !string.IsNullOrEmpty(entity.CorporateAccountNo) ? entity.CorporateAccountNo : string.Empty;
                hdnCorporateAccountName.Value = !string.IsNullOrEmpty(entity.CorporateAccountName) ? entity.CorporateAccountName : string.Empty; ;
                if (entity.AppointmentID != 0 && entity.AppointmentID != null)
                {
                    hdnAppointmentID.Value = entity.AppointmentID.ToString();
                }
                else
                {
                    hdnAppointmentID.Value = string.Empty;
                }
                if (entity.RegistrationID != 0 && entity.RegistrationID != null)
                {
                    hdnRegistrationID.Value = entity.RegistrationID.ToString();
                }
                else
                {
                    hdnRegistrationID.Value = string.Empty;
                }
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            int appointmentRegID = 0;

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "print")
                {
                    result = "print|";
                    if (PrintAppointmentRequestLabel(ref errMessage))
                    {
                        result += "success|";
                    }
                    else
                    {
                        result += "fail|" + errMessage;
                    }
                }
                else if (param[0] == "void")
                {
                    result = "void|";
                    if (ValidateVoidAppointment(ref errMessage))
                    {
                        result += "success|" + hdnParam.Value;
                    }
                    else
                    {
                        result += "fail|" + errMessage;
                    }
                }
                else if (param[0] == "multiple")
                {
                    if (param[1] == "process")
                    {
                        if (rblProcessType.SelectedValue == "0")
                        {
                            result = "multiple|process|appointment|";
                            if (validateAppointmentRequest(ref errMessage))
                            {
                                if (createAppointment(ref errMessage, ref appointmentRegID))
                                {
                                    result += "success|" + pageCount + "|" + appointmentRegID;
                                }
                                else
                                {
                                    result += "fail|" + errMessage;
                                }
                            }
                            else
                            {
                                result += "fail|" + errMessage;
                            }

                        }
                        else
                        {
                            result = "multiple|process|registration|";

                            if (txtFromAppointmentRequestDate.Text == hdnTodayDate.Value)
                            {
                                if (validateAppointmentRequest(ref errMessage))
                                {
                                    if (createAppointmentToRegistration(ref errMessage, ref appointmentRegID))
                                    {
                                        result += "success|" + pageCount + "|" + appointmentRegID;
                                    }
                                    else
                                    {
                                        result += "fail|" + errMessage;
                                    }
                                }
                                else
                                {
                                    result += "fail|" + errMessage;
                                }
                            }
                            else
                            {
                                result += "fail|Hanya bisa proses registrasi untuk tanggal hari ini.";
                            }
                        }
                    }
                    else if (param[1] == "print")
                    {

                    }
                }
                else if (param[0] == "createApp")
                {
                    if (createAppointment(ref errMessage, ref appointmentRegID))
                    {
                        result = "createApp|" + pageCount + "|" + appointmentRegID;
                    }
                    else
                    {
                        result += "fail|" + errMessage;
                    }
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Libs/Service/DownloadExcel.ashx");
        }

        private bool createAppointment(ref string errMessage, ref int appointmentRegID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao appointmentDao = new AppointmentDao(ctx);
            ParamedicScheduleDao paramedicScheduleDao = new ParamedicScheduleDao(ctx);
            ParamedicScheduleDateDao paramedicScheduleDateDao = new ParamedicScheduleDateDao(ctx);
            ParamedicLeaveScheduleDao paramedicLeaveScheduleDao = new ParamedicLeaveScheduleDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            AppointmentRequestDao appointmentRequestDao = new AppointmentRequestDao(ctx);
            AppointmentItemPackageDao entityApmItemPackageDao = new AppointmentItemPackageDao(ctx);

            string[] lstApmReqID = null;
            string[] lstParamedicID = null;
            string[] lstApmReqDate = null;
            string[] lstCustomerType = null;

            try
            {
                if (!String.IsNullOrEmpty(hdnParam.Value))
                {
                    if (hdnParam.Value.Contains(','))
                    {
                        lstApmReqID = hdnParam.Value.Split(',');
                    }
                    else
                    {
                        //hdnParam.Value = hdnParam.Value + ",";
                        List<string> lst = new List<string>();
                        lst.Add(hdnParam.Value);
                        lstApmReqID = lst.ToArray();
                    }

                    if (hdnLstParamedicID.Value.Contains(','))
                    {
                        lstParamedicID = hdnLstParamedicID.Value.Split(',');
                    }
                    else
                    {
                        //hdnParam.Value = hdnParam.Value + ",";
                        List<string> lst = new List<string>();
                        lst.Add(hdnLstParamedicID.Value);
                        lstParamedicID = lst.ToArray();
                    }

                    if (hdnLstApmDate.Value.Contains(','))
                    {
                        lstApmReqDate = hdnLstApmDate.Value.Split(',');
                    }
                    else
                    {
                        //hdnParam.Value = hdnParam.Value + ",";
                        List<string> lst = new List<string>();
                        lst.Add(hdnLstApmDate.Value);
                        lstApmReqDate = lst.ToArray();
                    }

                    if (hdnLstCustomerType.Value.Contains(','))
                    {
                        lstCustomerType = hdnLstCustomerType.Value.Split(',');
                    }
                    else
                    {
                        //hdnParam.Value = hdnParam.Value + ",";
                        List<string> lst = new List<string>();
                        lst.Add(hdnLstCustomerType.Value);
                        lstCustomerType = lst.ToArray();
                    }

                    for (int i = 0; i < lstApmReqID.Length; i++)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        AppointmentRequest appRequest = appointmentRequestDao.Get(Convert.ToInt32(lstApmReqID[i]));
                        if (appRequest != null)
                        {
                            if (appRequest.AppointmentID == null)
                            {
                                Appointment appointment = new Appointment();
                                DateTime stAppo = DateTime.Now;
                                DateTime stAppoValid = DateTime.Now;
                                int hour = 0;
                                int minute = 0;
                                string startTimeCheck = "";
                                string endTimeCheck = "";

                                int paramedicID = Convert.ToInt32(lstParamedicID[i]);
                                int healthcareServiceUnitID = appRequest.HealthcareServiceUnitID;
                                DateTime scheduleDate = Helper.GetDatePickerValue(lstApmReqDate[i]);

                                bool isValid = true;

                                appointment.StartDate = appointment.EndDate = scheduleDate;
                                Int16 daynumber = (Int16)scheduleDate.DayOfWeek;
                                if (daynumber == 0)
                                {
                                    daynumber = 7;
                                }

                                #region validate paramedic Schedule
                                vParamedicSchedule obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                                            healthcareServiceUnitID, paramedicID, daynumber)).FirstOrDefault();

                                vParamedicScheduleDate objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                                "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                                healthcareServiceUnitID, paramedicID, scheduleDate)).FirstOrDefault();

                                ValidateParamedicScSchedule(obj, objSchDate, paramedicID, scheduleDate);
                                #endregion

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                #region validate Visit Type
                                int visitDuration = 0;

                                List<ParamedicVisitType> lstVisitTypeParamedic = BusinessLayer.GetParamedicVisitTypeList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND VisitTypeID = {2}", healthcareServiceUnitID, paramedicID, appRequest.VisitTypeID));
                                vHealthcareServiceUnitCustom VisitTypeHealthcare = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID)).FirstOrDefault();
                                if (lstVisitTypeParamedic.Count > 0)
                                {
                                    visitDuration = lstVisitTypeParamedic.FirstOrDefault().VisitDuration;
                                    appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                    appointment.VisitTypeID = lstVisitTypeParamedic.FirstOrDefault().VisitTypeID;
                                }
                                else
                                {
                                    if (VisitTypeHealthcare.IsHasVisitType)
                                    {
                                        List<vServiceUnitVisitType> lstServiceUnitVisitType = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0} AND VisitTypeID = {1}", healthcareServiceUnitID, appRequest.VisitTypeID));
                                        visitDuration = lstServiceUnitVisitType.FirstOrDefault().VisitDuration;
                                        appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                        appointment.VisitTypeID = lstServiceUnitVisitType.FirstOrDefault().VisitTypeID;
                                    }
                                    else
                                    {
                                        List<VisitType> lstVisitType = BusinessLayer.GetVisitTypeList(string.Format("IsDeleted = 0"));
                                        visitDuration = 15;
                                        appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                        appointment.VisitTypeID = lstVisitType.FirstOrDefault().VisitTypeID;
                                    }
                                }
                                #endregion

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                #region Save Appointment
                                int session = 1;
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

                                    string filterExpression;
                                    List<Appointment> lstAppointment;
                                    DateTime startAppo, endAppo;

                                    if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = objSchDate.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!objSchDate.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                            {
                                                if (objSchDate.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = objSchDate.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = objSchDate.StartTime1;
                                                endTimeCheck = objSchDate.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime2;
                                                        appointment.EndTime = objSchDate.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime2;
                                                    endTimeCheck = objSchDate.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime3;
                                                        appointment.EndTime = objSchDate.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime3;
                                                        endTimeCheck = objSchDate.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime3;
                                                    endTimeCheck = objSchDate.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 4
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime4, objSchDate.EndTime4, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 4;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime4;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 4;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot4)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart4.TimeOfDay || endAppo.TimeOfDay > objSchEnd4.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList4)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime4;
                                                        appointment.EndTime = objSchDate.StartTime4;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime4;
                                                        endTimeCheck = objSchDate.EndTime4;

                                                        session = 4;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime4;
                                                    endTimeCheck = objSchDate.EndTime4;

                                                    session = 4;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 5
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime5, objSchDate.EndTime5, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 5;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime5.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime5.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime5;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 5;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot5)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart5.TimeOfDay || endAppo.TimeOfDay > objSchEnd5.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList5)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime5;
                                                        appointment.EndTime = objSchDate.StartTime5;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime5;
                                                        endTimeCheck = objSchDate.EndTime5;

                                                        session = 5;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime5;
                                                    endTimeCheck = objSchDate.EndTime5;

                                                    session = 5;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = objSchDate.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!objSchDate.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                            {
                                                if (objSchDate.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = objSchDate.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = objSchDate.StartTime1;
                                                endTimeCheck = objSchDate.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime2;
                                                        appointment.EndTime = objSchDate.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime2;
                                                    endTimeCheck = objSchDate.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime3;
                                                        appointment.EndTime = objSchDate.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime3;
                                                        endTimeCheck = objSchDate.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime3;
                                                    endTimeCheck = objSchDate.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 4
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime4, objSchDate.EndTime4, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 4;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime4;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 4;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot4)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart4.TimeOfDay || endAppo.TimeOfDay > objSchEnd4.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList4)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime4;
                                                        appointment.EndTime = objSchDate.StartTime4;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime4;
                                                        endTimeCheck = objSchDate.EndTime4;

                                                        session = 4;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime4;
                                                    endTimeCheck = objSchDate.EndTime4;

                                                    session = 4;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = objSchDate.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!objSchDate.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                            {
                                                if (objSchDate.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = objSchDate.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = objSchDate.StartTime1;
                                                endTimeCheck = objSchDate.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime2;
                                                        appointment.EndTime = objSchDate.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime2;
                                                    endTimeCheck = objSchDate.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime3;
                                                        appointment.EndTime = objSchDate.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime3;
                                                        endTimeCheck = objSchDate.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime3;
                                                    endTimeCheck = objSchDate.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = objSchDate.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!objSchDate.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                            {
                                                if (objSchDate.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = objSchDate.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = objSchDate.StartTime1;
                                                endTimeCheck = objSchDate.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime2;
                                                        appointment.EndTime = objSchDate.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime2;
                                                    endTimeCheck = objSchDate.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = objSchDate.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!objSchDate.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                            {
                                                if (objSchDate.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = objSchDate.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = objSchDate.StartTime1;
                                                endTimeCheck = objSchDate.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion
                                    }
                                }
                                else if (obj != null && objSchDate == null)
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

                                    string filterExpression;
                                    List<Appointment> lstAppointment;
                                    DateTime startAppo, endAppo;

                                    if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = obj.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!obj.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                            {
                                                if (obj.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = obj.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = obj.StartTime1;
                                                endTimeCheck = obj.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = obj.StartTime2;
                                                        appointment.EndTime = obj.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime2;
                                                    endTimeCheck = obj.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = obj.StartTime3;
                                                        appointment.EndTime = obj.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime3;
                                                        endTimeCheck = obj.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime3;
                                                    endTimeCheck = obj.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 4
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime4, obj.EndTime4, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 4;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime4.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime4;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 4;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot4)
                                            {
                                                if (startAppo.TimeOfDay < objStart4.TimeOfDay || endAppo.TimeOfDay > objEnd4.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList4)
                                                    {
                                                        appointment.StartTime = obj.StartTime4;
                                                        appointment.EndTime = obj.StartTime4;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime4;
                                                        endTimeCheck = obj.EndTime4;

                                                        session = 4;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime4;
                                                    endTimeCheck = obj.EndTime4;

                                                    session = 4;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 5
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime5, obj.EndTime5, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 5;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime5.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime5.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime5;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 5;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot5)
                                            {
                                                if (startAppo.TimeOfDay < objStart5.TimeOfDay || endAppo.TimeOfDay > objEnd5.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList5)
                                                    {
                                                        appointment.StartTime = obj.StartTime5;
                                                        appointment.EndTime = obj.StartTime5;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime5;
                                                        endTimeCheck = obj.EndTime5;

                                                        session = 5;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime5;
                                                    endTimeCheck = obj.EndTime5;

                                                    session = 5;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = obj.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!obj.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                            {
                                                if (obj.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = obj.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = obj.StartTime1;
                                                endTimeCheck = obj.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = obj.StartTime2;
                                                        appointment.EndTime = obj.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime2;
                                                    endTimeCheck = obj.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = obj.StartTime3;
                                                        appointment.EndTime = obj.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime3;
                                                        endTimeCheck = obj.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime3;
                                                    endTimeCheck = obj.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 4
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime4, obj.EndTime4, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 4;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime4.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime4;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 4;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot4)
                                            {
                                                if (startAppo.TimeOfDay < objStart4.TimeOfDay || endAppo.TimeOfDay > objEnd4.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList4)
                                                    {
                                                        appointment.StartTime = obj.StartTime4;
                                                        appointment.EndTime = obj.StartTime4;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime4;
                                                        endTimeCheck = obj.EndTime4;

                                                        session = 4;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime4;
                                                    endTimeCheck = obj.EndTime4;

                                                    session = 4;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = obj.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!obj.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                            {
                                                if (obj.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = obj.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = obj.StartTime1;
                                                endTimeCheck = obj.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = obj.StartTime2;
                                                        appointment.EndTime = obj.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime2;
                                                    endTimeCheck = obj.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = obj.StartTime3;
                                                        appointment.EndTime = obj.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime3;
                                                        endTimeCheck = obj.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime3;
                                                    endTimeCheck = obj.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = obj.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!obj.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                            {
                                                if (obj.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = obj.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = obj.StartTime1;
                                                endTimeCheck = obj.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = obj.StartTime2;
                                                        appointment.EndTime = obj.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime2;
                                                    endTimeCheck = obj.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = obj.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!obj.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                            {
                                                if (obj.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = obj.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = obj.StartTime1;
                                                endTimeCheck = obj.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }

                                #region finalisasi appointment
                                if (isValid)
                                {
                                    List<Appointment> lstAppointment;
                                    string filterExpression;
                                    if (appRequest.GuestID != null & appRequest.GuestID > 0)
                                    {
                                        filterExpression = string.Format("GuestID = {0} AND StartDate = '{1}' AND ParamedicID = {2} AND HealthcareServiceUnitID = {3}", appRequest.GuestID, appRequest.AppointmentDate.ToString(Constant.FormatString.DATE_FORMAT_112), appRequest.ParamedicID, appRequest.HealthcareServiceUnitID);
                                    }
                                    else
                                    {
                                        filterExpression = string.Format("MRN = {0} AND StartDate = '{1}' AND ParamedicID = {2} AND HealthcareServiceUnitID = {3}", appRequest.MRN, appRequest.AppointmentDate.ToString(Constant.FormatString.DATE_FORMAT_112), appRequest.ParamedicID, appRequest.HealthcareServiceUnitID);
                                    }

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);
                                    vRegistration7 reg = null;
                                    vPatient entityPatient = null;
                                    AppointmentItemPackage entityApmItemPackage = new AppointmentItemPackage();
                                    if (lstAppointment.Count > 0)
                                    {

                                        result = false;
                                        errMessage = string.Format("Sudah ada perjanjian untuk pasien ini dengan nomor perjanjian {0}", lstAppointment.FirstOrDefault().AppointmentNo);
                                        ctx.RollBackTransaction();

                                    }
                                    else
                                    {
                                        if (appRequest.RegistrationID != null)
                                        {
                                            reg = BusinessLayer.GetvRegistration7List(string.Format("RegistrationID = {0}", appRequest.RegistrationID)).FirstOrDefault();
                                        }
                                        else
                                        {
                                            if (appRequest.MRN > 0 && appRequest.MRN != null)
                                            {
                                                entityPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", appRequest.MRN)).FirstOrDefault();
                                            }
                                        }

                                        if (reg != null)
                                        {
                                            if (reg.MRN == 0 && reg.GuestID != 0)
                                            {
                                                appointment.IsNewPatient = true;
                                                appointment.Name = reg.PatientName;
                                                appointment.GCGender = reg.GCGender;
                                                appointment.StreetName = reg.StreetName;
                                                appointment.PhoneNo = reg.PhoneNo1;
                                                appointment.MobilePhoneNo = reg.MobilePhoneNo1;
                                                appointment.GCSalutation = reg.GCSalutation;
                                                appointment.CorporateAccountNo = reg.CorporateAccountNo;
                                                appointment.CorporateAccountName = reg.CorporateAccountName;
                                            }
                                            else if (reg.MRN != 0 && reg.GuestID == 0)
                                            {
                                                appointment.IsNewPatient = false;
                                                appointment.MRN = reg.MRN;
                                            }

                                            appointment.Notes = string.Format("From Appointment Request | No Registration: {0}", reg.RegistrationNo);
                                        }
                                        else
                                        {
                                            if (entityPatient != null)
                                            {
                                                appointment.IsNewPatient = false;
                                                appointment.MRN = entityPatient.MRN;
                                                appointment.Name = entityPatient.PatientName;
                                                appointment.GCGender = entityPatient.GCGender;
                                                appointment.StreetName = entityPatient.StreetName;
                                                appointment.PhoneNo = entityPatient.PhoneNo1;
                                                appointment.MobilePhoneNo = entityPatient.MobilePhoneNo1;
                                                appointment.GCSalutation = entityPatient.GCSalutation;
                                                appointment.CorporateAccountNo = entityPatient.CorporateAccountNo;
                                                appointment.CorporateAccountName = entityPatient.CorporateAccountName;
                                            }
                                            else
                                            {
                                                if (appRequest.GuestID != null && appRequest.GuestID > 0)
                                                {
                                                    Guest entityGuestCheck = BusinessLayer.GetGuestList(string.Format("GuestID = {0}", appRequest.GuestID)).FirstOrDefault();
                                                    if (entityGuestCheck != null)
                                                    {
                                                        appointment.IsNewPatient = false;
                                                        appointment.GuestID = entityGuestCheck.GuestID;
                                                        appointment.Name = entityGuestCheck.Name;
                                                        appointment.GCGender = entityGuestCheck.GCGender;
                                                        appointment.StreetName = entityGuestCheck.StreetName;
                                                        appointment.PhoneNo = entityGuestCheck.PhoneNo;
                                                        appointment.MobilePhoneNo = entityGuestCheck.MobilePhoneNo;
                                                        appointment.GCSalutation = entityGuestCheck.GCSalutation;
                                                        appointment.CorporateAccountNo = entityGuestCheck.CorporateAccountNo;
                                                        appointment.CorporateAccountName = entityGuestCheck.CorporateAccountName;
                                                    }
                                                    else
                                                    {
                                                        appointment.IsNewPatient = false;
                                                        appointment.MRN = entityPatient.MRN;
                                                        appointment.Name = entityPatient.PatientName;
                                                        appointment.GCGender = entityPatient.GCGender;
                                                        appointment.StreetName = entityPatient.StreetName;
                                                        appointment.PhoneNo = entityPatient.PhoneNo1;
                                                        appointment.MobilePhoneNo = entityPatient.MobilePhoneNo1;
                                                        appointment.GCSalutation = entityPatient.GCSalutation;
                                                        appointment.CorporateAccountNo = entityPatient.CorporateAccountNo;
                                                        appointment.CorporateAccountName = entityPatient.CorporateAccountName;
                                                    }
                                                }
                                            }
                                        }

                                        appointment.HealthcareServiceUnitID = healthcareServiceUnitID;
                                        appointment.ParamedicID = paramedicID;
                                        appointment.FromVisitID = appRequest.VisitID;
                                        //appointment.ItemID = appRequest.ItemID;
                                        //if (!string.IsNullOrEmpty(hdnGCCustomerType.Value))
                                        //{
                                        //    appointment.GCCustomerType = hdnGCCustomerType.Value;
                                        //}
                                        //else
                                        //{
                                        //    appointment.GCCustomerType = Constant.CustomerType.PERSONAL;
                                        //}
                                        if (!string.IsNullOrEmpty(lstCustomerType[i]))
                                        {
                                            appointment.GCCustomerType = lstCustomerType[i];
                                        }
                                        else
                                        {
                                            appointment.GCCustomerType = Constant.CustomerType.PERSONAL;
                                        }
                                        if (appRequest.BusinessPartnerID != 0 && appRequest.BusinessPartnerID != null)
                                        {
                                            appointment.BusinessPartnerID = appRequest.BusinessPartnerID;
                                        }
                                        appointment.ContractID = null;
                                        appointment.CoverageTypeID = null;
                                        appointment.CoverageLimitAmount = 0;
                                        appointment.IsCoverageLimitPerDay = false;
                                        appointment.GCTariffScheme = null;
                                        appointment.IsControlClassCare = false;
                                        appointment.ControlClassID = null;
                                        appointment.EmployeeID = null;
                                        appointment.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                                        appointment.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        appointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(appointment.TransactionCode, appointment.StartDate, ctx);
                                        appointment.Session = session;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(appointment.HealthcareServiceUnitID, Convert.ToInt32
(appointment.ParamedicID), appointment.StartDate, session, 1, ctx));
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        if (appRequest.GCAppointmentRequestMethod == Constant.AppointmentRequestMethod.MOBILE_APPS)
                                        {
                                            appointment.GCAppointmentMethod = Constant.AppointmentMethod.MOBILE;
                                        }
                                        else
                                        {
                                            appointment.GCAppointmentMethod = Constant.AppointmentMethod.CALLCENTER;
                                        }
                                        appointment.CreatedBy = AppSession.UserLogin.UserID;
                                        int appointmentID = appointmentDao.InsertReturnPrimaryKeyID(appointment);

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        appRequest.AppointmentID = appointmentID;
                                        appRequest.AppointmentDate = appointment.StartDate;
                                        appRequest.AppointmentTime = appointment.StartTime;
                                        appRequest.ParamedicID = appointment.ParamedicID;
                                        appRequest.AppointmentDate = appointment.StartDate;
                                        appRequest.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        appointmentRequestDao.Update(appRequest);

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        if (appRequest.RegistrationID != null && appRequest.RegistrationID != 0)
                                        {
                                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(Convert.ToInt32(appRequest.RegistrationID));
                                            if (regBPJS != null)
                                            {
                                                regBPJS.AppointmentID = appointmentID;
                                                regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                registrationBPJSDao.Update(regBPJS);
                                            }
                                        }

                                        appointmentRegID = appRequest.AppointmentRequestID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        InsertAppointmentItemPackageTariff(appointmentID, Convert.ToInt32(hdnMCUClassID.Value), Convert.ToInt32(appRequest.ItemID), DateTime.Now, ctx, entityApmItemPackage, entityApmItemPackageDao);

                                        //BridgingToMedinfrasMobileApps("save", appointment.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2), appointment.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2), Convert.ToInt32(hdnAppointmentRequestID.Value));

                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = "Maaf Tidak Ada Jadwal Praktek Dokter Untuk Tanggal Yang Dipilih";
                                    ctx.RollBackTransaction();
                                    break;
                                }
                                #endregion
                                #endregion
                            }

                            else
                            {
                                result = false;
                                errMessage = "Appointment Request ini sudah generate appointment";
                                ctx.RollBackTransaction();
                                break;
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Appointment Request Tidak ditemukan";
                            ctx.RollBackTransaction();
                            break;
                        }
                    }

                    if (result)
                    {
                        ctx.CommitTransaction();
                    }
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

        private bool createAppointmentToRegistration(ref string errMessage, ref int appointmentRegID)
        {
            bool result = true;
            int appointmentID = 0;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao appointmentDao = new AppointmentDao(ctx);
            ParamedicScheduleDao paramedicScheduleDao = new ParamedicScheduleDao(ctx);
            ParamedicScheduleDateDao paramedicScheduleDateDao = new ParamedicScheduleDateDao(ctx);
            ParamedicLeaveScheduleDao paramedicLeaveScheduleDao = new ParamedicLeaveScheduleDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            AppointmentRequestDao appointmentRequestDao = new AppointmentRequestDao(ctx);
            AppointmentItemPackageDao entityApmItemPackageDao = new AppointmentItemPackageDao(ctx);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            RegistrationPayerDao entityRegistrationPayerDao = new RegistrationPayerDao(ctx);
            ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);

            string[] lstApmReqID = null;
            string[] lstParamedicID = null;
            string[] lstApmReqDate = null;
            string[] lstCustomerType = null;

            try
            {
                if (!String.IsNullOrEmpty(hdnParam.Value))
                {
                    if (hdnParam.Value.Contains(','))
                    {
                        lstApmReqID = hdnParam.Value.Split(',');
                    }
                    else
                    {
                        //hdnParam.Value = hdnParam.Value + ",";
                        List<string> lst = new List<string>();
                        lst.Add(hdnParam.Value);
                        lstApmReqID = lst.ToArray();
                    }

                    if (hdnLstParamedicID.Value.Contains(','))
                    {
                        lstParamedicID = hdnLstParamedicID.Value.Split(',');
                    }
                    else
                    {
                        //hdnParam.Value = hdnParam.Value + ",";
                        List<string> lst = new List<string>();
                        lst.Add(hdnLstParamedicID.Value);
                        lstParamedicID = lst.ToArray();
                    }

                    if (hdnLstApmDate.Value.Contains(','))
                    {
                        lstApmReqDate = hdnLstApmDate.Value.Split(',');
                    }
                    else
                    {
                        //hdnParam.Value = hdnParam.Value + ",";
                        List<string> lst = new List<string>();
                        lst.Add(hdnLstApmDate.Value);
                        lstApmReqDate = lst.ToArray();
                    }

                    if (hdnLstCustomerType.Value.Contains(','))
                    {
                        lstCustomerType = hdnLstCustomerType.Value.Split(',');
                    }
                    else
                    {
                        //hdnParam.Value = hdnParam.Value + ",";
                        List<string> lst = new List<string>();
                        lst.Add(hdnLstCustomerType.Value);
                        lstCustomerType = lst.ToArray();
                    }

                    for (int i = 0; i < lstApmReqID.Length; i++)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        AppointmentRequest appRequest = appointmentRequestDao.Get(Convert.ToInt32(lstApmReqID[i]));
                        if (appRequest != null)
                        {
                            //if (appRequest.AppointmentID == null)
                            //{
                                
                            //}
                            //else
                            //{
                            //    result = false;
                            //    errMessage = "Appointment Request ini sudah generate appointment";
                            //    ctx.RollBackTransaction();
                            //    break;
                            //}
                            Appointment appointment = new Appointment();
                            DateTime stAppo = DateTime.Now;
                            DateTime stAppoValid = DateTime.Now;
                            int hour = 0;
                            int minute = 0;
                            string startTimeCheck = "";
                            string endTimeCheck = "";

                            int paramedicID = Convert.ToInt32(lstParamedicID[i]);
                            int healthcareServiceUnitID = appRequest.HealthcareServiceUnitID;
                            DateTime scheduleDate = Helper.GetDatePickerValue(lstApmReqDate[i]);

                            bool isValid = true;

                            appointment.StartDate = appointment.EndDate = scheduleDate;
                            Int16 daynumber = (Int16)scheduleDate.DayOfWeek;
                            if (daynumber == 0)
                            {
                                daynumber = 7;
                            }

                            List<Appointment> lstAppointmentCheck = new List<Appointment>();
                            RegistrationPayer entityRegistrationPayer = new RegistrationPayer();
                            vRegistration7 reg = null;
                            vPatient entityPatient = null;
                            AppointmentItemPackage entityApmItemPackage = new AppointmentItemPackage();

                            if (cboFilterType.Value.ToString() != "2" || cboFilterType.Value.ToString() != "0")
                            {
                                #region validate paramedic Schedule
                                vParamedicSchedule obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                                            healthcareServiceUnitID, paramedicID, daynumber)).FirstOrDefault();

                                vParamedicScheduleDate objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                                "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                                healthcareServiceUnitID, paramedicID, scheduleDate)).FirstOrDefault();

                                ValidateParamedicScSchedule(obj, objSchDate, paramedicID, scheduleDate);
                                #endregion

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                #region validate Visit Type
                                int visitDuration = 0;

                                List<ParamedicVisitType> lstVisitTypeParamedic = BusinessLayer.GetParamedicVisitTypeList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND VisitTypeID = {2}", healthcareServiceUnitID, paramedicID, appRequest.VisitTypeID));
                                vHealthcareServiceUnitCustom VisitTypeHealthcare = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID)).FirstOrDefault();
                                if (lstVisitTypeParamedic.Count > 0)
                                {
                                    visitDuration = lstVisitTypeParamedic.FirstOrDefault().VisitDuration;
                                    appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                    appointment.VisitTypeID = lstVisitTypeParamedic.FirstOrDefault().VisitTypeID;
                                }
                                else
                                {
                                    if (VisitTypeHealthcare.IsHasVisitType)
                                    {
                                        List<vServiceUnitVisitType> lstServiceUnitVisitType = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0} AND VisitTypeID = {1}", healthcareServiceUnitID, appRequest.VisitTypeID));
                                        visitDuration = lstServiceUnitVisitType.FirstOrDefault().VisitDuration;
                                        appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                        appointment.VisitTypeID = lstServiceUnitVisitType.FirstOrDefault().VisitTypeID;
                                    }
                                    else
                                    {
                                        List<VisitType> lstVisitType = BusinessLayer.GetVisitTypeList(string.Format("IsDeleted = 0"));
                                        visitDuration = 15;
                                        appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                        appointment.VisitTypeID = lstVisitType.FirstOrDefault().VisitTypeID;
                                    }
                                }
                                #endregion

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                #region Save Appointment
                                int session = 1;

                                #region Validate Schedule
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

                                    string filterExpression;
                                    List<Appointment> lstAppointment;
                                    DateTime startAppo, endAppo;

                                    if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = objSchDate.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!objSchDate.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                            {
                                                if (objSchDate.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = objSchDate.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = objSchDate.StartTime1;
                                                endTimeCheck = objSchDate.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime2;
                                                        appointment.EndTime = objSchDate.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime2;
                                                    endTimeCheck = objSchDate.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime3;
                                                        appointment.EndTime = objSchDate.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime3;
                                                        endTimeCheck = objSchDate.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime3;
                                                    endTimeCheck = objSchDate.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 4
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime4, objSchDate.EndTime4, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 4;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime4;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 4;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot4)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart4.TimeOfDay || endAppo.TimeOfDay > objSchEnd4.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList4)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime4;
                                                        appointment.EndTime = objSchDate.StartTime4;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime4;
                                                        endTimeCheck = objSchDate.EndTime4;

                                                        session = 4;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime4;
                                                    endTimeCheck = objSchDate.EndTime4;

                                                    session = 4;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 5
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime5, objSchDate.EndTime5, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 5;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime5.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime5.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime5;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 5;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot5)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart5.TimeOfDay || endAppo.TimeOfDay > objSchEnd5.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList5)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime5;
                                                        appointment.EndTime = objSchDate.StartTime5;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime5;
                                                        endTimeCheck = objSchDate.EndTime5;

                                                        session = 5;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime5;
                                                    endTimeCheck = objSchDate.EndTime5;

                                                    session = 5;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = objSchDate.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!objSchDate.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                            {
                                                if (objSchDate.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = objSchDate.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = objSchDate.StartTime1;
                                                endTimeCheck = objSchDate.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime2;
                                                        appointment.EndTime = objSchDate.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime2;
                                                    endTimeCheck = objSchDate.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime3;
                                                        appointment.EndTime = objSchDate.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime3;
                                                        endTimeCheck = objSchDate.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime3;
                                                    endTimeCheck = objSchDate.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 4
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime4, objSchDate.EndTime4, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 4;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime4;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 4;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot4)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart4.TimeOfDay || endAppo.TimeOfDay > objSchEnd4.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList4)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime4;
                                                        appointment.EndTime = objSchDate.StartTime4;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime4;
                                                        endTimeCheck = objSchDate.EndTime4;

                                                        session = 4;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime4;
                                                    endTimeCheck = objSchDate.EndTime4;

                                                    session = 4;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = objSchDate.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!objSchDate.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                            {
                                                if (objSchDate.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = objSchDate.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = objSchDate.StartTime1;
                                                endTimeCheck = objSchDate.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime2;
                                                        appointment.EndTime = objSchDate.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime2;
                                                    endTimeCheck = objSchDate.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime3;
                                                        appointment.EndTime = objSchDate.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime3;
                                                        endTimeCheck = objSchDate.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime3;
                                                    endTimeCheck = objSchDate.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = objSchDate.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!objSchDate.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                            {
                                                if (objSchDate.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = objSchDate.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = objSchDate.StartTime1;
                                                endTimeCheck = objSchDate.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime2;
                                                        appointment.EndTime = objSchDate.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime2;
                                                    endTimeCheck = objSchDate.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = objSchDate.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!objSchDate.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                            {
                                                if (objSchDate.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = objSchDate.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = objSchDate.StartTime1;
                                                endTimeCheck = objSchDate.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion
                                    }
                                }
                                else if (obj != null && objSchDate == null)
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

                                    string filterExpression;
                                    List<Appointment> lstAppointment;
                                    DateTime startAppo, endAppo;

                                    if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = obj.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!obj.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                            {
                                                if (obj.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = obj.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = obj.StartTime1;
                                                endTimeCheck = obj.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = obj.StartTime2;
                                                        appointment.EndTime = obj.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime2;
                                                    endTimeCheck = obj.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = obj.StartTime3;
                                                        appointment.EndTime = obj.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime3;
                                                        endTimeCheck = obj.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime3;
                                                    endTimeCheck = obj.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 4
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime4, obj.EndTime4, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 4;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime4.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime4;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 4;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot4)
                                            {
                                                if (startAppo.TimeOfDay < objStart4.TimeOfDay || endAppo.TimeOfDay > objEnd4.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList4)
                                                    {
                                                        appointment.StartTime = obj.StartTime4;
                                                        appointment.EndTime = obj.StartTime4;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime4;
                                                        endTimeCheck = obj.EndTime4;

                                                        session = 4;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime4;
                                                    endTimeCheck = obj.EndTime4;

                                                    session = 4;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 5
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime5, obj.EndTime5, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 5;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime5.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime5.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime5;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 5;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot5)
                                            {
                                                if (startAppo.TimeOfDay < objStart5.TimeOfDay || endAppo.TimeOfDay > objEnd5.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList5)
                                                    {
                                                        appointment.StartTime = obj.StartTime5;
                                                        appointment.EndTime = obj.StartTime5;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime5;
                                                        endTimeCheck = obj.EndTime5;

                                                        session = 5;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime5;
                                                    endTimeCheck = obj.EndTime5;

                                                    session = 5;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = obj.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!obj.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                            {
                                                if (obj.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = obj.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = obj.StartTime1;
                                                endTimeCheck = obj.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = obj.StartTime2;
                                                        appointment.EndTime = obj.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime2;
                                                    endTimeCheck = obj.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = obj.StartTime3;
                                                        appointment.EndTime = obj.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime3;
                                                        endTimeCheck = obj.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime3;
                                                    endTimeCheck = obj.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 4
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime4, obj.EndTime4, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 4;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime4.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime4;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 4;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot4)
                                            {
                                                if (startAppo.TimeOfDay < objStart4.TimeOfDay || endAppo.TimeOfDay > objEnd4.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList4)
                                                    {
                                                        appointment.StartTime = obj.StartTime4;
                                                        appointment.EndTime = obj.StartTime4;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime4;
                                                        endTimeCheck = obj.EndTime4;

                                                        session = 4;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime4;
                                                    endTimeCheck = obj.EndTime4;

                                                    session = 4;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = obj.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!obj.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                            {
                                                if (obj.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = obj.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = obj.StartTime1;
                                                endTimeCheck = obj.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = obj.StartTime2;
                                                        appointment.EndTime = obj.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime2;
                                                    endTimeCheck = obj.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion

                                        #region check slot time 3
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 3;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime3;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 3;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot3)
                                            {
                                                if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList3)
                                                    {
                                                        appointment.StartTime = obj.StartTime3;
                                                        appointment.EndTime = obj.StartTime3;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime3;
                                                        endTimeCheck = obj.EndTime3;

                                                        session = 3;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime3;
                                                    endTimeCheck = obj.EndTime3;

                                                    session = 3;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = obj.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!obj.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                            {
                                                if (obj.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = obj.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = obj.StartTime1;
                                                endTimeCheck = obj.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion

                                        #region check slot time 2
                                        if (!isValid)
                                        {
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 2;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime2;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 2;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot2)
                                            {
                                                if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList2)
                                                    {
                                                        appointment.StartTime = obj.StartTime2;
                                                        appointment.EndTime = obj.StartTime2;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime2;
                                                    endTimeCheck = obj.EndTime2;

                                                    session = 2;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                        }
                                        #endregion
                                    }
                                    else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                    {
                                        #region check slot time 1
                                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression, ctx);

                                        if (lstAppointment.Count > 0)
                                        {
                                            //set jam mulai dan jam selesai Appointment
                                            hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                            minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                            stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                            //end

                                            appointment.StartTime = stAppo.ToString("HH:mm");
                                            appointment.EndTime = stAppoValid.ToString("HH:mm");

                                            session = 1;
                                        }
                                        else
                                        {
                                            int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                            appointment.StartTime = obj.StartTime1;
                                            appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                            session = 1;
                                        }

                                        startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                        endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                        if (!obj.IsAppointmentByTimeSlot1)
                                        {
                                            if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                            {
                                                if (obj.IsAllowWaitingList1)
                                                {
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = obj.StartTime1;
                                                    appointment.IsWaitingList = true;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                            }
                                            else
                                            {
                                                appointment.IsWaitingList = false;
                                                startTimeCheck = obj.StartTime1;
                                                endTimeCheck = obj.EndTime1;

                                                session = 1;
                                            }
                                        }
                                        else
                                        {
                                            isValid = false;
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    isValid = false;
                                }
                                #endregion

                                #region finalisasi appointment
                                if (isValid)
                                {
                                    string filterExpression;
                                    if (appRequest.GuestID != null & appRequest.GuestID > 0)
                                    {
                                        filterExpression = string.Format("GuestID = {0} AND StartDate = '{1}' AND ParamedicID = {2} AND HealthcareServiceUnitID = {3}", appRequest.GuestID, appRequest.AppointmentDate.ToString(Constant.FormatString.DATE_FORMAT_112), appRequest.ParamedicID, appRequest.HealthcareServiceUnitID);
                                    }
                                    else
                                    {
                                        filterExpression = string.Format("MRN = {0} AND StartDate = '{1}' AND ParamedicID = {2} AND HealthcareServiceUnitID = {3}", appRequest.MRN, appRequest.AppointmentDate.ToString(Constant.FormatString.DATE_FORMAT_112), appRequest.ParamedicID, appRequest.HealthcareServiceUnitID);
                                    }

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    lstAppointmentCheck = BusinessLayer.GetAppointmentList(filterExpression, ctx);
                                    if (lstAppointmentCheck.Count > 0)
                                    {

                                        result = false;
                                        errMessage = string.Format("Sudah ada perjanjian untuk pasien ini dengan nomor perjanjian {0}", lstAppointmentCheck.FirstOrDefault().AppointmentNo);
                                        ctx.RollBackTransaction();

                                    }
                                    else
                                    {
                                        if (appRequest.RegistrationID != null)
                                        {
                                            reg = BusinessLayer.GetvRegistration7List(string.Format("RegistrationID = {0}", appRequest.RegistrationID)).FirstOrDefault();
                                        }
                                        else
                                        {
                                            if (appRequest.MRN > 0 && appRequest.MRN != null)
                                            {
                                                entityPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", appRequest.MRN)).FirstOrDefault();
                                            }
                                        }

                                        if (reg != null)
                                        {
                                            if (reg.MRN == 0 && reg.GuestID != 0)
                                            {
                                                appointment.IsNewPatient = true;
                                                appointment.Name = reg.PatientName;
                                                appointment.GCGender = reg.GCGender;
                                                appointment.StreetName = reg.StreetName;
                                                appointment.PhoneNo = reg.PhoneNo1;
                                                appointment.MobilePhoneNo = reg.MobilePhoneNo1;
                                                appointment.GCSalutation = reg.GCSalutation;
                                                appointment.CorporateAccountNo = reg.CorporateAccountNo;
                                                appointment.CorporateAccountName = reg.CorporateAccountName;
                                            }
                                            else if (reg.MRN != 0 && reg.GuestID == 0)
                                            {
                                                appointment.IsNewPatient = false;
                                                appointment.MRN = reg.MRN;
                                            }

                                            appointment.Notes = string.Format("From Appointment Request | No Registration: {0}", reg.RegistrationNo);
                                        }
                                        else
                                        {
                                            if (entityPatient != null)
                                            {
                                                appointment.IsNewPatient = false;
                                                appointment.MRN = entityPatient.MRN;
                                                appointment.Name = entityPatient.PatientName;
                                                appointment.GCGender = entityPatient.GCGender;
                                                appointment.StreetName = entityPatient.StreetName;
                                                appointment.PhoneNo = entityPatient.PhoneNo1;
                                                appointment.MobilePhoneNo = entityPatient.MobilePhoneNo1;
                                                appointment.GCSalutation = entityPatient.GCSalutation;
                                                appointment.CorporateAccountNo = entityPatient.CorporateAccountNo;
                                                appointment.CorporateAccountName = entityPatient.CorporateAccountName;
                                            }
                                            else
                                            {
                                                if (appRequest.GuestID != null && appRequest.GuestID > 0)
                                                {
                                                    Guest entityGuestCheck = BusinessLayer.GetGuestList(string.Format("GuestID = {0}", appRequest.GuestID)).FirstOrDefault();
                                                    if (entityGuestCheck != null)
                                                    {
                                                        appointment.IsNewPatient = false;
                                                        appointment.GuestID = entityGuestCheck.GuestID;
                                                        appointment.Name = entityGuestCheck.Name;
                                                        appointment.GCGender = entityGuestCheck.GCGender;
                                                        appointment.StreetName = entityGuestCheck.StreetName;
                                                        appointment.PhoneNo = entityGuestCheck.PhoneNo;
                                                        appointment.MobilePhoneNo = entityGuestCheck.MobilePhoneNo;
                                                        appointment.GCSalutation = entityGuestCheck.GCSalutation;
                                                        appointment.CorporateAccountNo = entityGuestCheck.CorporateAccountNo;
                                                        appointment.CorporateAccountName = entityGuestCheck.CorporateAccountName;
                                                    }
                                                    else
                                                    {
                                                        appointment.IsNewPatient = false;
                                                        appointment.MRN = entityPatient.MRN;
                                                        appointment.Name = entityPatient.PatientName;
                                                        appointment.GCGender = entityPatient.GCGender;
                                                        appointment.StreetName = entityPatient.StreetName;
                                                        appointment.PhoneNo = entityPatient.PhoneNo1;
                                                        appointment.MobilePhoneNo = entityPatient.MobilePhoneNo1;
                                                        appointment.GCSalutation = entityPatient.GCSalutation;
                                                        appointment.CorporateAccountNo = entityPatient.CorporateAccountNo;
                                                        appointment.CorporateAccountName = entityPatient.CorporateAccountName;
                                                    }
                                                }
                                            }
                                        }

                                        appointment.HealthcareServiceUnitID = healthcareServiceUnitID;
                                        appointment.ParamedicID = paramedicID;
                                        appointment.FromVisitID = appRequest.VisitID;
                                        //appointment.ItemID = appRequest.ItemID;
                                        //if (!string.IsNullOrEmpty(hdnGCCustomerType.Value))
                                        //{
                                        //    appointment.GCCustomerType = hdnGCCustomerType.Value;
                                        //}
                                        //else
                                        //{
                                        //    appointment.GCCustomerType = Constant.CustomerType.PERSONAL;
                                        //}
                                        if (!string.IsNullOrEmpty(lstCustomerType[i]))
                                        {
                                            appointment.GCCustomerType = lstCustomerType[i];
                                        }
                                        else
                                        {
                                            appointment.GCCustomerType = Constant.CustomerType.PERSONAL;
                                        }
                                        if (appRequest.BusinessPartnerID != 0 && appRequest.BusinessPartnerID != null)
                                        {
                                            appointment.BusinessPartnerID = appRequest.BusinessPartnerID;
                                        }
                                        appointment.ContractID = null;
                                        appointment.CoverageTypeID = null;
                                        appointment.CoverageLimitAmount = 0;
                                        appointment.IsCoverageLimitPerDay = false;
                                        appointment.GCTariffScheme = null;
                                        appointment.IsControlClassCare = false;
                                        appointment.ControlClassID = null;
                                        appointment.EmployeeID = null;
                                        appointment.GCAppointmentStatus = Constant.AppointmentStatus.COMPLETE;
                                        appointment.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        appointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(appointment.TransactionCode, appointment.StartDate, ctx);
                                        appointment.Session = session;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(appointment.HealthcareServiceUnitID, Convert.ToInt32
(appointment.ParamedicID), appointment.StartDate, session, 1, ctx));
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        if (appRequest.GCAppointmentRequestMethod == Constant.AppointmentRequestMethod.MOBILE_APPS)
                                        {
                                            appointment.GCAppointmentMethod = Constant.AppointmentMethod.MOBILE;
                                        }
                                        else
                                        {
                                            appointment.GCAppointmentMethod = Constant.AppointmentMethod.CALLCENTER;
                                        }
                                        appointment.CreatedBy = AppSession.UserLogin.UserID;
                                        appointmentID = appointmentDao.InsertReturnPrimaryKeyID(appointment);

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        appRequest.AppointmentID = appointmentID;
                                        appRequest.AppointmentDate = appointment.StartDate;
                                        appRequest.AppointmentTime = appointment.StartTime;
                                        appRequest.ParamedicID = appointment.ParamedicID;
                                        appRequest.AppointmentDate = appointment.StartDate;
                                        appRequest.LastUpdatedBy = AppSession.UserLogin.UserID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        if (appRequest.RegistrationID != null && appRequest.RegistrationID != 0)
                                        {
                                            RegistrationBPJS regBPJS = registrationBPJSDao.Get(Convert.ToInt32(appRequest.RegistrationID));
                                            if (regBPJS != null)
                                            {
                                                regBPJS.AppointmentID = appointmentID;
                                                regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                registrationBPJSDao.Update(regBPJS);
                                            }
                                        }

                                        appointmentRegID = appRequest.AppointmentRequestID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        InsertAppointmentItemPackageTariff(appointmentID, Convert.ToInt32(hdnMCUClassID.Value), Convert.ToInt32(appRequest.ItemID), DateTime.Now, ctx, entityApmItemPackage, entityApmItemPackageDao);

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = "Maaf Tidak Ada Jadwal Praktek Dokter Untuk Tanggal Yang Dipilih";
                                    ctx.RollBackTransaction();
                                    break;
                                }
                                #endregion
                                #endregion
                            }
                            else
                            {
                                appointment = BusinessLayer.GetAppointmentList(string.Format("AppointmentID = '{0}'", appRequest.AppointmentID)).FirstOrDefault();
                                appointmentID = appointment.AppointmentID;
                            }


                            #region Registration & Consult Visit
                            Registration entityRegistration = new Registration();
                            entityRegistration.TransactionCode = Constant.TransactionCode.MCU_REGISTRATION;
                            entityRegistration.AppointmentID = appointmentID;
                            entityRegistration.RegistrationDate = DateTime.Now;
                            entityRegistration.RegistrationTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            entityRegistration.ReferrerDate = DateTime.Now;
                            entityRegistration.ReferrerTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                            CustomerContract entityCustomerContract = BusinessLayer.GetCustomerContractList(string.Format("BusinessPartnerID = {0} AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0", appointment.BusinessPartnerID), ctx).FirstOrDefault();
                            Customer entityCustomer = BusinessLayer.GetCustomer(Convert.ToInt32(appointment.BusinessPartnerID));
                            CoverageType entityCoverage = null;
                            if (entityCustomerContract != null)
                            {
                                entityCoverage = BusinessLayer.GetCoverageTypeList(string.Format("CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = {0}) AND IsDeleted = 0 AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = {1}))", entityCustomerContract.ContractID, appointment.BusinessPartnerID)).FirstOrDefault();
                            }
                            entityRegistration.GCRegistrationStatus = Constant.VisitStatus.CHECKED_IN;
                            if (appointment.MRN > 0)
                            {
                                entityRegistration.MRN = appointment.MRN;
                                entityRegistration.AgeInYear = Convert.ToInt16(entityPatient.AgeInYear);
                                entityRegistration.AgeInMonth = Convert.ToInt16(entityPatient.AgeInMonth);
                                entityRegistration.AgeInDay = Convert.ToInt16(entityPatient.AgeInDay);
                            }
                            else
                            {
                                entityRegistration.GuestID = appointment.GuestID;
                            }
                            entityRegistration.BusinessPartnerID = Convert.ToInt16(appointment.BusinessPartnerID);
                            if (entityCustomerContract != null)
                            {
                                entityRegistration.ContractID = entityCustomerContract.ContractID;
                            }
                            if (entityCustomer != null)
                            {
                                entityRegistration.GCTariffScheme = entityCustomer.GCTariffScheme;
                                entityRegistration.GCCustomerType = entityCustomer.GCCustomerType;
                            }
                            else
                            {
                                entityRegistration.GCTariffScheme = Constant.TariffScheme.Standard;
                            }
                            if (entityCoverage != null)
                            {
                                entityRegistration.CoverageTypeID = entityCoverage.CoverageTypeID;
                            }
                            entityRegistration.GCAdmissionSource = Constant.AdmissionSource.EMERGENCY;
                            entityRegistration.CreatedBy = AppSession.UserLogin.UserID;
                            entityRegistration.CreatedDate = DateTime.Now;
                            entityRegistration.RegistrationNo = BusinessLayer.GenerateTransactionNo(entityRegistration.TransactionCode, entityRegistration.RegistrationDate, ctx);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entityRegistration.RegistrationID = entityRegistrationDao.InsertReturnPrimaryKeyID(entityRegistration);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            if (entityRegistration.BusinessPartnerID != 1)
                            {
                                entityRegistrationPayer.BusinessPartnerID = entityRegistration.BusinessPartnerID;
                                entityRegistrationPayer.ContractID = entityCustomerContract.ContractID;
                                entityRegistrationPayer.GCCustomerType = entityRegistration.GCCustomerType;
                                if (entityCoverage != null)
                                {
                                    entityRegistrationPayer.CoverageTypeID = entityCoverage.CoverageTypeID;
                                }
                                //entityRegistrationPayer.CorporateAccountNo = txtParticipantNo.Text;
                                //entityRegistrationPayer.CorporateAccountName = Request.Form[txtPatientName.UniqueID];
                                entityRegistrationPayer.CoverageLimitAmount = 0;
                                entityRegistrationPayer.IsPrimaryPayer = true;
                                entityRegistrationPayerDao.Insert(entityRegistrationPayer);
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            ConsultVisit entityVisit = new ConsultVisit();
                            entityVisit.RegistrationID = entityRegistration.RegistrationID;
                            entityVisit.HealthcareServiceUnitID = appointment.HealthcareServiceUnitID;
                            entityVisit.ClassID = Convert.ToInt32(BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode = '{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MEDICAL_CHECKUP_CLASS), ctx).FirstOrDefault().ParameterValue);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entityVisit.ChargeClassID = entityVisit.ClassID;

                            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", appointment.ParamedicID), ctx).FirstOrDefault();
                            entityVisit.ParamedicID = entityPM.ParamedicID;
                            entityVisit.SpecialtyID = entityPM.SpecialtyID;

                            List<GetParamedicVisitTypeList> lstVisit = BusinessLayer.GetParamedicVisitTypeList(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), "", ctx);
                            entityVisit.VisitTypeID = lstVisit.FirstOrDefault().VisitTypeID;
                            entityVisit.VisitDate = entityRegistration.RegistrationDate;
                            entityVisit.VisitTime = entityRegistration.RegistrationTime;
                            entityVisit.ActualVisitDate = entityVisit.VisitDate;
                            entityVisit.ActualVisitTime = entityVisit.VisitTime;
                            entityVisit.GCVisitReason = Constant.VisitReason.OTHER;
                            entityVisit.Session = appointment.Session;
                            entityVisit.QueueNo = appointment.QueueNo;
                            entityVisit.IsMainVisit = true;
                            entityVisit.IsNewVisit = true;
                            entityVisit.CreatedBy = AppSession.UserLogin.UserID;
                            entityVisit.CreatedDate = DateTime.Now;
                            entityVisit.GCVisitStatus = Constant.VisitStatus.CHECKED_IN;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entityVisit.VisitID = entityVisitDao.InsertReturnPrimaryKeyID(entityVisit);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            appRequest.RegistrationID = entityRegistration.RegistrationID;
                            appRequest.VisitID = entityVisit.VisitID;
                            appointmentRequestDao.Update(appRequest);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            if (entityRegistration.GCRegistrationStatus == Constant.VisitStatus.CHECKED_IN)
                            {
                                String ChargeCodeAdministration = string.Empty;
                                bool flagSkipAdm = false;
                                if (hdnIsControlAdministrationCharges.Value == "1" && entityRegistrationPayer.GCCustomerType != Constant.CustomerType.PERSONAL) ChargeCodeAdministration = hdnChargeCodeAdministrationForInstansi.Value;
                                int itemMCUID = 0;
                                itemMCUID = Convert.ToInt32(appRequest.ItemID);

                                if (hdnIsControlAdmCost.Value == "1")
                                {
                                    if (entityRegistration.MRN != 0 && entityRegistration.MRN != null)
                                    {
                                        int flagAdmCount = BusinessLayer.GetvPatientChargesDtList(string.Format("MRN = {0} AND ItemID = '{1}' AND datediff(day, RegistrationDate, '{2}') = 0  AND GCTransactionStatus = '{3}'", entityRegistration.MRN, hdnAdminID.Value, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.CLOSED), ctx).ToList().Count;
                                        if (flagAdmCount > 0) flagSkipAdm = true;
                                    }
                                }
                                //Check : Kontrol Pembayaran Kartu
                                if (hdnIsControlPatientCardPayment.Value != "1")
                                {
                                    Helper.InsertAutoBillItem(ctx, entityVisit, hdnDepartmentID.Value, (int)entityVisit.ChargeClassID, entityRegistration.GCCustomerType, entityRegistration.IsPrintingPatientCard, itemMCUID, ChargeCodeAdministration, flagSkipAdm);
                                }
                                else
                                {
                                    if (entityRegistration.IsPrintingPatientCard && hdnItemCardFee.Value != "")
                                    {
                                        Helper.InsertPatientCardBillItem(ctx, entityVisit, hdnDepartmentID.Value, (int)entityVisit.ChargeClassID);
                                    }
                                    Helper.InsertAutoBillItem(ctx, entityVisit, hdnDepartmentID.Value, (int)entityVisit.ChargeClassID, entityRegistration.GCCustomerType, false, itemMCUID, ChargeCodeAdministration, flagSkipAdm);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            result = false;
                            errMessage = "Appointment Request Tidak ditemukan";
                            ctx.RollBackTransaction();
                            break;
                        }
                    }

                    if (result)
                    {
                        ctx.CommitTransaction();
                    }
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

        private void ValidateParamedicScSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate, int paramedicID, DateTime appointmentDate)
        {
            //Int32 ParamedicID = Convert.ToInt32(hdnAppointmentRequestParamedicID.Value);
            //DateTime selectedDate = Helper.GetDatePickerValue(hdnAppointmentRequestDate.Value);
            Int32 ParamedicID = paramedicID;
            DateTime selectedDate = appointmentDate;
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            return result;
        }

        private bool validateAppointmentRequest(ref string errMessage)
        {
            bool result = true;
            StringBuilder err = new StringBuilder();

            string[] lstApmReqID = null;
            if (hdnParam.Value.Contains(','))
            {
                lstApmReqID = hdnParam.Value.Split(',');
            }
            else
            {
                List<string> lst = new List<string>();
                lst.Add(hdnParam.Value);
                lstApmReqID = lst.ToArray();
            }

            for (int i = 0; i < lstApmReqID.Length; i++)
            {
                vAppointmentRequest entity = BusinessLayer.GetvAppointmentRequestList(string.Format("AppointmentRequestID = {0}", lstApmReqID[i])).FirstOrDefault();
                if (rblProcessType.SelectedValue == "0") //PERJANJIAN
                {
                    if (!string.IsNullOrEmpty(entity.AppointmentNo))
                    {
                        err.AppendLine(string.Format("Tidak bisa melanjutkan proses. Terdapat permintaan perjanjian yang sudah diproses."));
                        break;
                    }
                }
                else //REGISTRASI
                {
                    if (!string.IsNullOrEmpty(entity.RegistrationNo))
                    {
                        err.AppendLine(string.Format("Tidak bisa melanjutkan proses. Terdapat permintaan perjanjian sudah diproses menjadi registrasi."));
                        break;
                    }
                } 
            }


            if (!string.IsNullOrEmpty(err.ToString()))
            {
                errMessage = err.ToString();
                result = false;
            }

            return result;
        }

        private bool ValidateVoidAppointment(ref string errMessage)
        {
            bool result = true;
            StringBuilder err = new StringBuilder();

            List<vAppointmentRequest> lst = BusinessLayer.GetvAppointmentRequestList(string.Format("AppointmentRequestID IN ({0})", hdnParam.Value));
            if (lst.Count > 0)
            {
                foreach (vAppointmentRequest a in lst)
                {
                    if (!string.IsNullOrEmpty(a.RegistrationNo))
                    {
                        err.AppendLine("Tidak bisa membatalkan permintaan, sudah ada data yang diregistrasikan");
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(err.ToString()))
            {
                errMessage = err.ToString();
                result = false;
            }

            return result;
        }

        private bool PrintAppointmentRequestLabel(ref string errMessage)
        {
            bool result = true;

            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                    ipAddress, Constant.DirectPrintType.LABEL_PERMINTAAN_MCU_KARYAWAN);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            List<vAppointmentRequest> lstApmReq = BusinessLayer.GetvAppointmentRequestList(string.Format("AppointmentRequestID IN ({0})", hdnParam.Value));

            if (lstPrinter.Count > 0)
            {
                if (lstApmReq.Count > 0)
                {
                    if (!ZebraPrinting.PrintMCUAppointmentRequestLabel(lstPrinter, lstApmReq, ref errMessage))
                    {
                        result = false;
                    }
                }
                else
                {
                    errMessage = string.Format("No Data To Be Printed", ipAddress);
                    result = false;
                }
            }
            else
            {
                errMessage = string.Format("No configuration for IP {0}", ipAddress);
                result = false;
            }

            return result;
        }

        #region Binding to AppointmentItemPackage
        public void InsertAppointmentItemPackageTariff(int apmID, int classID, int itemID, DateTime transactionDate, IDbContext ctx, AppointmentItemPackage entity, AppointmentItemPackageDao dao)
        {
            //string GCItemType = BusinessLayer.GetItemMaster(itemID).GCItemType;
            List<GetCurrentDraftItemTariff> list = BusinessLayer.GetCurrentDraftItemTariff(apmID, classID, itemID, 1, transactionDate, ctx, 0);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            if (list.Count > 0)
            {
                GetCurrentDraftItemTariff obj = list[0];
                entity = new AppointmentItemPackage();
                entity.AppointmentID = apmID;
                entity.ItemID = itemID;
                entity.Quantity = 1;
                entity.Tariff = obj.Price;
                entity.BaseTariff = obj.BasePrice;
                entity.TariffComp1 = obj.PriceComp1;
                entity.TariffComp2 = obj.PriceComp2;
                entity.TariffComp3 = obj.PriceComp3;
                entity.BaseComp1 = obj.BasePriceComp1;
                entity.BaseComp2 = obj.BasePriceComp2;
                entity.BaseComp3 = obj.BasePriceComp3;
                entity.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                entity.IsMainPackage = true;
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
            }
            //dao.InsertReturnPrimaryKeyID(entity);
            dao.Insert(entity);
        }
        #endregion
    }
}