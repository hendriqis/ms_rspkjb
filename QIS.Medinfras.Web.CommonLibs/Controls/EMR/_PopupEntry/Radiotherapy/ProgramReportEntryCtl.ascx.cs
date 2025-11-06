using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProgramReportEntryCtl : BaseEntryPopupCtl3
    {
        protected int gridProcedureGroupPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;

        protected static string _reportID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnID.Value = paramInfo[0];
            hdnProgramID.Value = paramInfo[1];
            hdnPopupVisitID.Value = paramInfo[2];
            hdnPopupMRN.Value = paramInfo[3];
            hdnPopupMedicalNo.Value = paramInfo[4];
            hdnPopupPatientName.Value = paramInfo[5];
            hdnPopupParamedicID.Value = paramInfo[6];
            hdnTotalFraction.Value = paramInfo[7];

            if (!string.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
            {
                IsAdd = false;
                OnControlEntrySettingLocal();
                ReInitControl();
                string filterExpression = string.Format("ProgramReportID = {0}", hdnID.Value);
                vRadiotherapyProgramReport entity = BusinessLayer.GetvRadiotherapyProgramReportList(filterExpression).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                OnControlEntrySettingLocal();
                ReInitControl();
                hdnID.Value = "0";
                _reportID = "0";
                IsAdd = true;
            }
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;

            string filterExpression = string.Format("ParamedicID = {0}", paramedicID);


            //if (AppSession.UserLogin.GCParamedicMasterType != Constant.ParamedicType.Physician)
            //{
            //    filterExpression = string.Format(
            //                                        "GCParamedicMasterType IN ('{0}')",
            //                                        Constant.ParamedicType.Physician, paramedicID);
            //}

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterExpression);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtReportDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtReportTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtEndDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtDuration, new ControlEntrySetting(true, true, true));

            SetControlProperties();

            if (!string.IsNullOrEmpty(AppSession.UserLogin.GCParamedicMasterType) && AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic.ToString()));
                cboParamedicID.Enabled = false;
            }
            else
            {
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            }

        }

        private void EntityToControl(vRadiotherapyProgramReport entity)
        {
            _reportID = entity.ProgramReportID.ToString();
            txtReportDate.Text = entity.ReportDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReportTime.Text = entity.ReportTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEndDate.Text = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDuration.Text = entity.ProgramDuration.ToString();
            txtFractionNo.Text = entity.TotalFraction.ToString();
            txtTotalDosage.Text = entity.TotalDosage.ToString();
            txtMedicalSummary.Text = entity.MedicalSummary;
            txtProgramSummary.Text = entity.ProgramSummary;
            txtPostProgramMedicalSummary.Text = entity.PostProgramMedicalSummary;
            txtToxicitySummary.Text = entity.ToxicitySummary;
            txtFollowupSummary.Text = entity.FollowupSummary;
        }

        private void ControlToEntity(RadiotherapyProgramReport entityHd)
        {
            entityHd.ProgramID = Convert.ToInt32(hdnProgramID.Value);
            entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entityHd.ReportDate = Helper.GetDatePickerValue(txtReportDate.Text);
            entityHd.ReportTime = txtReportTime.Text;
            entityHd.StartDate = Helper.GetDatePickerValue(txtStartDate.Text);
            entityHd.EndDate = Helper.GetDatePickerValue(txtEndDate.Text);
            entityHd.ProgramDuration = Convert.ToInt32(txtDuration.Text);
            entityHd.TotalFraction = Convert.ToInt16(txtFractionNo.Text);
            entityHd.TotalDosage = Convert.ToDecimal(txtTotalDosage.Text);
            entityHd.MedicalSummary = txtMedicalSummary.Text;
            entityHd.ProgramSummary = txtProgramSummary.Text;
            entityHd.PostProgramMedicalSummary = txtPostProgramMedicalSummary.Text;
            entityHd.ToxicitySummary = txtToxicitySummary.Text;
            entityHd.FollowupSummary = txtFollowupSummary.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            RadiotherapyProgramReportDao reportDao = new RadiotherapyProgramReportDao(ctx);
            int reportID = 0;

            try
            {
                if (IsValidated(ref errMessage))
                {
                    if (_reportID == "0")
                    {
                        RadiotherapyProgramReport entity = new RadiotherapyProgramReport();
                        ControlToEntity(entity);

                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        reportID = reportDao.InsertReturnPrimaryKeyID(entity);
                        _reportID = reportID.ToString();
                    }

                    retVal = _reportID;
                    ctx.CommitTransaction();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (!IsValidated(ref errMessage))
            {
                result = false;
                return result;
            }

            try
            {
                RadiotherapyProgramReport entityUpdate = BusinessLayer.GetRadiotherapyProgramReport(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateRadiotherapyProgramReport(entityUpdate);

                retVal = entityUpdate.ProgramReportID.ToString();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        private bool IsValidated(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            DateTime date;
            string format = Constant.FormatString.DATE_PICKER_FORMAT;

            if (!string.IsNullOrEmpty(txtFractionNo.Text))
            {
                if (!Methods.IsNumeric(txtFractionNo.Text))
                {
                    message.AppendLine("Nilai Fraksi Ke- harus berupa numerik/angka dan lebih besar dari 0");
                }
                else
                {
                    int fractionNo = Convert.ToInt32(txtFractionNo.Text);
                    if (fractionNo == 0)
                    {
                        message.AppendLine("Nilai Fraksi Ke- harus berupa numerik/angka dan lebih besar dari 0");
                    }
                    else
                    {
                        if (fractionNo > Convert.ToInt32(hdnTotalFraction.Value))
                        {
                            message.AppendLine(string.Format("Nilai Fraksi Ke- tidak boleh lebih besar dari jumlah Fraksi Program ({0})", hdnTotalFraction.Value));
                        }
                    }
                }
            }

            #region Report Date
            try
            {
                date = DateTime.ParseExact(txtReportDate.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                message.AppendLine("Format Tanggal Laporan tidak sesuai dengan format");
            };
            if (DateTime.Compare(Helper.GetDatePickerValue(txtReportDate.Text), DateTime.Now.Date) > 0)
            {
                message.AppendLine("Tanggal Laporan harus lebih kecil atau sama dengan tanggal saat ini.");
            }

            if (string.IsNullOrEmpty(txtReportTime.Text))
            {
                message.AppendLine("Jam laporan harus diisi");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtReportTime.Text))
                    message.AppendLine("Format Jam Laporan Tindakan tidak sesuai format (HH:MM)");
            }
            #endregion

            #region Start Date
            bool isStartDateValid = true;

            try
            {
                date = DateTime.ParseExact(txtStartDate.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                message.AppendLine("Format Tanggal Mulai Program tidak sesuai dengan format");
                isStartDateValid = false;
            };
            if (DateTime.Compare(Helper.GetDatePickerValue(txtReportDate.Text), DateTime.Now.Date) > 0)
            {
                message.AppendLine("Tanggal Mulai Program harus lebih kecil atau sama dengan tanggal saat ini.");
                isStartDateValid = false;
            }

            #endregion

            #region End Date
            bool isEndDateValid = true;

            try
            {
                date = DateTime.ParseExact(txtEndDate.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                message.AppendLine("Format Tanggal Selesai Program tidak sesuai dengan format");
                isEndDateValid = false;
            };
            if (DateTime.Compare(Helper.GetDatePickerValue(txtReportDate.Text), DateTime.Now.Date) > 0)
            {
                message.AppendLine("Tanggal Selesai Program harus lebih kecil atau sama dengan tanggal saat ini.");
                isEndDateValid = false;
            }

            #endregion

            if (isStartDateValid && isEndDateValid)
            {
                DateTime startDateTime = DateTime.ParseExact(string.Format("{0} 00:00", txtStartDate.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);
                DateTime endDateTime = DateTime.ParseExact(string.Format("{0} 00:00", txtEndDate.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

                if (endDateTime < startDateTime)
                {
                    message.AppendLine("Tanggal dan Jam Selesai Program harus lebih besar dari Tanggal dan Jam Mulai Tindakan");
                }
            }

            if (string.IsNullOrEmpty(txtDuration.Text))
                message.AppendLine("Durasi Program tidak boleh kosong atau 0");
            else
                if (Convert.ToInt32(txtDuration.Text) <= 0)
                    message.AppendLine("Durasi Program harus lebih besar dari 0");

            if (string.IsNullOrEmpty(txtTotalDosage.Text))
                message.AppendLine("Jumlah Dosis tidak boleh kosong atau harus lebih besar dari 0");
            else
                if (Convert.ToDecimal(txtTotalDosage.Text) <= 0)
                    message.AppendLine("Jumlah Dosis harus lebih besar dari 0");

            errMessage = message.ToString().Replace(Environment.NewLine, "<br />");

            return string.IsNullOrEmpty(errMessage);
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }
    }
}