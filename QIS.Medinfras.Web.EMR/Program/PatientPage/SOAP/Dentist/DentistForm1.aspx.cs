﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class DentistForm1 : BasePagePatientPageList
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridProcedurePageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;
        protected int gridDiagnosticPageCount = 1;
        protected int gridInstructionPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected static string _visitNoteID;

        protected string RegistrationDateTime = "";

        class SubjectiveContent
        {
            public string HistoryDate { get; set; }
            public string HistoryTime { get; set; }
            public string ParamedicName { get; set; }
            public string ChiefComplaintText { get; set; }
        }

        class DiagnosisInfo
        {
            public string DiagnosisDate { get; set; }
            public string DiagnosisTime { get; set; }
            public string PhysicianName { get; set; }
            public string DiagnosisText { get; set; }
            public string DiagnosisType { get; set; }
        }

        class LaboratoryHdInfo
        {
            public int ChargesID { get; set; }
            public string TestDate { get; set; }
            public string TestTime { get; set; }
            public string PhysicianName { get; set; }
            public int ItemID { get; set; }
            public string ItemName { get; set; }
        }

        class LaboratoryDtInfo
        {
            public int ChargesID { get; set; }
            public int ItemID { get; set; }
            public string FractionName { get; set; }
            public string ResultValue { get; set; }
            public string PDFValue { get; set; }
            public string ResultUnit { get; set; }
            public string RefRange { get; set; }
            public string ResultFlag { get; set; }
            public bool IsResultInPDF { get; set; }
        }

        class ImagingHdInfo
        {
            public int ChargesID { get; set; }
            public string TestDate { get; set; }
            public string TestTime { get; set; }
            public string PhysicianName { get; set; }
            public int ItemID { get; set; }
            public string ItemName { get; set; }
        }

        class ImagingDtInfo
        {
            public int ChargesID { get; set; }
            public int ItemID { get; set; }
            public string ItemName { get; set; }
            public string ResultValue { get; set; }
        }

        class ProceduresInfo
        {
            public string ProcedureDate { get; set; }
            public string ProcedureTime { get; set; }
            public string ParamedicName { get; set; }
            public string ProcedureText { get; set; }
            public string ProcedureName { get; set; }
        }

        class InstructionInfo
        {
            public string InstructionDate { get; set; }
            public string InstructionTime { get; set; }
            public string PhysicianName { get; set; }
            public string Description { get; set; }
            public string InstructionGroup { get; set; }
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        protected string GetVisitNoteID()
        {
            return _visitNoteID;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.OUTPATIENT_SOAP_DENTIST_ASSESSMENT;
        }

        protected override void InitializeDataControl()
        {
            SetControlProperties();
            SetSettingParameter();
            Helper.SetControlEntrySetting(cboVisitType, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtHPISummary, new ControlEntrySetting(true, true, false), "mpPatientStatus");

            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            SetEntityToControl();

            hdnPatientVisitNoteID.Value = _visitNoteID;
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewROS(1, true, ref gridROSPageCount);
            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
            BindGridViewProcedure(1, true, ref gridProcedurePageCount);
            BindGridViewLaboratory(1, true, ref gridLaboratoryPageCount);
            BindGridViewImaging(1, true, ref gridImagingPageCount);
            BindGridViewDiagnostic(1, true, ref gridDiagnosticPageCount);
            BindGridViewInstruction(1, true, ref gridInstructionPageCount);

            rptListTooth.DataSource = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TOOTH));
            rptListTooth.DataBind();

            CreateTableTooth();

            LoadBodyDiagram();

            hdnIsChanged.Value = "0";
        }

        private void SetSettingParameter()
        {
            string FilterExpression = string.Format("ParameterCode IN ('{0}')",
                                                        Constant.SettingParameter.EM_IS_DEFAULT_CHECKLIST_COPY_SOAP_DIAGNOSIS
                                                    );
            string FilterExpressionDt = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.EM_Display_Next_Visit_Schedule,
                                                        Constant.SettingParameter.CC_COPY_FROM_NCC
                                                    );
            List<SettingParameter> lstSetvar = BusinessLayer.GetSettingParameterList(FilterExpression);
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(FilterExpressionDt);

            string IsDisplayNextVisitSchedule = lstSetvarDt.Where(p => p.ParameterCode == Constant.SettingParameter.EM_Display_Next_Visit_Schedule).FirstOrDefault().ParameterValue;
            hdnIsCCCopyFromNCC.Value = lstSetvarDt.Where(p => p.ParameterCode == Constant.SettingParameter.CC_COPY_FROM_NCC).FirstOrDefault().ParameterValue;
            hdnIsDefaultChecklistCopySoapDiagnosis.Value = lstSetvar.Where(p => p.ParameterCode == Constant.SettingParameter.EM_IS_DEFAULT_CHECKLIST_COPY_SOAP_DIAGNOSIS).FirstOrDefault().ParameterValue;

            if (IsDisplayNextVisitSchedule == "0") {
                DisplayNextVisitSchedule.Attributes.Add("style", "display:none;");
            }

        }

        private void SetEntityToControl()
        {
            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES));

            hdnMRN.Value = entityVisit.MRN.ToString();
            hdnVisitID.Value = entityVisit.VisitID.ToString();
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnGCCustomerType.Value = entityVisit.GCCustomerType;
            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", entityVisit.cfPatientNameInLabel, entityVisit.MedicalNo, entityVisit.RegistrationNo, entityVisit.ServiceUnitName, entityVisit.DateOfBirthInString);
            cboVisitType.Value = entityVisit.VisitTypeID.ToString();

            if (entityVisit.StartServiceDate == null || entityVisit.StartServiceTime == "")
            {
                txtServiceDate.Text = entityVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = entityVisit.ActualVisitTime;
            }
            else
            {
                txtServiceDate.Text = entityVisit.StartServiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = entityVisit.StartServiceTime;
            }

            if (entityVisit.DischargeDate == null || entityVisit.DischargeTime == "")
            {
                txtDischargeDate.Text = entityVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = entityVisit.ActualVisitTime;
            }
            else
            {
                txtDischargeDate.Text = entityVisit.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = entityVisit.DischargeTime;
            }

            if (!string.IsNullOrEmpty(entityVisit.StartServiceTime))
            {
                RegistrationDateTime = entityVisit.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                RegistrationDateTime += entityVisit.StartServiceTime.Replace(":", "");
            }
            else
            {
                RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");
            }

            hdnDepartmentID.Value = entityVisit.DepartmentID;
            hdnIsPhysicianDischarge.Value = entityVisit.DischargeDate != DateTime.MinValue ? "1" : "0";

            chkIsComplexVisit.Checked = entityVisit.IsComplexVisit;
            chkIsFastTrack.Checked = entityVisit.IsFastTrack;

            if (hdnIsDefaultChecklistCopySoapDiagnosis.Value == "0")
            {
                chkIsCopyChiefComplaint.Checked = true;
                chkIsCopyHPI.Checked = true;
                chkIsCopyPastMedicalHistory.Checked = true;
                chkIsCopyPastMedicationHistory.Checked = true;
                chkIsCopyVitalSign.Checked = true;
                chkIsCopyROS.Checked = true;
                chkIsCopyDiagnosticResultSummary.Checked = true;
                chkIsCopyDiagnosis.Checked = false;
                chkIsCopyInstruction.Checked = false;
            }
            else
            {
                chkIsCopyChiefComplaint.Checked = false;
                chkIsCopyHPI.Checked = false;
                chkIsCopyPastMedicalHistory.Checked = false;
                chkIsCopyPastMedicationHistory.Checked = false;
                chkIsCopyVitalSign.Checked = false;
                chkIsCopyROS.Checked = false;
                chkIsCopyDiagnosticResultSummary.Checked = false;
                chkIsCopyDiagnosis.Checked = true;
                chkIsCopyInstruction.Checked = false;
            }

            if (lstPatientVisitNote.Count > 0)
            {
                PatientVisitNote entitypvn = lstPatientVisitNote.First();
                EntityToControl(entitypvn);
            }
            else
            {
                _visitNoteID = "0";
            }

            vChiefComplaint obj = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (obj != null)
            {
                if (obj.ID == 0)
                {
                    hdnChiefComplaintID.Value = "0";
                    hdnParamedicID.Value = "0";
                }
                else
                {
                    hdnChiefComplaintID.Value = obj.ID.ToString();
                    hdnParamedicID.Value = obj.ParamedicID.ToString();
                    txtChiefComplaint.Text = obj.ChiefComplaintText;
                    txtHPISummary.Text = obj.HPISummary;
                    txtMedicalHistory.Text = obj.PastMedicalHistory;
                    txtMedicationHistory.Text = obj.PastMedicationHistory;
                    txtDiagnosticResultSummary.Text = obj.DiagnosticResultSummary;
                    txtPlanningNotes.Text = obj.PlanningSummary;
                    chkIsPatientAllergyExists.Checked = !chkIsPatientAllergyExists.Checked;
                    chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;
                    chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;
                }
                txtMedicalProblem.Text = obj.MedicalProblem;
            }

            #region Nurse Anamnesis
            vNurseChiefComplaint entity = BusinessLayer.GetvNurseChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            txtNurseAnamnesis.Text = entity.NurseChiefComplaintText;
            if (obj != null)
            {
                if (hdnIsCCCopyFromNCC.Value == "1")
                {
                    if (String.IsNullOrEmpty(obj.ChiefComplaintText))
                    {
                        txtChiefComplaint.Text = entity.NurseChiefComplaintText;
                    }
                }
                if (String.IsNullOrEmpty(obj.PastMedicalHistory))
                {
                    txtMedicalHistory.Text = entity.MedicalHistory;
                }
                if (String.IsNullOrEmpty(obj.PastMedicationHistory))
                {
                    txtMedicationHistory.Text = entity.MedicationHistory;
                }
            }
            else
            {
                if (hdnIsCCCopyFromNCC.Value == "1")
                {
                    txtChiefComplaint.Text = entity.NurseChiefComplaintText;
                }
                txtMedicalHistory.Text = entity.MedicalHistory;
                txtMedicationHistory.Text = entity.MedicationHistory;

                chkIsPatientAllergyExists.Checked = entity.IsPatientAllergyExists;
            }
            #endregion

            #region Appointment
            Appointment oAppointment = BusinessLayer.GetAppointmentList(string.Format("FromVisitID  = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (oAppointment != null)
            {
                hdnAppointmentID.Value = oAppointment.AppointmentID.ToString();
                cboFollowupVisitType.Value = oAppointment.VisitTypeID.ToString();
                txtFollowupVisitDate.Text = oAppointment.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtFollowupVisitRemarks.Text = oAppointment.Notes;

                trKunjunganBerikutnya.Style.Add("display", "none");
                trTanggalKunjungan.Style.Add("display", "none");
                trCatatanKunjungan.Style.Add("display", "none");
                vAppointment entityAppointment = BusinessLayer.GetvAppointmentList(string.Format("FromVisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                txtAppointmentInfo.Text = string.Format("Perjanjian No. {0} dengan {1} tanggal {2} jam {3}", entityAppointment.AppointmentNo, entityAppointment.ParamedicName, (entityAppointment.StartDate).ToString(Constant.FormatString.DATE_FORMAT), entityAppointment.StartTime);
                txtAppointmentInfo.ReadOnly = true;

            }
            else
            {
                trCatatanPerjanjian.Style.Add("display", "none");
            }

            #endregion

            if (!entityVisit.IsNewPatient)
            {
                LoadPreviousVisitInformation(AppSession.RegisteredPatient.MRN, true);
            }
            else
            {
                lblVisitHistoryInfo.Text = "Not Available";
            }

            if (entityVisit.IsReferralVisit)
            {
                divFooterTab2.Attributes.Remove("display");
                LoadSourceReferralVisitInfo(AppSession.RegisteredPatient.VisitID);
            }
            else
            {
                divFooterTab2.Attributes.Add("style", "display:none;");
            }
        }

        private void LoadSourceReferralVisitInfo(int visitID)
        {
            string filterExpression = string.Format("ToVisitID = {0} AND IsDeleted = 0", visitID);
            vPatientReferral oReferral = BusinessLayer.GetvPatientReferralList(filterExpression).FirstOrDefault();
            if (oReferral != null)
            {
                hdnSourceReferralVisitID.Value = oReferral.VisitID.ToString();
                lblReferralVisitHistoryInfo.Text = string.Format("{0}, {1}", oReferral.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), oReferral.FromPhysicianName);
                lblReferralRegistrationNo.Text = string.Format("{0}", oReferral.FromServiceUnitName);
                txtSourceDiagnosisText.Text = oReferral.DiagnosisText;
                txtSourceMedicalResumeText.Text = oReferral.MedicalResumeText;
                txtSourcePlanningResumeText.Text = oReferral.PlanningResumeText;
            }
        }

        private void ValidateParamedicScSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate)
        {
            Int32 ParamedicID = (int)AppSession.UserLogin.ParamedicID;
            DateTime selectedDate = Helper.GetDatePickerValue(txtFollowupVisitDate.Text);
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

        private bool OnSaveAppointment(ref string errMessage, ref int retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);

            AppointmentDao AppointmentDao = new AppointmentDao(ctx);
            ParamedicScheduleDao ParamedicScheduleDao = new ParamedicScheduleDao(ctx);
            ParamedicScheduleDateDao ParamedicScheduleDateDao = new ParamedicScheduleDateDao(ctx);
            ParamedicLeaveScheduleDao ParamedicLeaveScheduleDao = new ParamedicLeaveScheduleDao(ctx);

            Appointment appointment = new Appointment();
            DateTime stAppo = DateTime.Now;
            DateTime stAppoValid = DateTime.Now;
            int hour = 0;
            int minute = 0;
            string startTimeCheck = "";
            string endTimeCheck = "";

            try
            {
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int visitID = Convert.ToInt32(hdnVisitID.Value);
                int paramedicID = (int)AppSession.UserLogin.ParamedicID;
                int healthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                DateTime scheduleDate = Helper.GetDatePickerValue(txtFollowupVisitDate.Text);
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

                ValidateParamedicScSchedule(obj, objSchDate);
                #endregion

                #region validate Visit Type
                int visitDuration = 0;
                ParamedicVisitType VisitTypeParamedic = BusinessLayer.GetParamedicVisitTypeList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND VisitTypeID = {2}", healthcareServiceUnitID, paramedicID, cboFollowupVisitType.Value)).FirstOrDefault();
                vHealthcareServiceUnitCustom VisitTypeHealthcare = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID)).FirstOrDefault();

                if (VisitTypeParamedic != null)
                {
                    appointment.VisitDuration = Convert.ToInt16(visitDuration);
                    appointment.VisitTypeID = VisitTypeParamedic.VisitTypeID;
                }
                else
                {
                    if (VisitTypeHealthcare.IsHasVisitType)
                    {
                        List<vServiceUnitVisitType> lstServiceUnitVisitType = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID));
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
                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, (int)AppSession.UserLogin.ParamedicID);
                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime4, objSchDate.EndTime4, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime5, objSchDate.EndTime5, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, (int)AppSession.UserLogin.ParamedicID);
                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime4, objSchDate.EndTime4, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, (int)AppSession.UserLogin.ParamedicID);
                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, (int)AppSession.UserLogin.ParamedicID);
                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, (int)AppSession.UserLogin.ParamedicID);
                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, (int)AppSession.UserLogin.ParamedicID);
                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime4, obj.EndTime4, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime5, obj.EndTime5, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, (int)AppSession.UserLogin.ParamedicID);
                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime4, obj.EndTime4, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, (int)AppSession.UserLogin.ParamedicID);
                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, (int)AppSession.UserLogin.ParamedicID);
                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, (int)AppSession.UserLogin.ParamedicID);
                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                        filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, (int)AppSession.UserLogin.ParamedicID);
                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

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
                    vRegistration reg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", registrationID)).FirstOrDefault();

                    if (reg.MRN == 0 && reg.GuestID != 0)
                    {
                        appointment.IsNewPatient = true;
                        appointment.Name = reg.PatientName;
                        appointment.GCGender = reg.GCGender;
                        appointment.StreetName = reg.StreetName;
                        appointment.PhoneNo = reg.PhoneNo1;
                        appointment.MobilePhoneNo = reg.MobilePhoneNo1;
                        appointment.GCSalutation = reg.GCSalutation;
                    }
                    else if (reg.MRN != 0 && reg.GuestID == 0)
                    {
                        appointment.IsNewPatient = false;
                        appointment.MRN = reg.MRN;
                    }

                    //appointment.Notes = string.Format("From Registration {0}", reg.RegistrationNo);
                    appointment.Notes = string.Format("{0}", txtFollowupVisitRemarks.Text);
                    appointment.HealthcareServiceUnitID = healthcareServiceUnitID;
                    appointment.ParamedicID = paramedicID;
                    appointment.FromVisitID = visitID;
                    appointment.GCCustomerType = Constant.CustomerType.PERSONAL;
                    appointment.BusinessPartnerID = 1;
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
                    appointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(appointment.TransactionCode, appointment.StartDate);
                    appointment.Session = session;

                    appointment.GCAppointmentMethod = Constant.AppointmentMethod.CALLCENTER;
                    appointment.CreatedBy = AppSession.UserLogin.UserID;

                    //cek jika sudah ada appointment di periode ini blok
                    string filterExpresion = string.Format("ParamedicID = {0} AND StartDate = '{1}' AND GCAppointmentStatus != '{2}' AND StartTime >= '{3}' AND EndTime <= '{4}' AND FromVisitID = {5}", appointment.ParamedicID, appointment.StartDate, Constant.AppointmentStatus.DELETED, startTimeCheck, endTimeCheck, appointment.FromVisitID);
                    List<Appointment> lstAppointmentCheck = BusinessLayer.GetAppointmentList(filterExpresion);

                    if (lstAppointmentCheck.Count < 1)
                    {
                        bool isBPJS = false;
                        if (appointment.GCCustomerType == Constant.CustomerType.BPJS)
                        {
                            isBPJS = true;
                        }
                        appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(appointment.HealthcareServiceUnitID, Convert.ToInt32(appointment.ParamedicID), appointment.StartDate, Convert.ToInt32(appointment.Session), false, isBPJS, 0, ctx));
                        //appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(appointment.HealthcareServiceUnitID, Convert.ToInt32(appointment.ParamedicID), appointment.StartDate, session, 1));
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        retval = AppointmentDao.InsertReturnPrimaryKeyID(appointment);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        //errMessage = "Maaf Pasien ini Sudah Membuat Perjanjian Dengan Dokter Ini di Tanggal yang dipilih";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf Tidak Ada Jadwal Praktek Dokter Untuk Tanggal Yang Dipilih";
                    ctx.RollBackTransaction();
                }
                #endregion
                #endregion
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

        private void LoadPreviousVisitInformation(int mrn, bool isShowSameServiceUnit)
        {
            string filterExpression = string.Format("MRN = {0} AND DepartmentID = '{1}' AND ParamedicID = {2} AND VisitID != {3} AND VisitID < {3} AND GCVisitStatus != '{4}' ORDER BY VisitID DESC", mrn, AppSession.RegisteredPatient.DepartmentID, AppSession.UserLogin.ParamedicID, AppSession.RegisteredPatient.VisitID, Constant.VisitStatus.CANCELLED);
            vConsultVisitCustom oLastVisit = BusinessLayer.GetvConsultVisitCustomList(filterExpression).FirstOrDefault();
            if (oLastVisit != null)
            {
                hdnPreviousVisitID.Value = oLastVisit.VisitID.ToString();
                lblVisitHistoryInfo.Text = string.Format("{0}, {1}", oLastVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), oLastVisit.ParamedicName);
                lblPreviousDiagnosis.Text = oLastVisit.PatientDiagnosis;
                LoadSubjectiveContent(oLastVisit.VisitID);
                LoadObjectiveContent(oLastVisit.VisitID);
                LoadAssessmentContent(oLastVisit.VisitID);
                LoadPlanningContent(oLastVisit.VisitID);
                LoadProcedureContent(oLastVisit.VisitID);
                LoadInstructionContent(oLastVisit.VisitID);
            }
        }

        private void LoadSubjectiveContent(int visitID)
        {
            List<SubjectiveContent> list = new List<SubjectiveContent>();

            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
            vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterExpression, 5, 1, "ID DESC").FirstOrDefault();

            if (oChiefComplaint != null)
            {
                SubjectiveContent oContent = new SubjectiveContent();
                oContent.HistoryDate = oChiefComplaint.ObservationDateInString;
                oContent.HistoryTime = oChiefComplaint.ObservationTime;
                oContent.ParamedicName = oChiefComplaint.ParamedicName;
                oContent.ChiefComplaintText = oChiefComplaint.ChiefComplaintText;
                list.Add(oContent);

                divHPI.InnerHtml = oChiefComplaint.HPISummary;

                divDiagnosticResultSummaryContent.InnerHtml = oChiefComplaint.DiagnosticResultSummary;
                divPlanningSummary.InnerHtml = oChiefComplaint.PlanningSummary;
            }
            else
            {
                SubjectiveContent oContent = new SubjectiveContent();
                #region Subjective Notes from Medical Record
                filterExpression = string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", visitID, Constant.PatientVisitNotes.SUBJECTIVE_NOTES);
                List<vPatientVisitNote> lstNotes = new List<vPatientVisitNote>();

                filterExpression += " ORDER BY NoteDate,NoteTime";
                lstNotes = BusinessLayer.GetvPatientVisitNoteList(filterExpression);
                if (lstNotes.Count > 0)
                {
                    foreach (vPatientVisitNote item in lstNotes)
                    {
                        oContent = new SubjectiveContent();
                        oContent.HistoryDate = item.NoteDateInString;
                        oContent.HistoryTime = item.NoteTime;
                        oContent.ParamedicName = string.Format("{0} (Entried By : {1})", item.ParamedicName, item.CreatedByName);
                        oContent.ChiefComplaintText = item.NoteText;
                        list.Add(oContent);
                    }
                }
                else
                {
                    oContent.HistoryDate = string.Empty;
                    oContent.HistoryTime = string.Empty;
                    oContent.ParamedicName = string.Empty;
                    oContent.ChiefComplaintText = string.Empty;
                    list.Add(oContent);
                }
                #endregion
            }

            rptChiefComplaint.DataSource = list;
            rptChiefComplaint.DataBind();
        }

        private void LoadObjectiveContent(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
            List<vReviewOfSystemHd> lstReviewOfSystemHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, 10, 1, "ID DESC");
            if (lstReviewOfSystemHd.Count > 0)
            {
                string lstID = String.Join(",", lstReviewOfSystemHd.Select(c => c.ID).ToArray());
                lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN ({0}) AND IsDeleted = 0", lstID));
                rptReviewOfSystemHd.DataSource = lstReviewOfSystemHd;
                rptReviewOfSystemHd.DataBind();
            }

            List<vVitalSignHd> lstVitalSignHd = BusinessLayer.GetvVitalSignHdList(filterExpression, 3, 1, "ID DESC");
            if (lstVitalSignHd.Count > 0)
            {
                string lstID = String.Join(",", lstVitalSignHd.Select(c => c.ID).ToArray());
                lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID IN ({0}) AND IsDeleted = 0", lstID));
                rptVitalSignHd.DataSource = lstVitalSignHd;
                rptVitalSignHd.DataBind();
            }
        }

        private void LoadAssessmentContent(int visitID)
        {
            List<DiagnosisInfo> lstDiagnosis = new List<DiagnosisInfo>();
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0", visitID);
            List<vPatientDiagnosis> lstPatientDiagnose = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
            foreach (vPatientDiagnosis dxItem in lstPatientDiagnose)
            {
                DiagnosisInfo oDiagnosis = new DiagnosisInfo();
                oDiagnosis.DiagnosisDate = dxItem.DifferentialDateInString;
                oDiagnosis.DiagnosisTime = dxItem.DifferentialTime;
                oDiagnosis.PhysicianName = dxItem.ParamedicName;
                oDiagnosis.DiagnosisText = dxItem.DiagnosisText;
                oDiagnosis.DiagnosisType = dxItem.DiagnoseType;
                lstDiagnosis.Add(oDiagnosis);
            }

            rptDifferentDiagnosis.DataSource = lstDiagnosis.OrderBy(p => p.DiagnosisType);
            rptDifferentDiagnosis.DataBind();
        }

        private void LoadPlanningContent(int visitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
            #region Laboratory
            List<LaboratoryHdInfo> lstLaboratoryHd = new List<LaboratoryHdInfo>();

            filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} ORDER BY TransactionDate DESC", visitID, hdnLaboratoryServiceUnitID.Value);
            List<vPatientVisitLaboratory> lstCharges = BusinessLayer.GetvPatientVisitLaboratoryList(filterExpression);
            if (lstCharges.Count > 0)
            {
                foreach (vPatientVisitLaboratory item in lstCharges)
                {
                    LaboratoryHdInfo oHeader = new LaboratoryHdInfo();
                    oHeader.ChargesID = item.TransactionID;
                    oHeader.TestDate = item.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
                    oHeader.TestTime = item.TransactionTime;
                    oHeader.ItemID = item.ItemID;
                    oHeader.ItemName = string.Format("{0} ({1})", item.ItemName1, item.DiagnoseTestOrder);
                    lstLaboratoryHd.Add(oHeader);
                }
            }

            divLaboratoryNA.Style.Add("display", "none");

            rptLabTestOrder.DataSource = lstLaboratoryHd;
            rptLabTestOrder.DataBind();

            #endregion

            #region Imaging
            List<ImagingHdInfo> lstImagingHd = new List<ImagingHdInfo>();
            filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} ORDER BY TransactionDate DESC", visitID, hdnImagingServiceUnitID.Value);
            List<vPatientVisitImaging> lstCharges2 = BusinessLayer.GetvPatientVisitImagingList(filterExpression);
            if (lstCharges.Count > 0)
            {
                foreach (vPatientVisitImaging item in lstCharges2)
                {
                    ImagingHdInfo oHeader = new ImagingHdInfo();
                    oHeader.ChargesID = item.TransactionID;
                    oHeader.TestDate = item.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT);
                    oHeader.TestTime = item.TransactionTime;
                    oHeader.ItemID = item.ItemID;
                    oHeader.ItemName = item.ItemName1;
                    lstImagingHd.Add(oHeader);
                }
            }
            divImagingNA.Style.Add("display", "none");


            rptImagingTestOrder.DataSource = lstImagingHd;
            rptImagingTestOrder.DataBind();

            #endregion

            #region Medication
            filterExpression = string.Format("VisitID = {0} ORDER BY PrescriptionOrderDetailID DESC", visitID);
            rptMedication.DataSource = BusinessLayer.GetvPatientVisitPrescriptionList(filterExpression);
            rptMedication.DataBind();

            divMedicationNA.Style.Add("display", "none");
            #endregion
        }

        private void LoadProcedureContent(int visitID)
        {
            List<ProceduresInfo> lstProcedures = new List<ProceduresInfo>();
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
            List<vPatientProcedure> lstPatientProcedure = BusinessLayer.GetvPatientProcedureList(filterExpression);
            foreach (vPatientProcedure dxItem in lstPatientProcedure)
            {
                ProceduresInfo oProcedure = new ProceduresInfo();
                oProcedure.ProcedureDate = dxItem.ProcedureDateInString;
                oProcedure.ProcedureTime = dxItem.ProcedureTime;
                oProcedure.ParamedicName = dxItem.ParamedicName;
                oProcedure.ProcedureText = dxItem.ProcedureText;
                oProcedure.ProcedureName = dxItem.ProcedureName;
                lstProcedures.Add(oProcedure);
            }

            rptPatientProcedure.DataSource = lstProcedures.OrderBy(p => p.ProcedureText);
            rptPatientProcedure.DataBind();
        }

        private void LoadInstructionContent(int visitID)
        {
            List<InstructionInfo> lstInstruction = new List<InstructionInfo>();
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY PatientInstructionID", visitID);
            List<vPatientInstruction> lstPatientInstruction = BusinessLayer.GetvPatientInstructionList(filterExpression);
            if (lstPatientInstruction.Count > 0)
            {
                foreach (vPatientInstruction item in lstPatientInstruction)
                {
                    if (!string.IsNullOrEmpty(item.PatientInstructionID.ToString()))
                    {
                        InstructionInfo oInstruction = new InstructionInfo();
                        oInstruction.InstructionDate = item.cfInstructionDatePickerFormat;
                        oInstruction.InstructionTime = item.InstructionTime;
                        oInstruction.PhysicianName = item.PhysicianName;
                        oInstruction.Description = item.Description;
                        oInstruction.InstructionGroup = item.InstructionGroup;
                        lstInstruction.Add(oInstruction);
                    }
                }
            }

            rptPatientInstruction.DataSource = lstInstruction.OrderBy(p => p.InstructionGroup);
            rptPatientInstruction.DataBind();
        }

        private void LoadBodyDiagram()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            int pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
            hdnPageCount.Value = pageCount.ToString();

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                OnLoadBodyDiagram(0);
                tblBodyDiagramNavigation.Style.Remove("display");
            }
            else
            {
                divBodyDiagram.Style.Add("display", "none");
                tblEmpty.Style.Remove("display");
            }
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.DIAGNOSIS_TYPE, Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE, Constant.StandardCode.PATIENT_INSTRUCTION_GROUP);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);


            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboAllergenType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE && sc.StandardCodeID != Constant.DiagnoseType.EARLY_DIAGNOSIS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_OUTCOME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboInstructionType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_INSTRUCTION_GROUP).ToList(), "StandardCodeName", "StandardCodeID");

            cboPatientOutcome.Value = Constant.PatientOutcome.BELUM_SEMBUH;
            cboDiagnosisStatus.Value = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;

            string defaultInstruction = lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_INSTRUCTION_GROUP && sc.IsDefault == true).FirstOrDefault().StandardCodeID;
            if (!string.IsNullOrEmpty(defaultInstruction))
            {
                cboInstructionType.Value = defaultInstruction;
            }

            List<GetParamedicVisitTypeList> visitTypeList = BusinessLayer.GetParamedicVisitTypeList(AppSession.RegisteredPatient.HealthcareServiceUnitID, (int)AppSession.UserLogin.ParamedicID, "");
            Methods.SetComboBoxField(cboVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
            Methods.SetComboBoxField(cboFollowupVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");

            hdnImagingServiceUnitID.Value = AppSession.ImagingServiceUnitID;
            hdnLaboratoryServiceUnitID.Value = AppSession.LaboratoryServiceUnitID;
        }

        private void EntityToControl(PatientVisitNote entitypvn)
        {
            hdnPatientVisitNoteID.Value = entitypvn.ID.ToString();
            hdnPlanningText.Value = entitypvn.PlanningText;
            _visitNoteID = hdnPatientVisitNoteID.Value;
        }

        private void ControlToEntity(PatientVisitNote entitypvn)
        {
            string soapNote = GenerateSOAPText();

            entitypvn.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
            entitypvn.NoteTime = txtServiceTime.Text;

            entitypvn.SubjectiveText = hdnSubjectiveText.Value;
            entitypvn.ObjectiveText = hdnObjectiveText.Value;
            entitypvn.AssessmentText = hdnAssessmentText.Value;
            entitypvn.PlanningText = hdnPlanningText.Value;
            entitypvn.InstructionText = txtInstructionText.Text;
            entitypvn.NoteText = soapNote;
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            StringBuilder sbSubjective = new StringBuilder();
            StringBuilder sbDiagnose = new StringBuilder();
            StringBuilder sbInstruction = new StringBuilder();
            sbNotes.AppendLine("SUBJEKTIVE :");
            sbNotes.AppendLine("-".PadRight(20, '-'));

            sbNotes.AppendLine(string.Format("Keluhan Utama  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", txtChiefComplaint.Text));
            if (!string.IsNullOrEmpty(txtHPISummary.Text))
            {
                sbNotes.AppendLine(string.Format("Riwayat Penyakit Sekarang  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtHPISummary.Text));
            }
            if (!string.IsNullOrEmpty(txtMedicalHistory.Text))
            {
                sbNotes.AppendLine(string.Format("Riwayat Penyakit Dahulu  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtMedicalHistory.Text));
            }
            if (!string.IsNullOrEmpty(txtMedicationHistory.Text))
            {
                sbNotes.AppendLine(string.Format("Riwayat Pengobatan Dahulu  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtMedicationHistory.Text));
            }

            hdnSubjectiveText.Value = sbNotes.ToString();

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("OBJEKTIF :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            vVitalSignHd entityVitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entityVitalSignHd != null)
            {
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", entityVitalSignHd.ID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
                if (lstVitalSignDt.Count > 0)
                {
                    sbNotes.AppendLine("Tanda Vital :");
                    foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                    {
                        sbNotes.AppendLine(string.Format(" - {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                    }
                }
            }

            vReviewOfSystemHd entityReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entityReviewOfSystem != null)
            {
                List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0 ORDER BY GCRoSystem", entityReviewOfSystem.ID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
                if (lstROS.Count > 0)
                {
                    sbNotes.AppendLine(" ");
                    sbNotes.AppendLine("Pemeriksaan Fisik :");
                    foreach (vReviewOfSystemDt item in lstROS)
                    {
                        sbNotes.AppendLine(string.Format(" - {0}: {1}", item.ROSystem, item.cfRemarks));
                    }
                }
            }

            hdnObjectiveText.Value = sbNotes.ToString();

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("ASSESSMENT :");
            sbNotes.AppendLine("-".PadRight(20, '-'));

            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entityDiagnose != null)
            {
                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0 ORDER BY GCDiagnoseType", AppSession.RegisteredPatient.VisitID);
                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
                if (lstDiagnosis.Count > 0)
                {
                    sbDiagnose = new StringBuilder();
                    foreach (vPatientDiagnosis item in lstDiagnosis)
                    {
                        if (string.IsNullOrEmpty(item.DiagnoseID))
                            sbDiagnose.AppendLine(string.Format(" {0}", item.cfDiagnosisText));
                        else
                            sbDiagnose.AppendLine(string.Format(" {0} ({1})", item.DiagnoseName, item.DiagnoseID));
                    }
                    hdnAssessmentText.Value = sbDiagnose.ToString();
                }
                else
                {
                    hdnAssessmentText.Value = null;
                }
            }
            else
            {
                hdnAssessmentText.Value = null;
            }
            sbNotes.AppendLine(string.Format(" {0}", hdnAssessmentText.Value));

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("PLANNING :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            if (!string.IsNullOrEmpty(hdnLaboratoryOrderSummaryText.Value))
            {
                sbNotes.AppendLine("Laboratorium :");
                sbNotes.AppendLine(hdnLaboratoryOrderSummaryText.Value);
            }
            if (!string.IsNullOrEmpty(hdnImagingSummary.Value))
            {
                sbNotes.AppendLine("Radiologi :");
                sbNotes.AppendLine(hdnImagingSummary.Value);
            }
            if (!string.IsNullOrEmpty(hdnDiagnosticSummary.Value))
            {
                sbNotes.AppendLine("Penunjang Medis Lainnya :");
                sbNotes.AppendLine(hdnDiagnosticSummary.Value);
            }

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("INSTRUKSI : ");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            if (!string.IsNullOrEmpty(txtInstructionText.Text))
            {
                hdnInstructionText.Value = txtInstructionText.Text;
            }
            sbNotes.AppendLine(hdnInstructionText.Value);

            return sbNotes.ToString();
        }

        private string GenerateSOAPSummaryText(ref string subjectiveText, ref string objectiveText, ref string assessmentText, ref string planningText)
        {
            StringBuilder sbNotes = new StringBuilder();
            sbNotes.AppendLine("Subjective :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            if ((AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID))
            {
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                if (oChiefComplaint != null)
                {

                    sbNotes.AppendLine(string.Format("{0}   ", oChiefComplaint.ChiefComplaintText));
                    if (!string.IsNullOrEmpty(oChiefComplaint.HPISummary))
                    {
                        sbNotes.AppendLine(string.Format("RPS  : "));
                        sbNotes.AppendLine(string.Format("{0}", txtHPISummary.Text));
                    }
                    if (!string.IsNullOrEmpty(txtMedicationHistory.Text))
                    {
                        sbNotes.AppendLine(string.Format("RPD  : "));
                        sbNotes.AppendLine(string.Format("{0}", txtMedicationHistory.Text));
                    }
                }

                subjectiveText = sbNotes.ToString();
            }

            string vitalSummary = string.Empty;
            sbNotes = new StringBuilder();
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstVitalSignDt.Count > 0)
            {
                foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                {
                    sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                }
            }

            vitalSummary = sbNotes.ToString();

            string rosSummary = string.Empty;
            sbNotes = new StringBuilder();
            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND  ObservationDate = '{1}') AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstROS.Count > 0)
            {
                foreach (vReviewOfSystemDt item in lstROS)
                {
                    sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                }
            }
            rosSummary = sbNotes.ToString();

            //sbNotes.AppendLine(" ");
            //sbNotes.AppendLine("Assessment");
            //sbNotes.AppendLine("-".PadRight(15, '-'));
            //if (!string.IsNullOrEmpty(txtDiagnoseText.Text))
            //{
            //    sbNotes.AppendLine(string.Format("{0} ({1})", txtDiagnoseText.Text, txtDiagnoseCode.Text));
            //}

            return sbNotes.ToString();
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx, ref string errMessage)
        {
            ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);

            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
            {
                entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
                entityConsultVisit.StartServiceTime = txtServiceTime.Text;
                entityConsultVisit.GCVisitStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
            }

            if (!string.IsNullOrEmpty(cboVisitType.Value.ToString()))
            {
                entityConsultVisit.VisitTypeID = Convert.ToInt16(cboVisitType.Value);
            }

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
            {
                entityConsultVisit.TimeElapsed0 = string.Format("{0}:{1}", hdnTimeElapsed0hour.Value.PadLeft(2, '0'), hdnTimeElapsed0minute.Value.PadLeft(2, '0'));
            }
            entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}", hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));
            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;

            consultVisitDao.Update(entityConsultVisit);

            Registration oRegistration = registrationDao.Get(entityConsultVisit.RegistrationID);
            if (oRegistration != null)
            {
                oRegistration.IsComplexVisit = chkIsComplexVisit.Checked;
                oRegistration.IsFastTrack = chkIsFastTrack.Checked;
                oRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                registrationDao.Update(oRegistration);
            }

            ChiefComplaint entity = null;
            bool isNewChiefComplaint = true;

            if (hdnChiefComplaintID.Value != "" && hdnChiefComplaintID.Value != "0")
            {
                entity = chiefComplaintDao.Get(Convert.ToInt32(hdnChiefComplaintID.Value));
                isNewChiefComplaint = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new ChiefComplaint();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ObservationTime = txtServiceTime.Text;
            entity.ChiefComplaintText = txtChiefComplaint.Text;
            entity.HPISummary = txtHPISummary.Text;
            entity.PastMedicalHistory = txtMedicalHistory.Text;
            entity.PastMedicationHistory = txtMedicationHistory.Text;
            entity.IsPatientAllergyExists = !chkIsPatientAllergyExists.Checked;
            entity.DiagnosticResultSummary = txtDiagnosticResultSummary.Text;
            entity.PlanningSummary = txtPlanningNotes.Text;

            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            entity.MedicalProblem = txtMedicalProblem.Text;

            if (isNewChiefComplaint)
            {
                chiefComplaintDao.Insert(entity);
                hdnChiefComplaintID.Value = BusinessLayer.GetChiefComplaintMaxID(ctx).ToString();
            }
            else
            {
                chiefComplaintDao.Update(entity);
            }

            #region Followup Visit
            Appointment oAppointment;
            AppointmentDao oAppointmentDao;
            if (!string.IsNullOrEmpty(hdnAppointmentID.Value))
            {
                oAppointmentDao = new AppointmentDao(ctx);
                oAppointment = oAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                if (oAppointment != null)
                {

                    //    oAppointment.StartDate = Helper.GetDatePickerValue(txtFollowupVisitDate);

                    //    if (!String.IsNullOrEmpty(txtFollowupVisitTime.Text))
                    //    {
                    //        oAppointment.StartTime = txtFollowupVisitTime.Text;
                    //        oAppointment.EndTime = txtFollowupVisitTime.Text;
                    //    }
                    //    else
                    //    {
                    //        oAppointment.StartTime = "00:00";
                    //        oAppointment.EndTime = "00:00";
                    //    }

                    //    oAppointment.VisitTypeID = Convert.ToInt32(cboFollowupVisitType.Value);
                    //    oAppointment.Notes = txtFollowupVisitRemarks.Text;
                    //    oAppointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //    oAppointment.LastUpdatedDate = DateTime.Now;

                    //    if (oAppointment.StartDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                    //    {
                    //        oAppointment.EndDate = oAppointment.StartDate;
                    //        oAppointmentDao.Update(oAppointment);
                    //    }
                }
            }
            else
            {
                if (cboFollowupVisitType.Value != null)
                {
                    int retval = 0;
                    OnSaveAppointment(ref errMessage, ref retval);
                }
            }
            #endregion
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);

            bool isAllowSave = true;

            try
            {
                if (AppSession.RegisteredPatient.ParamedicID != AppSession.UserLogin.ParamedicID)
                {
                    isAllowSave = false;
                }

                if (isAllowSave)
                {
                    if (type == "save")
                    {
                        ChiefComplaint obj = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                        if (obj != null)
                            hdnChiefComplaintID.Value = obj.ID.ToString();
                        else
                            hdnChiefComplaintID.Value = "0";

                        StringBuilder sbNotes = new StringBuilder();
                        sbNotes.AppendLine(string.Format("{0}   ", txtChiefComplaint.Text));
                        if (obj != null)
                        {
                            if (!string.IsNullOrEmpty(txtHPISummary.Text))
                            {
                                sbNotes.AppendLine(string.Format("RPS  : "));
                                sbNotes.AppendLine(string.Format("{0}", txtHPISummary.Text));
                            }
                            if (!string.IsNullOrEmpty(txtMedicationHistory.Text))
                            {
                                sbNotes.AppendLine(string.Format("RPD  : "));
                                sbNotes.AppendLine(string.Format("{0}", txtMedicationHistory.Text));
                            }
                        }

                        string subjectiveText = sbNotes.ToString();

                        #region Objective Content
                        sbNotes = new StringBuilder();
                        vVitalSignHd entityVitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                        if (entityVitalSignHd != null)
                        {
                            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", entityVitalSignHd.ID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
                            if (lstVitalSignDt.Count > 0)
                            {
                                foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                                {
                                    sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                                }
                            }
                        }

                        sbNotes.AppendLine(" ");

                        vReviewOfSystemHd entityReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                        if (entityReviewOfSystem != null)
                        {
                            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0 ORDER BY GCRoSystem", entityReviewOfSystem.ID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
                            if (lstROS.Count > 0)
                            {
                                foreach (vReviewOfSystemDt item in lstROS)
                                {
                                    sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(txtDiagnosticResultSummary.Text))
                        {
                            sbNotes.AppendLine(" ");
                            sbNotes.AppendLine("Catatan Hasil Pemeriksaan Penunjang: ");
                            sbNotes.AppendLine(txtDiagnosticResultSummary.Text);
                        }

                        string objectiveText = sbNotes.ToString();
                        #endregion

                        #region Assessment Content
                        sbNotes = new StringBuilder();
                        string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0 ORDER BY GCDiagnoseType", AppSession.RegisteredPatient.VisitID);
                        List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
                        if (lstDiagnosis.Count > 0)
                        {
                            foreach (vPatientDiagnosis item in lstDiagnosis)
                            {
                                if (string.IsNullOrEmpty(item.DiagnoseID))
                                    sbNotes.AppendLine(string.Format("- {0}", item.cfDiagnosisText));
                                else
                                    sbNotes.AppendLine(string.Format("- {0} ({1})", item.cfDiagnosisText, item.DiagnoseID));
                            }
                        }
                        string assessmentText = sbNotes.ToString();
                        #endregion

                        #region Planning Content
                        string planningText = string.Empty;
                        sbNotes = new StringBuilder();
                        if (!string.IsNullOrEmpty(hdnLaboratoryOrderSummaryText.Value))
                        {
                            sbNotes.AppendLine("Pemeriksaan Laboratorium :");
                            sbNotes.AppendLine(hdnLaboratoryOrderSummaryText.Value);
                        }
                        if (!string.IsNullOrEmpty(hdnImagingSummaryText.Value))
                        {
                            sbNotes.AppendLine("Pemeriksaan Radiologi :");
                            sbNotes.AppendLine(hdnImagingSummaryText.Value);
                        }
                        if (!string.IsNullOrEmpty(hdnOtherTestSummaryText.Value))
                        {
                            sbNotes.AppendLine("Pemeriksaan Penunjang :");
                            sbNotes.AppendLine(hdnOtherTestSummaryText.Value);
                        }
                        if (!string.IsNullOrEmpty(txtPlanningNotes.Text))
                        {
                            sbNotes.AppendLine("Rencana Tindakan :");
                            sbNotes.AppendLine(txtPlanningNotes.Text);
                        }
                        planningText = sbNotes.ToString();
                        #endregion

                        #region Instruction Content
                        string instructionText = string.Empty;
                        sbNotes = new StringBuilder();
                        string filterExpressionInstruction = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
                        List<vPatientInstruction> lstInstruction = BusinessLayer.GetvPatientInstructionList(filterExpressionInstruction);
                        if (lstInstruction.Count > 0)
                        {
                            foreach (vPatientInstruction item in lstInstruction)
                            {
                                if (!string.IsNullOrEmpty(item.PatientInstructionID.ToString()))
                                    sbNotes.AppendLine(string.Format("- {0}", item.Description));
                            }
                        }
                        instructionText = sbNotes.ToString();
                        #endregion

                        PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                        PatientVisitNote entityVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND CreatedBy = {2}", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, AppSession.UserLogin.UserID), ctx).FirstOrDefault();
                        bool isEntityVisitNoteNull = false;
                        if (entityVisitNote == null)
                        {
                            isEntityVisitNoteNull = true;
                            entityVisitNote = new PatientVisitNote();
                        }

                        entityVisitNote.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        entityVisitNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityVisitNote.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
                        entityVisitNote.NoteTime = txtServiceTime.Text;
                        entityVisitNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityVisitNote.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                        entityVisitNote.SubjectiveText = subjectiveText;
                        entityVisitNote.ObjectiveText = objectiveText;
                        entityVisitNote.AssessmentText = assessmentText;
                        entityVisitNote.PlanningText = planningText;
                        entityVisitNote.InstructionText = instructionText;
                        entityVisitNote.ChiefComplaintID = Convert.ToInt32(hdnChiefComplaintID.Value);
                        entityVisitNote.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}",
                            Environment.NewLine, subjectiveText, Environment.NewLine,
                            Environment.NewLine, objectiveText, Environment.NewLine,
                            Environment.NewLine, assessmentText, Environment.NewLine,
                            Environment.NewLine, planningText);

                        if (isEntityVisitNoteNull)
                        {
                            entityVisitNote.CreatedBy = AppSession.UserLogin.UserID;
                            patientVisitNoteDao.Insert(entityVisitNote);
                        }
                        else
                        {
                            entityVisitNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientVisitNoteDao.Update(entityVisitNote);
                        }

                        string errMessageUpdateConsultVisitRegistration = "";
                        UpdateConsultVisitRegistration(ctx, ref errMessageUpdateConsultVisitRegistration);

                        if (!String.IsNullOrEmpty(errMessageUpdateConsultVisitRegistration))
                        {
                            result = false;
                            errMessage = errMessageUpdateConsultVisitRegistration;
                        }



                    }
                    else if (type == "discharge")
                    {
                        string message = "";
                        result = ProcessDischargePatient(ctx, ref message);
                        if (!result)
                        {
                            errMessage = message;
                        }
                    }

                    ctx.CommitTransaction();

                    hdnIsSaved.Value = "1";
                    hdnIsChanged.Value = "0";
                }
                else
                {
                    errMessage = "Maaf, Pengkajian Pasien hanya bisa dilakukan oleh DPJP terhadap kunjungan pasien ini";
                    hdnIsChanged.Value = "0";

                    ctx.RollBackTransaction();
                    result = false; 
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                hdnIsSaved.Value = "0";
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private bool ProcessDischargePatient(IDbContext ctx, ref string errMessage)
        {
            if (IsValidToDischarge())
            {

                if (hdnIsChanged.Value == "1")
                {
                    ChiefComplaint obj = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                    if (obj != null)
                        hdnChiefComplaintID.Value = obj.ID.ToString();
                    else
                        hdnChiefComplaintID.Value = "0";

                    string errMessageUpdateConsultVisitRegistration = "";
                    UpdateConsultVisitRegistration(ctx, ref errMessageUpdateConsultVisitRegistration);

                    if (!String.IsNullOrEmpty(errMessageUpdateConsultVisitRegistration))
                    {
                        errMessage = errMessageUpdateConsultVisitRegistration;
                    }
                }

                ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", AppSession.RegisteredPatient.RegistrationID), ctx);
                ConsultVisit entity = lstConsultVisit.Where(t => t.VisitID == AppSession.RegisteredPatient.VisitID).FirstOrDefault();
                if (entity.GCVisitStatus != Constant.VisitStatus.DISCHARGED && entity.GCVisitStatus != Constant.VisitStatus.CLOSED)
                {
                    entity.GCVisitStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                }
                entity.GCDischargeCondition = cboPatientOutcome.Value.ToString(); ;
                entity.GCDischargeMethod = Constant.DischargeMethod.ATAS_PERSETUJUAN;
                entity.PhysicianDischargedBy = AppSession.UserLogin.UserID;
                entity.PhysicianDischargedDate = Helper.GetDatePickerValue(txtDischargeDate);
                entity.DischargeDate = Helper.GetDatePickerValue(txtDischargeDate);
                entity.DischargeTime = txtDischargeTime.Text;
                entity.LOSInDay = Convert.ToDecimal(hdnLOSInDay.Value);
                entity.LOSInHour = Convert.ToDecimal(hdnLOSInHour.Value);
                entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinute.Value);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                RegistrationDao registrationDao = new RegistrationDao(ctx);
                if (lstConsultVisit.Where(t => t.GCVisitStatus != Constant.VisitStatus.PHYSICIAN_DISCHARGE && t.GCVisitStatus != Constant.VisitStatus.DISCHARGED && t.GCVisitStatus != Constant.VisitStatus.CLOSED && t.GCVisitStatus != Constant.VisitStatus.CANCELLED).Count() == 0)
                {
                    Registration entityRegis = registrationDao.Get(entity.RegistrationID);
                    entityRegis.GCRegistrationStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                    entityRegis.IsComplexVisit = chkIsComplexVisit.Checked;
                    entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;
                    registrationDao.Update(entityRegis);
                }

                visitDao.Update(entity);

                #region Display Transaction Popup Entry

                #endregion

                return true;
            }
            else
            {
                errMessage = "You must be Registered Physician and must entry Patient Chief Complaint, Diagnosis before complete session";
                return false;
            }
        }

        private bool IsValidToDischarge()
        {
            bool isDPJPPhysician = AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID;

            vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isChiefComplaintExist = oChiefComplaint != null;

            vPatientDiagnosis oDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            bool isDiagnosisExist = oDiagnosis != null;

            bool isPatientOutcomeExist = cboPatientOutcome.Value != null;

            return isChiefComplaintExist && isDiagnosisExist && isDPJPPhysician;
        }

        private bool IsValidToSave(ref string errMessage)
        {
            bool result = true;
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                if (AppSession.UserLogin.ParamedicID != paramedicID)
                {
                    errMessage = "Perubahan Kajian Awal Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian";
                    result = false;
                }
            }
            return result;
        }

        private void BindGridViewAllergy(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAllergyRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAllergy> lstEntity = BusinessLayer.GetvPatientAllergyList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdAllergyView.DataSource = lstEntity;
            grdAllergyView.DataBind();

            chkIsPatientAllergyExists.Checked = !(lstEntity.Count > 0);
            chkIsPatientAllergyExists.Enabled = (lstEntity.Count == 0);
        }

        protected void cbpAllergyView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewAllergy(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewAllergy(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            filterExpression += string.Format("VisitID = {0} AND IsDischargeVitalSign = 0 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID));
            grdVitalSignView.DataSource = lstEntity;
            grdVitalSignView.DataBind();
        }

        protected void grdVitalSignView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpVitalSignView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewVitalSign(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewVitalSign(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
                ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                panel.JSProperties["cpResult"] = result;
            }
        }

        protected void cbpDeleteVitalSign_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnVitalSignRecordID.Value != "")
            {
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnVitalSignRecordID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVitalSignHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected void cbpAllergy_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            bool isError = false;
            int processType = 1;

            IDbContext ctx = DbFactory.Configure(true);
            PatientAllergyDao allergyDao = new PatientAllergyDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            PatientAllergy oAllergy;

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        oAllergy = new PatientAllergy();
                        oAllergy.MRN = AppSession.RegisteredPatient.MRN;
                        oAllergy.AllergyLogDate = DateTime.Now.Date;
                        oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                        oAllergy.Allergen = txtAllergenName.Text;
                        oAllergy.GCAllergySource = Constant.AllergenFindingSource.PATIENT;
                        oAllergy.GCAllergySeverity = Constant.AllergySeverity.UNKNOWN;
                        oAllergy.KnownDate = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oAllergy.Reaction = txtReaction.Text;
                        oAllergy.CreatedBy = AppSession.UserLogin.UserID;
                        allergyDao.Insert(oAllergy);

                        Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                        if (!oPatient.IsHasAllergy)
                        {
                            oPatient.IsHasAllergy = true;
                            oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientDao.Update(oPatient);
                        }

                        result = "1|add|1";

                        processType = 1;
                    }
                    else if (param[0] == "edit")
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        oAllergy = allergyDao.Get(allergyID);

                        if (oAllergy != null)
                        {
                            oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                            oAllergy.Allergen = txtAllergenName.Text;
                            oAllergy.Reaction = txtReaction.Text;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            allergyDao.Update(oAllergy);

                            result = "1|edit|1";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Allergy Record Information");
                            isError = true;
                        }

                        processType = 2;
                    }
                    else
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        oAllergy = allergyDao.Get(allergyID);

                        if (oAllergy != null)
                        {
                            //TODO : Prompt user for delete reason
                            oAllergy.IsDeleted = true;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            allergyDao.Update(oAllergy);
                            result = "1|delete|";
                            string isHasAllergy = "0";

                            List<PatientAllergy> lstAllergy = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN), ctx);
                            if (lstAllergy.Count > 0)
                            {
                                Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                                if (!oPatient.IsHasAllergy)
                                {
                                    oPatient.IsHasAllergy = true;
                                    oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientDao.Update(oPatient);
                                }

                                isHasAllergy = "1";
                            }
                            else
                            {
                                Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                                if (oPatient.IsHasAllergy)
                                {
                                    oPatient.IsHasAllergy = false;
                                    oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    patientDao.Update(oPatient);
                                }

                                isHasAllergy = "0";
                            }
                            result = "1|delete|"+isHasAllergy;

                            processType = 2;
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Allergy Record Information");
                            isError = true;
                        }
                    }

                    if (!isError)
                    {
                        ctx.CommitTransaction();

                        if (AppSession.SA0137 == "1")
                        {
                            if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                            {
                                BridgingToMedinfrasV1("allergy", processType, null, null, null);
                            }
                        }
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                    }
                }

            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|process|{0}", ex.Message);
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Review of System
        protected void grdROSView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Row.DataItem;
                Repeater rptReviewOfSystemDt = (Repeater)e.Row.FindControl("rptReviewOfSystemDt");
                rptReviewOfSystemDt.DataSource = GetReviewOfSystemDt(obj.ID);
                rptReviewOfSystemDt.DataBind();
            }
        }

        protected List<vReviewOfSystemDt> GetReviewOfSystemDt(Int32 ID)
        {
            return lstReviewOfSystemDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpROSView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewROS(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewROS(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewROS(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReviewOfSystemHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0} ORDER BY TagProperty", AppSession.RegisteredPatient.VisitID));
            grdROSView.DataSource = lstEntity;
            grdROSView.DataBind();
        }

        protected void cbpDeleteROS_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnReviewOfSystemID.Value != "")
            {
                ReviewOfSystemHd entity = BusinessLayer.GetReviewOfSystemHd(Convert.ToInt32(hdnReviewOfSystemID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateReviewOfSystemHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Body Diagram
        protected void cbpBodyDiagramView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageIndex = Convert.ToInt32(hdnPageIndex.Value);
            int pageCount = Convert.ToInt32(hdnPageCount.Value);
            if (e.Parameter == "refresh")
            {
                string filterExpression = "";
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

                pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
                result = "count|" + pageCount;
                if (pageCount > 0)
                    OnLoadBodyDiagram(0);
            }
            else if (e.Parameter == "edit")
            {
                result = "edit";
                OnLoadBodyDiagram(pageIndex);
            }
            else
            {
                if (e.Parameter == "next")
                {
                    pageIndex++;
                    if (pageIndex == pageCount)
                        pageIndex = 0;
                }
                else if (e.Parameter == "prev")
                {
                    pageIndex--;
                    if (pageIndex < 0)
                        pageIndex = pageCount - 1;
                }
                OnLoadBodyDiagram(pageIndex);
                result = "index|" + pageIndex;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void OnLoadBodyDiagram(int PageIndex)
        {
            string filterExpression = "";
            filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            vPatientBodyDiagramHd entity = BusinessLayer.GetvPatientBodyDiagramHd(filterExpression, PageIndex, "ID DESC");
            BodyDiagramToControl(entity);

            filterExpression = string.Format("ID = {0} AND IsDeleted = 0", entity.ID);
            rptRemarks.DataSource = BusinessLayer.GetvPatientBodyDiagramDtList(filterExpression);
            rptRemarks.DataBind();
        }

        private void BodyDiagramToControl(vPatientBodyDiagramHd entity)
        {
            spnParamedicName.InnerHtml = entity.ParamedicName;
            spnObservationDateTime.InnerHtml = entity.DisplayObservationDateTime;
            spnDiagramName.InnerHtml = entity.DiagramName;

            imgBodyDiagram.Src = entity.FileImageUrl;
            hdnBodyDiagramID.Value = entity.ID.ToString();

        }

        protected void cbpDeleteBodyDiagram_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnBodyDiagramID.Value != "")
            {
                PatientBodyDiagramHd entity = BusinessLayer.GetPatientBodyDiagramHd(Convert.ToInt32(hdnBodyDiagramID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientBodyDiagramHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Diagnosis
        private void BindGridViewDiagnosis(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCDiagnoseType");
            List<vPatientDiagnosis> lstMainDiagnosis = lstEntity.Where(lst => lst.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS).ToList();
            vPatientDiagnosis registrationDiagnosis = lstEntity.Where(lst => lst.GCDiagnoseType == Constant.DiagnoseType.EARLY_DIAGNOSIS).FirstOrDefault();

            hdnIsMainDiagnosisExists.Value = lstMainDiagnosis.Count > 0 ? "1" : "0";

            grdDiagnosisView.DataSource = lstEntity.Where(lst => lst.GCDiagnoseType != Constant.DiagnoseType.EARLY_DIAGNOSIS);
            grdDiagnosisView.DataBind();

            if (hdnIsMainDiagnosisExists.Value == "1")
                cboDiagnosisType.Value = Constant.DiagnoseType.COMPLICATION;
            else
                cboDiagnosisType.Value = Constant.DiagnoseType.MAIN_DIAGNOSIS;

            if (registrationDiagnosis != null)
                txtDiagnosis.Text = registrationDiagnosis.cfDiagnosisText;
            else
                txtDiagnosis.Text = string.Empty;

            //Create Diagnosis Summary for : CPOE Clinical Notes
            StringBuilder strDiagnosis = new StringBuilder();
            foreach (var item in lstEntity)
            {
                if (item.GCDifferentialStatus != Constant.DifferentialDiagnosisStatus.RULED_OUT)
                {
                    strDiagnosis.AppendLine(string.Format("{0}", item.cfDiagnosisText));
                }
            }
            hdnDiagnosisSummary.Value = strDiagnosis.ToString();
        }

        protected void cbpDiagnosisView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDiagnosis(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDiagnosis(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = hdnIsMainDiagnosisExists.Value;
            panel.JSProperties["cpDiagnosis"] = hdnDiagnosisSummary.Value;
        }
        protected void cbpDiagnosis_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";
            bool isValid = true;

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        if (hdnIsMainDiagnosisExists.Value == "1" && cboDiagnosisType.Value.ToString() == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                        {
                            result = string.Format("0|add|{0}", "Dalam 1 episode kunjungan/perawatan, hanya boleh ada 1 Diagnosa Utama");
                            isValid = false;
                        }

                        if (isValid)
                        {
                            PatientDiagnosis entity = new PatientDiagnosis();

                            entity.VisitID = AppSession.RegisteredPatient.VisitID;
                            entity.DifferentialDate = Helper.GetDatePickerValue(txtServiceDate);
                            entity.DifferentialTime = txtServiceTime.Text;

                            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            entity.GCDiagnoseType = cboDiagnosisType.Value.ToString();
                            hdnIsMainDiagnosisExists.Value = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                            entity.GCDifferentialStatus = cboDiagnosisStatus.Value.ToString();
                            entity.GCFinalStatus = cboDiagnosisStatus.Value.ToString();

                            if (hdnEntryDiagnoseID.Value != "")
                                entity.DiagnoseID = hdnEntryDiagnoseID.Value;
                            else
                                entity.DiagnoseID = null;

                            entity.DiagnosisText = txtDiagnosisText.Text;
                            entity.MorphologyID = null;
                            entity.IsChronicDisease = false;
                            entity.IsFollowUpCase = false;
                            entity.Remarks = string.Empty;
                            entity.CreatedBy = AppSession.UserLogin.UserID;

                            BusinessLayer.InsertPatientDiagnosis(entity);

                            if (AppSession.SA0137 == "1")
                            {
                                if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                {
                                    BridgingToMedinfrasV1("diagnose", 1, entity, null, null);
                                }
                            }

                            result = "1|add|";
                        }
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);

                        if (entity != null)
                        {
                            if ((hdnIsMainDiagnosisExists.Value == "1" && cboDiagnosisType.Value.ToString() == Constant.DiagnoseType.MAIN_DIAGNOSIS) && entity.GCDiagnoseType != Constant.DiagnoseType.MAIN_DIAGNOSIS)
                            {
                                result = string.Format("0|edit|{0}", "Dalam 1 episode kunjungan/perawatan, hanya boleh ada 1 Diagnosa Utama");
                                isValid = false;
                            }

                            if (isValid)
                            {
                                entity.DifferentialDate = Helper.GetDatePickerValue(txtServiceDate);
                                entity.DifferentialTime = txtServiceTime.Text;

                                entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                                entity.GCDiagnoseType = cboDiagnosisType.Value.ToString();
                                hdnIsMainDiagnosisExists.Value = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                                entity.GCDifferentialStatus = cboDiagnosisStatus.Value.ToString();
                                entity.GCFinalStatus = cboDiagnosisStatus.Value.ToString();

                                if (hdnEntryDiagnoseID.Value != "")
                                    entity.DiagnoseID = hdnEntryDiagnoseID.Value;
                                else
                                    entity.DiagnoseID = null;

                                entity.DiagnosisText = txtDiagnosisText.Text;
                                entity.MorphologyID = null;
                                entity.IsChronicDisease = false;
                                entity.IsFollowUpCase = false;
                                entity.Remarks = string.Empty;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePatientDiagnosis(entity);

                                if (AppSession.SA0137 == "1")
                                {
                                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                    {
                                        BridgingToMedinfrasV1("diagnose", 2, entity, null, null);
                                    }
                                }

                                result = "1|edit|";
                            }
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Diagnosis Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);
                        if (entity != null)
                        {
                            if (entity.ClaimDiagnosisID != null && entity.ClaimDiagnosisID != "")
                            {
                                result = string.Format("0|delete|{0}", "Data diagnosa pasien ini tidak dapat dihapus karena sudah dilengkapi oleh Casemix.");
                            }
                            else
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePatientDiagnosis(entity);

                                if (AppSession.SA0137 == "1")
                                {
                                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                    {
                                        BridgingToMedinfrasV1("diagnose", 3, entity, null, null);
                                    }
                                }

                                result = "1|delete|";
                            }
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Diagnosis Record Information");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
            }


            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Procedure
        private void BindGridViewProcedure(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientProcedureRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientProcedure> lstEntity = BusinessLayer.GetvPatientProcedureList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID");
            grdProcedureView.DataSource = lstEntity;
            grdProcedureView.DataBind();

        }

        protected void cbpProcedureView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewProcedure(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewProcedure(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpProcedure_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";
            bool isValid = true;

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        if (isValid)
                        {
                            PatientProcedure entity = new PatientProcedure();

                            entity.VisitID = AppSession.RegisteredPatient.VisitID;
                            entity.ProcedureDate = Helper.GetDatePickerValue(txtServiceDate);
                            entity.ProcedureTime = txtServiceTime.Text;
                            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            if (hdnEntryProcedureID.Value != "")
                                entity.ProcedureID = hdnEntryProcedureID.Value;
                            else
                                entity.ProcedureID = null;

                            entity.ProcedureText = txtProcedureText.Text;
                            entity.Remarks = string.Empty;
                            entity.CreatedBy = AppSession.UserLogin.UserID;

                            BusinessLayer.InsertPatientProcedure(entity);

                            if (AppSession.SA0137 == "1")
                            {
                                if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                {
                                    BridgingToMedinfrasV1("procedures", 1, null, entity, null);
                                }
                            }

                            result = "1|add|";
                        }
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnProcedureID.Value);
                        PatientProcedure entity = BusinessLayer.GetPatientProcedure(recordID);

                        if (entity != null)
                        {
                            if (isValid)
                            {
                                entity.ProcedureDate = Helper.GetDatePickerValue(txtServiceDate);
                                entity.ProcedureTime = txtServiceTime.Text;

                                entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                                if (hdnEntryProcedureID.Value != "")
                                    entity.ProcedureID = hdnEntryProcedureID.Value;
                                else
                                    entity.ProcedureID = null;

                                entity.ProcedureText = txtProcedureText.Text;
                                entity.Remarks = string.Empty;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdatePatientProcedure(entity);

                                if (AppSession.SA0137 == "1")
                                {
                                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                    {
                                        BridgingToMedinfrasV1("procedures", 2, null, entity, null);
                                    }
                                }

                                result = "1|edit|";
                            }
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Procedure Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnProcedureID.Value);
                        PatientProcedure entity = BusinessLayer.GetPatientProcedure(recordID);

                        if (entity != null)
                        {
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientProcedure(entity);

                            if (AppSession.SA0137 == "1")
                            {
                                if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                {
                                    BridgingToMedinfrasV1("procedures", 3, null, entity, null);
                                }
                            }

                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Procedure Record Information");
                        }
                        result = "1|delete|";
                    }

                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Laboratory
        private void BindGridViewLaboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode  = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnLaboratoryOrderSummaryText.Value = sbText.ToString();

            grdLaboratoryView.DataSource = lstEntity;
            grdLaboratoryView.DataBind();
        }

        protected void cbpLaboratoryView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewLaboratory(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewLaboratory(1, true, ref pageCount);
                    result = "refresh|" + pageCount + '|' + hdnLaboratoryOrderSummaryText.Value;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnLaboratoryOrderSummaryText.Value;
        }

        protected void grdLaboratoryView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptLaboratoryDt = (Repeater)e.Row.FindControl("rptLaboratoryDt");
                rptLaboratoryDt.DataSource = obj.cfTestOrderDetailList;
                rptLaboratoryDt.DataBind();
            }
        }

        private object GetTestOrderDt(int testOrderID)
        {
            List<vTestOrderDt> lstOrderDt = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0 ORDER BY ItemName1", testOrderID));
            return lstOrderDt;
        }
        #endregion

        #region Imaging
        private void BindGridViewImaging(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, hdnImagingServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnImagingSummary.Value = sbText.ToString();

            grdImagingView.DataSource = lstEntity;
            grdImagingView.DataBind();
        }

        protected void cbpImagingView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewImaging(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewImaging(1, true, ref pageCount);
                    result = "refresh|" + pageCount + '|' + hdnImagingSummary.Value;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnImagingSummary.Value;
        }

        protected void grdImagingView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptImagingDt = (Repeater)e.Row.FindControl("rptImagingDt");
                List<vTestOrderDt> lstOrderDt = (List<vTestOrderDt>)GetTestOrderDt(obj.TestOrderID);
                rptImagingDt.DataSource = lstOrderDt;
                rptImagingDt.DataBind();


                StringBuilder sbOrderInfo = new StringBuilder();
                foreach (vTestOrderDt item in lstOrderDt)
                {
                    string remarks = String.IsNullOrEmpty(item.Remarks) ? string.Empty : "(" + item.Remarks + ")";
                    sbOrderInfo.AppendLine(string.Format("{0} {1}", item.ItemName1, remarks));
                }
                hdnImagingSummaryText.Value = sbOrderInfo.ToString();
            }
        }
        #endregion

        #region Diagnostic
        private void BindGridViewDiagnostic(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID NOT IN ({1},{2}) AND GCTransactionStatus != '{3}'", AppSession.RegisteredPatient.VisitID, hdnLaboratoryServiceUnitID.Value, hdnImagingServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnDiagnosticSummary.Value = sbText.ToString();

            grdDiagnosticView.DataSource = lstEntity;
            grdDiagnosticView.DataBind();
        }

        protected void grdDiagnosticView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptDiagnosticDt = (Repeater)e.Row.FindControl("rptDiagnosticDt");
                rptDiagnosticDt.DataSource = obj.cfTestOrderDetailList;
                rptDiagnosticDt.DataBind();
            }
        }

        protected void cbpDiagnosticView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDiagnostic(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDiagnostic(1, true, ref pageCount);
                    result = "refresh|" + pageCount + '|' + hdnDiagnosticSummary.Value;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnDiagnosticSummary.Value;
        }
        #endregion

        #region Instruction
        private void BindGridViewInstruction(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string visitNoteID = !string.IsNullOrEmpty(_visitNoteID) ? _visitNoteID : "0";
            string filterExpression = string.Format("VisitID IN ({0}) AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, visitNoteID);

            if (visitNoteID != "0")
            {
                filterExpression += string.Format(" AND PatientVisitNoteID = {0}", visitNoteID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientInstructionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientInstruction> lstEntity = BusinessLayer.GetvPatientInstructionList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PatientInstructionID");

            StringBuilder instructionText = new StringBuilder();
            foreach (vPatientInstruction obj in lstEntity)
            {
                instructionText.AppendLine(string.Format("- {0}", obj.Description));
            }
            hdnInstructionText.Value = instructionText.ToString();

            grdInstructionView.DataSource = lstEntity;
            grdInstructionView.DataBind();

            //if (lstEntity.Count == 0)
            //    trWriteInstructionCPPT.Style.Add("display", "none");
            //else
            //    trWriteInstructionCPPT.Style.Remove("display");
        }

        protected void cbpInstructionView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewInstruction(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewInstruction(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = "";
        }
        protected void cbpInstruction_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        if (_visitNoteID == "0" || _visitNoteID == "")
                        {
                            PatientVisitNote soapNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES)).FirstOrDefault();
                            if (soapNote != null)
                            {
                                _visitNoteID = soapNote.ID.ToString();
                                hdnPatientVisitNoteID.Value = soapNote.ID.ToString();
                            }
                        }

                        PatientInstruction entity = new PatientInstruction();

                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.PatientVisitNoteID = Convert.ToInt32(_visitNoteID);
                        entity.InstructionDate = DateTime.Now.Date;
                        entity.InstructionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                        entity.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.GCInstructionGroup = cboInstructionType.Value.ToString();
                        entity.Description = txtInstructionText.Text;

                        if (!string.IsNullOrEmpty(hdnPatientVisitNoteID.Value) && hdnPatientVisitNoteID.Value != "0")
                        {
                            entity.PatientVisitNoteID = Convert.ToInt32(hdnPatientVisitNoteID.Value);
                        }

                        BusinessLayer.InsertPatientInstruction(entity);

                        hdnIsChanged.Value = "1";
                        hdnIsSaved.Value = "0";

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnInstructionID.Value);
                        PatientInstruction entity = BusinessLayer.GetPatientInstruction(recordID);

                        if (entity != null)
                        {
                            entity.GCInstructionGroup = cboInstructionType.Value.ToString();
                            entity.Description = txtInstructionText.Text;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientInstruction(entity);

                            hdnIsChanged.Value = "1";
                            hdnIsSaved.Value = "0";

                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Instruction Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnInstructionID.Value);
                        PatientInstruction entity = BusinessLayer.GetPatientInstruction(recordID);

                        if (entity != null)
                        {
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientInstruction(entity);

                            hdnIsChanged.Value = "1";
                            hdnIsSaved.Value = "0";

                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Instruction Record Information");
                        }
                        result = "1|delete|";
                    }

                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected void cbpDeleteTestOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (e.Parameter != null && e.Parameter != "")
            {
                string testOrderID = "0";

                switch (e.Parameter)
                {
                    case "LB":
                        testOrderID = hdnLaboratoryTestOrderID.Value;
                        break;
                    case "IS":
                        testOrderID = hdnImagingTestOrderID.Value;
                        break;
                    case "MD":
                        testOrderID = hdnDiagnosticTestOrderID.Value;
                        break;
                    default:
                        break;
                }

                if (testOrderID != "0")
                {
                    TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(testOrderID));
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTestOrderHd(entity);
                    result = string.Format("1|{0}", e.Parameter);
                }
                else
                {
                    result = string.Format("0|{0}|There is no record to be deleted !");
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpDoctorFee_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            int transactionID = Convert.ToInt32(param[2]);
            result = param[0] + "|" + param[1] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    if (param[1] != "PH")
                    {
                        TestOrderHd entity = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0}", transactionID))[0];
                        if (entity.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            BusinessLayer.UpdateTestOrderHd(entity);
                            try
                            {
                                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.HealthcareServiceUnitID));
                                string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                if (!String.IsNullOrEmpty(ipAddress))
                                {
                                    SendNotification(entity, ipAddress);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                result += string.Format("success|{0}|{1}", param[1], errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}|{1}", param[1], errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
        }

        protected void cbpSendOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            int transactionID = Convert.ToInt32(param[2]);
            result = param[0] + "|" + param[1] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    if (param[1] != "PH")
                    {
                        TestOrderHd entity = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0}", transactionID))[0];
                        if (entity.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.SendOrderDateTime = DateTime.Now;
                            entity.SendOrderBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateTestOrderHd(entity);

                            if (AppSession.SA0137 == "1")
                            {
                                if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                                {
                                    BridgingToMedinfrasV1("order", 1, null, null, entity);
                                }
                            }

                            try
                            {
                                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.HealthcareServiceUnitID));
                                string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                if (!String.IsNullOrEmpty(ipAddress))
                                {
                                    SendNotification(entity, ipAddress);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                result += string.Format("success|{0}{1}", param[1], errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}|{1}", param[1], errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
        }

        private void SendNotification(TestOrderHd order, string ipAddress)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format("No  : {0}", order.TestOrderNo));
            sbMessage.AppendLine(string.Format("Fr  : {0}", string.Format("{0} ({1})", AppSession.RegisteredPatient.ServiceUnitName, AppSession.UserLogin.UserFullName)));
            sbMessage.AppendLine(string.Format("Px  : {0}", AppSession.RegisteredPatient.PatientName));
            sbMessage.AppendLine(string.Format("PDx :    "));
            sbMessage.AppendLine(string.Format("{0}", order.Remarks));
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ipAddress), 6000);
            NetworkStream stream = client.GetStream();
            using (BinaryWriter w = new BinaryWriter(stream))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    w.Write(string.Format(@"{0}", sbMessage.ToString()).ToCharArray());
                }
            }
        }

        private string CleanHTMLTagFromResult(string resultText)
        {
            string result = resultText.Replace(@"<strong>", "");
            result = resultText.Replace(@"</strong>", "");
            result = resultText.Replace("&nbsp;", "");
            result = resultText.Replace(@"<br />", Environment.NewLine);
            return result;
        }

        public static string StripHTML(string HTMLText, bool decode = true)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            var stripped = reg.Replace(HTMLText, "");
            return decode ? HttpUtility.HtmlDecode(stripped) : stripped;
        }

        protected void rptReviewOfSystemHd_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Item.DataItem;
                Repeater rptReviewOfSystemDt = (Repeater)e.Item.FindControl("rptReviewOfSystemDt");
                rptReviewOfSystemDt.DataSource = lstReviewOfSystemDt.Where(p => p.ID == obj.ID).ToList();
                rptReviewOfSystemDt.DataBind();
            }
        }

        protected void rptVitalSignHd_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Item.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Item.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = lstVitalSignDt.Where(p => p.ID == obj.ID).ToList();
                rptVitalSignDt.DataBind();
            }
        }

        protected void rptLabTestOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LaboratoryHdInfo obj = (LaboratoryHdInfo)e.Item.DataItem;
                List<LaboratoryDtInfo> lstResultDt = new List<LaboratoryDtInfo>();
                Repeater rptLaboratoryDt = (Repeater)e.Item.FindControl("rptLaboratoryDt");
                if (obj.ItemID != 0) // Is Not From Migration
                {
                    List<vLaboratoryResultDt> lstLabEntity = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ChargeTransactionID = {0} AND ItemID = {1} ORDER BY DisplayOrder", obj.ChargesID, obj.ItemID));
                    if (lstLabEntity.Count > 0)
                    {
                        foreach (vLaboratoryResultDt result in lstLabEntity)
                        {
                            LaboratoryDtInfo oDetail = new LaboratoryDtInfo();
                            oDetail.ChargesID = result.ChargeTransactionID;
                            oDetail.ItemID = result.ItemID;
                            oDetail.FractionName = result.FractionName1;
                            if (!string.IsNullOrEmpty(result.TextNormalValue))
                            {
                                oDetail.IsResultInPDF = result.TextNormalValue.Trim() == "PDF";
                            }

                            if (!String.IsNullOrEmpty(result.TextValue))
                            {
                                if (!oDetail.IsResultInPDF)
                                {
                                    string resultText = CleanHTMLTagFromResult(result.TextValue);
                                    oDetail.ResultValue = StripHTML(resultText);
                                    oDetail.ResultUnit = result.MetricUnit;
                                }
                                else
                                {
                                    oDetail.ResultValue = "Hasil PDF";
                                    oDetail.PDFValue = result.TextValue;
                                    oDetail.ResultUnit = string.Empty;
                                }
                            }
                            else
                            {
                                oDetail.ResultValue = result.MetricResultValue.ToString("G29");
                                oDetail.ResultUnit = result.MetricUnit;
                            }
                            oDetail.RefRange = string.Format("{0} - {1}", result.MinMetricNormalValue.ToString("G29"), result.MaxMetricNormalValue.ToString("G29"));
                            oDetail.ResultFlag = string.IsNullOrEmpty(result.ResultFlag) ? "N" : result.ResultFlag;

                            lstResultDt.Add(oDetail);
                        }
                    }
                }

                rptLaboratoryDt.DataSource = lstResultDt;
                rptLaboratoryDt.DataBind();
            }
        }
        protected void rptImagingTestOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImagingHdInfo obj = (ImagingHdInfo)e.Item.DataItem;
                List<ImagingDtInfo> lstResultDt = new List<ImagingDtInfo>();
                List<vImagingResultDt> lstEntity = BusinessLayer.GetvImagingResultDtList(string.Format("ID IN (SELECT ID FROM ImagingResultHd WHERE ChargeTransactionID = {0}) AND ItemID = {1}", obj.ChargesID, obj.ItemID));
                Repeater rptImagingTestOrderDt = (Repeater)e.Item.FindControl("rptImagingTestOrderDt");
                if (obj.ItemID != 0)
                {
                    if (lstEntity.Count > 0)
                    {
                        foreach (vImagingResultDt result in lstEntity)
                        {
                            ImagingDtInfo oDetail = new ImagingDtInfo();
                            oDetail.ChargesID = obj.ChargesID;
                            oDetail.ItemID = result.ItemID;
                            string resultText = CleanHTMLTagFromResult(result.TestResult1);
                            oDetail.ResultValue = StripHTML(resultText);
                            lstResultDt.Add(oDetail);
                        }
                    }
                }

                rptImagingTestOrderDt.DataSource = lstResultDt;
                rptImagingTestOrderDt.DataBind();
            }
        }

        protected void cbpSOAPCopy_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                ChiefComplaint obj = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                if (obj != null)
                    hdnChiefComplaintID.Value = obj.ID.ToString();
                else
                    hdnChiefComplaintID.Value = "0";

                CopyFromPreviousVisit(ctx);
                ctx.CommitTransaction();

                hdnIsSaved.Value = "1";
                hdnIsChanged.Value = "0";

                result += string.Format("success|{0}", errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
                hdnIsSaved.Value = "0";
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = 0;
        }

        private void CopyFromPreviousVisit(IDbContext ctx)
        {
            int visitID = Convert.ToInt32(hdnPreviousVisitID.Value);

            ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            PastMedicalDao pastMedicalDao = new PastMedicalDao(ctx);
            PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);

            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
            {
                entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
                entityConsultVisit.StartServiceTime = txtServiceTime.Text;
            }

            if (!string.IsNullOrEmpty(cboVisitType.Value.ToString()))
            {
                entityConsultVisit.VisitTypeID = Convert.ToInt16(cboVisitType.Value);
            }

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
            {
                entityConsultVisit.TimeElapsed0 = string.Format("{0}:{1}", hdnTimeElapsed0hour.Value.PadLeft(2, '0'), hdnTimeElapsed0minute.Value.PadLeft(2, '0'));
            }
            entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}", hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));
            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;

            consultVisitDao.Update(entityConsultVisit);

            ChiefComplaint entity = null;
            bool isNewChiefComplaint = true;

            if (hdnChiefComplaintID.Value != "" && hdnChiefComplaintID.Value != "0")
            {
                entity = chiefComplaintDao.Get(Convert.ToInt32(hdnChiefComplaintID.Value));
                isNewChiefComplaint = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new ChiefComplaint();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ObservationTime = txtServiceTime.Text;

            if (chkIsCopyChiefComplaint.Checked)
            {
                #region Chief Complaint

                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterExpression, 5, 1, "ID DESC").FirstOrDefault();

                if (oChiefComplaint != null)
                {
                    string chiefComplaintText = oChiefComplaint.ChiefComplaintText;
                    string hpiText = oChiefComplaint.HPISummary;
                    if (chkIsCopyHPI.Checked)
                    {
                        if (hdnIsHPIVisible.Value == "1")
                            entity.HPISummary = oChiefComplaint.HPISummary;
                        else
                            entity.ChiefComplaintText = string.Format("{0}{1}{2}", chiefComplaintText, Environment.NewLine, hpiText);
                    }
                    else
                    {
                        entity.ChiefComplaintText = string.Format("{0}", chiefComplaintText);
                    }


                    entity.IsPatientAllergyExists = oChiefComplaint.IsPatientAllergyExists;


                    entity.IsAutoAnamnesis = Convert.ToBoolean(oChiefComplaint.IsAutoAnamnesis);
                    entity.IsAlloAnamnesis = Convert.ToBoolean(oChiefComplaint.IsAlloAnamnesis);

                    if (chkIsCopyPastMedicalHistory.Checked)
                    {
                        entity.PastMedicalHistory = oChiefComplaint.PastMedicalHistory;             
                    }

                    if (chkIsCopyPastMedicationHistory.Checked)
                    {
                        entity.PastMedicationHistory = oChiefComplaint.PastMedicationHistory;
                    }

                    if (chkIsCopyDiagnosticResultSummary.Checked)
                    {
                        entity.DiagnosticResultSummary = oChiefComplaint.DiagnosticResultSummary;
                    }
                }

                #endregion
            }
            else
            {
                entity.ChiefComplaintText = txtChiefComplaint.Text;
                entity.HPISummary = txtHPISummary.Text;
                entity.IsPatientAllergyExists = !chkIsPatientAllergyExists.Checked;

                entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
                entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            }

            if (isNewChiefComplaint)
            {
                hdnChiefComplaintID.Value = chiefComplaintDao.InsertReturnPrimaryKeyID(entity).ToString();
            }
            else
            {
                chiefComplaintDao.Update(entity);
            }

            if (chkIsCopyROS.Checked)
            {
                #region Review of System

                ReviewOfSystemHdDao entityROSDao = new ReviewOfSystemHdDao(ctx);
                ReviewOfSystemDtDao entityROSDtDao = new ReviewOfSystemDtDao(ctx);

                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
                List<vReviewOfSystemHd> lstReviewOfSystemHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, 10, 1, "ID DESC");
                foreach (vReviewOfSystemHd ros in lstReviewOfSystemHd)
                {
                    ReviewOfSystemHd entityROS = new ReviewOfSystemHd();
                    entityROS.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
                    entityROS.ObservationTime = txtServiceTime.Text;
                    entityROS.ParamedicID = AppSession.UserLogin.ParamedicID;
                    entityROS.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityROS.CreatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    Int32 oROS_id = entityROSDao.InsertReturnPrimaryKeyID(entityROS);

                    List<ReviewOfSystemDt> lstEntityDt = BusinessLayer.GetReviewOfSystemDtList(string.Format("ID IN ({0})", ros.ID), ctx);
                    foreach (ReviewOfSystemDt entityROSDt in lstEntityDt)
                    {
                        entityROSDt.ID = oROS_id;
                        entityROSDtDao.Insert(entityROSDt);
                    }
                }

                #endregion
            }

            if (chkIsCopyVitalSign.Checked)
            {
                #region Vital Sign

                VitalSignHdDao entityVSDao = new VitalSignHdDao(ctx);
                VitalSignDtDao entityVSDtDao = new VitalSignDtDao(ctx);

                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
                vVitalSignHd oVitalSignHd = BusinessLayer.GetvVitalSignHdList(filterExpression, 3, 1, "ID DESC").FirstOrDefault();
                if (oVitalSignHd != null)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<VitalSignDt> lstEntityVSDt = BusinessLayer.GetVitalSignDtList(string.Format("ID = {0}", oVitalSignHd.ID), ctx);

                    VitalSignHd entityVS = new VitalSignHd();
                    entityVS.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
                    entityVS.ObservationTime = txtServiceTime.Text;
                    entityVS.Remarks = oVitalSignHd.Remarks;
                    entityVS.IsDischargeVitalSign = false;
                    entityVS.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityVS.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                    entityVS.CreatedBy = AppSession.UserLogin.UserID;
                    entityVS.ID = entityVSDao.InsertReturnPrimaryKeyID(entityVS);

                    foreach (VitalSignDt entityDt in lstEntityVSDt)
                    {
                        entityDt.ID = entityVS.ID;
                        entityVSDtDao.Insert(entityDt);
                    }
                }

                #endregion
            }

            if (chkIsCopyDiagnosis.Checked)
            {
                #region Patient Diagnosa

                PatientDiagnosisDao diagnosisDao = new PatientDiagnosisDao(ctx);

                string filterExpression = "";

                if (chkIsCopyEarlyDiagnose.Checked)
                {
                    filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND IsNutritionDiagnosis = 0 AND ((DiagnoseID IS NOT NULL AND DiagnoseID != '') OR (DiagnosisText IS NOT NULL AND DiagnosisText != '')) ORDER BY ID", visitID);
                }
                else
                {
                    filterExpression = string.Format("VisitID = {0} AND GCDiagnoseType != '{1}' AND IsDeleted = 0 AND IsNutritionDiagnosis = 0 AND ((DiagnoseID IS NOT NULL AND DiagnoseID != '') OR (DiagnosisText IS NOT NULL AND DiagnosisText != '')) ORDER BY ID", visitID, Constant.DiagnoseType.EARLY_DIAGNOSIS);
                }

                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
                if (lstDiagnosis.Count > 0)
                {
                    foreach (vPatientDiagnosis item in lstDiagnosis)
                    {
                        PatientDiagnosis oDiagnosis = new PatientDiagnosis();

                        oDiagnosis.VisitID = AppSession.RegisteredPatient.VisitID;
                        oDiagnosis.DifferentialDate = Helper.GetDatePickerValue(txtServiceDate);
                        oDiagnosis.DifferentialTime = txtServiceTime.Text;

                        oDiagnosis.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        oDiagnosis.GCDiagnoseType = item.GCDiagnoseType;
                        hdnIsMainDiagnosisExists.Value = oDiagnosis.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                        oDiagnosis.GCDifferentialStatus = item.GCDifferentialStatus;

                        if (hdnEntryDiagnoseID.Value != "")
                            oDiagnosis.DiagnoseID = item.DiagnoseID;
                        else
                            oDiagnosis.DiagnoseID = null;

                        oDiagnosis.DiagnosisText = item.DiagnosisText;
                        oDiagnosis.MorphologyID = null;
                        oDiagnosis.IsChronicDisease = false;
                        oDiagnosis.IsFollowUpCase = false;
                        oDiagnosis.Remarks = string.Empty;
                        oDiagnosis.CreatedBy = AppSession.UserLogin.UserID;

                        diagnosisDao.Insert(oDiagnosis);
                    }
                }

                #endregion
            }

            #region Patient Instruction

            PatientVisitNote entityInstruction = null;
            bool isPatientVisitNote = true;

            if (chkIsCopyInstruction.Checked)
            {
                if (hdnPatientVisitNoteID.Value != "" && hdnPatientVisitNoteID.Value != "0")
                {
                    entityInstruction = patientVisitNoteDao.Get(Convert.ToInt32(hdnPatientVisitNoteID.Value));
                    isPatientVisitNote = false;
                    entityInstruction.LastUpdatedBy = AppSession.UserLogin.UserID;
                }
                else
                {
                    entityInstruction = new PatientVisitNote();
                    entityInstruction.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    entityInstruction.CreatedBy = AppSession.UserLogin.UserID;
                }

                entityInstruction.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
                entityInstruction.NoteTime = txtServiceTime.Text;

                string filterExpressionInstruction = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
                List<vPatientInstruction> lstInstruction = BusinessLayer.GetvPatientInstructionList(filterExpressionInstruction);
                if (lstInstruction.Count > 0)
                {
                    foreach (vPatientInstruction item in lstInstruction)
                    {
                        if (!string.IsNullOrEmpty(item.PatientInstructionID.ToString()))
                        {
                            string instructionText = item.Description;

                            entityInstruction.InstructionText = string.Format("{0}", instructionText);
                            entityInstruction.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                        }
                    }
                }
                else
                {
                    string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", visitID);
                    vPatientVisitNote oPatientVisitNote = BusinessLayer.GetvPatientVisitNoteList(filterExpression, 5, 1, "ID DESC").FirstOrDefault();

                    if (oPatientVisitNote != null)
                    {
                        string instructionText = oPatientVisitNote.InstructionText;

                        entityInstruction.InstructionText = string.Format("{0}", instructionText);
                        entityInstruction.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                    }
                }

                if (isPatientVisitNote)
                {
                    hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(entityInstruction).ToString();
                }
                else
                {
                    patientVisitNoteDao.Update(entityInstruction);
                }
            }
            #endregion
        }

        private void BridgingToMedinfrasV1(string type, int ProcessType, PatientDiagnosis entity, PatientProcedure entityProcedure, TestOrderHd entityOrder)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = string.Empty;
            if (type == "diagnose")
            {
                serviceResult = oService.OnSendPatientDiagnoseInformation(ProcessType, entity);
            }
            else if (type == "procedures")
            {
                serviceResult = oService.OnSendPatientProceduresInformation(ProcessType, entityProcedure);
            }
            else if (type == "order")
            {
                serviceResult = oService.OnSendOrderMedicalDiagnosticServices(ProcessType, entityOrder, null, null);
            }
            else if (type == "allergy")
            {
                serviceResult = oService.OnSendPatientAllergiesInformation(ProcessType, AppSession.RegisteredPatient.RegistrationNo);
            }
            if (!string.IsNullOrEmpty(serviceResult))
            {
                string[] serviceResultInfo = serviceResult.Split('|');
                if (serviceResultInfo[0] == "1")
                {
                    apiLog.IsSuccess = true;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                }
                else
                {
                    apiLog.IsSuccess = false;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                    apiLog.ErrorMessage = serviceResultInfo[2];
                }
                BusinessLayer.InsertAPIMessageLog(apiLog);
            }
        }

        protected void cbpIntegrationNote_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            result = "fail|no error information available";

            if ((hdnChiefComplaintID.Value != "" && hdnChiefComplaintID.Value != "0") && !IsValidToSave(ref errMessage))
            {
                hdnIsSaved.Value = "0";
                result = "fail|" + errMessage;
            }
            else
            {

                IDbContext ctx = DbFactory.Configure(true);
                PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);

                try
                {
                    PatientVisitNote entityOutpatientCaseNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES), ctx).FirstOrDefault();
                    bool isEntityOutpatientCaseNoteNull = false;
                    if (entityOutpatientCaseNote == null)
                    {
                        isEntityOutpatientCaseNoteNull = true;
                        entityOutpatientCaseNote = new PatientVisitNote();
                    }

                    #region Summarize Instruction Text
                    string filterExpression = string.Format("VisitID IN ({0}) AND PatientVisitNoteID = {1} AND IsDeleted = 0 ORDER BY PatientInstructionID", AppSession.RegisteredPatient.VisitID, _visitNoteID);
                    List<PatientInstruction> lstEntity = BusinessLayer.GetPatientInstructionList(filterExpression, ctx);

                    StringBuilder instructionText = new StringBuilder();
                    foreach (PatientInstruction obj in lstEntity)
                    {
                        instructionText.AppendLine(string.Format("- {0}", obj.Description));
                    }

                    hdnInstructionText.Value = instructionText.ToString();
                    #endregion

                    ControlToEntity(entityOutpatientCaseNote);

                    if (isEntityOutpatientCaseNoteNull)
                    {
                        entityOutpatientCaseNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityOutpatientCaseNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityOutpatientCaseNote.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                        entityOutpatientCaseNote.CreatedBy = AppSession.UserLogin.UserID;
                        hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(entityOutpatientCaseNote).ToString();
                    }
                    else
                    {
                        entityOutpatientCaseNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        entityOutpatientCaseNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(entityOutpatientCaseNote);

                        hdnPatientVisitNoteID.Value = entityOutpatientCaseNote.ID.ToString();
                    }

                    _visitNoteID = hdnPatientVisitNoteID.Value;

                    ctx.CommitTransaction();

                    result += string.Format("success|{0}", errMessage);
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result += string.Format("fail|{0}", errMessage);
                }

                ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                panel.JSProperties["cpResult"] = result;
                panel.JSProperties["cpVisitNoteID"] = _visitNoteID;
            }
        }

        #region List Tooth
        protected string GetToothNumber(string StandardCodeID)
        {
            return StandardCodeID.Replace("X044^0", "");
        }

        protected void cbpListTooth_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            CreateTableTooth();
        }

        #region Create Table Tooth
        class Tooth
        {
            public Int32 Line;
            public String ImageUrl;
            public Int32 Number;
            public Int32 IndexImage;
        }

        private void CreateTableTooth()
        {
            List<Tooth> lstTooth = new List<Tooth>();

            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0 ORDER BY ID DESC", AppSession.RegisteredPatient.MRN);
            List<vPatientDental> lstEntity = BusinessLayer.GetvPatientDentalList(filterExpression);

            for (int ctr = 1, i = 18, j = 28; i > 10; --i, --j, ctr++)
            {
                string imgUrl = "";
                if (i > 13)
                    imgUrl = "tooth.png";
                else
                    imgUrl = "tooth_2.png";
                lstTooth.Add(new Tooth { Line = 2, ImageUrl = getToothImage(imgUrl, lstEntity, i), Number = i, IndexImage = ctr });
                lstTooth.Add(new Tooth { Line = 2, ImageUrl = getToothImage(imgUrl, lstEntity, j), Number = j, IndexImage = 17 - ctr });

                lstTooth.Add(new Tooth { Line = 3, ImageUrl = getToothImage(imgUrl, lstEntity, i + 30), Number = i + 30, IndexImage = ctr });
                lstTooth.Add(new Tooth { Line = 3, ImageUrl = getToothImage(imgUrl, lstEntity, j + 10), Number = j + 10, IndexImage = 17 - ctr });
            }

            for (int ctr = 1, i = 55, j = 65; i > 50; --i, --j, ctr++)
            {
                string imgUrl = "";
                if (i > 13)
                    imgUrl = "tooth.png";
                else
                    imgUrl = "tooth_2.png";
                lstTooth.Add(new Tooth { Line = 1, ImageUrl = getToothImage(imgUrl, lstEntity, i), Number = i, IndexImage = ctr });
                lstTooth.Add(new Tooth { Line = 1, ImageUrl = getToothImage(imgUrl, lstEntity, j), Number = j, IndexImage = 11 - ctr });

                lstTooth.Add(new Tooth { Line = 4, ImageUrl = getToothImage(imgUrl, lstEntity, i + 30), Number = i + 30, IndexImage = ctr });
                lstTooth.Add(new Tooth { Line = 4, ImageUrl = getToothImage(imgUrl, lstEntity, j + 10), Number = j + 10, IndexImage = 11 - ctr });
            }

            containerTableTooth.Controls.Add(CreateTableTitle("upper right", "upper left"));

            for (int i = 1; i < 5; ++i)
            {
                HtmlTable tbl = new HtmlTable();
                tbl.Attributes.Add("Class", "tblTooth");
                HtmlTableRow row = new HtmlTableRow();
                List<Tooth> lst = lstTooth.Where(p => p.Line == i).OrderBy(p => p.IndexImage).ToList();
                foreach (Tooth tooth in lst)
                {
                    HtmlTableCell cell = new HtmlTableCell();

                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.InnerHtml = tooth.Number.ToString();

                    HtmlImage img = new HtmlImage();
                    img.Src = string.Format("{0}{1}", Page.ResolveUrl("~/Libs/Images/Medical/"), tooth.ImageUrl);
                    img.Alt = "";

                    if (i < 3)
                    {
                        cell.Controls.Add(div);
                        cell.Controls.Add(img);
                    }
                    else
                    {
                        cell.Controls.Add(img);
                        cell.Controls.Add(div);
                    }

                    row.Cells.Add(cell);
                }
                tbl.Rows.Add(row);

                containerTableTooth.Controls.Add(tbl);
            }
            containerTableTooth.Controls.Add(CreateTableTitle("lower right", "lower left"));
        }

        private string getToothImage(string imgUrl, List<vPatientDental> lstEntity, int ctr)
        {
            string toothID = string.Format("X044^0{0}", ctr.ToString("00"));
            vPatientDental entity = lstEntity.FirstOrDefault(p => p.GCTooth == toothID);
            if (entity != null)
                return string.Format("tooth_{0}.png", entity.ToothStatus.Substring(0, 3));
            return imgUrl;
        }

        private HtmlTable CreateTableTitle(string leftTitle, string rightTitle)
        {
            HtmlTable tbl = new HtmlTable();
            tbl.Attributes.Add("Class", "tblToothHeader");

            HtmlTableRow row = new HtmlTableRow();

            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.InnerHtml = leftTitle;
            HtmlTableCell cell2 = new HtmlTableCell();
            cell2.InnerHtml = rightTitle;

            row.Cells.Add(cell1);
            row.Cells.Add(cell2);

            tbl.Rows.Add(row);
            return tbl;
        }
        #endregion

        protected void cbpTreatmentDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGrdTreatmentDetail(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGrdTreatmentDetail(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Treatment Detail
        private void BindGrdTreatmentDetail(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            pageCount = 0;
            if (hdnToothID.Value != "")
            {
                string toothID = string.Format("X044^0{0}", Convert.ToInt32(hdnToothID.Value).ToString("00"));

                filterExpression = string.Format("MRN = {0} AND GCTooth = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, toothID);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientDentalRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

            }

            List<vPatientDental> lstEntity = BusinessLayer.GetvPatientDentalList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdTreatmentDetail.DataSource = lstEntity;
            grdTreatmentDetail.DataBind();
        }

        protected void cbpDeleteDentalTreatment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteDentalTreatment(param);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "1")
                    result += string.Format("success|{0}", string.Empty);
                else
                    result += string.Format("fail|{0}", retValInfo[1]);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = param;
        }

        private string DeleteDentalTreatment(string recordID)
        {
            string result = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);
            PatientDentalDao recordDao = new PatientDentalDao(ctx);
            PatientProcedureDao entityProcedureDao = new PatientProcedureDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PatientDental entity = recordDao.Get(Convert.ToInt32(hdnTreatmentDetailID.Value));
                if (entity != null)
                {
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    recordDao.Update(entity);

                    if (!string.IsNullOrEmpty(entity.ProcedureID))
                    {
                        List<PatientProcedure> lstEntityProcedure = BusinessLayer.GetPatientProcedureList(string.Format("ReferenceID = {0}", entity.ID), ctx);
                        if (lstEntityProcedure.Count > 0)
                        {
                            PatientProcedure entityProcedure = lstEntityProcedure[0];
                            entityProcedure.IsDeleted = true;
                            entityProcedure.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityProcedureDao.Update(entityProcedure);
                        } 
                    }

                    ctx.CommitTransaction();

                    result = string.Format("1|{0}", string.Empty);
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
        #endregion
        #endregion
    }
}
