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
    public partial class ChargesEditCtl : BaseEntryPopupCtl
    {
        private bool _isFirstRecord;

        public override void InitializeDataControl(string param)
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                                                        AppSession.UserLogin.HealthcareID,
                                                                                                        Constant.SettingParameter.EMR_ALLOW_CHANGE_CHARGES_QTY
                                                                                                    ));
            hdnIsAllowChangeChargesQty.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EMR_ALLOW_CHANGE_CHARGES_QTY).ParameterValue;

            string[] paramInfo = param.Split('|');
            if (paramInfo[6] != "0")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[6];
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

            if (hdnIsAllowChangeChargesQty.Value == "1")
            {
                txtServiceQty.Enabled = true;
            }
            else
            {
                txtServiceQty.Enabled = false;
            }
        }

        protected override void OnControlEntrySetting()
        {
        }

        private void ControlToEntity(PatientDiagnosis entity)
        {
            //entity.DifferentialDate = Helper.GetDatePickerValue(txtDiagnosisDate);
            //entity.DifferentialTime = txtDiagnosisTime.Text;

            //entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
            //entity.GCDiagnoseType = cboDiagnoseType.Value.ToString();
            //entity.GCDifferentialStatus = cboStatus.Value.ToString();

            //entity.DiagnoseID = hdnDiagnoseID.Value;
            //entity.DiagnosisText = hdnDiagnoseText.Value;
            //entity.MorphologyID = hdnMorphologyID.Value;
            //entity.IsChronicDisease = chkIsChronic.Checked;
            //entity.IsFollowUpCase = chkIsFollowUp.Checked;
        }

        private void EntityToControl(PatientDiagnosis entity)
        {
            //txtDiagnosisDate.Text = entity.DifferentialDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            //txtDiagnosisTime.Text = entity.DifferentialTime.ToString();
            //cboDiagnoseType.Value = entity.GCDiagnoseType.ToString();
            //ledDiagnose.Value = entity.DiagnoseID.ToString();
            //hdnDiagnoseText.Value = ledDiagnose.Text;
            //if (entity.MorphologyID != null)
            //{
            //    ledMorphology.Value = entity.MorphologyID.ToString(); 
            //}
            //cboStatus.Value = entity.GCDifferentialStatus;
            //chkIsChronic.Checked = entity.IsChronicDisease;
            //chkIsFollowUp.Checked = entity.IsFollowUpCase;
        }

        private void SetControlProperties()
        {
            string filterExpression = string.Format("ID = {0}", hdnID.Value);
            List<vPatientChargesDt> oChargesDtList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            if (oChargesDtList.Count > 0)
            {
                vPatientChargesDt oChargesDt = oChargesDtList.FirstOrDefault();
                txtServiceItemCode.Text = oChargesDt.ItemCode;
                txtServiceItemName.Text = oChargesDt.ItemName1;
            }

            //#region Physician Combobox
            //List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            //Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            //cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            //if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            //{
            //    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
            //    cboPhysician.ClientEnabled = false;
            //    cboPhysician.Value = userLoginParamedic.ToString();
            //}

            //cboPhysician.Enabled = false; 
            //#endregion

            //String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.DIAGNOSIS_TYPE);
            //List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            //Methods.SetComboBoxField<StandardCode>(cboStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboDiagnoseType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            ////ledDiagnose.FilterExpression = filterExpression;

            //if (IsAdd)
            //{
            //    if (_isFirstRecord)
            //        cboDiagnoseType.Value = Constant.DiagnoseType.MAIN_DIAGNOSIS; 
            //    else
            //        cboDiagnoseType.Value = Constant.DiagnoseType.COMPLICATION;

            //    cboStatus.Value = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
            //}
        }
    }
}