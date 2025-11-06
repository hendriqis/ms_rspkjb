using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
    public partial class FallRiskAssessmentFormEntry : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                IsAdd = paramInfo[0] == "1";
                SetControlProperties(paramInfo);
                if (IsAdd)
                {
                    PopulateFormContent(); 
                }
            }
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\FallRisk\{1}.html", filePath, hdnFormType.Value.Replace('^', '_'));
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent.InnerHtml = innerHtml.ToString();
            hdnFormLayout.Value = innerHtml.ToString();
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnFormType.Value = paramInfo[1];
            hdnID.Value = paramInfo[2];

            string visitID = AppSession.RegisteredPatient.VisitID.ToString();

            if (paramInfo.Length >= 12)
            {
                visitID = paramInfo[11];
            }

            #region Visit and Patient Information
            hdnPageVisitID.Value = paramInfo[3];
            txtMedicalNo.Text = paramInfo[4];
            txtPatientName.Text = paramInfo[5];
            txtDateOfBirth.Text = paramInfo[6];
            txtRegistrationNo.Text = paramInfo[7];
            #endregion

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2}",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 ORDER BY DisplayOrder", Constant.StandardCode.FALL_RISK_INTERPRETATION));
            
            if (hdnFormType.Value == "X202^05")
            {
                lstStandardCode = lstStandardCode.Where(t => t.StandardCodeID != "X383^04" && t.StandardCodeID != "X383^05").ToList();
            }

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
                chkIsInitialAssessment.Checked = false;
            }
            else
            {
                cboParamedicID.ClientEnabled = false;

                FallRiskAssessment obj = BusinessLayer.GetFallRiskAssessment(Convert.ToInt32(hdnID.Value));
                if (obj != null)
                {
                    txtObservationDate.Text = obj.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtObservationTime.Text = obj.AssessmentTime;
                    cboParamedicID.Value = obj.ParamedicID.ToString();
                    txtTotalScore.Text = obj.FallRiskScore.ToString();
                    cboFallRiskScoreType.Value = obj.GCFallRiskScoreType;
                    chkIsFallRisk.Checked = obj.IsFallRisk;
                    hdnFormLayout.Value = obj.AssessmentFormLayout;
                    hdnFormValues.Value = obj.AssessmentFormValue;
                    divFormContent.InnerHtml = hdnFormLayout.Value;
                    chkIsInitialAssessment.Checked = chkIsInitialAssessment.Checked;                 
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
            IDbContext ctx = DbFactory.Configure(true);
            FallRiskAssessmentDao entityDao = new FallRiskAssessmentDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                FallRiskAssessment entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.AssessmentFormLayout = hdnFormLayout.Value;
                entity.AssessmentFormValue = hdnFormValues.Value;
                entity.IsFallRisk = chkIsFallRisk.Checked;
                entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
                entity.FallRiskScore = Convert.ToInt16(txtTotalScore.Text);
                entity.GCFallRiskScoreType = cboFallRiskScoreType.Value.ToString();
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                string filterFR = string.Format("RegistrationID = {0} AND IsDeleted = 0", entity.RegistrationID);
                FallRiskAssessment lastFR = BusinessLayer.GetFallRiskAssessmentList(filterFR, ctx).LastOrDefault();
                if (lastFR != null)
                {
                    Registration oRegistration = regDao.Get(entity.RegistrationID);
                    if (oRegistration.IsFallRisk != lastFR.IsFallRisk)
                    {
                        oRegistration.IsFallRisk = lastFR.IsFallRisk;
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}