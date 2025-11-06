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
using System.Globalization;

namespace QIS.Medinfras.Web.MedicalDiagnostic.Program
{
    public partial class OperatingRoomPatientList1 : BasePageTrx
    {
        protected int PageCount = 1;
        protected int PageCountDt = 1;

        public class OperatingRoomSchedule
        {
            public String StartTime { get; set; }
            public String EndTime { get; set; }
            public String DisplayTime { get; set; }
            public List<vRoomSchedule> OrderList { get; set; }
        }

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.OPERATING_ROOM_PATIENT_LIST;
                default: return Constant.MenuCode.MedicalDiagnostic.OPERATING_ROOM_PATIENT_LIST;
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

                GetSettingParameter();
                InitializeFilterParameter();

                hdnOperatingRoomID.Value = AppSession.MD0006;
                hdnCalAppointmentSelectedDate.Value = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                

                LoadServiceUnitRoom();

                BindGridView(1, true, ref PageCount);

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            }
        }

        private void LoadServiceUnitRoom()
        {
            List<vServiceUnitRoom> lstRoom = BusinessLayer.GetvServiceUnitRoomList(string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0", AppSession.MD0006));
            lstRoomCode.DataSource = lstRoom;
            lstRoomCode.DataBind();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnHealthcareServiceUnitImagingID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitLaboratoryID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            hdnIsBridgingToMedinfrasMobileApps.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        private void InitializeFilterParameter()
        {
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
            return result;
        }

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("HealthcareServiceUnitID = {0} AND ScheduledDate = '{1}' AND GCTransactionStatus IN ('{2}','{3}') AND GCOrderStatus = '{4}'", hdnOperatingRoomID.Value, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.OrderStatus.OPEN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvSurgeryTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vSurgeryTestOrderHd1> lstEntity = BusinessLayer.GetvSurgeryTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);

            string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND ScheduledDate = '{1}' AND GCTransactionStatus NOT IN ('{2}','{3}') AND GCOrderStatus != '{4}' AND RoomCode = '{5}' AND IsDeleted = 0", hdnOperatingRoomID.Value, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112),Constant.TransactionStatus.OPEN,  Constant.TransactionStatus.VOID, Constant.OrderStatus.OPEN, hdnOperatingRoomCode.Value);

            List<vRoomSchedule> lstOrder = BusinessLayer.GetvRoomScheduleList(filterExpression);

            //List<OperatingRoomSchedule> lstSchedule = new List<OperatingRoomSchedule>();
            //SettingScheduleTable(lstSchedule, lstOrder, "07", "23", 1);

            //grdAppointment.DataSource = lstSchedule.OrderBy(o => o.StartTime).ToList();
            //grdAppointment.DataBind();

            grdAppointment.DataSource = lstOrder.OrderBy(o => o.ScheduledTime).ToList();
            grdAppointment.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vAppointmentRequest entity = e.Item.DataItem as vAppointmentRequest;
                HtmlInputHidden hdnKey = (HtmlInputHidden)e.Item.FindControl("hdnKey");
                HtmlInputHidden hdnParamedicID = (HtmlInputHidden)e.Item.FindControl("hdnParamedicID");
                HtmlInputHidden hdnHealthcareServiceUnitPerRowID = (HtmlInputHidden)e.Item.FindControl("hdnHealthcareServiceUnitPerRowID");
                HtmlInputHidden hdnGCCustomerType = (HtmlInputHidden)e.Item.FindControl("hdnGCCustomerType");
                HtmlGenericControl lblParamedicName = (HtmlGenericControl)e.Item.FindControl("lblParamedicName");
                HtmlGenericControl lblCustomerType = (HtmlGenericControl)e.Item.FindControl("lblCustomerType");
                TextBox txtAppointmentDate = e.Item.FindControl("txtAppointmentDate") as TextBox;
                txtAppointmentDate.Text = entity.AppointmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
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
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";

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

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridViewDt(1, false, ref pageCount);
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            return result;
        }

        protected void grdAppointment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //OperatingRoomSchedule obj = e.Row.DataItem as OperatingRoomSchedule;
            //if (e.Row.RowType == DataControlRowType.DataRow) {
            //    Repeater rptOrderList = (Repeater)e.Row.FindControl("rptOrderInformation");
            //    rptOrderList.DataSource = obj.OrderList;
            //    rptOrderList.DataBind();
            //}    
        }

        private void SettingScheduleTable(List<OperatingRoomSchedule> lstSchedule, List<vRoomSchedule> lstOrder, String start, String end, Int32 interval)
        {
            bool result = true;

            if (result)
            {
                OperatingRoomSchedule entity = new OperatingRoomSchedule();

                #region 00-00 - 07.00
                entity.StartTime = "00:00";
                entity.DisplayTime = string.Format("00:00 - {0}", string.Format("{0}:00", start));
                DateTime startTime = DateTime.Parse(String.Format("{0} 00:00", Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).Date.ToString(Constant.FormatString.DATE_FORMAT)));
                DateTime endTime = DateTime.Parse(String.Format("{0} 06:59", Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).Date.ToString(Constant.FormatString.DATE_FORMAT)));
                List<vRoomSchedule> lstOrderByTime = lstOrder.Where(lst => lst.cfScheduledStartDateTime == startTime || (lst.cfScheduledStartDateTime > startTime && lst.cfScheduledStartDateTime < endTime)).OrderBy(lst => lst.ScheduledTime).ToList();
                entity.OrderList = lstOrderByTime;
                lstSchedule.Add(entity);
                #endregion

                startTime = DateTime.Parse(String.Format("{0} {1}:00", Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).Date.ToString(Constant.FormatString.DATE_FORMAT), start));
                endTime = DateTime.Parse(String.Format("{0} {1}:59", Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).Date.ToString(Constant.FormatString.DATE_FORMAT), end));

                while (startTime < endTime)
                {
                    entity = new OperatingRoomSchedule();
                    entity.StartTime = startTime.ToString("HH:mm");
                    entity.DisplayTime = string.Format("{0} - {1}", startTime.ToString("HH:mm"), startTime.AddHours(interval).ToString("HH:mm"));

                    lstOrderByTime = lstOrder.Where(lst => lst.cfScheduledStartDateTime == startTime || (lst.cfScheduledStartDateTime > startTime && lst.cfScheduledStartDateTime < startTime.AddHours(interval))).OrderBy(lst => lst.ScheduledTime).ToList();

                    entity.OrderList = lstOrderByTime;

                    startTime = startTime.AddHours(interval);

                    lstSchedule.Add(entity);
                }

                //Add Time Slot for H+1 : 00.00 - 07.00
                //startTime = DateTime.Parse(String.Format("{0} {1}:00", Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).Date.AddDays(1).ToString(Constant.FormatString.DATE_FORMAT), "00"));
                //endTime = DateTime.Parse(String.Format("{0} {1}:00", Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).Date.AddDays(1).ToString(Constant.FormatString.DATE_FORMAT), "07"));
                //while (startTime < endTime)
                //{
                //    OperatingRoomSchedule entity = new OperatingRoomSchedule();
                //    entity.StartTime = startTime.ToString("HH:mm");
                //    entity.DisplayTime = string.Format("{0} - {1}", startTime.ToString("HH:mm"), startTime.AddHours(7).ToString("HH:mm"));

                //    List<vRoomSchedule> lstOrderByTime = lstOrder.Where(lst => lst.cfScheduledStartDateTime == startTime || (lst.cfScheduledStartDateTime > startTime && lst.cfScheduledStartDateTime < startTime.AddHours(7))).ToList();

                //    entity.OrderList = lstOrderByTime;

                //    startTime = startTime.AddHours(interval);

                //    lstSchedule.Add(entity);
                //}
            }
        }

        protected void cbpDeleteRoomSchedule_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] paramInfo = e.Parameter.Split('|');
                string roomScheduleID = paramInfo[0];
                string visitID = paramInfo[1];
                string testOrderID = paramInfo[2];

                if (roomScheduleID != "0")
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    RoomScheduleDao scheduleDao = new RoomScheduleDao(ctx);
                    TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);

                    try
                    {
                        bool isValid = true;
                        string errMessage = string.Empty;
                        string responseData = string.Empty;

                        //Delete Schedule
                        RoomSchedule oSchedule = scheduleDao.Get(Convert.ToInt32(roomScheduleID));
                        if (oSchedule != null)
                        {
                            #region Jika Appointment - Hapus Appointment
                            if (oSchedule.AppointmentID != null)
                            {
                                AppointmentDao oAppointmentDao = new AppointmentDao(ctx);
                                Appointment oAppointment = oAppointmentDao.Get(Convert.ToInt32(oSchedule.AppointmentID));
                                if (oAppointment != null)
                                {
                                    oAppointment.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                                    oAppointment.GCDeleteReason = Constant.AppointmentDeleteReason.OTHER;
                                    oAppointment.DeleteReason = "Bagian dari proses penghapusan jadwal order kamar operasi";
                                    oAppointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    oAppointment.LastUpdatedDate = DateTime.Now;
                                    oAppointmentDao.Update(oAppointment);

                                    DraftTestOrderHdDao draftTestOrderHdDao = new DraftTestOrderHdDao(ctx);
                                    string filterExp = string.Format("TransactionCode = '{0}' AND AppointmentID = '{1}' AND GCTransactionStatus != '{2}'", Constant.TransactionCode.DRAFT_OTHER_TEST_ORDER, oAppointment.AppointmentID, Constant.TransactionStatus.VOID);
                                    DraftTestOrderHd draftTestOrderHd = BusinessLayer.GetDraftTestOrderHdList(filterExp, ctx).FirstOrDefault();
                                    if (draftTestOrderHd != null)
                                    {
                                        draftTestOrderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                        draftTestOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        draftTestOrderHd.LastUpdatedDate = DateTime.Now;
                                        draftTestOrderHdDao.Update(draftTestOrderHd);
                                    }
                                }
                            }
                            #endregion

                            oSchedule.IsDeleted = true;
                            oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;

                            scheduleDao.Update(oSchedule);
                        }
                        else
                        {
                            isValid = false;
                            errMessage = "Jadwal Kamar Operasi tidak ditemukan";
                        }

                        if (isValid)
                        {
                            //Reopen Order
                            TestOrderHd entity = orderHdDao.Get(Convert.ToInt32(testOrderID));
                            if (entity != null)
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.PROCESSED)
                                {
                                    isValid = false;
                                    errMessage = "Order Jadwal Kamar Operasi sudah dilakukan proses realisasi pengisian transaksi pasien."; 
                                }
                                else
                                {
                                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    entity.GCOrderStatus = Constant.OrderStatus.OPEN;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    responseData = entity.TestOrderNo;
                                    orderHdDao.Update(entity);
                                }
                            }
                            else
                            {
                                isValid = false;
                                errMessage = "Order Jadwal Kamar Operasi tidak ditemukan"; 
                            }
                        }

                        if (isValid)
                        {
                            result = string.Format("1|{0}", responseData);
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = string.Format("0|{0}", errMessage);
                            ctx.RollBackTransaction();
                        }
                    }
                    catch (Exception ex)
                    {
                        ctx.RollBackTransaction();
                        result = string.Format("0|0|{0}",ex.Message);
                    }
                    finally
                    {
                        ctx.Close();
                    }
                }
                else
                {
                    result = string.Format("0|0|Jadwal Kamar Operasi tidak valid!");
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpStartOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] paramInfo = e.Parameter.Split('|');
                string roomScheduleID = paramInfo[0];
                string visitID = paramInfo[1];
                string testOrderID = paramInfo[2];

                if (roomScheduleID != "0")
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    RoomScheduleDao scheduleDao = new RoomScheduleDao(ctx);
                    TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);

                    try
                    {
                        bool isValid = true;
                        string errMessage = string.Empty;
                        string responseData = string.Empty;
                        //Delete Schedule
                        RoomSchedule oSchedule = scheduleDao.Get(Convert.ToInt32(roomScheduleID));
                        if (oSchedule != null)
                        {
                            oSchedule.GCScheduleStatus = Constant.ScheduleStatus.STARTED;
                            oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                            scheduleDao.Update(oSchedule);
                        }
                        else
                        {
                            isValid = false;
                            errMessage = "Jadwal Kamar Operasi tidak ditemukan";
                        }

                        if (isValid)
                        {
                            //Update Order Status
                            TestOrderHd entity = orderHdDao.Get(Convert.ToInt32(testOrderID));
                            if (entity != null)
                            {

                                List<PatientChargesHd> lstCharges = BusinessLayer.GetPatientChargesHdList(string.Format("TestOrderID='{0}' AND GCTransactionStatus NOT IN ('{1}')", entity.TestOrderID, Constant.TransactionStatus.VOID));
                                if (lstCharges.Count == 0)
                                {
                                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                }
                                entity.GCOrderStatus = Constant.OrderStatus.IN_PROGRESS;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                responseData = entity.TestOrderNo;
                                orderHdDao.Update(entity);
                            }
                            else
                            {
                                isValid = false;
                                errMessage = "Order Jadwal Kamar Operasi tidak ditemukan";
                            }
                        }

                        if (isValid)
                        {
                            result = string.Format("1|{0}", responseData);
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = string.Format("0|0|{0}", errMessage);
                            ctx.RollBackTransaction();
                        }
                    }
                    catch (Exception ex)
                    {
                        ctx.RollBackTransaction();
                        result = string.Format("0|0|{0}", ex.Message);
                    }
                    finally
                    {
                        ctx.Close();
                    }
                }
                else
                {
                    result = string.Format("0|0|Jadwal Kamar Operasi tidak valid!");
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpStopOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] paramInfo = e.Parameter.Split('|');
                string roomScheduleID = paramInfo[0];
                string visitID = paramInfo[1];
                string testOrderID = paramInfo[2];

                if (roomScheduleID != "0")
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    RoomScheduleDao scheduleDao = new RoomScheduleDao(ctx);
                    TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);

                    try
                    {
                        bool isValid = true;
                        string errMessage = string.Empty;
                        string responseData = string.Empty;
                        //Delete Schedule
                        RoomSchedule oSchedule = scheduleDao.Get(Convert.ToInt32(roomScheduleID));
                        if (oSchedule != null)
                        {
                            oSchedule.GCScheduleStatus = Constant.ScheduleStatus.COMPLETED;
                            oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                            scheduleDao.Update(oSchedule);
                        }
                        else
                        {
                            isValid = false;
                            errMessage = "Jadwal Kamar Operasi tidak ditemukan";
                        }

                        if (isValid)
                        {
                            //Update Order Status
                            TestOrderHd entity = orderHdDao.Get(Convert.ToInt32(testOrderID));
                            if (entity != null)
                            {
                                entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                entity.GCOrderStatus = Constant.OrderStatus.COMPLETED;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                responseData = entity.TestOrderNo;
                                orderHdDao.Update(entity);
                            }
                            else
                            {
                                isValid = false;
                                errMessage = "Order Jadwal Kamar Operasi tidak ditemukan";
                            }
                        }

                        if (isValid)
                        {
                            result = string.Format("1|{0}", responseData);
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = string.Format("0|0|{0}", errMessage);
                            ctx.RollBackTransaction();
                        }
                    }
                    catch (Exception ex)
                    {
                        ctx.RollBackTransaction();
                        result = string.Format("0|0|{0}", ex.Message);
                    }
                    finally
                    {
                        ctx.Close();
                    }
                }
                else
                {
                    result = string.Format("0|0|Jadwal Kamar Operasi tidak valid!");
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}