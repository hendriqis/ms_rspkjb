using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class DiagnoseEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.DIAGNOSE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                vDiagnose entity = BusinessLayer.GetvDiagnoseList(string.Format("DiagnoseID = '{0}'", ID)).FirstOrDefault();
                EntityToControl(entity);
                if (entity.DTDNo != null && entity.DTDNo != "")
                {
                    DTD entityDTD = BusinessLayer.GetDTD(entity.DTDNo);
                    txtDTDName.Text = entityDTD.DTDName;
                }
            }
            else
            {
                IsAdd = true;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<SettingParameterDt> lstSetpar = BusinessLayer.GetSettingParameterDtList(string.Format(
                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                            AppSession.UserLogin.HealthcareID, //0
                            Constant.SettingParameter.IS_BPJS_BRIDGING //1
                            ));

            hdnIsBridgingBPJS.Value = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.IS_BPJS_BRIDGING).FirstOrDefault().ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDiagnoseID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtDiagnoseName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDTDNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDTDName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEKlaimDiagnoseCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEKlaimDiagnoseName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtEKlaimDiagnoseINACode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEKlaimDiagnoseINAName, new ControlEntrySetting(false, false, false));
           
            SetControlEntrySetting(txtVKlaimDiagnoseCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtVKlaimDiagnoseName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtINACBGLabel, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtKeyword, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsDisease, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsInfectious, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsExternalDiagnosis, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsPotentialPRB, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsNutritionDiagnosis, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsSpecialCMG, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vDiagnose entity)
        {
            txtDiagnoseID.Text = entity.DiagnoseID;
            txtDiagnoseName.Text = entity.DiagnoseName;
            txtDTDNo.Text = entity.DTDNo;
            txtEKlaimDiagnoseCode.Text = entity.INACBGLabel;
            txtEKlaimDiagnoseName.Text = entity.INACBGText;
            txtVKlaimDiagnoseCode.Text = entity.BPJSReferenceInfo;
            if (hdnIsBridgingBPJS.Value == "1")
            {
                txtVKlaimDiagnoseName.Text = entity.BPJSReferenceInfoText;
            }
            txtINACBGLabel.Text = entity.INACBGLabel;
            txtEKlaimDiagnoseINACode.Text = entity.INACBGINALabel;
            txtEKlaimDiagnoseINAName.Text = entity.INACBGINAText; 
            txtKeyword.Text = entity.Keyword;
            chkIsChronicDisease.Checked = entity.IsChronicDisease;
            chkIsDisease.Checked = entity.IsDisease;
            chkIsInfectious.Checked = entity.IsInfectious;
            chkIsExternalDiagnosis.Checked = entity.IsExternalDiagnosis;
            chkIsPotentialPRB.Checked = entity.IsPotentialPRB;
            chkIsNutritionDiagnosis.Checked = entity.IsNutritionDiagnosis;
            chkIsSpecialCMG.Checked = entity.IsSpecialCMG; 
        }

        private void ControlToEntity(Diagnose entity)
        {
            entity.DiagnoseID = txtDiagnoseID.Text;
            entity.DiagnoseName = txtDiagnoseName.Text;
            entity.DTDNo = txtDTDNo.Text;
            entity.BPJSReferenceInfo = txtVKlaimDiagnoseCode.Text;
            entity.INACBGLabel = txtEKlaimDiagnoseCode.Text;
            entity.INACBGINALabel = txtEKlaimDiagnoseINACode.Text; 
            entity.Keyword = txtKeyword.Text;
            entity.IsChronicDisease = chkIsChronicDisease.Checked;
            entity.IsDisease = chkIsDisease.Checked;
            entity.IsExternalDiagnosis = chkIsExternalDiagnosis.Checked;
            entity.IsPotentialPRB = chkIsPotentialPRB.Checked;
            entity.IsNutritionDiagnosis = chkIsNutritionDiagnosis.Checked;
            entity.IsSpecialCMG = chkIsSpecialCMG.Checked; 
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("DiagnoseID = '{0}'", txtDiagnoseID.Text);
            List<Diagnose> lst = BusinessLayer.GetDiagnoseList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Diagnose Group with Code " + txtDiagnoseID.Text + " is already exist!";
            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                Diagnose entity = new Diagnose();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertDiagnose(entity);
                retval = entity.DiagnoseID;
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
                Diagnose entity = BusinessLayer.GetDiagnose(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateDiagnose(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}