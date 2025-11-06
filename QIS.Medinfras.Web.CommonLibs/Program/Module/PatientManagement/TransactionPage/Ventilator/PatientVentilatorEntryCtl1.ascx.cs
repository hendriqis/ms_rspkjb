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
    public partial class PatientVentilatorEntryCtl1 : BaseEntryPopupCtl
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
            else
            {
                hdnPopupID.Value = "";
                IsAdd = true;
                SetControlProperties();
            }
        }

        private void SetControlProperties()
        {
            InitialiazeParamedicFields();

            if (hdnPopupTypeID.Value == Constant.MonitoringDeviceType.Ventilator)
                trETTSizeInfo.Style.Add("display", "table-row");

            else
                trETTSizeInfo.Style.Add("display", "none");

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
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT  )));
            SetControlEntrySetting(txtETTReason, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PatientETTLogHd entity)
        {
            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;
            cboParamedicID.Value = entity.ParamedicID1.Value.ToString();
            txtETTSize.Text = entity.ETTSize.ToString();
            txtETTReason.Text = entity.ETTReason;

            if (entity.ParamedicID1 == AppSession.UserLogin.ParamedicID)
                cboParamedicID.ClientEnabled = false;
        }

        private void ControlToEntity(PatientETTLogHd entity)
        {
            entity.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
            entity.HealthcareServiceUnitID = Convert.ToInt32(AppSession.RegisteredPatient.HealthcareServiceUnitID);
            entity.GCDeviceType = hdnPopupTypeID.Value;
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.StartTime = txtStartTime.Text;
            entity.ParamedicID1 = Convert.ToInt32(cboParamedicID.Value);
            entity.ETTSize = Convert.ToDecimal(txtETTSize.Text);
            entity.ETTReason = txtETTReason.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientETTLogHdDao entityDao = new PatientETTLogHdDao(ctx);

            try
            {
                if (IsValid(ref errMessage))
                {
                    PatientETTLogHd entity = new PatientETTLogHd();
                    ControlToEntity(entity);

                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.CreatedDate = DateTime.Now;
                    int id = entityDao.InsertReturnPrimaryKeyID(entity);

                    ctx.CommitTransaction();
                    retVal = id.ToString();
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

        private bool IsValid(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            DateTime startDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtStartDate.Text, txtStartTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

            if (startDateTime.Date > DateTime.Now.Date)
            {
                message.AppendLine("Tanggal Pemasangan harus lebih kecil atau sama dengan tanggal hari ini.|");
            }

            if (!Methods.IsNumeric(txtETTSize.Text))
            {
                message.AppendLine("Ukuran alat harus dalam bentuk angka|");
            }

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