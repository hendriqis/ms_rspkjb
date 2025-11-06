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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class GeneralConsentCtl1 : BaseEntryPopupCtl4
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            lblUserName.InnerHtml = AppSession.UserLogin.UserFullName;

            if (paramInfo[0] != "" && paramInfo[0] != "0")
            {
                hdnPopupRegistrationID.Value = paramInfo[0];
                hdnPopupRegistrationNo.Value = paramInfo[1];
                hdnReferenceID.Value = paramInfo[0];
                hdnReferenceIDType.Value = "03";
                hdnSignatureIndex.Value = "3";

                #region Visit and Patient Information
                if (paramInfo.Length >= 6)
                {
                    txtRegistrationNo.Text = paramInfo[1];
                    hdnPopupMRN.Value = paramInfo[2];
                    txtMedicalNo.Text = paramInfo[3];
                    txtPatientName.Text = paramInfo[4];
                    txtDateOfBirth.Text = paramInfo[5];

                    txtSignatureName.Text = paramInfo[4];
                    hdnSignatureName.Value = paramInfo[4];
                }
                #endregion

                vGeneralConsent entity = BusinessLayer.GetvGeneralConsentList(string.Format("RegistrationID = {0}", hdnPopupRegistrationID.Value)).FirstOrDefault();
                IsAdd = entity == null;

                if (entity != null)
                {
                    EntityToControl(entity);

                    if (string.IsNullOrEmpty(rblSATUSEHAT.SelectedValue))
                    {
                        if (!string.IsNullOrEmpty(hdnPopupMRN.Value) && hdnPopupMRN.Value != "0")
	                    {
		                   Patient oPatient = BusinessLayer.GetPatient(Convert.ToInt32(hdnPopupMRN.Value));
                           if (oPatient != null)
                           {
                               if (!string.IsNullOrEmpty(oPatient.PelepasanInformasiSatuSEHAT))
                               {
                                   rblSATUSEHAT.SelectedValue = oPatient.PelepasanInformasiSatuSEHAT;
                               }
                           }
	                    }                        
                    }
                }
            }
            else
            {
                hdnPopupRegistrationID.Value = "0";
                IsAdd = true;
                rblIsPatientFamily.SelectedValue = "0";
                hdnConsentObjectType.Value = "0";
                trFamilyInfo.Style.Add("display", "none");
            }
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtConsentDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtConsentTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            if (rblIsPatientFamily.SelectedValue == "1")
            {
                SetControlEntrySetting(txtSignature2Name, new ControlEntrySetting(true, true, true));
            }
        }

        private void EntityToControl(vGeneralConsent entity)
        {
            txtConsentDate.Text = entity.ConsentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtConsentTime.Text = entity.ConsentTime;
            txtNoteDateTime.Text = string.Format("{0} - {1}", entity.ConsentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), entity.ConsentTime);
            rblIsPatientFamily.SelectedValue = entity.ConsentObjectType;

            if (entity.ConsentObjectType != "0" && entity.ConsentObjectType.Trim() != string.Empty)
            {
                trFamilyInfo.Style.Add("display", "table-row");

                hdnFamilyID.Value = entity.FamilyID.ToString();
                txtSignature2Name.Text = entity.Signature2Name;
                txtRelationship.Text = entity.RelationShip;
            }
            else
            {
                trFamilyInfo.Style.Add("display", "none");
            }

            if (!string.IsNullOrEmpty(entity.Pembayaran))
                rblPembayaran.SelectedValue = entity.Pembayaran;

            if (!string.IsNullOrEmpty(entity.HKP))
                rblHKP.SelectedValue = entity.HKP;

            if (!string.IsNullOrEmpty(entity.TataTertib))
                rblTataTertib.SelectedValue = entity.TataTertib;

            if (!string.IsNullOrEmpty(entity.Penterjemah))
                rblPenterjemah.SelectedValue = entity.Penterjemah;

            if (!string.IsNullOrEmpty(entity.Rohaniawan))
                rblRohaniawan.SelectedValue = entity.Rohaniawan;

            if (!string.IsNullOrEmpty(entity.PelepasanInformasiPenjamin))
                rblPenjamin.SelectedValue = entity.PelepasanInformasiPenjamin;

            if (!string.IsNullOrEmpty(entity.PelepasanInformasiPenelitian))
                rblPenelitian.SelectedValue = entity.PelepasanInformasiPenelitian;

            if (!string.IsNullOrEmpty(entity.PelepasanInformasiKeluarga))
                rblKeluarga.SelectedValue = entity.PelepasanInformasiKeluarga;

            if (!string.IsNullOrEmpty(entity.PelepasanInformasiRujukan))
                rblRujukan.SelectedValue = entity.PelepasanInformasiRujukan;

            if (!string.IsNullOrEmpty(entity.PelepasanInformasiSatuSEHAT))
                rblSATUSEHAT.SelectedValue = entity.PelepasanInformasiSatuSEHAT;

            if (!string.IsNullOrEmpty(entity.Remarks))
                txtRemarks.Text = entity.Remarks;

            //if (hdnExistingSignature.Value == "True")
            //{
            //    rblIsPatientFamily.Enabled = false;
            //    txtSignature2Name.Enabled = false;
            //}

            if (entity.Signature1 == null || entity.Signature1 == "")
            {
                trImage.Style.Add("display", "none");
            }
            else
            {
                trCanvas.Style.Add("display", "none");
                trInfo.Style.Add("display", "none");
                btnAcceptSignature.Style.Add("display", "none");
                btnClearSignature.Style.Add("display", "none");
            }

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            System.Web.UI.WebControls.Image imgSignature = new System.Web.UI.WebControls.Image();
            imgSignature.Height = 180;
            imgSignature.Width = 400;
            imgSignature.ImageUrl = entity.Signature1;
            plImage.Controls.Add(imgSignature);
        }

        private void ControlToEntity(GeneralConsent entity)
        {
            entity.ConsentDate = Helper.GetDatePickerValue(txtConsentDate);
            entity.ConsentTime = txtConsentTime.Text;
            entity.RegistrationID = Convert.ToInt32(hdnPopupRegistrationID.Value);
            entity.ConsentObjectType = rblIsPatientFamily.SelectedValue;
            if (rblIsPatientFamily.SelectedValue == "1")
            {
                if (!string.IsNullOrEmpty(hdnFamilyID.Value) && hdnFamilyID.Value != "0")
                {
                    entity.FamilyID = Convert.ToInt32(hdnFamilyID.Value);
                } 
            }
            entity.Signature1Name = AppSession.UserLogin.UserFullName;
            entity.Signature2Name = Request.Form[txtSignature2Name.UniqueID];
            entity.Pembayaran = rblPembayaran.SelectedValue;
            entity.HKP = rblHKP.SelectedValue;
            entity.TataTertib = rblTataTertib.SelectedValue;
            entity.Penterjemah = rblPenterjemah.SelectedValue;
            entity.Rohaniawan = rblRohaniawan.SelectedValue;
            entity.PelepasanInformasiPenjamin = rblPenjamin.SelectedValue;
            entity.PelepasanInformasiPenelitian = rblPenelitian.SelectedValue;
            entity.PelepasanInformasiKeluarga = rblKeluarga.SelectedValue;
            entity.PelepasanInformasiRujukan = rblRujukan.SelectedValue;
            entity.PelepasanInformasiSatuSEHAT = rblSATUSEHAT.SelectedValue;
            entity.Remarks = txtRemarks.Text;
            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            GeneralConsentDao entityDao = new GeneralConsentDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            try
            {
                if (IsValid(ref errMessage))
                {
                    GeneralConsent entity = new GeneralConsent();

                    ControlToEntity(entity);
                    entityDao.Insert(entity);

                    Patient oPatient = patientDao.Get(Convert.ToInt32(hdnPopupMRN.Value));
                    if (oPatient != null)
                    {
                        if (oPatient.PelepasanInformasiSatuSEHAT != entity.PelepasanInformasiSatuSEHAT)
                        {
                            oPatient.PelepasanInformasiSatuSEHAT = entity.PelepasanInformasiSatuSEHAT;
                            oPatient.SatuSEHATConsentLastUpdate = entity.ConsentDate;
                            patientDao.Update(oPatient);
                        }
                    }

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
            GeneralConsentDao entityDao = new GeneralConsentDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            try
            {
                if (IsValid(ref errMessage))
                {
                    GeneralConsent entity = entityDao.Get(Convert.ToInt32(hdnPopupRegistrationID.Value));

                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityDao.Update(entity);

                    Patient oPatient = patientDao.Get(Convert.ToInt32(hdnPopupMRN.Value));
                    if (oPatient != null)
                    {
                        if (oPatient.PelepasanInformasiSatuSEHAT != entity.PelepasanInformasiSatuSEHAT)
                        {
                            oPatient.PelepasanInformasiSatuSEHAT = entity.PelepasanInformasiSatuSEHAT;
                            oPatient.SatuSEHATConsentLastUpdate = entity.ConsentDate;
                            patientDao.Update(oPatient);
                        }
                    }

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

        private bool IsValid(ref string errMessage)
        {
            StringBuilder sbMessage = new StringBuilder();

            bool isValid = true;
            if (rblIsPatientFamily.SelectedValue != "0")
            {
                if (string.IsNullOrEmpty(Request.Form[txtSignature2Name.UniqueID]))
                {
                    sbMessage.AppendLine("Nama Pemberi Pernyataan tidak boleh kosong atau harus diisi");
                }

                if (string.IsNullOrEmpty(Request.Form[txtRelationship.UniqueID]))
                {
                    sbMessage.AppendLine("Hubungan Nama Pemberi Pernyataan dengan Pasien tidak boleh kosong atau harus diisi");
                }
            }

            if (string.IsNullOrEmpty(rblPembayaran.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal ketentuan Pembayaran tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(rblHKP.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal Penjelasan Hak dan Kewajiban tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(rblTataTertib.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal Penjelasan Tata Tertib Rumah Sakit tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(rblPenterjemah.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal Kebutuhan Penterjemah tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(rblRohaniawan.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal Kebutuhan Rohaniawan tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(rblPenjamin.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal ketentuan pelepasan informasi ke Penjamin Bayar tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(rblPenelitian.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal ketentuan pelepasan informasi untuk Peserta Didik/Penelitian tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(rblKeluarga.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal ketentuan pelepasan informasi untuk Anggota Keluarga tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(rblRujukan.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal ketentuan pelepasan informasi untuk Fasyankes Rujukan tidak boleh kosong atau harus diisi");
            }

            if (string.IsNullOrEmpty(rblSATUSEHAT.SelectedValue))
            {
                sbMessage.AppendLine("Pernyataan soal ketentuan pelepasan informasi untuk Platform SatuSEHAT KEMENKES RI tidak boleh kosong atau harus diisi");
            }

            errMessage = sbMessage.ToString().Replace(Environment.NewLine,"<br />");

            isValid = string.IsNullOrEmpty(errMessage);

            return isValid;
        }

        protected string GetPatientFamilyFilterExp()
        {
            return string.Format("MRN = '{0}' AND IsDeleted = 0", hdnPopupMRN.Value);
        }

        protected string GetEmergencyContactFilterExp()
        {
            return string.Format("RegistrationID = '{0}'", hdnPopupRegistrationID.Value);
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] paramInfo = e.Parameter.Split('|');

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            UpdateConsentFormDigitalSignature(paramInfo[1], hdnSignatureIndex.Value);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string UpdateConsentFormDigitalSignature(string streamData, string signatureIndex)
        {
            string result = string.Empty;

            try
            {
                bool isNewRecord = false;
                string GCSignatureType = Constant.ElectronicSignatureType.CONSENT_FORM;
                ESignature oSignature = BusinessLayer.GetESignatureList(string.Format("ReferenceID = {0} AND GCSignatureType = '{1}'", hdnReferenceID.Value, GCSignatureType)).FirstOrDefault();
                if (oSignature == null)
                {
                    oSignature = new ESignature();
                    isNewRecord = true;

                    oSignature.ReferenceID = Convert.ToInt32(hdnReferenceID.Value);
                    oSignature.GCSignatureType = Constant.ElectronicSignatureType.CONSENT_FORM;
                }

                oSignature.Signature1 = streamData;
                oSignature.Signature1DateTime = DateTime.Now;
                oSignature.Signature1ID = AppSession.UserLogin.ParamedicID;

                if (isNewRecord)
                    BusinessLayer.InsertESignature(oSignature);
                else
                    BusinessLayer.UpdateESignature(oSignature);

                result = string.Format("process|1|{0}|{1}|{2}", "Digital Signature was processed successfully", string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}|{1}|{2}", ex.Message, string.Empty, string.Empty);
            }

            return result;
        }
    }
}