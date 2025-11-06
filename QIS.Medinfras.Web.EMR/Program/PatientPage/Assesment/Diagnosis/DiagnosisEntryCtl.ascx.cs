using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class DiagnosisEntryCtl : BasePagePatientPageEntryCtl
    {
        private bool _isFirstRecord;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "0")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[0];
                SetControlProperties();
                PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                _isFirstRecord = paramInfo[1] == "0";
                IsAdd = true;
                SetControlProperties();
            }
            ledDiagnose.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDiagnosisDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtDiagnosisTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboStatus, new ControlEntrySetting(true, false, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientDiagnosis entity = new PatientDiagnosis();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientDiagnosis(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientDiagnosis(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private void ControlToEntity(PatientDiagnosis entity)
        {
            entity.DifferentialDate = Helper.GetDatePickerValue(txtDiagnosisDate);
            entity.DifferentialTime = txtDiagnosisTime.Text;

            entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
            entity.GCDiagnoseType = cboDiagnoseType.Value.ToString();
            entity.GCDifferentialStatus = cboStatus.Value.ToString();

            entity.DiagnoseID = hdnDiagnoseID.Value;
            entity.DiagnosisText = hdnDiagnoseText.Value;
            entity.MorphologyID = hdnMorphologyID.Value;
            entity.IsChronicDisease = chkIsChronic.Checked;
            entity.IsFollowUpCase = chkIsFollowUp.Checked;
        }

        private void EntityToControl(PatientDiagnosis entity)
        {
            txtDiagnosisDate.Text = entity.DifferentialDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDiagnosisTime.Text = entity.DifferentialTime.ToString();
            cboDiagnoseType.Value = entity.GCDiagnoseType.ToString();
            ledDiagnose.Value = entity.DiagnoseID.ToString();
            hdnDiagnoseText.Value = ledDiagnose.Text;
            if (entity.MorphologyID != null)
            {
                ledMorphology.Value = entity.MorphologyID.ToString(); 
            }
            cboStatus.Value = entity.GCDifferentialStatus;
            chkIsChronic.Checked = entity.IsChronicDisease;
            chkIsFollowUp.Checked = entity.IsFollowUpCase;
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

            String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.DIAGNOSIS_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDiagnoseType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            //ledDiagnose.FilterExpression = filterExpression;

            if (IsAdd)
            {
                if (_isFirstRecord)
		            cboDiagnoseType.Value = Constant.DiagnoseType.MAIN_DIAGNOSIS; 
                else
                    cboDiagnoseType.Value = Constant.DiagnoseType.COMPLICATION;

                cboStatus.Value = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
            }
        }
    }
}