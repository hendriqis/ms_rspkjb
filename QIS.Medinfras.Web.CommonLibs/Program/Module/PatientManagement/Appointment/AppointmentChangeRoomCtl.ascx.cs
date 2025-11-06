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
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentChangeRoomCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {

            string[] paramsplit = param.Split('|');
            hdnParamedicIDCtl.Value = paramsplit[0];
            hdnSelectedDateCtl.Value = paramsplit[1];
            hdnServiceUnitIDCtl.Value = paramsplit[2];
            hdnStartTimeCtl.Value = paramsplit[3];
            hdnEndTimeCtl.Value = paramsplit[4];

            ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnParamedicIDCtl.Value));
            vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnServiceUnitIDCtl.Value)).FirstOrDefault();

            txtPhysicianCode.Text = pm.ParamedicCode;
            txtPhysicianName.Text = pm.FullName;
            txtServiceUnitCode.Text = vsu.ServiceUnitCode;
            txtServiceUnitName.Text = vsu.ServiceUnitName;
            txtDate.Text = hdnSelectedDateCtl.Value;
            txtSession.Text = paramsplit[5];
        }

        protected string OnGetParamedicFilterExpression()
        {
            return "";
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnRoomID, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRoomName, new ControlEntrySetting(false, false, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityDao = new AppointmentDao(ctx);
            try
            {
                int paramedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
                int healthcareServiceUnitID = Convert.ToInt32(hdnServiceUnitIDCtl.Value);
                DateTime date = Helper.GetDatePickerValue(hdnSelectedDateCtl.Value);

                string filter = string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND StartDate = '{2}' AND GCAppointmentStatus = '{3}' AND (StartTime BETWEEN '{4}' AND '{5}')", paramedicID, healthcareServiceUnitID, date, Constant.AppointmentStatus.STARTED, hdnStartTimeCtl.Value, hdnEndTimeCtl.Value);
                List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filter, ctx);

                if (lstAppointment.Count > 0)
                {
                    foreach (Appointment e in lstAppointment) {
                        e.RoomID = Convert.ToInt32(hdnRoomID.Value);
                        e.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(e);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Tidak ada data appointment yang dapat diproses";
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
    }
}