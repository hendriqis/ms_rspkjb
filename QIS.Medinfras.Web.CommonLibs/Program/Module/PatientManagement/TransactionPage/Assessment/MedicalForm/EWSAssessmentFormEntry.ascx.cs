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
    public partial class EWSAssessmentFormEntry : BasePagePatientPageEntryCtl
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

            string fileName = string.Format(@"{0}\medicalForm\EWS\{1}.html", filePath, hdnFormType.Value.Replace('^', '_'));
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
            if (paramInfo.Length >= 8)
            {
                hdnPageVisitID.Value = paramInfo[3];
                txtMedicalNo.Text = paramInfo[4];
                txtPatientName.Text = paramInfo[5];
                txtDateOfBirth.Text = paramInfo[6];
                txtRegistrationNo.Text = paramInfo[7];
            }
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

            //List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.EWS_INTERPRETATION));

            //if (hdnFormType.Value == "X202^05")
            //{
            //    lstStandardCode = lstStandardCode.Where(t => t.StandardCodeID != "X383^04" && t.StandardCodeID != "X383^05").ToList();
            //}

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
                chkIsInitialAssessment.Checked = false;
            }
            else
            {
                cboParamedicID.ClientEnabled = false;

                EWSAssessment obj = BusinessLayer.GetEWSAssessment(Convert.ToInt32(hdnID.Value));
                if (obj != null)
                {
                    txtObservationDate.Text = obj.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtObservationTime.Text = obj.AssessmentTime;
                    cboParamedicID.Value = obj.ParamedicID.ToString();
                    txtTotalScore.Text = obj.EWSScore.ToString();
                    //cboEWSScoreType.Value = obj.GCEWSScoreType;
                    chkIsEWSAlert.Checked = obj.IsEWSAlert;
                    hdnFormLayout.Value = obj.AssessmentFormLayout;
                    hdnFormValues.Value = obj.AssessmentFormValue;
                    divFormContent.InnerHtml = hdnFormLayout.Value;
                    chkIsInitialAssessment.Checked = chkIsInitialAssessment.Checked;
                    txtRemarks.Text = obj.Remarks;
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
                    vitalSignHd.Remarks = string.Format("Digenerate otomatis sebagai hasil pengkajian EWS {0}",string.Empty);
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
            IDbContext ctx = DbFactory.Configure(true);
            EWSAssessmentDao entityDao = new EWSAssessmentDao(ctx);
            VitalSignHdDao vitalSignHdDao = new VitalSignHdDao(ctx);
            VitalSignDtDao vitalSignDtDao = new VitalSignDtDao(ctx);
            try
            {
                EWSAssessment entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.AssessmentFormLayout = hdnFormLayout.Value;
                entity.AssessmentFormValue = hdnFormValues.Value;
                entity.IsEWSAlert = chkIsEWSAlert.Checked;
                entity.IsInitialAssessment = chkIsInitialAssessment.Checked;
                entity.EWSScore = Convert.ToInt16(txtTotalScore.Text);
                //entity.GCEWSScoreType = cboEWSScoreType.Value.ToString();
                entity.Remarks = txtRemarks.Text;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                if (!string.IsNullOrEmpty(AppSession.EM0010) && AppSession.EM0010 != "0")
                {
                    VitalSignHd vitalSignHd = BusinessLayer.GetVitalSignHdList(string.Format("EWSAssessmentID = {0}", entity.AssessmentID), ctx).FirstOrDefault();
                    bool isNewRecordVitalSign = false;
                    int headerID = 0;

                    if (vitalSignHd == null)
                    {
                        isNewRecordVitalSign = true;
                        vitalSignHd = new VitalSignHd();
                    }
                    else
                    {
                        headerID = vitalSignHd.ID;
                    }

                    vitalSignHd.ObservationDate = Helper.GetDatePickerValue(txtObservationDate);
                    vitalSignHd.ObservationTime = txtObservationTime.Text;
                    vitalSignHd.VisitID = AppSession.RegisteredPatient.VisitID;
                    vitalSignHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    vitalSignHd.Remarks = string.Format("Digenerate otomatis sebagai hasil pengkajian EWS {0}", string.Empty);
                    vitalSignHd.IsInitialAssessment = chkIsInitialAssessment.Checked;
                    vitalSignHd.IsLinkedToAssessment = true;
                    vitalSignHd.EWSAssessmentID = entity.AssessmentID;

                    if (isNewRecordVitalSign)
                    {
                        vitalSignHd.CreatedBy = AppSession.UserLogin.UserID;
                        headerID = vitalSignHdDao.InsertReturnPrimaryKeyID(vitalSignHd);

                        VitalSignDt vitalSignDt = new VitalSignDt();
                        vitalSignDt.ID = headerID;
                        vitalSignDt.VitalSignID = Convert.ToInt32(AppSession.EM0010);
                        vitalSignDt.VitalSignValue = txtTotalScore.Text;
                        vitalSignDt.IsAutoGenerated = true;

                        vitalSignDtDao.Insert(vitalSignDt);
                    }
                    else
                    {
                        vitalSignHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        vitalSignHdDao.Update(vitalSignHd);

                        VitalSignDt vitalSignDt = BusinessLayer.GetVitalSignDtList(string.Format("ID = {0} AND VitalSignID = {1}", headerID, AppSession.EM0010)).FirstOrDefault();
                        if (vitalSignDt != null)
                        {
                            vitalSignDt.VitalSignID = Convert.ToInt32(AppSession.EM0010);
                            vitalSignDt.VitalSignValue = txtTotalScore.Text;
                            vitalSignDt.IsAutoGenerated = true;
                            vitalSignDtDao.Update(vitalSignDt);
                        }
                        else
                        {
                            vitalSignDt = new VitalSignDt();
                            vitalSignDt.ID = headerID;
                            vitalSignDt.VitalSignID = Convert.ToInt32(AppSession.EM0010);
                            vitalSignDt.VitalSignValue = txtTotalScore.Text;
                            vitalSignDt.IsAutoGenerated = true;
                            vitalSignDtDao.Insert(vitalSignDt);
                        }
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