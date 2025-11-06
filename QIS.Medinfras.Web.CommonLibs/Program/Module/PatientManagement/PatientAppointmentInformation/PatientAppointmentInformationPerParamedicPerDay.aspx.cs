using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientAppointmentInformationPerParamedicPerDay : BasePageContent
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string module = Helper.GetModuleID(Helper.GetModuleName());
            switch (module)
            {
                case Constant.Module.OUTPATIENT: return Constant.MenuCode.Outpatient.APPOINTMENT_INFORMATION_TRANSFER_PER_PARAMEDIC_PER_DAY; break;
                case Constant.Module.EMR: return Constant.MenuCode.EMR.APPOINTMENT_INFORMATION_TRANSFER_PER_PARAMEDIC_PER_DAY; break;
                default: return Constant.MenuCode.Outpatient.APPOINTMENT_INFORMATION_TRANSFER_PER_PARAMEDIC_PER_DAY; break;
            }
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
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
                cboSession.SelectedIndex = 0;

                List<StandardCode> lstSession = new List<StandardCode>();
                lstSession.Insert(0, new StandardCode { StandardCodeName = "Session 1", StandardCodeID = "0" });
                lstSession.Insert(1, new StandardCode { StandardCodeName = "Session 2", StandardCodeID = "1" });
                lstSession.Insert(2, new StandardCode { StandardCodeName = "Session 3", StandardCodeID = "2" });
                lstSession.Insert(3, new StandardCode { StandardCodeName = "Session 4", StandardCodeID = "3" });
                lstSession.Insert(4, new StandardCode { StandardCodeName = "Session 5", StandardCodeID = "4" });
                Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                cboSession.SelectedIndex = 0;

                UserAttribute ua = BusinessLayer.GetUserAttribute(AppSession.UserLogin.UserID);
                if (ua.ParamedicID != 0 && ua.ParamedicID != null)
                {
                    ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(ua.ParamedicID));
                    hdnParamedicID.Value = Convert.ToString(pm.ParamedicID);
                    txtParamedicName.Text = pm.FullName;
                }

                hdnCalAppointmentSelectedDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                PageCount = 0;
                List<vAppointmentInformation> lstEntity = null;
                lvwView.DataSource = lstEntity;
                lvwView.DataBind();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string HealthcareServiceUnitID = hdnHealthcareServiceUnitID.Value;
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
            Int32 Session = Convert.ToInt32(cboSession.Value);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            vParamedicScheduleDate ParamedicScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                        HealthcareServiceUnitID, hdnParamedicID.Value, selectedDate)).FirstOrDefault();


            vParamedicSchedule ParamedicSchedule = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                HealthcareServiceUnitID, hdnParamedicID.Value, daynumber)).FirstOrDefault();

            string startTime = "";
            string EndTime = "";
            bool isTimeSlot = false;
            if (ParamedicScheduleDate != null)
            {
                if (Session == 0)
                {
                    if (ParamedicScheduleDate.StartTime1 != "")
                    {
                        isTimeSlot = ParamedicScheduleDate.IsAppointmentByTimeSlot; 
                        startTime = ParamedicScheduleDate.StartTime1;
                        EndTime = ParamedicScheduleDate.EndTime1;
                    }
                }
                else if (Session == 1)
                {
                    if (ParamedicScheduleDate.StartTime2 != "")
                    {
                        isTimeSlot = ParamedicScheduleDate.IsAppointmentByTimeSlot1; 
                        startTime = ParamedicScheduleDate.StartTime2;
                        EndTime = ParamedicScheduleDate.EndTime2;
                    }
                }
                else if (Session == 2)
                {
                    if (ParamedicScheduleDate.StartTime3 != "")
                    {
                        isTimeSlot = ParamedicScheduleDate.IsAppointmentByTimeSlot2; 
                        startTime = ParamedicScheduleDate.StartTime3;
                        EndTime = ParamedicScheduleDate.EndTime3;
                    }
                }
                else if (Session == 3)
                {
                    if (ParamedicScheduleDate.StartTime4 != "")
                    {
                        isTimeSlot = ParamedicScheduleDate.IsAppointmentByTimeSlot3; 
                        startTime = ParamedicScheduleDate.StartTime4;
                        EndTime = ParamedicScheduleDate.EndTime4;
                    }
                }
                else if (Session == 4)
                {
                    if (ParamedicScheduleDate.StartTime5 != "")
                    {
                        isTimeSlot = ParamedicScheduleDate.IsAppointmentByTimeSlot4; 
                        startTime = ParamedicScheduleDate.StartTime5;
                        EndTime = ParamedicScheduleDate.EndTime5;
                    }
                }
            }
            else if (ParamedicScheduleDate == null && ParamedicSchedule != null)
            {
                if (Session == 0)
                {
                    if (ParamedicSchedule.StartTime1 != "")
                    {
                        isTimeSlot = ParamedicSchedule.IsAppointmentByTimeSlot1; 
                        startTime = ParamedicSchedule.StartTime1;
                        EndTime = ParamedicSchedule.EndTime1;
                    }
                }
                else if (Session == 1)
                {
                    if (ParamedicSchedule.StartTime2 != "")
                    {
                        isTimeSlot = ParamedicSchedule.IsAppointmentByTimeSlot2; 
                        startTime = ParamedicSchedule.StartTime2;
                        EndTime = ParamedicSchedule.EndTime2;
                    }
                }
                else if (Session == 2)
                {
                    if (ParamedicSchedule.StartTime3 != "")
                    {

                        isTimeSlot = ParamedicSchedule.IsAppointmentByTimeSlot3; 
                        startTime = ParamedicSchedule.StartTime3;
                        EndTime = ParamedicSchedule.EndTime3;
                    }
                }
                else if (Session == 3)
                {
                    if (ParamedicSchedule.StartTime4 != "")
                    {
                        isTimeSlot = ParamedicSchedule.IsAppointmentByTimeSlot4; 
                        startTime = ParamedicSchedule.StartTime4;
                        EndTime = ParamedicSchedule.EndTime4;
                    }
                }
                else if (Session == 4)
                {
                    if (ParamedicSchedule.StartTime5 != "")
                    {
                        isTimeSlot = ParamedicSchedule.IsAppointmentByTimeSlot5; 
                        startTime = ParamedicSchedule.StartTime5;
                        EndTime = ParamedicSchedule.EndTime5;
                    }
                }
            }

            if (startTime != "" && EndTime != "")
            {
                string filterExpression = string.Format("ParamedicID = {0} AND GCAppointmentStatus != '{1}' AND HealthcareServiceUnitID = '{2}' AND StartDate = '{3}' AND (StartTime BETWEEN '{4}' AND '{5}')", hdnParamedicID.Value, Constant.AppointmentStatus.DELETED, hdnHealthcareServiceUnitID.Value, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), startTime, EndTime);
                string orderBy = string.Format("HealthcareServiceUnitID, ParamedicID, QueueNo");
                if (isTimeSlot)
                {
                    orderBy = string.Format("HealthcareServiceUnitID, ParamedicID, StartTime");
                }

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvAppointmentInformationRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
                }

                List<vAppointmentInformation> lstEntity = BusinessLayer.GetvAppointmentInformationList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "HealthcareServiceUnitID, ParamedicID, QueueNo");
                
                List<vAppointmentInformation> lstEntity2 = BusinessLayer.GetvAppointmentInformationList(filterExpression);
                int totalApm = lstEntity2.Where(p => p.IsAutoAppointment == false).Count();
                int totalIsReceivingTreatment = lstEntity2.Where(p => p.IsReceivingTreatment == 1).Count();
                int TotalRegisApm = lstEntity2.Where(p => p.IsAutoAppointment == false && p.RegistrationNo != "").Count();
                int TotalGoShow = lstEntity2.Where(p => p.IsAutoAppointment == true).Count();
                int TotalNonRegis = lstEntity2.Where(p => p.RegistrationNo == "").Count();
                int totalPatient = TotalGoShow + TotalRegisApm;

                hdnTotalApm.Value = totalApm.ToString();
                hdnTotalReceivingTreatment.Value = totalIsReceivingTreatment.ToString();
                hdnTotalRegisApm.Value = TotalRegisApm.ToString();
                hdnTotalGoShow.Value = TotalGoShow.ToString();
                hdnTotalNonRegis.Value = TotalNonRegis.ToString();
                hdnTotalPatient.Value = totalPatient.ToString(); 

                lvwView.DataSource = lstEntity;
                lvwView.DataBind();
            }
            else
            {
                pageCount = 0; 
                hdnTotalApm.Value = "0";
                hdnTotalReceivingTreatment.Value = "0";
                hdnTotalRegisApm.Value = "0";
                hdnTotalGoShow.Value = "0";
                hdnTotalPatient.Value = "0";
                hdnTotalNonRegis.Value = "0";
                List<vAppointmentInformation> lstEntity = null;
                lvwView.DataSource = lstEntity;
                lvwView.DataBind();
            }
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vAppointmentInformation entity = e.Item.DataItem as vAppointmentInformation;
                ListViewDataItem dataitem = (ListViewDataItem)e.Item;
                if (!String.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(dataitem.DataItem, "RegistrationNo"))))
                {
                    System.Web.UI.HtmlControls.HtmlTableRow tr = (System.Web.UI.HtmlControls.HtmlTableRow)dataitem.FindControl("trItem");
                    //tr.BgColor = System.Drawing.Color.AliceBlue.ToString();
                    tr.Attributes.Add("class", "LvColor");
                }

                if (entity.IsReceivingTreatment == 1)
                {
                    HtmlImage imgReceivingTreatmentUri = (HtmlImage)e.Item.FindControl("imgReceivingTreatmentUri");
                    imgReceivingTreatmentUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/done.png"));
                }
            }
        }
    }
}