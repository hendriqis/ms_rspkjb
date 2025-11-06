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
    public partial class CopyEWSAssessmentFormEntry : BasePagePatientPageEntryCtl
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

            vEWSAssessment obj = BusinessLayer.GetvEWSAssessmentList(string.Format("AssessmentID = {0}", hdnID.Value)).FirstOrDefault();
            if (obj != null)
            {
                chkIsInitialAssessment.Checked = obj.IsInitialAssessment;
                hdnFormLayout.Value = obj.AssessmentFormLayout;
                hdnFormValues.Value = obj.AssessmentFormValue;
                divFormContent.InnerHtml = hdnFormLayout.Value;
                chkIsEWSAlert.Checked = obj.IsEWSAlert;
                txtTotalScore.Text = obj.EWSScore.ToString();
                cboEWSScoreType.Value = obj.GCEWSScoreType;
                txtRemarks.Text = obj.Remarks;
            }

            if (paramInfo.Length >= 6)
            {
                txtMedicalNo.Text = paramInfo[2];
                txtPatientName.Text = paramInfo[3];
                txtDateOfBirth.Text = paramInfo[4];
                txtRegistrationNo.Text = paramInfo[5];
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

            //List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.FALL_RISK_INTERPRETATION));
            //Methods.SetComboBoxField<StandardCode>(cboEWSScoreType, lstStandardCode, "StandardCodeName", "StandardCodeID");

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

        private void ControlToEntity(EWSAssessment entity)
        {
            entity.AssessmentDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.AssessmentTime = txtObservationTime.Text;
            entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            EWSAssessmentDao entityDao = new EWSAssessmentDao(ctx);
            VitalSignHdDao vitalSignHdDao = new VitalSignHdDao(ctx);
            VitalSignDtDao vitalSignDtDao = new VitalSignDtDao(ctx);
            try
            {
                int assessmentID = 0;

                EWSAssessment entity = new EWSAssessment();
                ControlToEntity(entity);
                entity.GCEWSAssessmentType = hdnFormType.Value;
                entity.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entity.AssessmentFormLayout = hdnFormLayout.Value;
                entity.AssessmentFormValue = hdnFormValues.Value;
                entity.IsEWSAlert = chkIsEWSAlert.Checked;
                entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
                entity.EWSScore = Convert.ToInt16(txtTotalScore.Text);
                //entity.GCEWSScoreType = cboEWSScoreType.Value.ToString();
                entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entity.Remarks = txtRemarks.Text;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                assessmentID = entityDao.InsertReturnPrimaryKeyID(entity);

                if (!string.IsNullOrEmpty(AppSession.EM0010) && AppSession.EM0010 != "0")
                {
                    VitalSignHd vitalSignHd = new VitalSignHd();

                    vitalSignHd.ObservationDate = Helper.GetDatePickerValue(txtObservationDate);
                    vitalSignHd.ObservationTime = txtObservationTime.Text;
                    vitalSignHd.VisitID = AppSession.RegisteredPatient.VisitID;
                    vitalSignHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    vitalSignHd.Remarks = string.Format("Digenerate otomatis sebagai hasil pengkajian EWS {0}", string.Empty);
                    vitalSignHd.IsInitialAssessment = chkIsInitialAssessment.Checked;
                    vitalSignHd.EWSAssessmentID = assessmentID;
                    vitalSignHd.IsLinkedToAssessment = true;
                    vitalSignHd.CreatedBy = AppSession.UserLogin.UserID;

                    int headerID = vitalSignHdDao.InsertReturnPrimaryKeyID(vitalSignHd);

                    VitalSignDt vitalSignDt = new VitalSignDt();
                    vitalSignDt.ID = headerID;
                    vitalSignDt.VitalSignID = Convert.ToInt32(AppSession.EM0010);
                    vitalSignDt.VitalSignValue = txtTotalScore.Text;
                    vitalSignDt.IsAutoGenerated = true;

                    vitalSignDtDao.Insert(vitalSignDt);
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