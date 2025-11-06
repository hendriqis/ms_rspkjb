using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProcessMultiVisitScheduleOrderCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;
            //hdnRegistrationIDCtl.Value = param;
            string[] paramArr = param.Split('|');
            hdnProcessTypeCtl.Value = paramArr[0];
            hdnScheduleIDCtl.Value = paramArr[1];

            hdnCalAppointmentSelectedDateCtl.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CUSTOMER_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboRegistrationPayerCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0 AND IsUsedInChargeClass = 1");
            Methods.SetComboBoxField<ClassCare>(cboControlClassCareCtl, lstClassCare, "ClassName", "ClassID");
            cboControlClassCareCtl.SelectedIndex = 0;

            vDiagnosticVisitSchedule oSchedule = BusinessLayer.GetvDiagnosticVisitScheduleList(string.Format("ID = {0}", hdnScheduleIDCtl.Value)).FirstOrDefault();
            hdnTestOrderID.Value = oSchedule.TestOrderID.ToString();
            txtOrderNoCtl.Text = oSchedule.TestOrderNo;
            txtMRNCtl.Text = oSchedule.MedicalNo;
            hdnMRNCtl.Value = oSchedule.MRN.ToString();
            txtPatientNameCtl.Text = oSchedule.PatientName;
            hdnDepartmentIDCtl.Value = oSchedule.OrderDepartmentID;
            txtSequenceNo.Text = oSchedule.SequenceNo.ToString();
            txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnHealthcareServiceUnitIDCtl.Value = oSchedule.OrderHealthcareServiceUnitID.ToString();
            txtItemName.Text = oSchedule.ItemName1;
            cboRegistrationPayerCtl.Value = oSchedule.FromGCCustomerType;
            hdnPayerIDCtl.Value = oSchedule.FromBusinessPartnerID.ToString();
            txtPayerCompanyCodeCtl.Text = oSchedule.FromBusinessPartnerCode;
            txtPayerCompanyNameCtl.Text = oSchedule.FromBusinessPartnerName;
            txtSchedulePeriode.Text = string.Format("{0} s/d {1}", oSchedule.cfStartDateSchedule, oSchedule.cfEndDateSchedule);

            if (oSchedule.FromGCCustomerType == Constant.CustomerType.PERSONAL)
            {
                tblPayerCompanyCtl.Attributes.Add("style", "display:none");
                chkUsingCOBCtl.Attributes.Add("style", "display:none");
            }
            else
            {
                tblPayerCompanyCtl.Attributes.Remove("style");
                chkUsingCOBCtl.Attributes.Remove("style");
                hdnContractIDCtl.Value = oSchedule.FromContractID.ToString();
                txtContractNoCtl.Text = oSchedule.FromContractNo;
                hdnCoverageTypeIDCtl.Value = oSchedule.FromCoverageTypeID.ToString();
                txtCoverageLimitCtl.Text = oSchedule.FromCoverageLimitAmount.ToString("N2");
                txtCoverageTypeCodeCtl.Text = oSchedule.FromCoverageTypeCode;
                txtCoverageTypeNameCtl.Text = oSchedule.FromCoverageTypeName;
                chkIsCoverageLimitPerDayCtl.Checked = oSchedule.FromIsCoverageLimitPerDay;
                hdnGCTariffSchemeCtl.Value = oSchedule.FromGCTariffScheme;
                hdnIsControlClassCareCtl.Value = oSchedule.FromIsControlClassCare.ToString();
                cboControlClassCareCtl.Value = oSchedule.FromControlClassID.ToString();
                hdnEmployeeIDCtl.Value = oSchedule.FromEmployeeID.ToString();
                txtParticipantNoCtl.Text = oSchedule.FromCorporateAccountNo;

                if (oSchedule.FromGCCustomerType == Constant.CustomerType.HEALTHCARE)
                {
                    trEmployeeCtl.Attributes.Remove("style");
                }
                else
                {
                    trEmployeeCtl.Attributes.Add("style", "display:none");
                }


                BindGridPhysician(1, true, ref PageCount);

            }
        }

        protected string GetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM CustomerContract WHERE '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0)", DateTime.Now.ToString("yyyyMMdd"));
        }

        protected string GetCustomerTypePersonal()
        {
            return Constant.CustomerType.PERSONAL;
        }

        protected string GetCustomerTypeHealthcare()
        {
            return Constant.CustomerType.HEALTHCARE;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPatientNameCtl, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMRNCtl, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtOrderNoCtl, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(cboRegistrationPayerCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnPayerIDCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPayerCompanyCodeCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPayerCompanyNameCtl, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboControlClassCareCtl, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(hdnContractIDCtl, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtContractNoCtl, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(hdnCoverageTypeIDCtl, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCoverageTypeCodeCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCoverageTypeNameCtl, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(hdnEmployeeIDCtl, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtEmployeeCodeCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEmployeeNameCtl, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(txtParticipantNoCtl, new ControlEntrySetting(true, true, false));
        }

        #region List Physician
        protected void cbpParamedicViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridPhysician(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "get")
                {
                    if (lstScheduleByDate != null)
                    {
                        lstScheduleByDate.Clear();
                    }
                    BindGridPhysician(1, false, ref pageCount);
                }
                else if (param[0] == "paramedic")
                {
                    int paramedicID = Convert.ToInt32(param[1]);
                    string json = BindSessionCtl(paramedicID);
                    result = param[0] + "|" + json;
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

        protected void CbpGetPhysicianSchedule_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridPhysician(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "get")
                {
                    if (lstScheduleByDate != null)
                    {
                        lstScheduleByDate.Clear();
                    }
                    BindGridPhysician(1, false, ref pageCount);
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

        private void BindGridPhysician(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            pageCount = 0;
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);

            lstScheduleByDate = BusinessLayer.GetParamedicScheduleByDateList(selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112));

            //cboSessionCtl.SelectedIndex = 0;
            grdPhysician.DataSource = lstScheduleByDate.Where(w => w.HealthcareServiceUnitID == Convert.ToInt32(hdnHealthcareServiceUnitIDCtl.Value)).ToList();
            grdPhysician.DataBind();
        }

        private string BindSessionCtl(int paramedicID)
        {
            GetParamedicScheduleByDate obj = lstScheduleByDate.Where(w => w.HealthcareServiceUnitID == Convert.ToInt32(hdnHealthcareServiceUnitIDCtl.Value) && w.ParamedicID == paramedicID).FirstOrDefault();

            string[] sessionArr = obj.OperationalTimeSession.Split('#');
            
            List<StandardCode> lstSession = new List<StandardCode>();
            for (int i = 1; i <= sessionArr.Length - 1; i++)
            {
                int idx = i - 1;
                string[] session = sessionArr[i].Split('|');
                string[] sessionTime = session[1].Split('-');
                if (!string.IsNullOrEmpty(sessionTime[0]))
                {
                    lstSession.Insert(idx, new StandardCode
                    {
                        StandardCodeName = string.Format("Sesi {0} ({1} - {2})", i, sessionTime[0], sessionTime[1]),
                        StandardCodeID = i.ToString(),
                        IsHeader = true
                    });
                }
            }
            Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
            //cboSessionCtl.SelectedIndex = 0;
            return JsonConvert.SerializeObject(lstSession);
        }

        public List<GetParamedicScheduleByDate> lstScheduleByDate
        {
            get
            {
                if (Session["__lstScheduleByDate"] == null)
                    Session["__lstScheduleByDate"] = new List<GetParamedicScheduleByDate>();

                return (List<GetParamedicScheduleByDate>)Session["__lstScheduleByDate"];
            }
            set { Session["__lstScheduleByDate"] = value; }
        }
        #endregion

        #region List Appointment
        public class AppointmentScheduleInfo
        {
            public String StartTime { get; set; }
            public String EndTime { get; set; }
            public String DisplayTime { get; set; }
            public List<vAppointmentCustom> lstAppointment { get; set; }
        }

        public class vAppointmentCustom
        {
            public Int32 AppointmentID { get; set; }
            public String AppointmentNo { get; set; }
            public String MedicalNo { get; set; }
            public String PatientName { get; set; }
            public String ParamedicName { get; set; }
            public String BusinessPartnerName { get; set; }
            public String StartTime { get; set; }
            public String EndTime { get; set; }
            public String ListOrder { get; set; }

        }

        protected void cbpViewDtCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            string summarySch = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "load")
                {
                    int paramedicID = Convert.ToInt32(param[1]);
                    int Session = Convert.ToInt32(param[2]);
                    BindGridViewDt(paramedicID, Session, ref summarySch);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = summarySch;
        }

        protected void grdAppointment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            AppointmentScheduleInfo obj = e.Row.DataItem as AppointmentScheduleInfo;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Repeater rptOrderList = (Repeater)e.Row.FindControl("rptOrderInformation");
                rptOrderList.DataSource = obj.lstAppointment;
                rptOrderList.DataBind();
            }
        }

        private void BindGridViewDt(int paramedicID, int session, ref string summarySch)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);

            string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = '{1}' AND CONVERT(VARCHAR, StartDate, 112) = '{2}' AND GCAppointmentStatus NOT IN ('{3}','{4}') AND Session = {5}", hdnHealthcareServiceUnitIDCtl.Value, paramedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.DELETED, session);

            List<vAppointment> lstOrder = BusinessLayer.GetvAppointmentList(filterExpression);

            GetParamedicScheduleByDate obj = lstScheduleByDate.Where(w => w.HealthcareServiceUnitID == Convert.ToInt32(hdnHealthcareServiceUnitIDCtl.Value) && w.ParamedicID == paramedicID).FirstOrDefault();

            string[] sessionArr = obj.OperationalTimeSession.Split('#')[session].Split('|');
            string[] sessionTime = sessionArr[1].Split('-');
            string startTime = sessionTime[0];
            string endTime = sessionTime[1];

            List<AppointmentScheduleInfo> lstSchedule = new List<AppointmentScheduleInfo>();
            SettingScheduleTable(lstSchedule, lstOrder, obj, startTime, endTime, 1);

            grdAppointment.DataSource = lstSchedule.OrderBy(o => o.StartTime).ToList();
            grdAppointment.DataBind();

            #region Load Summary Schedule Information
            //#1|NON=200&BPJS=200
            string[] totalQuota = obj.MaximumAppointment.Split('#')[session].Split('|');
            int quotaBPJS = Convert.ToInt32(totalQuota[1].Split('&')[1].Split('=')[1]);
            int quotaNonBPJS = Convert.ToInt32(totalQuota[1].Split('&')[0].Split('=')[1]);

            summarySch = string.Format("TotalAppointment={0}|TotalQuota={1}", 
                lstOrder.Count, quotaBPJS + quotaNonBPJS);
            #endregion
        }

        private void SettingScheduleTable(List<AppointmentScheduleInfo> lstSchedule, List<vAppointment> lstOrder, GetParamedicScheduleByDate schedule, String start, String end, Int32 interval)
        {
            bool result = true;

            if (result)
            {
                AppointmentScheduleInfo entity = new AppointmentScheduleInfo();

                #region 00-00 - 07.00
                entity.StartTime = start;
                entity.DisplayTime = string.Format("{0} - {1}", start, end);
                DateTime startTime = DateTime.Parse(String.Format("{0} {1}", Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).Date.ToString(Constant.FormatString.DATE_FORMAT), start));
                DateTime endTime = DateTime.Parse(String.Format("{0} {1}", Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).Date.ToString(Constant.FormatString.DATE_FORMAT), end));
                List<vAppointment> lstOrderByTime = lstOrder.Where(lst => DateTime.Parse(string.Format("{0} {1}", lst.StartDate.ToString(Constant.FormatString.DATE_FORMAT), lst.StartTime)) == startTime || (DateTime.Parse(string.Format("{0} {1}", lst.StartDate.ToString(Constant.FormatString.DATE_FORMAT), lst.StartTime)) > startTime && DateTime.Parse(string.Format("{0} {1}", lst.StartDate.ToString(Constant.FormatString.DATE_FORMAT), lst.StartTime)) < endTime)).OrderBy(lst => lst.StartTime).ToList();
                //entity.lstAppointment = lstOrderByTime;
                //lstSchedule.Add(entity);
                #endregion

                startTime = DateTime.Parse(String.Format("{0} {1}", Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).Date.ToString(Constant.FormatString.DATE_FORMAT), start));
                endTime = DateTime.Parse(String.Format("{0} {1}", Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).Date.ToString(Constant.FormatString.DATE_FORMAT), end));

                while (startTime < endTime)
                {
                    entity = new AppointmentScheduleInfo();
                    entity.StartTime = startTime.ToString("HH:mm");
                    entity.DisplayTime = string.Format("{0} - {1}", startTime.ToString("HH:mm"), startTime.AddHours(interval).ToString("HH:mm"));

                    lstOrderByTime = lstOrder.Where(lst => DateTime.Parse(string.Format("{0} {1}", lst.StartDate.ToString(Constant.FormatString.DATE_FORMAT), lst.StartTime)) == startTime || (DateTime.Parse(string.Format("{0} {1}", lst.StartDate.ToString(Constant.FormatString.DATE_FORMAT), lst.StartTime)) > startTime && DateTime.Parse(string.Format("{0} {1}", lst.StartDate.ToString(Constant.FormatString.DATE_FORMAT), lst.StartTime)) < startTime.AddHours(interval))).OrderBy(lst => lst.StartTime).ToList();

                    List<vAppointmentCustom> lstCustom = new List<vAppointmentCustom>();
                    if (lstOrderByTime.Count > 0)
                    {
                        foreach (vAppointment apm in lstOrderByTime)
                        {
                            vAppointmentCustom apmCustom = new vAppointmentCustom();
                            apmCustom.AppointmentID = apm.AppointmentID;
                            apmCustom.AppointmentNo = apm.AppointmentNo;
                            apmCustom.BusinessPartnerName = apm.BusinessPartnerName;
                            apmCustom.EndTime = apm.EndTime;
                            apmCustom.StartTime = apm.StartTime;
                            apmCustom.MedicalNo = apm.MedicalNo;
                            apmCustom.ParamedicName = apm.ParamedicName;
                            apmCustom.PatientName = apm.PatientName;
                            List<vDraftTestOrderDt> lstOrderDt = BusinessLayer.GetvDraftTestOrderDtList(string.Format("AppointmentID = {0} AND IsDeleted = 0", apm.AppointmentID));
                            string itemName = "Rencana Tindakan: ";
                            if (lstOrderDt.Count > 0)
                            {
                                foreach (vDraftTestOrderDt dt in lstOrderDt)
                                {
                                    itemName += string.Format("{0},", dt.ItemName1);
                                }
                                apmCustom.ListOrder = itemName.Remove(itemName.Length - 1, 1);
                            }
                            lstCustom.Add(apmCustom);
                        }
                    }

                    entity.lstAppointment = lstCustom;

                    startTime = startTime.AddHours(interval);

                    lstSchedule.Add(entity);
                }


            }
        }
        #endregion

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            switch (hdnProcessTypeCtl.Value)
            {
                case "process" :
                    result = OnCreateAppointment(ref errMessage, ref retval);
                    break;
            }

            return result;
        }

        private bool OnCreateAppointment(ref string errMessage, ref string retval)
        {
            bool result = true;
            bool isBPJS = false;
            int hsuID = Convert.ToInt32(hdnHealthcareServiceUnitIDCtl.Value);
            int paramedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
            DateTime appointmentDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
            int duration = Convert.ToInt16(hdnVisitDurationCtl.Value);
            int apmID = 0;
            int session = Convert.ToInt32(cboSessionCtl.Value.ToString());

            string startTime = string.Format("{0}:{1}", hdnScheduleInHour.Value, txtScheduleInMinute.Text);
            string endTime = string.Empty;
            string[] time = startTime.Split(':');
            string hour = time[0];
            string minute = time[1];
            DateTime dateNew = new DateTime(1900, 1, 1, Convert.ToInt32(hour), Convert.ToInt32(minute), 0);
            DateTime dateAfterDuration = dateNew.AddMinutes(duration);
            startTime = dateNew.ToString(Constant.FormatString.TIME_FORMAT);
            endTime = dateAfterDuration.ToString(Constant.FormatString.TIME_FORMAT);

            if (cboRegistrationPayerCtl.Value.ToString() == Constant.CustomerType.BPJS)
            {
                isBPJS = true;
            }

            if (OnCheckParamedicSchedule(hsuID, paramedicID, appointmentDate, duration, isBPJS, session, ref errMessage))
            {
                #region Check Appointment Is Exist
                Appointment entityApm = null;
                if (!string.IsNullOrEmpty(hdnToAppointmentID.Value))
                {
                    entityApm = new Appointment();
                    entityApm = BusinessLayer.GetAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND CONVERT(VARCHAR, StartDate, 112) = '{2}' AND GCAppointmentStatus = '{3}' AND MRN = {4}", hsuID, paramedicID, appointmentDate.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.AppointmentStatus.STARTED, hdnMRNCtl.Value)).FirstOrDefault();
                }
                #endregion

                DiagnosticVisitSchedule schedule = BusinessLayer.GetDiagnosticVisitSchedule(Convert.ToInt32(hdnScheduleIDCtl.Value));
                TestOrderHd tohd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(schedule.TestOrderID));
                List<TestOrderDt> lstTodt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", tohd.TestOrderID, schedule.ItemID));

                #region Process
                IDbContext ctx = DbFactory.Configure(true);
                AppointmentDao entityDao = new AppointmentDao(ctx);
                DraftTestOrderHdDao entityOrderHdDao = new DraftTestOrderHdDao(ctx);
                DraftTestOrderDtDao entityOrderDtDao = new DraftTestOrderDtDao(ctx);
                DiagnosticVisitScheduleDao diagVisitSchDao = new DiagnosticVisitScheduleDao(ctx);

                try
                {
                    DraftTestOrderHd draftHd = new DraftTestOrderHd();
                    if (entityApm == null)
                    {
                        #region Appointment
                        Appointment entity = new Appointment();
                        entity.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_APPOINTMENT;
                        entity.GCCustomerType = cboRegistrationPayerCtl.Value.ToString();

                        //if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                        //{
                        //    isBPJS = true;
                        //}

                        if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
                        {
                            entity.BusinessPartnerID = 1;
                            entity.ContractID = null;
                            entity.CoverageTypeID = null;
                            entity.CoverageLimitAmount = 0;
                            entity.IsCoverageLimitPerDay = false;
                            entity.GCTariffScheme = hdnGCTariffSchemePersonalCtl.Value;
                            entity.IsControlClassCare = false;
                            entity.ControlClassID = null;
                            entity.EmployeeID = null;
                        }
                        else
                        {
                            entity.BusinessPartnerID = entity.GCCustomerType != Constant.CustomerType.PERSONAL ? Convert.ToInt32(hdnPayerIDCtl.Value) : 1;
                            entity.ContractID = Convert.ToInt32(hdnContractIDCtl.Value);
                            entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeIDCtl.Value);
                            entity.CorporateAccountNo = txtParticipantNoCtl.Text;
                            entity.CoverageLimitAmount = Convert.ToDecimal(txtCoverageLimitCtl.Text);
                            entity.IsCoverageLimitPerDay = chkIsCoverageLimitPerDayCtl.Checked;
                            entity.GCTariffScheme = hdnGCTariffSchemeCtl.Value;
                            entity.IsControlClassCare = (hdnIsControlClassCareCtl.Value == "1");

                            if (entity.IsControlClassCare)
                            {
                                entity.ControlClassID = Convert.ToInt32(cboControlClassCareCtl.Value);
                            }
                            else
                            {
                                entity.ControlClassID = null;
                            }

                            if (hdnEmployeeIDCtl.Value == "" || hdnEmployeeIDCtl.Value == "0")
                            {
                                entity.EmployeeID = null;
                            }
                            else
                            {
                                entity.EmployeeID = Convert.ToInt32(hdnEmployeeIDCtl.Value);
                            }
                        }

                        entity.FromVisitID = tohd.VisitID;
                        entity.IsNewPatient = false;
                        entity.MRN = Convert.ToInt32(hdnMRNCtl.Value);
                        entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitIDCtl.Value);
                        entity.ParamedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
                        entity.StartDate = appointmentDate;
                        entity.EndDate = entity.StartDate;
                        entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeIDCtl.Value);
                        entity.VisitDuration = Convert.ToInt16(duration);
                        entity.StartTime = startTime;
                        entity.EndTime = endTime;
                        entity.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                        entity.GCAppointmentMethod = Constant.AppointmentMethod.CALLCENTER;
                        entity.Notes = string.Format("Penjadwalan Multi Kunjungan dari order {0}", tohd.TestOrderNo);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        //entity.Session = BusinessLayer.GetRegistrationSession(entity.HealthcareServiceUnitID, entity.ParamedicID, entity.StartDate, entity.StartTime, ctx, 0);
                        entity.Session = session;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(Convert.ToInt32(entity.HealthcareServiceUnitID), Convert.ToInt32(entity.ParamedicID), entity.StartDate, Convert.ToInt32(entity.Session), false, isBPJS, 1, 0, ctx));

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        entity.AppointmentNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entity.StartDate, ctx);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entity.CreatedDate = DateTime.Now;
                        entity.AppointmentID = entityDao.InsertReturnPrimaryKeyID(entity);
                        apmID = entity.AppointmentID;
                        #endregion

                        #region DraftTestOrderHd

                        draftHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_DRAFT_TEST_ORDER_APPOINTMENT;
                        draftHd.AppointmentID = entity.AppointmentID;
                        draftHd.HealthcareServiceUnitID = hsuID;
                        draftHd.ParamedicID = paramedicID;
                        draftHd.ScheduledDate = entity.StartDate;
                        draftHd.ScheduledTime = entity.StartTime;
                        draftHd.DraftTestOrderDate = entity.StartDate;
                        draftHd.DraftTestOrderTime = entity.StartTime;
                        draftHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
                        draftHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        draftHd.Remarks = entity.Notes;
                        draftHd.CreatedBy = AppSession.UserLogin.UserID;
                        draftHd.CreatedDate = DateTime.Now;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        draftHd.DraftTestOrderNo = BusinessLayer.GenerateTransactionNo(draftHd.TransactionCode, draftHd.DraftTestOrderDate, ctx);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        draftHd.DraftTestOrderID = entityOrderHdDao.InsertReturnPrimaryKeyID(draftHd);
                        #endregion
                    }
                    else
                    {
                        draftHd = BusinessLayer.GetDraftTestOrderHdList(string.Format("AppointmentID = {0} AND GCTransactionStatus = '{1}'", entityApm.AppointmentID, Constant.TransactionStatus.OPEN)).FirstOrDefault();
                        apmID = entityApm.AppointmentID;
                    }
                    #region DraftTestOrderDt
                    if (lstTodt.Count > 0)
                    {
                        foreach (TestOrderDt dt in lstTodt)
                        {
                            if (appointmentDate >= dt.StartDateSchedule && appointmentDate <= dt.EndDateSchedule)
                            {
                                DraftTestOrderDt draftDt = new DraftTestOrderDt();
                                draftDt.DraftTestOrderID = draftHd.DraftTestOrderID;
                                draftDt.ItemID = dt.ItemID;
                                draftDt.GCDraftTestOrderStatus = Constant.TestOrderStatus.OPEN;
                                draftDt.ItemQty = 1;
                                draftDt.ItemUnit = dt.ItemUnit;
                                draftDt.Remarks = string.Format("{0}|Tindakan ke : {1}", dt.Remarks, schedule.SequenceNo);
                                draftDt.CreatedBy = AppSession.UserLogin.UserID;
                                draftDt.CreatedDate = DateTime.Now;
                                entityOrderDtDao.Insert(draftDt);
                            }
                            else
                            {
                                result = false;
                                if (appointmentDate < dt.StartDateSchedule)
                                {
                                    errMessage = "Pembuatan jadwal tidak bisa kurang dari tanggal mulai (Tanggal mulai : " + dt.StartDateSchedule.ToString(Constant.FormatString.DATE_FORMAT) + ")";
                                }
                                else if (appointmentDate > dt.EndDateSchedule)
                                {
                                    errMessage = "Pembuatan jadwal tidak bisa lebih dari tanggal akhir (Tanggal akhir : " + dt.EndDateSchedule.ToString(Constant.FormatString.DATE_FORMAT) + ")";
                                }
                                ctx.RollBackTransaction();
                                break;
                            }
                        }
                    }
                    #endregion

                    if (result)
                    {
                        #region Diagnostic Multi Visit
                        schedule.GCDiagnosticScheduleStatus = Constant.DiagnosticVisitScheduleStatus.STARTED;
                        schedule.ScheduledDate = appointmentDate;
                        schedule.AppointmentID = apmID;
                        schedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                        schedule.LastUpdatedDate = DateTime.Now;

                        diagVisitSchDao.Update(schedule);
                        #endregion

                        ctx.CommitTransaction();

                        retval = string.Format("{0}|{1}|{2}|{3}", schedule.ID, schedule.GCDiagnosticScheduleStatus, schedule.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), schedule.AppointmentID);
                    }
                }
                catch (Exception ex)
                {
                    ctx.RollBackTransaction();
                    errMessage = ex.Message;
                    result = false;
                }
                finally
                {
                    ctx.Close();
                }
                #endregion
            }
            else
            {
                result = false;
            }

            return result;
        }

        private string OnCreateRegistration(ref string errMessage)
        {
            string result = "1|";

            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityDao = new RegistrationDao(ctx);
            ConsultVisitDao entityCVDao = new ConsultVisitDao(ctx);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);

            try
            {
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = "0|" + ex.Message;
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private bool OnCheckParamedicSchedule(int hsuID, int paramedicID, DateTime appointmentDate, int duration, bool isBPJS, int session, ref string errMessage)
        {
            bool result = true;

            if (!chkIsUsingSameAppointment.Checked)
            {

                int daynumber = (int)appointmentDate.DayOfWeek;
                if (daynumber == 0)
                {
                    daynumber = 7;
                }

                ParamedicLeaveSchedule leave = BusinessLayer.GetParamedicLeaveScheduleList(string.Format("ParamedicID = {0} AND ('{1}' BETWEEN CONVERT(VARCHAR,StartDate,112) AND CONVERT(VARCHAR,EndDate,112)) AND IsDeleted = 0 ORDER BY ID DESC", hsuID, appointmentDate.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
                if (leave != null)
                {
                    result = false;
                    errMessage = string.Format("Dokter / Tenaga Medis sedang dalam masa cuti ({0} s/d {1})", leave.StartDate.ToString(Constant.FormatString.DATE_FORMAT), leave.EndDate.ToString(Constant.FormatString.DATE_FORMAT));
                }
                else
                {
                    #region Schedule
                    vParamedicScheduleDate schDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND CONVERT(VARCHAR,ScheduleDate,112) = '{2}'", hsuID, paramedicID, appointmentDate.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
                    if (schDate != null)
                    {

                        if (!OnCheckAppointmentQuota(hsuID, paramedicID, appointmentDate, isBPJS, session, ref errMessage, schDate))
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        vParamedicSchedule sch = BusinessLayer.GetvParamedicScheduleList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}", hsuID, paramedicID, daynumber)).FirstOrDefault();
                        if (sch != null)
                        {
                            if (!OnCheckAppointmentQuota(hsuID, paramedicID, appointmentDate, isBPJS, session, ref errMessage, null, sch))
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Tidak ada jadwal dokter");
                        }
                    }
                    #endregion
                }
            }
            return result;
        }

        private bool OnCheckAppointmentQuota(int hsuID, int paramedicID, DateTime appointmentDate, bool isBPJS, int session, ref string errMessage, vParamedicScheduleDate oSchDate = null, vParamedicSchedule oSch = null)
        {
            bool result = true;

            string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND CONVERT(VARCHAR,StartDate,112) = '{2}' AND GCAppointmentStatus NOT IN ('{3}','{4}')", hsuID, paramedicID, appointmentDate.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED);

            if (isBPJS)
            {
                filterExpression += string.Format(" AND GCCustomerType = '{0}'", Constant.CustomerType.BPJS);
            }

            List<Appointment> lstApm = BusinessLayer.GetAppointmentList(filterExpression);

            #region Variable
            int totalQuota = 0;
            int totalApm = lstApm.Count;
            string noQuotaMessage = "Tidak dapat melakukan penjadwalan, sudah melebihi kuota jadwal dokter.";
            string notAcceptPatientBPJSMessage = "Tidak menerima pasien BPJS. Harap cek konfigurasi jadwal dokter.";
            string notAcceptPatientMessage = "Tidak menerima pasien NON BPJS. Harap cek konfigurasi jadwal dokter.";
            #endregion

            if (lstApm.Count > 0)
            {
                if (session == 5)
                {
                    #region Session 5
                    if (oSchDate != null)
                    {
                        if (isBPJS)
                        {
                            if (oSchDate.IsBPJS5)
                            {
                                totalQuota = oSchDate.MaximumAppointmentBPJS5;
                                if (totalApm >= totalQuota)
                                {
                                    result = false;
                                    errMessage = noQuotaMessage;
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = notAcceptPatientBPJSMessage;
                            }
                        }
                        else
                        {
                            if (oSchDate.IsNonBPJS5)
                            {
                                totalQuota = oSchDate.MaximumAppointment5 + oSchDate.MaximumAppointmentBPJS5;
                                if (totalApm >= totalQuota)
                                {
                                    result = false;
                                    errMessage = noQuotaMessage;
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = notAcceptPatientMessage;
                            }
                        }
                    }
                    else
                    {
                        if (oSch != null)
                        {
                            if (isBPJS)
                            {
                                if (oSch.IsBPJS5)
                                {
                                    totalQuota = oSch.MaximumAppointmentBPJS5;
                                    if (totalApm >= totalQuota)
                                    {
                                        result = false;
                                        errMessage = noQuotaMessage;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = notAcceptPatientBPJSMessage;
                                }
                            }
                            else
                            {
                                if (oSch.IsNonBPJS5)
                                {
                                    totalQuota = oSch.MaximumAppointment5 + oSch.MaximumAppointmentBPJS5;
                                    if (totalApm >= totalQuota)
                                    {
                                        result = false;
                                        errMessage = noQuotaMessage;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = notAcceptPatientMessage;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (session == 4)
                {
                    #region Session 4
                    if (oSchDate != null)
                    {
                        if (isBPJS)
                        {
                            if (oSchDate.IsBPJS4)
                            {
                                totalQuota = oSchDate.MaximumAppointmentBPJS4;
                                if (totalApm >= totalQuota)
                                {
                                    result = false;
                                    errMessage = noQuotaMessage;
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = notAcceptPatientBPJSMessage;
                            }
                        }
                        else
                        {
                            if (oSchDate.IsNonBPJS4)
                            {
                                totalQuota = oSchDate.MaximumAppointment4 + oSchDate.MaximumAppointmentBPJS4;
                                if (totalApm >= totalQuota)
                                {
                                    result = false;
                                    errMessage = noQuotaMessage;
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = notAcceptPatientMessage;
                            }
                        }
                    }
                    else
                    {
                        if (oSch != null)
                        {
                            if (isBPJS)
                            {
                                if (oSch.IsBPJS4)
                                {
                                    totalQuota = oSch.MaximumAppointmentBPJS4;
                                    if (totalApm >= totalQuota)
                                    {
                                        result = false;
                                        errMessage = noQuotaMessage;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = notAcceptPatientBPJSMessage;
                                }
                            }
                            else
                            {
                                if (oSch.IsNonBPJS4)
                                {
                                    totalQuota = oSch.MaximumAppointment4 + oSch.MaximumAppointmentBPJS4;
                                    if (totalApm >= totalQuota)
                                    {
                                        result = false;
                                        errMessage = noQuotaMessage;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = notAcceptPatientMessage;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (session == 3)
                {
                    #region Session 3
                    if (oSchDate != null)
                    {
                        if (isBPJS)
                        {
                            if (oSchDate.IsBPJS3)
                            {
                                totalQuota = oSchDate.MaximumAppointmentBPJS3;
                                if (totalApm >= totalQuota)
                                {
                                    result = false;
                                    errMessage = noQuotaMessage;
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = notAcceptPatientBPJSMessage;
                            }
                        }
                        else
                        {
                            if (oSchDate.IsNonBPJS3)
                            {
                                totalQuota = oSchDate.MaximumAppointment3 + oSchDate.MaximumAppointmentBPJS3;
                                if (totalApm >= totalQuota)
                                {
                                    result = false;
                                    errMessage = noQuotaMessage;
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = notAcceptPatientMessage;
                            }
                        }
                    }
                    else
                    {
                        if (oSch != null)
                        {
                            if (isBPJS)
                            {
                                if (oSch.IsBPJS3)
                                {
                                    totalQuota = oSch.MaximumAppointmentBPJS3;
                                    if (totalApm >= totalQuota)
                                    {
                                        result = false;
                                        errMessage = noQuotaMessage;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = notAcceptPatientBPJSMessage;
                                }
                            }
                            else
                            {
                                if (oSch.IsNonBPJS3)
                                {
                                    totalQuota = oSch.MaximumAppointment3 + oSch.MaximumAppointmentBPJS3;
                                    if (totalApm >= totalQuota)
                                    {
                                        result = false;
                                        errMessage = noQuotaMessage;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = notAcceptPatientMessage;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (session == 2)
                {
                    #region Session 2
                    if (oSchDate != null)
                    {
                        if (isBPJS)
                        {
                            if (oSchDate.IsBPJS2)
                            {
                                totalQuota = oSchDate.MaximumAppointmentBPJS2;
                                if (totalApm >= totalQuota)
                                {
                                    result = false;
                                    errMessage = noQuotaMessage;
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = notAcceptPatientBPJSMessage;
                            }
                        }
                        else
                        {
                            if (oSchDate.IsNonBPJS2)
                            {
                                totalQuota = oSchDate.MaximumAppointment2 + oSchDate.MaximumAppointmentBPJS2;
                                if (totalApm >= totalQuota)
                                {
                                    result = false;
                                    errMessage = noQuotaMessage;
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = notAcceptPatientMessage;
                            }
                        }
                    }
                    else
                    {
                        if (oSch != null)
                        {
                            if (isBPJS)
                            {
                                if (oSch.IsBPJS2)
                                {
                                    totalQuota = oSch.MaximumAppointmentBPJS2;
                                    if (totalApm >= totalQuota)
                                    {
                                        result = false;
                                        errMessage = noQuotaMessage;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = notAcceptPatientBPJSMessage;
                                }
                            }
                            else
                            {
                                if (oSch.IsNonBPJS2)
                                {
                                    totalQuota = oSch.MaximumAppointment2 + oSch.MaximumAppointmentBPJS2;
                                    if (totalApm >= totalQuota)
                                    {
                                        result = false;
                                        errMessage = noQuotaMessage;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = notAcceptPatientMessage;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Session 1
                    if (oSchDate != null)
                    {
                        if (isBPJS)
                        {
                            if (oSchDate.IsBPJS1)
                            {
                                totalQuota = oSchDate.MaximumAppointmentBPJS1;
                                if (totalApm >= totalQuota)
                                {
                                    result = false;
                                    errMessage = noQuotaMessage;
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = notAcceptPatientBPJSMessage;
                            }
                        }
                        else
                        {
                            if (oSchDate.IsNonBPJS1)
                            {
                                totalQuota = oSchDate.MaximumAppointment1 + oSchDate.MaximumAppointmentBPJS1;
                                if (totalApm >= totalQuota)
                                {
                                    result = false;
                                    errMessage = noQuotaMessage;
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = notAcceptPatientMessage;
                            }
                        }
                    }
                    else
                    {
                        if (oSch != null)
                        {
                            if (isBPJS)
                            {
                                if (oSch.IsBPJS1)
                                {
                                    totalQuota = oSch.MaximumAppointmentBPJS1;
                                    if (totalApm >= totalQuota)
                                    {
                                        result = false;
                                        errMessage = noQuotaMessage;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = notAcceptPatientBPJSMessage;
                                }
                            }
                            else
                            {
                                if (oSch.IsNonBPJS1)
                                {
                                    totalQuota = oSch.MaximumAppointment1 + oSch.MaximumAppointmentBPJS1;
                                    if (totalApm >= totalQuota)
                                    {
                                        result = false;
                                        errMessage = noQuotaMessage;
                                    }
                                }
                                else
                                {
                                    result = false;
                                    errMessage = notAcceptPatientMessage;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }

            return result;
        }
    }
}