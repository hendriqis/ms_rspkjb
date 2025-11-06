using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Globalization;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SurgeryOrderRegistrationCtl1 : BaseProcessPopupCtl
    {
        protected int gridProcedureGroupPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;

        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnHealthcareServiceUnitID.Value = AppSession.MD0006;

            hdnID.Value = paramInfo[0];
            hdnVisitID.Value = paramInfo[1];
            hdnTestOrderID.Value = paramInfo[2];

            IsAdd = false;
            _orderID = hdnID.Value;
            string filterExpression = string.Format("ID = {0}", hdnID.Value);
            vRoomSchedule entity = BusinessLayer.GetvRoomScheduleList(filterExpression).FirstOrDefault();
            OnControlEntrySettingLocal(entity);
            ReInitControl();
            EntityToControl(entity);
        }

        private void OnControlEntrySettingLocal(vRoomSchedule entity)
        {
            SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlProperties();
        }

        private void EntityToControl(vRoomSchedule entity)
        {
            txtOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.TestOrderTime;
            txtMedicalNo.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtOrderNo.Text = entity.TestOrderNo;
            txtPhysicianName.Text = entity.ParamedicName;
            chkIsUsedRequestTime.Checked = entity.IsUsedRequestTime;
            txtScheduleDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleTime.Text = entity.IsUsedRequestTime ? entity.ScheduledTime : string.Empty;

            txtEstimatedDuration.Text = entity.EstimatedDuration.ToString();
            hdnRoomID.Value = entity.RoomID.ToString();
            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;

            hdnAppointmentID.Value = entity.AppointmentID.ToString();
            txtAppointmentNo.Text = entity.AppointmentNo;
        }

        private void CopyOrder(TestOrderHd sourceOrder, TestOrderHd destinationOrder)
        {
            destinationOrder.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            destinationOrder.FromHealthcareServiceUnitID = Convert.ToInt32(hdnToHealthcareServiceUnitID.Value);
            destinationOrder.VisitID = Convert.ToInt32(hdnToVisitID.Value);
            destinationOrder.ParamedicID = sourceOrder.ParamedicID;
            destinationOrder.TestOrderDate = sourceOrder.TestOrderDate;
            destinationOrder.TestOrderTime = sourceOrder.TestOrderTime;
            destinationOrder.RoomID = sourceOrder.RoomID;
            destinationOrder.GCToBePerformed = Constant.ToBePerformed.SCHEDULLED;
            destinationOrder.EstimatedDuration = Convert.ToInt32(txtEstimatedDuration.Text);
            destinationOrder.ScheduledDate = sourceOrder.ScheduledDate;
            destinationOrder.IsUsedRequestTime = sourceOrder.IsUsedRequestTime;
            destinationOrder.ScheduledTime = sourceOrder.ScheduledTime;
            destinationOrder.IsEmergency = sourceOrder.IsEmergency;
            destinationOrder.IsODSVisit = sourceOrder.IsODSVisit;
            destinationOrder.IsCITO = sourceOrder.IsCITO;
            destinationOrder.IsOperatingRoomOrder = true;
            destinationOrder.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
            destinationOrder.Remarks = sourceOrder.Remarks;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao oOrderHdDao = new TestOrderHdDao(ctx);
            RoomScheduleDao oScheduleDao = new RoomScheduleDao(ctx);
            bool isError = false;
            int orderID = 0;

            try
            {
                TestOrderHd destinationOrder = new TestOrderHd();
                TestOrderHd sourceOrder = oOrderHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                CopyOrder(sourceOrder, destinationOrder);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                destinationOrder.TestOrderNo = BusinessLayer.GenerateTransactionNo(destinationOrder.TransactionCode, destinationOrder.TestOrderDate, ctx);

                destinationOrder.GCOrderStatus = Constant.OrderStatus.RECEIVED; //Schedulled
                destinationOrder.CreatedBy = AppSession.UserLogin.UserID;
                orderID = oOrderHdDao.InsertReturnPrimaryKeyID(destinationOrder);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                orderID = oOrderHdDao.InsertReturnPrimaryKeyID(destinationOrder);

                string filterExpression = string.Format("ID = {0} AND IsDeleted = 0", hdnID.Value);
                RoomSchedule oSchedule = BusinessLayer.GetRoomScheduleList(filterExpression, ctx).FirstOrDefault();

                if (oSchedule != null)
                {
                    oSchedule.GCScheduleType = Constant.ScheduleType.OPERATING_ROOM;
                    oSchedule.VisitID = Convert.ToInt32(hdnToVisitID.Value);
                    oSchedule.TestOrderID = orderID;
                    oSchedule.AppointmentID = null;
                    oSchedule.GCScheduleStatus = Constant.ScheduleStatus.OPEN;
                    oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                    oScheduleDao.Update(oSchedule);
                }
                else
                {
                    isError = true;
                    errMessage = "Tidak ditemukan jadwal order penggunaan kamar operasi.";
                }

                if (!isError)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                }

                retVal = hdnID.Value;
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

            return result;
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }
    }
}