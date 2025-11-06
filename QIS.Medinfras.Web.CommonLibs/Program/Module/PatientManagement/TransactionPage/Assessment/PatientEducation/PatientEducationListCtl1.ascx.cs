using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web;
using System.IO;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program.PatientPage
{
    public partial class PatientEducationListCtl1 : BaseEntryPopupCtl4
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnEducationFormGroup.Value = paramInfo[0];
            if (paramInfo.Length > 2)
            {
                hdnExistingSignature.Value = paramInfo[2];
            }
            if (paramInfo[1] != "" && paramInfo[1] != "0")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[1];
                Helper.SetControlEntrySetting(cboGCPatientEducationType, new ControlEntrySetting(true, false, true), "mpEntryPopup");
                PatientEducationForm entity = BusinessLayer.GetPatientEducationForm(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
            }
            cboGCPatientEducationType.Value = hdnEducationFormGroup.Value;
            cboGCPatientEducationType.Enabled = false;
            LoadFormEducationType(cboGCPatientEducationType.Value.ToString());
            cboGCPatientEducationForm.Enabled = IsAdd;
            if (cboGCPatientEducationForm.Value != null)
            {
                hdnPatientEducationForm.Value = cboGCPatientEducationForm.Value.ToString();
            }
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cbpFormEducationType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCUnderstandingLevel, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCEducationMethod, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCEducationMaterial, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCEducationEvaluation, new ControlEntrySetting(true, true, true));

            if (rblIsPatientFamily.SelectedValue == "1")
            {
                SetControlEntrySetting(txtSignature2Name, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(cboFamilyRelation, new ControlEntrySetting(true, true, true));
            }

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

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

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_EDUCATION_TYPE, Constant.StandardCode.EDUCATION_UNDERSTANDING_LEVEL, Constant.StandardCode.EDUCATION_METHOD, Constant.StandardCode.EDUCATION_MATERIAL, Constant.StandardCode.EDUCATION_EVALUATION, Constant.StandardCode.FAMILY_RELATION);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboGCPatientEducationType, lstSc.Where(p => p.ParentID == Constant.StandardCode.PATIENT_EDUCATION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCUnderstandingLevel, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_UNDERSTANDING_LEVEL || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCEducationMethod, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_METHOD || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCEducationMaterial, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_MATERIAL || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCEducationEvaluation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_EVALUATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboFamilyRelation, lstSc.Where(p => p.ParentID == Constant.StandardCode.FAMILY_RELATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
        }

        private void EntityToControl(PatientEducationForm entity)
        {
            txtObservationDate.Text = entity.EducationFormDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.EducationFormTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            rblIsPatientFamily.SelectedValue = entity.IsPatientFamily ? "1" : "0";
            cboGCUnderstandingLevel.Value = entity.GCUnderstandingLevel;
            cboGCEducationMaterial.Value = entity.GCEducationMaterial;
            cboGCEducationMethod.Value = entity.GCEducationMethod;
            cboGCEducationEvaluation.Value = entity.GCEducationEvaluation;
            if (entity.IsPatientFamily)
                trFamilyInfo.Style.Add("display", "table-row");
            else
                trFamilyInfo.Style.Add("display", "none");
            cboFamilyRelation.Value = entity.GCFamilyRelation;
            txtSignature2Name.Text = entity.SignatureName2;
            txtSignature3Name.Text = entity.SignatureName3;
            txtSignature4Name.Text = entity.SignatureName4;

            cboGCPatientEducationForm.Value = entity.GCEducationFormType;
            divFormContent.InnerHtml = entity.EducationFormLayout;
            hdnDivFormLayout.Value = entity.EducationFormLayout;
            hdnFormValues.Value = entity.EducationFormValue;

            if (hdnExistingSignature.Value == "True")
            {
                rblIsPatientFamily.Enabled = false;
                txtSignature2Name.Enabled = false;
                cboFamilyRelation.ClientEnabled = false;
            }
        }

        private void ControlToEntity(PatientEducationForm entity)
        {
            entity.EducationFormDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.EducationFormTime = txtObservationTime.Text;
            entity.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            if (!string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID))
            {
                entity.HealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
            }
            else
            {
                entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            }
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.SignatureName1 = cboParamedicID.Text;
            entity.IsPatientFamily = rblIsPatientFamily.SelectedValue == "1" ? true : false;

            Patient entityPatient = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
            entity.SignatureName2 = rblIsPatientFamily.SelectedValue == "1" ? txtSignature2Name.Text : entityPatient.FullName;
            if (entity.IsPatientFamily)
            {
                if (cboFamilyRelation.Value != null)
                {
                    if (!string.IsNullOrEmpty(cboFamilyRelation.Value.ToString()))
                        entity.GCFamilyRelation = cboFamilyRelation.Value.ToString();
                }
            }
            else
            {
                entity.GCFamilyRelation = null;
            }

            entity.SignatureName3 = txtSignature3Name.Text;
            entity.SignatureName4 = txtSignature4Name.Text;
            entity.GCEducationFormGroup = hdnEducationFormGroup.Value;
            entity.GCEducationFormType = cboGCPatientEducationForm.Value.ToString();
            entity.EducationFormLayout = hdnDivFormLayout.Value;
            entity.EducationFormValue = hdnFormValues.Value;

            if (cboGCUnderstandingLevel.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCUnderstandingLevel.Value.ToString()))
                    entity.GCUnderstandingLevel = cboGCUnderstandingLevel.Value.ToString();
            }
            if (cboGCEducationMethod.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCEducationMethod.Value.ToString()))
                    entity.GCEducationMethod = cboGCEducationMethod.Value.ToString();
            }
            if (cboGCEducationMaterial.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCEducationMaterial.Value.ToString()))
                    entity.GCEducationMaterial = cboGCEducationMaterial.Value.ToString();
            }
            if (cboGCEducationEvaluation.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCEducationEvaluation.Value.ToString()))
                    entity.GCEducationEvaluation = cboGCEducationEvaluation.Value.ToString();
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (AppSession.UserLogin.ParamedicID == null)
            {
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", "Field Tenaga Medis tidak boleh dikosongkan", "User Login saat ini tidak terhubung dengan kode tenaga medis");
                result = false;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PatientEducationFormDao entityDao = new PatientEducationFormDao(ctx);
            try
            {
                if (IsValid(ref errMessage))
                {
                    PatientEducationForm entity = new PatientEducationForm();
                    List<PatientEducationDt> lstEntityDt = new List<PatientEducationDt>();

                    ControlToEntity(entity);
                    entity.VisitID = AppSession.RegisteredPatient.VisitID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    int id = entityDao.InsertReturnPrimaryKeyID(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", ex.Message, ex.Source);
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientEducationFormDao entityDao = new PatientEducationFormDao(ctx);
            try
            {
                if (IsValid(ref errMessage))
                {
                    PatientEducationForm entity = entityDao.Get(Convert.ToInt32(hdnID.Value));

                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", ex.Message, ex.Source);
                result = false;
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
            //List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PATIENT_EDUCATION_TYPE));

            //rptEducationType.DataSource = lst;
            //rptEducationType.DataBind();
        }

        protected void cbpFormEducationType_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                LoadFormEducationType(cboGCPatientEducationType.Value.ToString());
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpFormLayout"] = result;
        }

        private void LoadFormEducationType(string param)
        {
            string filterExpression = string.Format("ParentID IN ('{0}') AND TagProperty = '{1}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.EDUCATION_FORM, cboGCPatientEducationType.Value.ToString());
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboGCPatientEducationForm, lstSc, "StandardCodeName", "StandardCodeID");

            cboGCPatientEducationType.Value = hdnEducationFormGroup.Value;
        }

        protected void cbpAssessmentContent_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

                string fileName = string.Format(@"{0}\medicalForm\education\{1}.html", filePath, cboGCPatientEducationForm.Value.ToString().Replace('^', '_'));
                IEnumerable<string> lstText = File.ReadAllLines(fileName);
                StringBuilder innerHtml = new StringBuilder();
                foreach (string text in lstText)
                {
                    innerHtml.AppendLine(text);
                }

                result = innerHtml.ToString();

                ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                panel.JSProperties["cpFormLayout"] = result;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);

                ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                panel.JSProperties["cpFormLayout"] = string.Empty;
            }
        }

        private bool IsValid(ref string errMessage)
        {
            bool isValid = true;
            if (rblIsPatientFamily.SelectedValue == "1")
            {
                if (string.IsNullOrEmpty(txtSignature2Name.Text))
                {
                    if (cboFamilyRelation.Value == null)
                    {
                        isValid = false;
                        errMessage = "Harap Isi Nama Penerima dan Hubungan nya Terlebih Dahulu";
                    }
                    else
                    {
                        isValid = false;
                        errMessage = "Harap Isi Nama Penerima Terlebih Dahulu";
                    }
                }
                else
                {
                    if (cboFamilyRelation.Value == null)
                    {
                        isValid = false;
                        errMessage = "Harap Isi Hubungan nya Terlebih Dahulu";
                    }
                    else
                    {
                        isValid = true;
                    }
                }
            }

            if (isValid)
            {
                if (cboGCPatientEducationForm.Value == null)
                {
                    isValid = false;
                    errMessage = "Harap Isi Form Edukasi nya Terlebih Dahulu";
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;
        }
    }
}