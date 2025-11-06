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
using System.Text;
using System.IO;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CopyFallRiskAssessmentFormEntry : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                IsAdd = true;
                SetControlProperties(paramInfo);
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnFormType.Value = paramInfo[0];
            hdnID.Value = paramInfo[1];
            chkIsInitialAssessment.Checked = paramInfo[5] == "True" ? true : false;
            hdnFormLayout.Value = paramInfo[6];
            hdnFormValues.Value = paramInfo[7];
            divFormContent.InnerHtml = hdnFormLayout.Value;
            chkIsFallRisk.Checked = paramInfo[8] == "1" ? true : false;
            cboFallRiskScoreType.Value = paramInfo[9];

            if (paramInfo.Length >= 15)
            {
                txtMedicalNo.Text = paramInfo[11];
                txtPatientName.Text = paramInfo[12];
                txtDateOfBirth.Text = paramInfo[13];
                txtRegistrationNo.Text = paramInfo[14];
            }

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2}",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.FALL_RISK_INTERPRETATION));
            Methods.SetComboBoxField<StandardCode>(cboFallRiskScoreType, lstStandardCode, "StandardCodeName", "StandardCodeID");

            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                    cboParamedicID.ClientEnabled = false;
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        private void ControlToEntity(FallRiskAssessment entity)
        {
            entity.AssessmentDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.AssessmentTime = txtObservationTime.Text;
            entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FallRiskAssessmentDao entityDao = new FallRiskAssessmentDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                FallRiskAssessment entity = new FallRiskAssessment();
                ControlToEntity(entity);
                entity.GCFallRiskAssessmentType = hdnFormType.Value;
                entity.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entity.AssessmentFormLayout = hdnFormLayout.Value;
                entity.AssessmentFormValue = hdnFormValues.Value;
                entity.IsFallRisk = chkIsFallRisk.Checked;
                entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
                entity.FallRiskScore = Convert.ToInt16(txtTotalScore.Text);
                entity.GCFallRiskScoreType = cboFallRiskScoreType.Value.ToString();
                entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.AssessmentID = entityDao.InsertReturnPrimaryKeyID(entity);

                Registration oRegistration = regDao.Get(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    if (chkIsFallRisk.Checked)
                    {
                        oRegistration.IsFallRisk = true;
                        oRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                        regDao.Update(oRegistration);
                    }
                    else
                    {
                        oRegistration.IsFallRisk = false;
                        oRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                        regDao.Update(oRegistration);
                    }
                }

                ctx.CommitTransaction();
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
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}