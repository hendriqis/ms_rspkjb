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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PainAssessmentFormEntry : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                IsAdd = paramInfo[0] == "1";
                SetControlProperties(paramInfo);
                PopulateFormContent();
            }
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\Pain\{1}.html", filePath, hdnFormType.Value.Replace('^', '_'));
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent.InnerHtml = innerHtml.ToString();
            hdnDivHTML.Value = innerHtml.ToString();
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnFormType.Value = paramInfo[1];
            hdnID.Value = paramInfo[2];

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1}) AND ParamedicID = {2}",
                Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID, paramedicID));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PAIN_SCALE_INTERPRETATION, Constant.StandardCode.EXACERBATED, Constant.StandardCode.QUALITY, Constant.StandardCode.COURSE_TIMING, Constant.StandardCode.PAIN_REGIO);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboPainScoreType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAIN_SCALE_INTERPRETATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboProvocation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EXACERBATED || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboQuality, lstSc.Where(p => p.ParentID == Constant.StandardCode.QUALITY || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboRegio, lstSc.Where(p => p.ParentID == Constant.StandardCode.PAIN_REGIO || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboTime, lstSc.Where(p => p.ParentID == Constant.StandardCode.COURSE_TIMING || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

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
            else
            {
                cboParamedicID.ClientEnabled = false;

                txtObservationDate.Text = paramInfo[3];
                txtObservationTime.Text = paramInfo[4];
                cboParamedicID.Value = paramInfo[5];
                txtTotalScore.Text = paramInfo[6];
                cboPainScoreType.Value = paramInfo[7];
                chkIsPain.Checked = paramInfo[8] == "1";
                hdnFormValues.Value = paramInfo[9];
                cboProvocation.Value = paramInfo[10].Replace("&nbsp;", "");
                txtProvocation.Text = paramInfo[11].Replace("&nbsp;", "");
                cboQuality.Value = paramInfo[12].Replace("&nbsp;", "");
                txtQuality.Text = paramInfo[13].Replace("&nbsp;", "");
                cboRegio.Value = paramInfo[14].Replace("&nbsp;", "");
                txtRegio.Text = paramInfo[15].Replace("&nbsp;", "");
                cboTime.Value = paramInfo[16].Replace("&nbsp;", "");
                txtTime.Text = paramInfo[17].Replace("&nbsp;", "");
                chkIsInitialAssessment.Checked = paramInfo[18] == "1" ? true : false;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboProvocation, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboQuality, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboRegio, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboTime, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(VitalSignHd entity, List<vVitalSignDt> lstEntityDt)
        {
            txtObservationDate.Text = entity.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.ObservationTime;
            if (AppSession.UserLogin.ParamedicID != null) cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            else cboParamedicID.Value = entity.ParamedicID.ToString();
        }

        private void ControlToEntity(PainAssessment entity)
        {
            entity.AssessmentDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.AssessmentTime = txtObservationTime.Text;
            if (cboProvocation.Value == null)
                entity.GCProvoking = "";
            else
                entity.GCProvoking = cboProvocation.Value.ToString();

            if (cboQuality.Value == null)
                entity.GCQuality = "";
            else
                entity.GCQuality = cboQuality.Value.ToString();

            if (cboRegio.Value == null)
                entity.GCRegio = "";
            else
                entity.GCRegio = cboRegio.Value.ToString();

            if (cboTime.Value == null)
                entity.GCTime = "";
            else
                entity.GCTime = cboTime.Value.ToString();

            entity.Provoking = txtProvocation.Text.TrimEnd();
            entity.Quality = txtQuality.Text.TrimEnd();
            entity.Regio = txtRegio.Text.TrimEnd();
            entity.Time = txtTime.Text.TrimEnd();

            entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PainAssessmentDao entityDao = new PainAssessmentDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                PainAssessment entity = new PainAssessment();
                ControlToEntity(entity);
                entity.GCPainAssessmentType = hdnFormType.Value;
                entity.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                entity.AssessmentFormLayout = hdnDivHTML.Value;
                entity.AssessmentFormValue = hdnFormValues.Value;
                entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
                entity.IsPain = chkIsPain.Checked;
                entity.PainScore = Convert.ToInt16(txtTotalScore.Text);
                entity.GCPainScoreType = cboPainScoreType.Value.ToString();
                entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.AssessmentID = entityDao.InsertReturnPrimaryKeyID(entity);


                if (chkIsPain.Checked)
                {
                    Registration oRegistration = new Registration();
                    oRegistration = regDao.Get(AppSession.RegisteredPatient.RegistrationID);
                    if (oRegistration != null)
                    {
                        oRegistration.IsPain = true;
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
            PainAssessmentDao entityDao = new PainAssessmentDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                PainAssessment entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.AssessmentFormLayout = hdnDivHTML.Value;
                entity.AssessmentFormValue = hdnFormValues.Value;
                entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
                entity.IsPain = chkIsPain.Checked;
                entity.PainScore = Convert.ToInt16(txtTotalScore.Text);
                entity.GCPainScoreType = cboPainScoreType.Value.ToString();
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                if (chkIsPain.Checked)
                {
                    Registration oRegistration = new Registration();
                    oRegistration = regDao.Get(AppSession.RegisteredPatient.RegistrationID);
                    if (oRegistration != null)
                    {
                        oRegistration.IsPain = true;
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