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
    public partial class ConsentFormCtl1 : BaseEntryPopupCtl4
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnConsentFormGroup.Value = paramInfo[0];
            if (paramInfo.Length > 2)
            {
                hdnExistingSignature.Value = paramInfo[2];
            }
            if (paramInfo[1] != "" && paramInfo[1] != "0")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[1];
                Helper.SetControlEntrySetting(cboGCConsentFormGroup, new ControlEntrySetting(true, false, true), "mpEntryPopup");
                ConsentForm entity = BusinessLayer.GetConsentForm(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
            }
            cboGCConsentFormGroup.Value = hdnConsentFormGroup.Value;
            cboGCConsentFormGroup.Enabled = false;
            LoadConsentFormType(cboGCConsentFormGroup.Value.ToString());
            cboGCConsentFormType.Enabled = IsAdd;
            if (cboGCConsentFormGroup.Value != null)
            {
                hdnConsentFormGroup.Value = cboGCConsentFormGroup.Value.ToString();
            }
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboGCConsentFormType, new ControlEntrySetting(true, true, true));


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

            string filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_CONSENT_FORM_GROUP, Constant.StandardCode.FAMILY_RELATION);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboGCConsentFormGroup, lstSc.Where(p => p.ParentID == Constant.StandardCode.PATIENT_CONSENT_FORM_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboFamilyRelation, lstSc.Where(p => p.ParentID == Constant.StandardCode.FAMILY_RELATION).ToList(), "StandardCodeName", "StandardCodeID");
        }

        private void EntityToControl(ConsentForm entity)
        {
            txtObservationDate.Text = entity.ConsentFormDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.ConsentFormTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            rblIsPatientFamily.SelectedValue = entity.IsPatientFamily ? "1" : "0";
            if (entity.IsPatientFamily)
                trFamilyInfo.Style.Add("display", "table-row");
            else
                trFamilyInfo.Style.Add("display", "none");
            cboFamilyRelation.Value = entity.GCFamilyRelation;
            txtSignature2Name.Text = entity.SignatureName2;

            cboGCConsentFormType.Value = entity.GCConsentFormType;
            divFormContent.InnerHtml = entity.ConsentFormLayout;
            hdnDivFormLayout.Value = entity.ConsentFormLayout;
            hdnFormValues.Value = entity.ConsentFormValue;

            if (hdnExistingSignature.Value == "True")
            {
                rblIsPatientFamily.Enabled = false;
                txtSignature2Name.Enabled = false;
                cboFamilyRelation.ClientEnabled = false;
            }
            txtSignature3Name.Text = entity.SignatureName3;
            txtSignature4Name.Text = entity.SignatureName4;
        }

        private void ControlToEntity(ConsentForm entity)
        {
            entity.ConsentFormDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ConsentFormTime = txtObservationTime.Text;
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

            entity.GCConsentFormGroup = hdnConsentFormGroup.Value;
            entity.GCConsentFormType = cboGCConsentFormType.Value.ToString();
            entity.ConsentFormLayout = hdnDivFormLayout.Value;
            entity.ConsentFormValue = hdnFormValues.Value;

            entity.SignatureName3 = txtSignature3Name.Text;
            entity.SignatureName4 = txtSignature4Name.Text;

            entity.IsReadAndUnderstand = rblIsReadAndUnderstand.SelectedValue;
            entity.IsApproveOrReject = rblIsApproveOrReject.SelectedValue;

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
            ConsentFormDao entityDao = new ConsentFormDao(ctx);
            try
            {
                if (IsValid(ref errMessage))
                {
                    ConsentForm entity = new ConsentForm();

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
            ConsentFormDao entityDao = new ConsentFormDao(ctx);
            try
            {
                if (IsValid(ref errMessage))
                {
                    ConsentForm entity = entityDao.Get(Convert.ToInt32(hdnID.Value));

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
        }

        protected void cbpFormConsentType_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                LoadConsentFormType(cboGCConsentFormGroup.Value.ToString());
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpFormLayout"] = result;
        }

        private void LoadConsentFormType(string param)
        {
            string filterExpression = string.Format("ParentID IN ('{0}') AND TagProperty = '{1}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_CONSENT_FORM_TYPE, cboGCConsentFormGroup.Value.ToString());
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboGCConsentFormType, lstSc, "StandardCodeName", "StandardCodeID");

            cboGCConsentFormGroup.Value = hdnConsentFormGroup.Value;
        }

        protected void cbpConsentFormContent_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

                string fileName = string.Format(@"{0}\medicalForm\ConsentForm\{1}.html", filePath, cboGCConsentFormType.Value.ToString().Replace('^', '_'));
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

            StringBuilder sbMessage = new StringBuilder();

            if (cboGCConsentFormType.Value == null)
            {
                sbMessage.AppendLine("Harap isi Jenis Form Consent terlebih Dahulu");
            }

            if (rblIsPatientFamily.SelectedValue == "1")
            {
                if (string.IsNullOrEmpty(txtSignature2Name.Text))
                {
                    if (cboFamilyRelation.Value == null)
                    {
                        sbMessage.AppendLine("Harap Isi Nama Penerima dan Hubungan nya Terlebih Dahulu");
                    }
                    else
                    {
                        sbMessage.AppendLine("Harap Isi Nama Penerima Terlebih Dahulu");
                    }
                }
                else
                {
                    if (cboFamilyRelation.Value == null)
                    {
                        sbMessage.AppendLine("Harap Isi Hubungan nya Terlebih Dahulu");
                    }
                }
            }

            if (string.IsNullOrEmpty(txtSignature3Name.Text))
            {
                sbMessage.AppendLine("Nama Saksi Pertama (1) tidak boleh kosong");
            }

            if (string.IsNullOrEmpty(txtSignature4Name.Text))
            {
                sbMessage.AppendLine("Nama Saksi Kedua (2) tidak boleh kosong");
            }

            if (string.IsNullOrEmpty(rblIsReadAndUnderstand.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal Informasi sudah dibaca dan dimengerti tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(rblIsApproveOrReject.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal Persetujuan Informasi/Tindakan tidak boleh kosong atau harus diisi");
            }

            errMessage = sbMessage.ToString().Replace(Environment.NewLine, "<br />");
            isValid = string.IsNullOrEmpty(errMessage) ? true : false;

            return isValid;
        }
    }
}