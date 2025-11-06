using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientVentilatorStopEntryCtl1 : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPopupVisitID.Value = paramInfo[1];
            hdnPopupMRN.Value = paramInfo[2];
            hdnPopupTypeID.Value = paramInfo[3];
            
            if (paramInfo[0] != "" && paramInfo[0] != "0")
            {
                IsAdd = false;
                hdnPopupID.Value = paramInfo[0];
                SetControlProperties();
                PatientETTLogHd entity = BusinessLayer.GetPatientETTLogHd(Convert.ToInt32(hdnPopupID.Value));
                EntityToControl(entity);
            }
        }

        private void SetControlProperties()
        {
            InitialiazeParamedicFields();

            hdnDateToday.Value = Helper.GetCurrentDate().ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        private void InitialiazeParamedicFields()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2}",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            }
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");


            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                    cboParamedicID.ClientEnabled = false;
                    cboParamedicID.Value = userLoginParamedic.ToString();
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }
            }
            else
            {
                cboParamedicID.SelectedIndex = 0;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtStopDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtStopTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT  )));
        }

        private void EntityToControl(PatientETTLogHd entity)
        {
            if (entity.IsReleased)
            {
                txtStopDate.Text = Convert.ToDateTime(entity.EndDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtStopTime.Text = entity.EndTime;
            }
            else
            {
                txtStopDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtStopTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            if (entity.ParamedicID2 != null)
            {
                cboParamedicID.Value = entity.ParamedicID2.Value.ToString();

                if (entity.ParamedicID2 == AppSession.UserLogin.ParamedicID)
                    cboParamedicID.ClientEnabled = false;
            }
            else
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                    cboParamedicID.ClientEnabled = false;
                    cboParamedicID.Value = userLoginParamedic.ToString();
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }
            }

            txtETTStopReason.Text = entity.ETTStopReason;
            hdnStartDate.Value = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnStartTime.Value = entity.StartTime;
        }

        private void ControlToEntity(PatientETTLogHd entity)
        {
            entity.IsReleased = true;
            entity.EndDate = Helper.GetDatePickerValue(txtStopDate);
            entity.EndTime = txtStopTime.Text;
            entity.ParamedicID2 = Convert.ToInt32(cboParamedicID.Value);
            entity.ETTStopReason = txtETTStopReason.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            return result;
        }

        private bool IsValid(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            DateTime stopDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtStopDate.Text, txtStopTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);
            DateTime startDateTime = DateTime.ParseExact(string.Format("{0} {1}", hdnStartDate.Value, hdnStartTime.Value), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

            if (stopDateTime.Date > DateTime.Now.Date)
            {
                message.AppendLine("Tanggal Stop Pemasangan harus lebih kecil atau sama dengan tanggal hari ini.|");
            }

            if (stopDateTime.Date < startDateTime.Date)
            {
                message.AppendLine("Tanggal Stop Pemasangan harus lebih besar atau sama dengan Tanggal Pemasangan.|");
            }

            #region Validasi Jam

            #region Pembacaan Jam Pelepasan
            PatientETTLogHd patientETT = new PatientETTLogHd();
            patientETT.EndTime = txtStopTime.Text;
            string[] newTimeStop = patientETT.EndTime.Split(':');
            int hourStop = Convert.ToInt32(newTimeStop[0]);
            int minuteStop = Convert.ToInt32(newTimeStop[1]);
            DateTime newDateStop = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hourStop, minuteStop, 0);
            string hourStopInString = newDateStop.Hour.ToString();
            string minuteStopInString = newDateStop.Minute.ToString();
            string stopTime = string.Format("{0}:{1}", hourStopInString, minuteStopInString);
            if (newDateStop.Hour < 10)
            {
                hourStopInString = string.Format("0{0}", newDateStop.Hour);
            }

            if (newDateStop.Minute < 10)
            {
                minuteStopInString = string.Format("0{0}", newDateStop.Minute);
            }
            DateTime stopHourTime = DateTime.ParseExact(string.Format("{0}:{1}", hourStopInString, minuteStopInString), Common.Constant.FormatString.TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None);
            #endregion

            #region Pembacaan Jam Pemasangan
            patientETT.StartTime = hdnStartTime.Value;
            string[] newTimeStart = patientETT.StartTime.Split(':');
            int hourStart = Convert.ToInt32(newTimeStart[0]);
            int minuteStart = Convert.ToInt32(newTimeStart[1]);
            DateTime newDateStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hourStart, minuteStart, 0);
            string hourStartInString = newDateStart.Hour.ToString();
            string minuteStartInString = newDateStart.Minute.ToString();
            string startTime = string.Format("{0}:{1}", hourStartInString, minuteStartInString);
            if (newDateStart.Hour < 10)
            {
                hourStartInString = string.Format("0{0}", newDateStart.Hour);
            }

            if (newDateStart.Minute < 10)
            {
                minuteStartInString = string.Format("0{0}", newDateStart.Minute);
            }
            DateTime startHourTime = DateTime.ParseExact(string.Format("{0}:{1}", hourStartInString, minuteStartInString), Common.Constant.FormatString.TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None);
            #endregion

            if (stopDateTime.Date == startDateTime.Date)
            {
                if (stopHourTime < startHourTime)
                {
                    message.AppendLine("Jam Stop Pemasangan harus lebih besar atau sama dengan Jam Pemasangan.|");
                }
            }
            #endregion

            errMessage = message.ToString().Replace(@"|", "<br />");

            return string.IsNullOrEmpty(errMessage);
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientETTLogHdDao entityDao = new PatientETTLogHdDao(ctx);

            try
            {
                if (IsValid(ref errMessage))
                {
                    PatientETTLogHd entity = BusinessLayer.GetPatientETTLogHd(Convert.ToInt32(hdnPopupID.Value));
                    ControlToEntity(entity);

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);

                    ctx.CommitTransaction();
                    retVal = entity.ID.ToString();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                retVal = "0";
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}