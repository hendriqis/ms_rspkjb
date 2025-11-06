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
    public partial class ProcedureReportEntryCtl : BaseEntryPopupCtl3
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

            hdnIsEMR.Value = paramInfo[8];

            hdnPopupUserID.Value = AppSession.UserLogin.UserID.ToString();

            txtTotalFraction.Text = paramInfo[7];

            if (!string.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
            {
                IsAdd = false;
                OnControlEntrySettingLocal();
                ReInitControl();
                string filterExpression = string.Format("BrachytherapyProcedureReportID = {0}", hdnID.Value);
                vBrachytherapyProcedureReport entity = BusinessLayer.GetvBrachytherapyProcedureReportList(filterExpression).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                OnControlEntrySettingLocal();
                ReInitControl();
                hdnID.Value = "0";
                _reportID = "0";
                IsAdd = true;
                BindGridViewParamedicTeam(1, true, ref gridParamedicTeamPageCount);
            }
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;

            string filterExpression = string.Format("IsDeleted = 0", paramedicID);


            //if (AppSession.UserLogin.GCParamedicMasterType != Constant.ParamedicType.Physician)
            //{
            //    filterExpression = string.Format(
            //                                        "GCParamedicMasterType IN ('{0}')",
            //                                        Constant.ParamedicType.Physician, paramedicID);
            //}

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterExpression);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = paramedicID.ToString();

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0",
                    Constant.StandardCode.SURGERY_TEAM_ROLE,
                    Constant.StandardCode.APPLICATOR_TYPE,
                    Constant.StandardCode.INTRAUTERINE_LENGTH,
                    Constant.StandardCode.INTRAUTERINE_CORNER,
                    Constant.StandardCode.CYLINDER,
                    Constant.StandardCode.HEMORRHAGE));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.SURGERY_TEAM_ROLE).ToList();
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.APPLICATOR_TYPE).ToList();
            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.INTRAUTERINE_LENGTH).ToList();
            List<StandardCode> lstCode4 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.INTRAUTERINE_CORNER).ToList();
            List<StandardCode> lstCode5 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.CYLINDER).ToList();
            List<StandardCode> lstCode6 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.HEMORRHAGE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboParamedicType, lstCode1, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCApplicatorType, lstCode2, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCIntrauterineLength, lstCode3, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCIntrauterineCorner, lstCode4, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCCylinder, lstCode5, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCHemorrhage1, lstCode6, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCApplicatorReleaseHemorrhage, lstCode6, "StandardCodeName", "StandardCodeID");

            VisitInfoForRadiotherapyProgram oInfo = BusinessLayer.GetVisitInfoForRadiotherapyProgram(Convert.ToInt32(hdnPopupVisitID.Value)).FirstOrDefault();
            if (oInfo != null)
            {
                txtDiagnosisInfo.Text = oInfo.PatientDiagnosis.Replace("+", Environment.NewLine);
            }
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtReportDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtReportTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtEndDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtEndTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboGCApplicatorType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCIntrauterineLength, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCIntrauterineCorner, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCCylinder, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDuration, new ControlEntrySetting(true, true, true));

            SetControlProperties();

            if (!string.IsNullOrEmpty(AppSession.UserLogin.GCParamedicMasterType))
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic.ToString()));
                cboParamedicID.Enabled = false;
            }
        }

        private void EntityToControl(vBrachytherapyProcedureReport entity)
        {
            _reportID = entity.BrachytherapyProcedureReportID.ToString();
            txtReportDate.Text = entity.ReportDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReportTime.Text = entity.ReportTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtFractionNo.Text = entity.FractionNo.ToString();
            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;
            txtEndDate.Text = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEndTime.Text = entity.EndTime;
            txtDuration.Text = entity.Duration.ToString();
            if (!string.IsNullOrEmpty(entity.GCApplicatorType))
            {
                cboGCApplicatorType.Value = entity.GCApplicatorType;
            }
            if (!string.IsNullOrEmpty(entity.GCIntrauterineLength))
            {
                cboGCIntrauterineLength.Value = entity.GCIntrauterineLength;
            }
            if (!string.IsNullOrEmpty(entity.GCIntrauterineCorner))
            {
                cboGCIntrauterineCorner.Value = entity.GCIntrauterineCorner;
            }
            if (!string.IsNullOrEmpty(entity.GCCylinder))
            {
                cboGCCylinder.Value = entity.GCCylinder;
            }

            txtNeddleDepth.Text = entity.NeedleDepth.ToString();
            txtTotalNeddle.Text = entity.TotalNeedle.ToString();
            chkIsNeedleLocation1.Checked = entity.IsNeedleLocation1;
            chkIsNeedleLocation2.Checked = entity.IsNeedleLocation2;
            chkIsNeedleLocation3.Checked = entity.IsNeedleLocation3;
            chkIsNeedleLocation4.Checked = entity.IsNeedleLocation4;
            chkIsNeedleLocation5.Checked = entity.IsNeedleLocation5;
            chkIsNeedleLocation6.Checked = entity.IsNeedleLocation6;
            chkIsNeedleLocation7.Checked = entity.IsNeedleLocation7;
            chkIsNeedleLocation8.Checked = entity.IsNeedleLocation8;
            chkIsNeedleLocation9.Checked = entity.IsNeedleLocation9;
            chkIsNeedleLocation10.Checked = entity.IsNeedleLocation10;
            chkIsNeedleLocation11.Checked = entity.IsNeedleLocation11;
            chkIsNeedleLocation12.Checked = entity.IsNeedleLocation12;

            txtTotalDosage.Text = entity.TotalDosage.ToString();
            txtBladderDosageLimitation.Text = entity.BladderDosageLimitation.ToString();
            txtRectumDosageLimitation.Text = entity.RectumDosageLimitation.ToString();
            txtSigmoidDosageLimitation.Text = entity.SigmoidDosageLimitation.ToString();
            txtBowelDosageLimitation.Text = entity.BowelDosageLimitation.ToString();

            txtProcedureRemarks.Text = entity.ProcedureRemarks;

            chkIsHasProcedureComplication.Checked = entity.IsHasProcedureComplication;
            chkIsHasProcedureHemorrhage.Enabled = entity.IsHasProcedureComplication;
            chkIsHasProcedureHemorrhage.Checked = entity.IsHasProcedureHemorrhage;
            if (entity.IsHasProcedureHemorrhage)
            {
                cboGCHemorrhage1.Value = entity.GCProcedureHemorrhage;
            }
            cboGCHemorrhage1.ClientEnabled = entity.IsHasProcedureHemorrhage;
            txtProcedurePainIndex.Enabled = entity.IsHasProcedureComplication;
            txtProcedurePainIndex.Text = entity.ProcedurePainIndex.ToString();
            txtAnesthesiaComplicationRemarks.Enabled = entity.IsHasProcedureComplication;
            txtAnesthesiaComplicationRemarks.Text = entity.AnesthesiaComplicationRemarks;
            txtProcedureComplicationRemarks.Enabled = entity.IsHasProcedureComplication;
            txtProcedureComplicationRemarks.Text = entity.ProcedureComplicationRemarks;

            chkIsHasApplicatorReleaseComplication.Checked = entity.IsHasApplicatorReleaseComplication;
            chkIsHasApplicatorReleaseHemorrhage.Enabled = entity.IsHasApplicatorReleaseComplication;
            chkIsHasApplicatorReleaseHemorrhage.Checked = entity.IsHasApplicatorReleaseHemorrhage;
            if (entity.IsHasApplicatorReleaseHemorrhage)
            {
                cboGCApplicatorReleaseHemorrhage.Value = entity.GCApplicatorReleaseHemorrhage;
            }
            cboGCApplicatorReleaseHemorrhage.ClientEnabled = entity.IsHasApplicatorReleaseHemorrhage;
            txtPainIndex.Enabled = entity.IsHasApplicatorReleaseComplication;
            txtPainIndex.Text = entity.PainIndex.ToString();
            txtApplicatorReleaseComplicationRemarks.Enabled = entity.IsHasApplicatorReleaseComplication;
            txtApplicatorReleaseComplicationRemarks.Text = entity.ApplicatorReleaseComplicationRemarks;

            BindGridViewParamedicTeam(1, true, ref gridParamedicTeamPageCount);
        }

        private void ControlToEntity(BrachytherapyProcedureReport entityHd)
        {
            entityHd.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
            entityHd.MRN = Convert.ToInt32(hdnPopupMRN.Value);
            entityHd.ProgramID = Convert.ToInt32(hdnProgramID.Value);
            entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entityHd.ReportDate = Helper.GetDatePickerValue(txtReportDate.Text);
            entityHd.ReportTime = txtReportTime.Text;
            entityHd.FractionNo = Convert.ToInt16(txtFractionNo.Text);
            entityHd.StartDate = Helper.GetDatePickerValue(txtStartDate.Text);
            entityHd.StartTime = txtStartTime.Text;
            entityHd.EndDate = Helper.GetDatePickerValue(txtEndDate.Text);
            entityHd.EndTime = txtEndTime.Text;
            entityHd.Duration = Convert.ToDecimal(txtDuration.Text);
            entityHd.GCApplicatorType = cboGCApplicatorType.Value.ToString();
            entityHd.GCIntrauterineLength = cboGCIntrauterineLength.Value.ToString();
            entityHd.GCIntrauterineCorner = cboGCIntrauterineCorner.Value.ToString();
            entityHd.GCCylinder = cboGCCylinder.Value.ToString();
            entityHd.NeedleDepth = Convert.ToInt32(txtNeddleDepth.Text);
            entityHd.TotalNeedle = Convert.ToInt32(txtTotalNeddle.Text);
            entityHd.IsNeedleLocation1 = chkIsNeedleLocation1.Checked;
            entityHd.IsNeedleLocation2 = chkIsNeedleLocation2.Checked;
            entityHd.IsNeedleLocation3 = chkIsNeedleLocation3.Checked;
            entityHd.IsNeedleLocation4 = chkIsNeedleLocation4.Checked;
            entityHd.IsNeedleLocation5 = chkIsNeedleLocation5.Checked;
            entityHd.IsNeedleLocation6 = chkIsNeedleLocation6.Checked;
            entityHd.IsNeedleLocation7 = chkIsNeedleLocation7.Checked;
            entityHd.IsNeedleLocation8 = chkIsNeedleLocation8.Checked;
            entityHd.IsNeedleLocation9 = chkIsNeedleLocation9.Checked;
            entityHd.IsNeedleLocation10 = chkIsNeedleLocation10.Checked;
            entityHd.IsNeedleLocation11 = chkIsNeedleLocation11.Checked;
            entityHd.IsNeedleLocation12 = chkIsNeedleLocation12.Checked;
            entityHd.TotalDosage = Convert.ToDecimal(txtTotalDosage.Text);
            if (!string.IsNullOrEmpty(txtBladderDosageLimitation.Text) && txtBladderDosageLimitation.Text != "0")
            {
                entityHd.BladderDosageLimitation = Convert.ToDecimal(txtBladderDosageLimitation.Text);
            }
            if (!string.IsNullOrEmpty(txtRectumDosageLimitation.Text) && txtRectumDosageLimitation.Text != "0")
            {
                entityHd.RectumDosageLimitation = Convert.ToDecimal(txtRectumDosageLimitation.Text);
            }
            if (!string.IsNullOrEmpty(txtSigmoidDosageLimitation.Text) && txtSigmoidDosageLimitation.Text != "0")
            {
                entityHd.SigmoidDosageLimitation = Convert.ToDecimal(txtSigmoidDosageLimitation.Text);
            }
            if (!string.IsNullOrEmpty(txtBowelDosageLimitation.Text) && txtBowelDosageLimitation.Text != "0")
            {
                entityHd.BowelDosageLimitation = Convert.ToDecimal(txtBowelDosageLimitation.Text);
            }
            entityHd.ProcedureRemarks = txtProcedureRemarks.Text;
            entityHd.IsHasProcedureComplication = chkIsHasProcedureComplication.Checked;
            if (chkIsHasProcedureComplication.Checked)
            {
                entityHd.IsHasProcedureHemorrhage = chkIsHasProcedureHemorrhage.Checked;
                if (cboGCHemorrhage1.Value != null)
                {
                    entityHd.GCProcedureHemorrhage = cboGCHemorrhage1.Value.ToString();
                }
                entityHd.ProcedurePainIndex = Convert.ToInt16(txtProcedurePainIndex.Text);
                entityHd.AnesthesiaComplicationRemarks = txtAnesthesiaComplicationRemarks.Text;
                entityHd.ProcedureComplicationRemarks = txtProcedureComplicationRemarks.Text;
            }
            entityHd.IsHasApplicatorReleaseComplication = chkIsHasApplicatorReleaseComplication.Checked;
            if (chkIsHasApplicatorReleaseComplication.Checked)
            {
                entityHd.IsHasApplicatorReleaseHemorrhage = chkIsHasApplicatorReleaseHemorrhage.Checked;
                if (cboGCApplicatorReleaseHemorrhage.Value != null)
                {
                    entityHd.GCApplicatorReleaseHemorrhage = cboGCApplicatorReleaseHemorrhage.Value.ToString();
                }
                entityHd.PainIndex = Convert.ToInt16(txtPainIndex.Text);
                entityHd.ApplicatorReleaseComplicationRemarks = txtApplicatorReleaseComplicationRemarks.Text;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            BrachytherapyProcedureReportDao reportDao = new BrachytherapyProcedureReportDao(ctx);
            int reportID = 0;

            try
            {
                if (IsValidated(ref errMessage))
                {
                    if (_reportID == "0")
                    {
                        BrachytherapyProcedureReport entity = new BrachytherapyProcedureReport();
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
                BrachytherapyProcedureReport entityUpdate = BusinessLayer.GetBrachytherapyProcedureReport(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateBrachytherapyProcedureReport(entityUpdate);

                retVal = entityUpdate.BrachytherapyProcedureReportID.ToString();
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
                message.AppendLine("Format Tanggal Mulai Tindakan tidak sesuai dengan format");
                isStartDateValid = false;
            };
            if (DateTime.Compare(Helper.GetDatePickerValue(txtReportDate.Text), DateTime.Now.Date) > 0)
            {
                message.AppendLine("Tanggal Mulai Tindakan harus lebih kecil atau sama dengan tanggal saat ini.");
                isStartDateValid = false;
            }

            if (string.IsNullOrEmpty(txtStartTime.Text))
            {
                message.AppendLine("Jam Mulai Tindakan harus diisi");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtStartTime.Text))
                    message.AppendLine("Format Jam Mulai Tindakan tidak sesuai format (HH:MM)");
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
                message.AppendLine("Format Tanggal Selesai Tindakan tidak sesuai dengan format");
                isEndDateValid = false;
            };
            if (DateTime.Compare(Helper.GetDatePickerValue(txtReportDate.Text), DateTime.Now.Date) > 0)
            {
                message.AppendLine("Tanggal Selesai Tindakan harus lebih kecil atau sama dengan tanggal saat ini.");
                isEndDateValid = false;
            }

            if (string.IsNullOrEmpty(txtEndTime.Text))
            {
                message.AppendLine("Jam Selesai Tindakan harus diisi");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtEndTime.Text))
                    message.AppendLine("Format Jam Selesai Tindakan tidak sesuai format (HH:MM)");
            }
            #endregion

            if (isStartDateValid && isEndDateValid)
            {
                DateTime startDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtStartDate.Text, txtStartTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);
                DateTime endDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtEndDate.Text, txtEndTime.Text), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

                if (endDateTime < startDateTime)
                {
                    message.AppendLine("Tanggal dan Jam Selesai Tindakan harus lebih besar dari Tanggal dan Jam Mulai Tindakan");
                }
            }

            if (string.IsNullOrEmpty(txtDuration.Text))
                message.AppendLine("Durasi Tindakan tidak boleh kosong atau 0");
            else
                if (Convert.ToInt32(txtDuration.Text) <= 0)
                    message.AppendLine("Durasi tindakan harus lebih besar dari 0");

            if (cboGCApplicatorType.Value == null)
            {
                message.AppendLine("Jenis Aplikator tidak boleh kosong");
            }
            else
            {
                if (string.IsNullOrEmpty(cboGCApplicatorType.Value.ToString()))
                {
                    message.AppendLine("Jenis Aplikator tidak boleh kosong");
                }
            }

            if (cboGCIntrauterineLength.Value == null)
            {
                message.AppendLine("Panjang Intra uterine tidak boleh kosong");
            }
            else
            {
                if (string.IsNullOrEmpty(cboGCIntrauterineLength.Value.ToString()))
                {
                    message.AppendLine("Panjang Intra uterine tidak boleh kosong");
                }
            }

            if (cboGCIntrauterineCorner.Value == null)
            {
                message.AppendLine("Sudut Intra uterine tidak boleh kosong");
            }
            else
            {
                if (string.IsNullOrEmpty(cboGCIntrauterineCorner.Value.ToString()))
                {
                    message.AppendLine("Sudut Intra uterine tidak boleh kosong");
                }
            }

            if (cboGCCylinder.Value == null)
            {
                message.AppendLine("Diameter Ovoid/Silinder tidak boleh kosong");
            }
            else
            {
                if (string.IsNullOrEmpty(cboGCCylinder.Value.ToString()))
                {
                    message.AppendLine("Diameter Ovoid/Silinder tidak boleh kosong");
                }
            }

            if (string.IsNullOrEmpty(txtNeddleDepth.Text))
                message.AppendLine("Kedalaman Jarum tidak boleh kosong atau harus lebih besar dari 0");
            else
                if (Convert.ToInt32(txtNeddleDepth.Text) <= 0)
                    message.AppendLine("Kedalaman Jarum harus lebih besar dari 0");

            if (string.IsNullOrEmpty(txtTotalNeddle.Text))
                message.AppendLine("Jumlah Jarum tidak boleh kosong atau harus lebih besar dari 0");
            else
                if (Convert.ToInt32(txtTotalNeddle.Text) <= 0)
                    message.AppendLine("Jumlah Jarum harus lebih besar dari 0");

            if (string.IsNullOrEmpty(txtTotalDosage.Text))
                message.AppendLine("Jumlah Dosis tidak boleh kosong atau harus lebih besar dari 0");
            else
                if (Convert.ToDecimal(txtTotalDosage.Text) <= 0)
                    message.AppendLine("Jumlah Dosis harus lebih besar dari 0");

            if (!string.IsNullOrEmpty(txtBladderDosageLimitation.Text))
                if (Convert.ToDecimal(txtBladderDosageLimitation.Text) < 0)
                    message.AppendLine("Jumlah Limitasi Dosis Bladder harus lebih besar atau sama dengan 0");

            if (!string.IsNullOrEmpty(txtRectumDosageLimitation.Text))
                if (Convert.ToDecimal(txtRectumDosageLimitation.Text) < 0)
                    message.AppendLine("Jumlah Limitasi Dosis Rectum harus lebih besar atau sama dengan 0");

            if (!string.IsNullOrEmpty(txtSigmoidDosageLimitation.Text))
                if (Convert.ToDecimal(txtSigmoidDosageLimitation.Text) < 0)
                    message.AppendLine("Jumlah Limitasi Dosis Sigmoid harus lebih besar atau sama dengan 0");

            if (!string.IsNullOrEmpty(txtBowelDosageLimitation.Text))
                if (Convert.ToDecimal(txtBowelDosageLimitation.Text) < 0)
                    message.AppendLine("Jumlah Limitasi Dosis Bowel harus lebih besar atau sama dengan 0");

            if (chkIsHasProcedureComplication.Checked)
            {
                if (!string.IsNullOrEmpty(txtProcedurePainIndex.Text) && !Methods.IsNumeric(txtProcedurePainIndex.Text))
                {
                    message.AppendLine("Skala Nyeri ketika tindakan harus berupa angka dengan nilai antara 1 s/d 10.");
                }
                else
                {
                    if (Convert.ToInt16(txtProcedurePainIndex.Text) < 0 || Convert.ToInt16(txtProcedurePainIndex.Text) > 10)
                    {
                        message.AppendLine("Skala Nyeri ketika tindakan harus diisi dengan nilai antara 1 s/d 10. (Jika 0 = Tidak ada Nyeri)");
                    }
                }
            }

            if (chkIsHasApplicatorReleaseComplication.Checked)
            {
                if (!string.IsNullOrEmpty(txtPainIndex.Text) && !Methods.IsNumeric(txtPainIndex.Text))
                {
                    message.AppendLine("Skala Nyeri harus berupa angka dengan nilai antara 1 s/d 10.");
                }
                else
                {
                    if (Convert.ToInt16(txtPainIndex.Text) < 0 || Convert.ToInt16(txtPainIndex.Text) > 10)
                    {
                        message.AppendLine("Skala Nyeri harus diisi dengan nilai antara 1 s/d 10. (Jika 0 = Tidak ada Nyeri)");
                    }
                }
            }

            errMessage = message.ToString().Replace(Environment.NewLine, "<br />");

            return string.IsNullOrEmpty(errMessage);
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        #region Paramedic Team
        private void BindGridViewParamedicTeam(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (Page.IsCallback && _reportID != "0")
            {
                hdnID.Value = _reportID;
            }

            List<vBrachytherapyProcedureTeam> lstEntity = new List<vBrachytherapyProcedureTeam>();
            if (hdnID.Value != "0")
            {
                string filterExpression = string.Format("BrachytherapyProcedureReportID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, _reportID);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvBrachytherapyProcedureTeamRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                lstEntity = BusinessLayer.GetvBrachytherapyProcedureTeamList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);

            }

            grdParamedicTeamView.DataSource = lstEntity;
            grdParamedicTeamView.DataBind();
        }
        protected void cbpParamedicTeamView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewParamedicTeam(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewParamedicTeam(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        protected void cbpParamedicTeam_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            IDbContext ctx = DbFactory.Configure(true);
            BrachytherapyProcedureTeamDao paramedicTeamDao = new BrachytherapyProcedureTeamDao(ctx);
            BrachytherapyProcedureReportDao reportDao = new BrachytherapyProcedureReportDao(ctx);

            try
            {
                string errMessage = string.Empty;
                if (!IsValidated(ref errMessage))
                {
                    result = string.Format("0|process|{0}", errMessage);
                    ctx.RollBackTransaction();
                }
                else
                {
                    if (e.Parameter != null && e.Parameter != "")
                    {
                        string[] param = e.Parameter.Split('|');
                        int reportID = 0;
                        if (param[0] == "add")
                        {
                            if (_reportID == "0")
                            {
                                BrachytherapyProcedureReport entityHd = new BrachytherapyProcedureReport();
                                ControlToEntity(entityHd);
                                reportID = reportDao.InsertReturnPrimaryKeyID(entityHd);

                                _reportID = reportID.ToString();
                                hdnID.Value = reportID.ToString();
                            }
                            else
                            {
                                hdnID.Value = _reportID;
                                reportID = Convert.ToInt32(hdnID.Value);
                            }

                            BrachytherapyProcedureTeam obj = new BrachytherapyProcedureTeam();

                            obj.BrachytherapyProcedureReportID = reportID;
                            obj.ParamedicID = Convert.ToInt32(hdnEntryParamedicID.Value);
                            obj.GCParamedicRole = cboParamedicType.Value.ToString();
                            obj.CreatedBy = AppSession.UserLogin.UserID;
                            paramedicTeamDao.Insert(obj);

                            result = "1|add|" + _reportID;
                        }
                        else if (param[0] == "edit")
                        {
                            int recordID = Convert.ToInt32(hdnOrderDtParamedicTeamID.Value);
                            TestOrderDtParamedicTeam entity = BusinessLayer.GetTestOrderDtParamedicTeam(recordID);

                            if (entity != null)
                            {
                                entity.ParamedicID = Convert.ToInt32(hdnEntryParamedicID.Value);
                                entity.GCParamedicRole = cboParamedicType.Value.ToString();
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateTestOrderDtParamedicTeam(entity);
                                result = "1|edit|" + _reportID;
                            }
                            else
                            {
                                result = string.Format("0|delete|{0}", "Informasi Dokter/Tenaga Medis tidak valid");
                            }
                        }
                        else
                        {
                            int recordID = Convert.ToInt32(hdnOrderDtParamedicTeamID.Value);
                            BrachytherapyProcedureTeam entity = BusinessLayer.GetBrachytherapyProcedureTeam(recordID);

                            if (entity != null)
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateBrachytherapyProcedureTeam(entity);
                                result = "1|delete|";
                            }
                            else
                            {
                                result = string.Format("0|edit|{0}", "Jenis Dokter/Tenaga Medis tidak valid");
                            }
                            result = "1|delete|";
                        }
                        ctx.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}