using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class GenerateBPJSFollowup1Ctl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = false;
            SetControlProperties();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMedicalResumeDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtAssessmentText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPlanningResumeText, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFollowupVisitDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.Date.AddDays(7).ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            errMessage = string.Empty;

            IDbContext ctx = DbFactory.Configure(true);

            try
            {
                RegistrationBPJSDao oBPJSDao = new RegistrationBPJSDao(ctx);

                AppointmentRequestDao oAppointmentRequestDao = new AppointmentRequestDao(ctx);
                int appointmentID = Convert.ToInt32(hdnAppointmentRequestID.Value);

                if (rblReferBackType.SelectedValue == "2")
                {
                    #region Appointment Information
                    AppointmentRequest oAppointmentRequest;

                    if (!string.IsNullOrEmpty(hdnAppointmentRequestID.Value) && hdnAppointmentRequestID.Value != "0")
                    {
                        oAppointmentRequest = oAppointmentRequestDao.Get(Convert.ToInt32(hdnAppointmentRequestID.Value));
                        if (oAppointmentRequest != null)
                        {
                            oAppointmentRequest.AppointmentDate = Helper.GetDatePickerValue(txtFollowupVisitDate);
                            oAppointmentRequest.AppointmentTime = oAppointmentRequest.AppointmentTime;
                            oAppointmentRequest.VisitTypeID = Convert.ToInt32(cboFollowupVisitType.Value);
                            oAppointmentRequest.Remarks = GenerateAppointmentRemarks();
                            oAppointmentRequest.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oAppointmentRequest.LastUpdatedDate = DateTime.Now;
                            oAppointmentRequestDao.Update(oAppointmentRequest);
                        }
                    }
                    else
                    {
                        if (cboFollowupVisitType.Value != null)
                        {
                            oAppointmentRequest = new AppointmentRequest();
                            oAppointmentRequest.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                            oAppointmentRequest.VisitID = AppSession.RegisteredPatient.VisitID;
                            oAppointmentRequest.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                            oAppointmentRequest.ParamedicID = AppSession.RegisteredPatient.ParamedicID;
                            oAppointmentRequest.VisitTypeID = Convert.ToInt32(cboFollowupVisitType.Value);
                            oAppointmentRequest.AppointmentDate= Helper.GetDatePickerValue(txtFollowupVisitDate);
                            oAppointmentRequest.AppointmentTime = "00:00";
                            oAppointmentRequest.Remarks = GenerateAppointmentRemarks();
                            oAppointmentRequest.CreatedBy = AppSession.UserLogin.UserID;
                            oAppointmentRequest.CreatedDate = DateTime.Now;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            appointmentID = oAppointmentRequestDao.InsertReturnPrimaryKeyID(oAppointmentRequest);
                        }
                    }
                    #endregion 
                }

                #region BPJS Registration Information
                RegistrationBPJS entity = BusinessLayer.GetRegistrationBPJS(AppSession.RegisteredPatient.RegistrationID);
                ControlToEntity(entity);
                if (!string.IsNullOrEmpty(Request.Form[txtNoSuratKontrol1.UniqueID]))
                {
                    entity.NoRujukanKe = entity.NoSuratKontrol;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                }
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                oBPJSDao.Update(entity);
                #endregion

                ctx.CommitTransaction();

                retval = appointmentID.ToString();
                return true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                return false;
            }
            finally
            {
                ctx.Close();
            }
        }

        private string GenerateAppointmentRemarks()
        {
            StringBuilder remarks = new StringBuilder();
            if (chkPlanForMedication.Checked)
                remarks.AppendLine("Terapi Farmakologis lanjutan yang tidak tersedia di Faskes Tingkat 1");
            if (chkPlanForTheraphy.Checked)
                remarks.AppendLine("Follow up hasil pemeriksaan dan terapi");
            if (chkPlanForOthers.Checked)
                remarks.AppendLine(txtPlanForOtherRemarks.Text);
            return remarks.ToString();
        }

        private void ControlToEntity(RegistrationBPJS entity)
        {
            entity.AssessmentSummaryText = txtAssessmentText.Text;
            entity.PlanningResumeText = txtPlanningResumeText.Text;

            entity.IsReferBack = rblReferBackType.SelectedValue == "1" ? true : false;
            if (entity.IsReferBack)
            {
                entity.IsNotReferBackDueToMedication = false;
                entity.IsNotReferBackDueToTheraphy = false;
                entity.IsNotReferBackDueToOthers = false;

                entity.TanggalRujukanKe = DateTime.MinValue;

                entity.IsPlanForMedication = false;
                entity.IsPlanForTheraphy = false;
                entity.IsPlanForOthers = false;
                entity.PlanOtherRemarks = string.Empty;

                entity.CatatanDirujuk = txtReferralNotes.Text;                
            }
            else
            {
                entity.IsNotReferBackDueToMedication = chkRefferalReasonMedication.Checked;
                entity.IsNotReferBackDueToTheraphy = chkRefferalReasonFollowup.Checked;
                entity.IsNotReferBackDueToOthers = chkRefferalReasonOther.Checked;
                entity.NotReferBackOtherRemarks = txtRefferalReasonOtherText.Text;

                entity.TanggalRujukanKe = Helper.GetDatePickerValue(txtFollowupVisitDate);
                entity.TipeRujukanKe = rblReferBackType.SelectedValue == "1" ? "1" : "2";

                if (!string.IsNullOrEmpty(hdnBPJSPoli.Value))
                {
                    entity.RefferalUnitID = Convert.ToInt32(cboClinic.Value);
                    string[] bpjsInfo = hdnBPJSPoli.Value.Split('|');
                    entity.PoliRujukanKe = bpjsInfo[0];
                }
                else
                {
                    entity.RefferalUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entity.PoliRujukanKe = entity.KodePoliklinik;
                }

                if (!string.IsNullOrEmpty(hdnPhysicianID.Value) && hdnPhysicianID.Value != "0")
                {
                    entity.RefferalPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
                }

                if (!string.IsNullOrEmpty(hdnPhysicianBPJSReferenceInfo.Value) && hdnPhysicianBPJSReferenceInfo.Value.Contains(';'))
                {
                    string[] bpjsInfo = hdnPhysicianBPJSReferenceInfo.Value.Split(';');
                    if (!string.IsNullOrEmpty(bpjsInfo[1]))
                    {
                        string[] hfisInfo = bpjsInfo[1].Split('|');
                        entity.KodeDPJPRujukan = hfisInfo[0];
                        entity.NamaDPJPRujukan = hfisInfo[1];
                    }
                }
                else
                {
                    entity.KodeDPJPRujukan = null;
                    entity.NamaDPJPRujukan = null;
                }

                if (cboDiagnosis.Value != null)
                {
                    entity.DiagnoseID = cboDiagnosis.Value.ToString();
                    entity.DiagnosaDirujuk = hdnBPJSDiagnoseCode.Value;
                }

                entity.IsPlanForMedication = chkPlanForMedication.Checked;
                entity.IsPlanForTheraphy = chkPlanForTheraphy.Checked;
                entity.IsPlanForOthers = chkPlanForOthers.Checked;
                entity.PlanOtherRemarks = txtPlanForOtherRemarks.Text;

                entity.FollowupVisitDate = Helper.GetDatePickerValue(txtFollowupVisitDate);
            }
        }

        private void SetControlProperties()
        {
            #region Physician Combobox
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                cboPhysician.Value = userLoginParamedic.ToString();
            }

            cboPhysician.Enabled = false;
            #endregion

            #region Service Unit
            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboClinic.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            cboClinic.Enabled = false;
            #endregion

            PopulateVisitTypeList(cboClinic.Value.ToString());

            RegistrationBPJS entity = BusinessLayer.GetRegistrationBPJS(AppSession.RegisteredPatient.RegistrationID);
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.NoSuratKontrol))
                {
                    txtNoSuratKontrol1.Text = entity.NoSuratKontrol.Substring(0, 6);
                    txtNoSuratKontrol2.Text = entity.NoSuratKontrol.Substring(7, 6);
                }
                StringBuilder sbNotes = new StringBuilder();
                string filterExpression1 = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY GCDiagnoseType", AppSession.RegisteredPatient.VisitID);
                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterExpression1);
                Methods.SetComboBoxField<vPatientDiagnosis>(cboDiagnosis, lstDiagnosis, "cfDiagnosisText", "DiagnoseID");
                if (string.IsNullOrEmpty(entity.AssessmentSummaryText))
                {
                    #region Assessment Content
                    sbNotes = new StringBuilder();
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
                    txtAssessmentText.Text = sbNotes.ToString();
                    #endregion
                }
                else
                {
                    txtAssessmentText.Text = entity.AssessmentSummaryText;
                }

                cboDiagnosis.Value = entity.DiagnoseID;

                if (string.IsNullOrEmpty(entity.PlanningResumeText))
                {
                    #region Planning Content
                    sbNotes = new StringBuilder();
                    string filterExpression = string.Format("VisitID = {0} AND ParamedicID = {1} AND TestOrderDate = '{2}' AND IsDeleted=0", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID, Helper.GetDatePickerValue(txtMedicalResumeDate).ToString(Constant.FormatString.DATE_FORMAT_112));
                    List<vTestOrderDt> lstOrder = BusinessLayer.GetvTestOrderDtList(filterExpression);
                    if (lstOrder.Count > 0)
                    {
                        foreach (vTestOrderDt item in lstOrder)
                        {
                            sbNotes.AppendLine(string.Format("- {0}", item.ItemName1));
                        }
                    }

                    txtPlanningResumeText.Text = sbNotes.ToString();
                    #endregion
                }
                else
                {
                    txtPlanningResumeText.Text = entity.PlanningResumeText;
                }

                rblReferBackType.SelectedValue = entity.IsReferBack ? "1" : "2";
                chkRefferalReasonMedication.Checked = entity.IsNotReferBackDueToMedication;
                chkRefferalReasonFollowup.Checked = entity.IsNotReferBackDueToTheraphy;
                chkRefferalReasonOther.Checked = entity.IsNotReferBackDueToOthers;
                txtRefferalReasonOtherText.Text = entity.NotReferBackOtherRemarks;

                txtReferralNotes.Text = entity.CatatanDirujuk;

                chkPlanForMedication.Checked = entity.IsPlanForMedication;
                chkPlanForTheraphy.Checked = entity.IsPlanForTheraphy;
                chkPlanForOthers.Checked = entity.IsPlanForOthers;
                txtPlanForOtherRemarks.Text = entity.PlanOtherRemarks;
                txtFollowupVisitDate.Text = entity.FollowupVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                #region Followup Physician
                int paramedicID = entity.RefferalPhysicianID == null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : Convert.ToInt32(entity.RefferalPhysicianID);
                vParamedicMaster oParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", paramedicID)).FirstOrDefault();
                if (oParamedic != null)
                {
                    txtPhysicianCode.Text = oParamedic.ParamedicCode;
                    txtPhysicianName.Text = oParamedic.ParamedicName;
                    hdnPhysicianID.Value = entity.RefferalPhysicianID.ToString();
                    hdnPhysicianBPJSReferenceInfo.Value = oParamedic.BPJSReferenceInfo;
                }
                #endregion

                AppointmentRequest oAppointmentRequest = BusinessLayer.GetAppointmentRequestList(string.Format("RegistrationID  = {0} AND VisitID = {1}", AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                if (oAppointmentRequest != null)
                {
                    hdnAppointmentRequestID.Value = oAppointmentRequest.AppointmentRequestID.ToString();
                    txtFollowupVisitDate.Text = oAppointmentRequest.AppointmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    cboFollowupVisitType.Value = oAppointmentRequest.VisitTypeID.ToString();
                }
                else
                {
                    hdnAppointmentRequestID.Value = "0";
                }
            }
        }

        protected void cbpPopulateVisitType_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (e.Parameter != null && e.Parameter != "")
            {
                string healthcareServiceUnitID = e.Parameter;
                PopulateVisitTypeList(healthcareServiceUnitID);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void PopulateVisitTypeList(string healthcareServiceUnitID)
        {
            bool isByServiceUnit = false;
            if (cboClinic.Value != null)
                isByServiceUnit = !string.IsNullOrEmpty(cboClinic.Value.ToString());

            if (isByServiceUnit)
            {
                List<vServiceUnitVisitType> visitTypeList = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID));
                Methods.SetComboBoxField(cboFollowupVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
                if (visitTypeList.Count > 0)
                    cboFollowupVisitType.SelectedIndex = 0;
            }
            else
            {
                List<VisitType> visitTypeList = BusinessLayer.GetVisitTypeList(string.Format("IsDeleted = 0"));
                Methods.SetComboBoxField(cboFollowupVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
                if (visitTypeList.Count > 0)
                    cboFollowupVisitType.SelectedIndex = 0;
            }
        }
    }
}