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
    public partial class CopyPatientEducationFormEntry : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                hdnID.Value = paramInfo[0];
                hdnPopupVisitID.Value = paramInfo[1];
                txtObservationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtObservationTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                if (paramInfo[0] != "" && paramInfo[0] != "0")
                {
                    vPatientEducationForm entity = BusinessLayer.GetvPatientEducationFormList(string.Format("EducationFormID = {0}", Convert.ToInt32(hdnID.Value))).FirstOrDefault();
                    EntityToControl(entity);
                }
            }
        }

        private void EntityToControl(vPatientEducationForm entity)
        {
            txtParamedicName.Text = entity.ParamedicName;
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
            txtPatientEducationGroup.Text = entity.EducationFormGroup;
            txtFormEducationType.Text = entity.EducationFormType;
            divFormContent.InnerHtml = entity.EducationFormLayout;
            hdnDivFormLayout.Value = entity.EducationFormLayout;
            hdnFormValues.Value = entity.EducationFormValue;

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

            System.Web.UI.WebControls.Image imgSignature = new System.Web.UI.WebControls.Image();
            imgSignature.Height = 180;
            imgSignature.Width = 400;
            imgSignature.ImageUrl = entity.SignatureID1Type1;
            plSignature1.Controls.Add(imgSignature);

            imgSignature = new System.Web.UI.WebControls.Image();
            imgSignature.Height = 180;
            imgSignature.Width = 400;
            imgSignature.ImageUrl = entity.SignatureID2Type1;
            plSignature2.Controls.Add(imgSignature);

            imgSignature = new System.Web.UI.WebControls.Image();
            imgSignature.Height = 180;
            imgSignature.Width = 400;
            imgSignature.ImageUrl = entity.SignatureID3Type1;
            plSignature3.Controls.Add(imgSignature);

            imgSignature = new System.Web.UI.WebControls.Image();
            imgSignature.Height = 180;
            imgSignature.Width = 400;
            imgSignature.ImageUrl = entity.SignatureID4Type1;
            plSignature4.Controls.Add(imgSignature);
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            if (rblIsPatientFamily.SelectedValue == "1")
            {
                SetControlEntrySetting(txtSignature2Name, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(cboFamilyRelation, new ControlEntrySetting(true, true, true));
            }

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_EDUCATION_TYPE, Constant.StandardCode.EDUCATION_UNDERSTANDING_LEVEL, Constant.StandardCode.EDUCATION_METHOD, Constant.StandardCode.EDUCATION_MATERIAL, Constant.StandardCode.EDUCATION_EVALUATION, Constant.StandardCode.FAMILY_RELATION);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboGCUnderstandingLevel, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_UNDERSTANDING_LEVEL || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCEducationMethod, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_METHOD || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCEducationMaterial, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_MATERIAL || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCEducationEvaluation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_EVALUATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboFamilyRelation, lstSc.Where(p => p.ParentID == Constant.StandardCode.FAMILY_RELATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
        }

        private void ControlToEntity(PatientEducationForm sourceRecord, PatientEducationForm entity)
        {
            entity.EducationFormDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.EducationFormTime = txtObservationTime.Text;
            entity.RegistrationID = sourceRecord.RegistrationID;
            entity.VisitID = sourceRecord.VisitID;
            if (!string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID))
            {
                entity.HealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
            }
            else
            {
                entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            }
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entity.SignatureName1 = txtParamedicName.Text;
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
            entity.GCEducationFormGroup = sourceRecord.GCEducationFormGroup;
            entity.GCEducationFormType = sourceRecord.GCEducationFormType;
            entity.EducationFormLayout = sourceRecord.EducationFormLayout;
            entity.EducationFormValue = sourceRecord.EducationFormValue;
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
            IDbContext ctx = DbFactory.Configure(true);
            PatientEducationFormDao entityDao = new PatientEducationFormDao(ctx);
            try
            {
                if (IsValid(ref errMessage))
                {
                    if (AppSession.UserLogin.ParamedicID != null && AppSession.UserLogin.ParamedicID != 0)
                    {
                        PatientEducationForm sourceRecord = entityDao.Get(Convert.ToInt32(hdnID.Value));
                        if (sourceRecord != null)
                        {
                            PatientEducationForm entity = new PatientEducationForm();
                            ControlToEntity(sourceRecord, entity);
                            entity.CreatedBy = AppSession.UserLogin.UserID;
                            retVal = string.Format("formEntry|{0}", entityDao.InsertReturnPrimaryKeyID(entity).ToString());

                            ctx.CommitTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        retVal = string.Format("formEntry|{0}", "0");
                        errMessage = "Hanya User Tenaga Medis/Dokter yang bisa melakukan Edukasi Pasien";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                retVal = string.Format("formEntry|{0}", "0");
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
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

            return isValid;
        }
    }
}