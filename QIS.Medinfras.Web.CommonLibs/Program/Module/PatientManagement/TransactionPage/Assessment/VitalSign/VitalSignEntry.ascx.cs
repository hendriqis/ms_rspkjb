using System;
using System.Collections.Generic;
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
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VitalSignEntry : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                SetControlProperties();
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnID.Value));
                List<vVitalSignDt> entityDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0}", param));
                EntityToControl(entity, entityDt);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
                SetControlProperties();
            }
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2} AND IsDeleted = 0",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}' AND IsDeleted = 0", Constant.ParamedicType.Physician));
            }
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");


            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }

                Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    chkIsFallRisk.Checked = oRegistration.IsFallRisk;
                    chkIsRAPUH.Checked = oRegistration.IsRAPUH;
                    chkIsTerminalPatient.Checked = oRegistration.IsTerminalPatient;
                    chkIsDNR.Checked = oRegistration.IsDNR;
                }
            }
            else
            {
                if (AppSession.UserLogin.GCParamedicMasterType != Constant.ParamedicType.Nurse)
                {
                    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                    cboParamedicID.ClientEnabled = false;
                    cboParamedicID.Value = userLoginParamedic.ToString();
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsFallRisk, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsDNR, new ControlEntrySetting(true, true, false, false));
        }

        private void EntityToControl(VitalSignHd entity, List<vVitalSignDt> lstEntityDt)
        {
            txtObservationDate.Text = entity.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.ObservationTime;
            if (AppSession.UserLogin.ParamedicID != null) cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            else cboParamedicID.Value = entity.ParamedicID.ToString();
            chkIsInitialAssessment.Checked = entity.IsInitialAssessment;
            hdnVisitNoteID.Value = entity.PatientVisitNoteID.ToString();
            if (hdnVisitNoteID.Value != "" && hdnVisitNoteID.Value != "0")
            {
                PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnVisitNoteID.Value));
                if (oVisitNote != null)
                {
                    txtPatientVisitNoteText.Text = oVisitNote.NoteText;
                }
            }
            txtRemarks.Text = entity.Remarks;

            #region Vital Sign Dt
            foreach (RepeaterItem item in rptVitalSign.Items)
            {
                HtmlInputHidden hdnVitalSignID = (HtmlInputHidden)item.FindControl("hdnVitalSignID");
                HtmlInputHidden hdnVitalSignType = (HtmlInputHidden)item.FindControl("hdnVitalSignType");

                vVitalSignDt entityDt = lstEntityDt.FirstOrDefault(p => p.VitalSignID == Convert.ToInt32(hdnVitalSignID.Value));
                if (entityDt != null)
                {
                    if (hdnVitalSignType.Value == Constant.ControlType.TEXT_BOX)
                    {
                        TextBox txt = (TextBox)item.FindControl("txtVitalSignType");
                        txt.Text = entityDt.VitalSignValue;
                    }
                    else if (hdnVitalSignType.Value == Constant.ControlType.COMBO_BOX)
                    {
                        ASPxComboBox ddl = (ASPxComboBox)item.FindControl("cboVitalSignType");
                        ddl.Value = entityDt.GCVitalSignValue;
                    }
                }
            }
            #endregion

            Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            if (oRegistration != null)
            {
                chkIsFallRisk.Checked = oRegistration.IsFallRisk;
                chkIsDNR.Checked = oRegistration.IsDNR;
            }
        }

        private void ControlToEntity(VitalSignHd entity, List<VitalSignDt> lstEntityDt)
        {
            entity.ObservationDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ObservationTime = txtObservationTime.Text;
            entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
            if (hdnVisitNoteID.Value != "" && hdnVisitNoteID.Value != "0")
            {
                entity.PatientVisitNoteID = Convert.ToInt32(hdnVisitNoteID.Value);   
            }
            entity.Remarks = txtRemarks.Text;

            string filterExpression = string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')",
                                                        Constant.SettingParameter.VITAL_SIGN_HEIGHT,
                                                        Constant.SettingParameter.VITAL_SIGN_WEIGHT,
                                                        Constant.SettingParameter.VITAL_SIGN_BMI,
                                                        Constant.SettingParameter.VITAL_SIGN_NBPs,
                                                        Constant.SettingParameter.VITAL_SIGN_NBPd,
                                                        Constant.SettingParameter.VITAL_SIGN_MAP,
                                                        Constant.SettingParameter.VITAL_SIGN_RR,
                                                        Constant.SettingParameter.VITAL_SIGN_HR,
                                                        Constant.SettingParameter.VITAL_SIGN_SPO2,
                                                        Constant.SettingParameter.VITAL_SIGN_AVPU,
                                                        Constant.SettingParameter.VITAL_SIGN_EWS,
                                                        Constant.SettingParameter.VITAL_SIGN_TEMPERATURE,
                                                        Constant.SettingParameter.EM0018,
                                                        Constant.SettingParameter.EM0019,
                                                        Constant.SettingParameter.EM0020,
                                                        Constant.SettingParameter.EM0021,
                                                        Constant.SettingParameter.VITAL_SIGN_PEWS_BEHAVIOR,
                                                        Constant.SettingParameter.VITAL_SIGN_PEWS_CARDIOVASKULAR,
                                                        Constant.SettingParameter.VITAL_SIGN_PEWS_RESPIRATION,
                                                        Constant.SettingParameter.VITAL_SIGN_PEWS);
            List<SettingParameter> lstParam = BusinessLayer.GetSettingParameterList(filterExpression);

            string _WeightVitalSignID = "0";
            string _HeightVitalSignID = "0";
            string _BMIVitalSignID = "0";
            string _NBPsVitalSignID = "0";
            string _NBPdVitalSignID = "0";
            string _MAPVitalSignID = "0";
            string _RRVitalSignID = "0";
            string _HRVitalSignID = "0";
            string _SPO2VitalSignID = "0";
            string _AVPUVitalSignID = "0";
            string _TempVitalSignID = "0";
            string _EWSVitalSignID = "0";
            string _GCSTotalEMVVitalSignID = "0";
            string _GCS_E_VitalSignID = "0";
            string _GCS_M_VitalSignID = "0";
            string _GCS_V_VitalSignID = "0";
            string _PEWS_Behavior_VitalSignID = "0";
            string _PEWS_Cardiovascular_VitalSignID = "0";
            string _PEWS_Respiration_VitalSignID = "0";
            string _PEWS_VitalSignID = "0";

            if (lstParam.Count > 0)
            {
                SettingParameter oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_HEIGHT).FirstOrDefault();
                if (oParam != null)
                    _HeightVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_WEIGHT).FirstOrDefault();
                if (oParam != null)
                    _WeightVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_BMI).FirstOrDefault();
                if (oParam != null)
                    _BMIVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_NBPs).FirstOrDefault();
                if (oParam != null)
                    _NBPsVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_NBPd).FirstOrDefault();
                if (oParam != null)
                    _NBPdVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_MAP).FirstOrDefault();
                if (oParam != null)
                    _MAPVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_RR).FirstOrDefault();
                if (oParam != null)
                    _RRVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_HR).FirstOrDefault();
                if (oParam != null)
                    _HRVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_SPO2).FirstOrDefault();
                if (oParam != null)
                    _SPO2VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_AVPU).FirstOrDefault();
                if (oParam != null)
                    _AVPUVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_TEMPERATURE).FirstOrDefault();
                if (oParam != null)
                    _TempVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_EWS).FirstOrDefault();
                if (oParam != null)
                    _EWSVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM0018).FirstOrDefault();
                if (oParam != null)
                    _GCSTotalEMVVitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM0019).FirstOrDefault();
                if (oParam != null)
                    _GCS_E_VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM0020).FirstOrDefault();
                if (oParam != null)
                    _GCS_M_VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM0021).FirstOrDefault();
                if (oParam != null)
                    _GCS_V_VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_PEWS_BEHAVIOR).FirstOrDefault();
                if (oParam != null)
                    _PEWS_Behavior_VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_PEWS_CARDIOVASKULAR).FirstOrDefault();
                if (oParam != null)
                    _PEWS_Cardiovascular_VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_PEWS_RESPIRATION).FirstOrDefault();
                if (oParam != null)
                    _PEWS_Respiration_VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";

                oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VITAL_SIGN_PEWS).FirstOrDefault();
                if (oParam != null)
                    _PEWS_VitalSignID = oParam.ParameterValue != null ? oParam.ParameterValue.ToString() : "0";
            }

            bool isWeightExists = false;
            decimal weight = 0;
            bool isHeightExists = false;
            decimal height = 0;
            bool isSystolicBPExists = false;
            decimal systolicBP = 0;
            bool isDiastolicBPExists = false;
            decimal diastolicBP = 0;

            bool isRRExists = false;
            decimal rr = 0;
            bool isHRExists = false;
            decimal hr = 0;
            bool isSPO2Exists = false;
            decimal spo2 = 0;
            bool isAVPUExists = false;
            string avpu = string.Empty;
            bool isTempExists = false;
            decimal temp = 0;

            bool isGCS_E_Exists = false;
            string gcsE = string.Empty;
            bool isGCS_M_Exists = false;
            string gcsM = string.Empty;
            bool isGCS_V_Exists = false;
            string gcsV = string.Empty;

            bool isPEWS_Behavior_Exists = false;
            string pewsBehavior = string.Empty;
            bool isPEWS_Cardio_Exists = false;
            string pewsCardio = string.Empty;
            bool isPEWS_Respiratory_Exists = false;
            string pewsRespiratory = string.Empty;


            #region Vital Sign Dt
            foreach (RepeaterItem item in rptVitalSign.Items)
            {
                HtmlInputHidden hdnVitalSignID = (HtmlInputHidden)item.FindControl("hdnVitalSignID");
                HtmlInputHidden hdnVitalSignType = (HtmlInputHidden)item.FindControl("hdnVitalSignType");
                if (hdnVitalSignType.Value == Constant.ControlType.TEXT_BOX)
                {
                    TextBox txt = (TextBox)item.FindControl("txtVitalSignType");
                    VitalSignDt entityDt = new VitalSignDt();
                    entityDt.VitalSignID = Convert.ToInt32(hdnVitalSignID.Value);
                    entityDt.VitalSignValue = txt.Text;
                    if (entityDt.VitalSignValue != "")
                    {
                        lstEntityDt.Add(entityDt);
                    }

                    if (hdnVitalSignID.Value == _HeightVitalSignID)
                    {
                        if ((!String.IsNullOrEmpty(txt.Text)))
                        {
                            isHeightExists = true;
                            height = Convert.ToDecimal(txt.Text.Replace(',', '.')) / 100;
                        }
                        else
                        {
                            isHeightExists = false;
                            height = 0;
                        }
                    }

                    if (hdnVitalSignID.Value == _WeightVitalSignID)
                    {
                        if ((!String.IsNullOrEmpty(txt.Text)))
                        {
                            isWeightExists = true;
                            weight = Convert.ToDecimal(txt.Text.Replace(',', '.'));
                        }
                        else
                        {
                            isWeightExists = false;
                            weight = 0;
                        }
                    }

                    if (hdnVitalSignID.Value == _NBPsVitalSignID)
                    {
                        if ((!String.IsNullOrEmpty(txt.Text)))
                        {
                            isSystolicBPExists = true;
                            systolicBP = Convert.ToDecimal(txt.Text);
                        }
                        else
                        {
                            isSystolicBPExists = false;
                            systolicBP = 0;
                        }
                    }

                    if (hdnVitalSignID.Value == _NBPdVitalSignID)
                    {
                        if ((!String.IsNullOrEmpty(txt.Text)))
                        {
                            isDiastolicBPExists = true;
                            diastolicBP = Convert.ToDecimal(txt.Text);
                        }
                        else
                        {
                            isDiastolicBPExists = false;
                            diastolicBP = 0;
                        }
                    }

                    if (hdnVitalSignID.Value == _RRVitalSignID)
                    {
                        if ((!String.IsNullOrEmpty(txt.Text)))
                        {
                            isRRExists = true;
                            rr = Convert.ToDecimal(txt.Text);
                        }
                        else
                        {
                            isRRExists = false;
                            rr = 0;
                        }
                    }

                    if (hdnVitalSignID.Value == _HRVitalSignID)
                    {
                        if ((!String.IsNullOrEmpty(txt.Text)))
                        {
                            isHRExists = true;
                            hr = Convert.ToDecimal(txt.Text);
                        }
                        else
                        {
                            isHRExists = false;
                            hr = 0;
                        }
                    }

                    if (hdnVitalSignID.Value == _SPO2VitalSignID)
                    {
                        if ((!String.IsNullOrEmpty(txt.Text)))
                        {
                            isSPO2Exists = true;
                            spo2 = Convert.ToDecimal(txt.Text);
                        }
                        else
                        {
                            isSPO2Exists = false;
                            spo2 = 0;
                        }
                    }

                    if (hdnVitalSignID.Value == _TempVitalSignID)
                    {
                        if ((!String.IsNullOrEmpty(txt.Text)))
                        {
                            isTempExists = true;
                            temp = Convert.ToDecimal(txt.Text.Replace(',','.'));
                        }
                        else
                        {
                            isTempExists = false;
                            temp = 0;
                        }
                    }
                }
                else if (hdnVitalSignType.Value == Constant.ControlType.COMBO_BOX)
                {
                    ASPxComboBox ddl = (ASPxComboBox)item.FindControl("cboVitalSignType");
                    VitalSignDt entityDt = new VitalSignDt();
                    if (ddl.Value != null)
                    {
                        entityDt.VitalSignID = Convert.ToInt32(hdnVitalSignID.Value);
                        entityDt.VitalSignValue = ddl.Value.ToString();

                        if (entityDt.VitalSignValue != "")
                        {
                            lstEntityDt.Add(entityDt);
                        }

                        if (hdnVitalSignID.Value == _AVPUVitalSignID)
                        {
                            isAVPUExists = true;
                            avpu = ddl.Value.ToString();
                        }

                        if (hdnVitalSignID.Value == _GCS_E_VitalSignID)
                        {
                            isGCS_E_Exists = true;
                            gcsE = ddl.Text;
                        }

                        if (hdnVitalSignID.Value == _GCS_M_VitalSignID)
                        {
                            isGCS_M_Exists = true;
                            gcsM = ddl.Text;
                        }

                        if (hdnVitalSignID.Value == _GCS_V_VitalSignID)
                        {
                            isGCS_V_Exists = true;
                            gcsV = ddl.Text;
                        }

                        if (hdnVitalSignID.Value == _PEWS_Behavior_VitalSignID)
                        {
                            isPEWS_Behavior_Exists = true;
                            pewsBehavior = ddl.Value.ToString();
                        }

                        if (hdnVitalSignID.Value == _PEWS_Cardiovascular_VitalSignID)
                        {
                            isPEWS_Cardio_Exists = true;
                            pewsCardio = ddl.Value.ToString();
                        }

                        if (hdnVitalSignID.Value == _PEWS_Respiration_VitalSignID)
                        {
                            isPEWS_Respiratory_Exists = true;
                            pewsRespiratory = ddl.Value.ToString();
                        }
                    }
                }
            }

            if ((isRRExists && isSPO2Exists && isTempExists && isSystolicBPExists && isHRExists && isAVPUExists) || (isPEWS_Behavior_Exists && isPEWS_Cardio_Exists && isPEWS_Respiratory_Exists))
            {
                decimal ews = 0;
                bool isEWS = true;
                int ageInYear = Function.GetPatientAgeInYear(AppSession.RegisteredPatient.DateOfBirth, AppSession.RegisteredPatient.VisitDate);
                isEWS = ageInYear > 16;
                if (isEWS)
                    ews = Methods.CalculateEWSScore(rr, spo2, temp, systolicBP, hr, avpu);
                else
                    ews = Methods.CalculatePEWSScore(pewsBehavior, pewsCardio, pewsRespiratory);

                VitalSignDt entityDt = new VitalSignDt();
                if (isEWS)
                    entityDt.VitalSignID = Convert.ToInt32(_EWSVitalSignID);
                else
                    entityDt.VitalSignID = Convert.ToInt32(_PEWS_VitalSignID);

                entityDt.VitalSignValue = ews.ToString("G29");
                if (entityDt.VitalSignValue != "")
                {
                    lstEntityDt.Add(entityDt);
                }
            }

            if (isHeightExists && isWeightExists)
            {
                decimal bmi = Math.Round(weight / (height * height), 2);
                VitalSignDt entityDt = new VitalSignDt();
                entityDt.VitalSignID = Convert.ToInt32(_BMIVitalSignID);
                entityDt.VitalSignValue = bmi.ToString("G29");
                if (entityDt.VitalSignValue != "")
                {
                    lstEntityDt.Add(entityDt);
                    isHeightExists = isWeightExists = false;
                }
            }

            if (isSystolicBPExists && isDiastolicBPExists)
            {
                decimal map = Math.Round(((2 * diastolicBP) + (1 * systolicBP)) / 3, 2);
                VitalSignDt entityDt = new VitalSignDt();
                entityDt.VitalSignID = Convert.ToInt32(_MAPVitalSignID);
                entityDt.VitalSignValue = map.ToString("G29");
                if (entityDt.VitalSignValue != "")
                {
                    lstEntityDt.Add(entityDt);
                    isSystolicBPExists = isDiastolicBPExists = false;
                }
            }

            if (isGCS_E_Exists || isGCS_M_Exists || isGCS_V_Exists)
            {
                string valueE = string.Empty;
                string valueM = string.Empty;
                string valueV = string.Empty;
                string gcsTotal = string.Empty;
                if (isGCS_E_Exists)
                    valueE = gcsE.Substring(0, 1);
                if (isGCS_M_Exists)
                    valueM = gcsM.Substring(0, 1);
                if (isGCS_V_Exists)
                    valueV = gcsV.Substring(0, 1);

                if (isGCS_E_Exists && isGCS_M_Exists && isGCS_V_Exists)
                {
                    gcsTotal = (Convert.ToInt16(valueE) + Convert.ToInt16(valueM) + Convert.ToInt16(valueV)).ToString();
                }
                else
                {
                    gcsTotal = string.Format("{0}-{1}-{2}", (isGCS_E_Exists ? valueE : "X"), (isGCS_M_Exists ? valueM : "X"), (isGCS_V_Exists ? valueV : "X"));
                }

                VitalSignDt entityDt = new VitalSignDt();
                entityDt.VitalSignID = Convert.ToInt32(_GCSTotalEMVVitalSignID);
                entityDt.VitalSignValue = gcsTotal;
                if (entityDt.VitalSignValue != "")
                {
                    lstEntityDt.Add(entityDt);
                }
            }
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            VitalSignHdDao entityDao = new VitalSignHdDao(ctx);
            VitalSignDtDao entityDtDao = new VitalSignDtDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                VitalSignHd entity = new VitalSignHd();
                List<VitalSignDt> lstEntityDt = new List<VitalSignDt>();
                ControlToEntity(entity, lstEntityDt);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);

                foreach (VitalSignDt entityDt in lstEntityDt)
                {
                    entityDt.ID = entity.ID;
                    entityDtDao.Insert(entityDt);
                }

                Registration oRegistration = regDao.Get(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    oRegistration.IsFallRisk = chkIsFallRisk.Checked;
                    oRegistration.IsDNR = chkIsDNR.Checked;
                    oRegistration.IsRAPUH = chkIsRAPUH.Checked;
                    oRegistration.IsTerminalPatient = chkIsTerminalPatient.Checked;
                    regDao.Update(oRegistration);
                }
                ctx.CommitTransaction();

                if (AppSession.SA0137 == "1")
                {
                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                    {
                        BridgingToMedinfrasV1();
                    }
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            VitalSignHdDao entityDao = new VitalSignHdDao(ctx);
            VitalSignDtDao entityDtDao = new VitalSignDtDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                VitalSignHd entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                List<VitalSignDt> lstEntityDt = BusinessLayer.GetVitalSignDtList(string.Format("ID = {0}", hdnID.Value), ctx);
                List<VitalSignDt> lstNewEntityDt = new List<VitalSignDt>();

                ControlToEntity(entity, lstNewEntityDt);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);

                foreach (VitalSignDt entityDt in lstNewEntityDt)
                {
                    VitalSignDt obj = lstEntityDt.FirstOrDefault(p => p.VitalSignID == entityDt.VitalSignID);
                    entityDt.ID = entity.ID;
                    if (obj == null)
                    {
                        entityDtDao.Insert(entityDt);
                    }
                    else
                    {
                        entityDtDao.Update(entityDt);
                    }
                }

                Registration oRegistration = regDao.Get(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    oRegistration.IsFallRisk = chkIsFallRisk.Checked;
                    oRegistration.IsDNR = chkIsDNR.Checked;
                    oRegistration.IsRAPUH = chkIsRAPUH.Checked;
                    oRegistration.IsTerminalPatient = chkIsTerminalPatient.Checked;
                    regDao.Update(oRegistration);
                }

                ctx.CommitTransaction();

                if (AppSession.SA0137 == "1")
                {
                    if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                    {
                        BridgingToMedinfrasV1();
                    }
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            List<vSpecialtyVitalSign> lst = BusinessLayer.GetvSpecialtyVitalSignList(string.Format("SpecialtyID ='{0}' AND IsAutoGenerated = 0 AND IsDeleted = 0", AppSession.RegisteredPatient.SpecialtyID)).OrderBy(p => p.DisplayOrder).ToList();

            rptVitalSign.DataSource = lst;
            rptVitalSign.DataBind();
        }

        protected void rptVitalSign_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vSpecialtyVitalSign obj = (vSpecialtyVitalSign)e.Item.DataItem;
                HtmlGenericControl div = null;

                if (obj.GCValueType == Constant.ControlType.TEXT_BOX)
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divTxt");
                    TextBox txt = (TextBox)e.Item.FindControl("txtVitalSignType");
                    if (obj.IsNumericValue)
                    {
                        txt.Attributes.Add("validationgroup", "mpEntryPopup");
                        txt.CssClass = "number";
                    }
                    //ctl = (TextBox)e.Item.FindControl("txtVitalSignType");
                }
                else if (obj.GCValueType == Constant.ControlType.COMBO_BOX)
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divDdl");
                    ASPxComboBox ddl = (ASPxComboBox)e.Item.FindControl("cboVitalSignType");
                    List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", obj.GCValueCode));
                    lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
                    Methods.SetComboBoxField<StandardCode>(ddl, lstSc, "StandardCodeName", "StandardCodeID");
                }

                if (div != null)
                    div.Visible = true;

            }
        }

        private void BridgingToMedinfrasV1()
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = string.Empty;
            serviceResult = oService.OnSendPatientVitalSignInfo(AppSession.RegisteredPatient.RegistrationNo);
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
    }
}