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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientDiagnoseCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                SetControlProperties();
                PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
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
            txtDifferentialDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDifferentialTime.Text = AppSession.RegisteredPatient.VisitTime;

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            //hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                //hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS, Constant.StandardCode.DIAGNOSIS_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDiagnoseType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDifferentialDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDifferentialTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDiagnoseType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDiagnoseText, new ControlEntrySetting(true, true, true));
            //SetControlEntrySetting(ledMorphology, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(ddlMonth, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(cboStatus, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsFollowUp, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsChronic, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PatientDiagnosis entity)
        {
              txtDifferentialDate.Text = entity.DifferentialDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
              txtDifferentialTime.Text = entity.DifferentialTime;
              cboPhysician.Value = entity.ParamedicID.ToString();
              cboDiagnoseType.Value = entity.GCDiagnoseType;
              cboStatus.Value = entity.GCDifferentialStatus;
              hdnDiagnoseID.Value = entity.DiagnoseID;
              hdnDiagnoseText.Value = entity.DiagnosisText;
              txtDiagnoseText.Text = entity.DiagnosisText;
              hdnMorphologyID.Value = entity.MorphologyID;
              ledDiagnose.Text = entity.DiagnosisText;
              
              chkIsChronic.Checked = entity.IsChronicDisease;
              chkIsFollowUp.Checked = entity.IsFollowUpCase;
        }

        private void ControlToEntity(PatientDiagnosis entity)
        {
            entity.DifferentialDate = Helper.GetDatePickerValue(txtDifferentialDate);
            entity.DifferentialTime = txtDifferentialTime.Text;

            entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
            entity.GCDiagnoseType = cboDiagnoseType.Value.ToString();
            entity.GCDifferentialStatus = cboStatus.Value.ToString();

            entity.DiagnoseID = hdnDiagnoseID.Value;
            if (hdnDiagnoseText.Value != "")
            {
                entity.DiagnosisText = hdnDiagnoseText.Value;
            }
            else
            {
                entity.DiagnosisText = txtDiagnoseText.Text;
            }
            entity.MorphologyID = hdnMorphologyID.Value;
            entity.IsChronicDisease = chkIsChronic.Checked;
            entity.IsFollowUpCase = chkIsFollowUp.Checked;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientDiagnosis entity = new PatientDiagnosis();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                if (IsValid(entity))
                {
                    BusinessLayer.InsertPatientDiagnosis(entity);
                    return true;
                }
                else
                {
                    errMessage = String.Format("Diagnosis {0} Hanya Boleh Ada Satu",cboDiagnoseType.Text.ToString());
                    return false;
                }
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
                if (IsValid(entity))
                {
                    BusinessLayer.UpdatePatientDiagnosis(entity);
                    return true;
                }
                else 
                {
                    errMessage = String.Format("Diagnosis {0} Hanya Boleh Ada Satu", cboDiagnoseType.Text.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool IsValid(PatientDiagnosis entity)
        {
            bool result = true;
            if (entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS)
            {
                //Check if main-diagnosis already exists
                String filterExpression = String.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS);
                if(hdnID.Value != "")
                    filterExpression += String.Format("AND ID != {0}", entity.ID);
                List<PatientDiagnosis> lstDiagnosis = BusinessLayer.GetPatientDiagnosisList(filterExpression);
                if (lstDiagnosis.Count > 0)
                    result = false;
                else
                    result = true;
                return result;
            }
            else if (entity.GCDiagnoseType == Constant.DiagnoseType.EARLY_DIAGNOSIS)
            {
                //Check if main-diagnosis already exists
                String filterExpression = String.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS);
                if (hdnID.Value != "")
                    filterExpression += String.Format("AND ID != {0}", entity.ID);
                List<PatientDiagnosis> lstDiagnosis = BusinessLayer.GetPatientDiagnosisList(filterExpression);
                if (lstDiagnosis.Count > 0)
                    result = false;
                else
                    result = true;
                return result;
            }
            return result;
        }
    }
}