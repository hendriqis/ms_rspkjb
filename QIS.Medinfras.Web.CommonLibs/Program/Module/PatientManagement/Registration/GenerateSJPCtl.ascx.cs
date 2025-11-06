using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.Service;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class GenerateSJPCtl : BaseViewPopupCtl
    {

        protected string GetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }

        public override void InitializeDataControl(string param)
        {
            string[] parameter = param.Split('|');
            string registrationID = parameter[0];
            string departmentID = parameter[1];
            string inhealthParticipantNo = parameter[2];
            hdnRegistrationID.Value = parameter[0];
            hdnDepartmentID.Value = parameter[1];
            txtNoKartuInh.Text = parameter[2];

            hdnUsernameLogin.Value = AppSession.UserLogin.UserFullName;

            hdnTokenInhealth.Value = AppSession.Inheatlh_Access_Token;
            hdnKodeProviderInhealth.Value = AppSession.Inhealth_Provider_Code;
            hdnUsername.Value = AppSession.UserLogin.UserName;
            GetSettingParameter();
            if (registrationID != "")
            {
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", registrationID))[0];
                RegistrationInhealth  entityRegInhealth = BusinessLayer.GetRegistrationInhealthList(string.Format("RegistrationID = {0}", registrationID)).FirstOrDefault();
                ServiceUnitMaster entitySUM = BusinessLayer.GetServiceUnitMasterList(string.Format("ServiceUnitID = (SELECT ServiceUnitID FROM vHealthcareServiceUnit WITH(NOLOCK) WHERE HealthcareServiceUnitID = {0})", entity.HealthcareServiceUnitID))[0];
                txtDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                hdnRegistrationNo.Value = entity.RegistrationNo;
                if (entity.GCReferrerGroup == Constant.Referrer.RUMAH_SAKIT)
                    hdnAsalRujukan.Value = "2";
                else
                    hdnAsalRujukan.Value = "1";

                hdnKelasRawat.Value = BusinessLayer.GetClassCare(entity.ClassID).InhealthClassCode;

                hdnIsPoliExecutive.Value = "0";
                if (!string.IsNullOrEmpty(entity.GCClinicGroup))
                {
                    if (entity.GCClinicGroup == Constant.ClinicGroup.CLINIC_GROUP_NON_BPJS)
                    {
                        hdnIsPoliExecutive.Value = "1";
                    }
                }

                //int rowCount = BusinessLayer.GetRegistrationInhealthRowCount(string.Format("RegistrationID = {0} AND NoSJP != ''", registrationID));
                int rowCount = BusinessLayer.GetRegistrationInhealthRowCount(string.Format("RegistrationID = {0}", registrationID));
                hdnID.Value = registrationID;
                hdnMRN.Value = entity.MRN.ToString();
                txtTglAsalRujukan.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTanggalPulang.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                if (rowCount > 0)
                {
                    hdnIsBpjsRegistrationCtl.Value = "1";
                    hdnIsAdd.Value = "0";
                    RegistrationInhealth entityInhealth = BusinessLayer.GetRegistrationInhealthList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
                    EntityToControl(entityInhealth);
                }
                else
                {
                    hdnIsBpjsRegistrationCtl.Value = "0";
                    hdnIsAdd.Value = "1";
                    txtMRN.Text = entity.MedicalNo;
                    departmentID = entity.DepartmentID;

                    txtTglSJP.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtTanggalPulang.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "" });
                lstVariable.Add(new Variable { Code = "1", Value = "Rawat Jalan Tingkat Pertama" });
                lstVariable.Add(new Variable { Code = "2", Value = "Rawat Inap Tingkat Pertama" });
                lstVariable.Add(new Variable { Code = "3", Value = "Rawat Jalan Tingkat Lanjutan" });
                lstVariable.Add(new Variable { Code = "4", Value = "Rawat Inap Tingkat Lanjutan" });
                Methods.SetComboBoxField<Variable>(cboJenisPelayanan, lstVariable, "Value", "Code");

                if (departmentID == "INPATIENT")
                {
                    cboJenisPelayanan.SelectedIndex = 4;
                }
                else if (departmentID == "OUTPATIENT")
                {
                    cboJenisPelayanan.SelectedIndex = 3;
                }
                else
                {
                    cboJenisPelayanan.SelectedIndex = 0;
                }

                List<Variable> lstKasusPelayanan = new List<Variable>();
                lstKasusPelayanan.Add(new Variable { Code = "0", Value = "" });
                lstKasusPelayanan.Add(new Variable { Code = "1", Value = "Biasa" });
                lstKasusPelayanan.Add(new Variable { Code = "2", Value = "Kecelakaan Kerja" });
                lstKasusPelayanan.Add(new Variable { Code = "3", Value = "Kecelakaan Lalu Lintas" });
                Methods.SetComboBoxField<Variable>(cboKasusPelayanan, lstKasusPelayanan, "Value", "Code");
                cboKasusPelayanan.SelectedIndex = 0;

                Patient entityPatient = BusinessLayer.GetPatient(entity.MRN);
                txtNamaPeserta.Text = entityPatient.FullName;
                txtMRN.Text = entity.MedicalNo;
                txtGender.Text = entity.Sex;
                hdnPatientMobileNo.Value = entity.MobilePhoneNo1;
                hdnPatientEmail.Value = entity.EmailAddress;

                SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.OP_IS_DEFAULT_SPECIALTY_CATARACT);

                if (!String.IsNullOrEmpty(entitySUM.InhealthPoli))
                {
                    if (entitySUM.InhealthPoli.Contains("|"))
                    {
                        string[] poliInfo = entitySUM.InhealthPoli.Split('|');
                        txtKodePoli.Text = poliInfo[0];
                        txtNamaPoli.Text = poliInfo[1];
                    }
                }
                else
                {
                    txtKodePoli.Text = string.Empty;
                    txtNamaPoli.Text = string.Empty;
                }

                SetControlProperties();
            }
        }

        private void GetSettingParameter()
        {
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}')",
                Constant.SettingParameter.IS_BRIDGING_TO_INHEALTH));
            hdnIsBridgingToInhealth.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_INHEALTH).ParameterValue;
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int RegistrationID = 0;
            string[] registrationID = e.Parameter.Split('|');
            result = registrationID[0] + "|";
            if (registrationID[0].Contains("save"))
            {
                if (hdnID.Value.ToString() != "")
                {
                    RegistrationID = Convert.ToInt32(hdnID.Value);
                    if (hdnIsAdd.Value == "0")
                    {
                        if (OnSaveEditRecord(ref errMessage, RegistrationID))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage, RegistrationID))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            else if (registrationID[0].Contains("delete"))
            {
                RegistrationID = Convert.ToInt32(hdnID.Value);
                if (OnDeleteRecord(ref errMessage, RegistrationID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (registrationID[0].Contains("update_pulang"))
            {
                bool isSuccess = true;
                RegistrationID = Convert.ToInt32(hdnID.Value);
                OnUpdateTanggalPulang(RegistrationID, ref isSuccess, ref errMessage);
                if (isSuccess)
                {
                    result += "success";
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void EntityToControl(RegistrationInhealth entity)
        {
            txtNoSJP.Text = entity.NoSJP;
            txtTglSJP.Text = entity.TanggalSJP.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoAsalRujukan.Text = entity.NoRujukan;
            //txtTglAsalRujukan.Text = entity.TanggalRujukan.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtKodeProviderAsal.Text = entity.KodePPKAsalRujukan;
            txtNamaProviderAsal.Text = entity.NamaPPKAsalRujukan;
            txtKodePoli.Text = entity.KodePoli;
            txtNamaPoli.Text = entity.NamaPoli;
            txtDiagnoseCode.Text = entity.KodeDiagnosa;
            txtDiagnoseName.Text = entity.NamaDiagnosa;
            //txtNoKartuInh.Text = entity.NoKartuPeserta;
            txtNamaPeserta.Text = entity.NamaPeserta;
            txtKodeProduk.Text = entity.KodePlan;
            txtNamaProduk.Text = entity.NamaPlan;
            txtKodeKelasRawat.Text = entity.KodeKelas;
            txtKelasRawat.Text = entity.NamaKelas;
            txtMRN.Text = entity.NoRM;
            if (entity.JenisKelamin == Constant.Gender.MALE)
            {
                txtGender.Text = "Male";
            }
            else
            {
                txtGender.Text = "Female";
            }
            txtDOB.Text = entity.TanggalLahir.ToString(Constant.FormatString.DATE_FORMAT);
            txtKodeBadanUsaha.Text = entity.KodeBU;
            txtBadanUsaha.Text = entity.NamaBU;
            hdnIdAkomodasi.Value = entity.IDAkomodasi;
            txtTipeSJP.Text = entity.NoKartuPesertaBPJS;
            txtKodeProvider.Text = entity.KodeProvider;
            txtNamaProvider.Text = entity.NamaProvider;
            txtKodeProviderBPJS.Text = entity.KodeProviderBPJS;
            txtNamaProviderBPJS.Text = entity.NamaProviderBPJS;
            if (entity.FlagPesertaBPJS == "1")
            {
                chkIsPatientBPJS.Checked = true;
            }
            else
            {
                chkIsPatientBPJS.Checked = false;
            }
            txtProdukCOB.Text = entity.ProdukCOB;
            txtTipeCOB.Text = entity.TipeCOB;
            txtTipeSJP.Text = entity.TipeSJP;
            txtDiagnoseCodeAdditional.Text = entity.KodeDiagnosaTambahan;
            txtDiagnoseNameAdditional.Text = entity.NamaDiagnosaTambahan;
            txtInformasiTambahan.Text = entity.InformasiTambahan;
            txtKelasBPJS.Text = entity.KelasBPJS;
            hdnKodeJenpelRanap.Value = entity.KodeAkomodasi;
            if (!string.IsNullOrEmpty(entity.KodeAkomodasi))
            {
                ItemMaster entityIM = BusinessLayer.GetItemMasterList(string.Format("InhealthKodeJenPelRanap = '{0}'", entity.KodeAkomodasi)).FirstOrDefault();
                if (entityIM != null)
                {
                    txtKodeAkomodasi.Text = entityIM.ItemCode;
                    txtNamaAkomodasi.Text = entityIM.ItemName1;
                }
            }
        }

        #region Control To Entity

        private void ControlToEntity(RegistrationInhealth entity, int registrationId)
        {
            entity.RegistrationID = registrationId;
            entity.NoSJP = txtNoSJP.Text;
            entity.TanggalSJP = Helper.GetDatePickerValue(txtTglSJP.Text);
            entity.JamSJP = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            entity.NoRujukan = txtNoAsalRujukan.Text;
            entity.TanggalRujukan = Helper.GetDatePickerValue(txtTglAsalRujukan.Text);
            entity.KodePPKAsalRujukan = txtKodeProviderAsal.Text;
            entity.NamaPPKAsalRujukan = txtNamaProviderAsal.Text;
            entity.KodePlan = txtKodeProduk.Text;
            entity.NamaPlan = txtNamaProduk.Text;
            entity.KodePoli = txtKodePoli.Text;
            entity.NamaPoli = txtNamaPoli.Text;
            entity.KodeDiagnosa = txtDiagnoseCode.Text;
            entity.NamaDiagnosa = txtDiagnoseName.Text;
            entity.NoKartuPeserta = txtNoKartuInh.Text;
            entity.NamaPeserta = txtNamaPeserta.Text;
            entity.KodePlan = txtKodeProduk.Text;
            entity.NamaPlan = txtNamaProduk.Text;
            entity.KodeKelas = txtKodeKelasRawat.Text;
            entity.NamaKelas = txtKelasRawat.Text;
            entity.NoRM = txtMRN.Text;
            if (txtGender.Text == "Male")
            {
                entity.JenisKelamin = Constant.Gender.MALE;
            }
            else
            {
                entity.JenisKelamin = Constant.Gender.FEMALE;
            }
            entity.TanggalLahir = Helper.GetDatePickerValue(txtDOB.Text);
            entity.KodeBU = txtKodeBadanUsaha.Text;
            entity.NamaBU = txtBadanUsaha.Text;
            entity.IDAkomodasi = hdnIdAkomodasi.Value;
            entity.TipeSJP = txtTipeSJP.Text;
            entity.TipeCOB = txtTipeCOB.Text;
            entity.NoKartuPesertaBPJS = txtNoBPJS.Text;
            entity.KodeProviderBPJS = txtKodeProviderBPJS.Text;
            entity.NamaProviderBPJS = txtNamaProviderBPJS.Text;
            if (chkIsPatientBPJS.Checked)
            {
                entity.FlagPesertaBPJS = "1";
            }
            else
            {
                entity.FlagPesertaBPJS = "0";
            }
            entity.ProdukCOB = txtProdukCOB.Text;
            entity.KodeProvider = txtKodeProvider.Text;
            entity.NamaProvider = txtNamaProvider.Text;
            entity.KodeAkomodasi = hdnKodeJenpelRanap.Value;
            entity.JenisPelayanan = Convert.ToInt32(cboJenisPelayanan.Value.ToString());
            entity.CreatedBy = AppSession.UserLogin.UserID;
            entity.CreatedDate = DateTime.Now;
        }
        #endregion

        private void SetControlProperties()
        {
            txtNoBPJS.Attributes.Add("readonly", "readonly");
            txtNamaProviderAsal.Attributes.Add("readonly", "readonly");
            txtNamaPoli.Attributes.Add("readonly", "readonly");
            txtDiagnoseName.Attributes.Add("readonly", "readonly");
            txtNamaPeserta.Attributes.Add("readonly", "readonly");
            txtNamaProduk.Attributes.Add("readonly", "readonly");
            txtKelasRawat.Attributes.Add("readonly", "readonly");
            txtMRN.Attributes.Add("readonly", "readonly");
            txtDOB.Attributes.Add("readonly", "readonly");
            txtBadanUsaha.Attributes.Add("readonly", "readonly");
            txtTipeSJP.Attributes.Add("readonly", "readonly");
            txtTipeCOB.Attributes.Add("readonly", "readonly");
            txtNamaProvider.Attributes.Add("readonly", "readonly");
            txtNamaProviderBPJS.Attributes.Add("readonly", "readonly");
            txtDiagnoseNameAdditional.Attributes.Add("readonly", "readonly");
            txtProdukCOB.Attributes.Add("readonly", "readonly");
            txtKelasBPJS.Attributes.Add("readonly", "readonly");
            cboJenisPelayanan.Attributes.Add("readonly", "readonly");
            txtNamaAkomodasi.Attributes.Add("readonly", "readonly");
        }

        private bool OnSaveEditRecord(ref string errMessage, int RegistrationID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationInhealthDao entityInhealthDao = new RegistrationInhealthDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            try
            {
                RegistrationInhealth entityInhealth = entityInhealthDao.Get(RegistrationID);
                Patient entityPatient = patientDao.Get(Convert.ToInt32(hdnMRN.Value));

                ControlToEntity(entityInhealth, RegistrationID);
                entityInhealth.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityInhealth.LastUpdatedDate = DateTime.Now;
                entityInhealthDao.Update(entityInhealth);

                Registration regis = BusinessLayer.GetRegistration(RegistrationID);
                if (regis != null)
                {
                    RegistrationDao registrationDao = new RegistrationDao(ctx);
                    regis.LastUpdatedBy = AppSession.UserLogin.UserID;
                    registrationDao.Update(regis);
                }

                ConsultVisit visit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
                if (visit != null)
                {
                    PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
                    PatientDiagnosis diffDx = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}'", visit.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS)).FirstOrDefault();
                    bool flagAdd = false;
                    if (diffDx == null)
                    {
                        diffDx = new PatientDiagnosis();
                        flagAdd = true;
                    }
                    diffDx.VisitID = visit.VisitID;
                    diffDx.ParamedicID = (int)visit.ParamedicID;
                    diffDx.DifferentialDate = DateTime.Now;
                    diffDx.DifferentialTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    diffDx.GCDiagnoseType = Constant.DiagnoseType.EARLY_DIAGNOSIS;
                    diffDx.DiagnoseID = txtDiagnoseCode.Text;
                    diffDx.DiagnosisText = Request.Form[txtDiagnoseName.UniqueID];
                    diffDx.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                    diffDx.CreatedBy = AppSession.UserLogin.UserID;
                    diffDx.CreatedDate = DateTime.Now;
                    if (flagAdd)
                        patientDiagnosisDao.Insert(diffDx);
                    else patientDiagnosisDao.Update(diffDx);

                    ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
                    visitDao.Update(visit);
                }
                entityPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                patientDao.Update(entityPatient);
                ctx.CommitTransaction();
 
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage, int RegistrationID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            InhealthPatientBenefitsDao entityBenefitsDao = new InhealthPatientBenefitsDao(ctx);
            RegistrationInhealthDao entityDao = new RegistrationInhealthDao(ctx);
            try
            {
                Registration entityReg = BusinessLayer.GetRegistration(RegistrationID);
                RegistrationInhealth entity = BusinessLayer.GetRegistrationInhealth(RegistrationID);
                //List<InhealthPatientBenefits> lstBenefits = BusinessLayer.GetInhealthPatientBenefitsList(string.Format("RegistrationID = {0} AND IsDeleted = 0", RegistrationID), ctx);
                //if (lstBenefits.Count > 0)
                //{
                //    foreach (InhealthPatientBenefits benefit in lstBenefits)
                //    {
                //        benefit.IsDeleted = true;
                //        benefit.LastUpdatedBy = AppSession.UserLogin.UserID;
                //        benefit.LastUpdatedDate = DateTime.Now;
                //        entityBenefitsDao.Update(benefit);
                //    }
                //}

                entity.NoSJP = string.Empty;
                entity.TanggalSJP = entityReg.RegistrationDate;
                entity.JamSJP = string.Empty;
                entity.NoRujukan = string.Empty;
                entity.TanggalRujukan = DateTime.Now;
                entity.KodeAkomodasi = string.Empty;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                entityDao.Update(entity);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnSaveAddRecord(ref string errMessage, int RegistrationID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationInhealthDao entityInhealthDao = new RegistrationInhealthDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            InhealthPatientBenefitsDao benefitsDao = new InhealthPatientBenefitsDao(ctx);
            try
            {
                RegistrationInhealth entityRegInhealth = new RegistrationInhealth();
                InhealthPatientBenefits entityBenefits = new InhealthPatientBenefits();
                Patient entityPatient = patientDao.Get(Convert.ToInt32(hdnMRN.Value));

                #region Get Info Benefits
                //InhealthService oService = new InhealthService();
                //APIMessageLog entityAPILog = new APIMessageLog();
                //string apiResult = oService.InfoBenefit(hdnTokenInhealth.Value, hdnKodeProviderInhealth.Value, txtNoKartuInh.Text, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2), AppSession.UserLogin.UserName);
                //string[] apiResultInfo = apiResult.Split('|');
                //if (apiResultInfo[0] == "0")
                //{
                //    entityAPILog.IsSuccess = false;
                //    entityAPILog.MessageText = apiResultInfo[1];
                //    entityAPILog.Response = apiResultInfo[1];
                //    Exception ex = new Exception(apiResultInfo[1]);
                //    Helper.InsertErrorLog(ex);
                //}
                //else
                //{
                //    entityAPILog.MessageText = apiResultInfo[0];
                //    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                //    List<ModelInfoBenefit> resultInfoTindakan = JsonConvert.DeserializeObject<List<ModelInfoBenefit>>(apiResultInfo[1]);
                //    if (resultInfoTindakan.Count > 0)
                //    {
                //        List<InhealthPatientBenefits> lstBenefits = BusinessLayer.GetInhealthPatientBenefitsList(string.Format("RegistrationID = {0} AND IsDeleted = 0", hdnRegistrationID.Value), ctx);
                //        if (lstBenefits.Count == 0)
                //        {
                //            foreach (ModelInfoBenefit info in resultInfoTindakan)
                //            {
                //                entityBenefits.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                //                entityBenefits.Benefit = info.BENEFIT;
                //                entityBenefits.BenefitDescription = info.BENEFITDESC;
                //                entityBenefits.IsDeleted = false;
                //                entityBenefits.CreatedBy = AppSession.UserLogin.UserID;
                //                entityBenefits.CreatedDate = DateTime.Now;
                //                benefitsDao.Insert(entityBenefits);
                //            }
                //        }
                //    }
                //}
                #endregion

                if (hdnErrorCode.Value == "00")
                {
                    RegistrationInhealth entityInhealth = BusinessLayer.GetRegistrationInhealthList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
                    if (entityInhealth == null)
                    {
                        ControlToEntity(entityRegInhealth, RegistrationID);
                        entityInhealthDao.Insert(entityRegInhealth);
                    }
                    else
                    {
                        ControlToEntity(entityInhealth, RegistrationID);
                        entityInhealth.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityInhealth.LastUpdatedDate = DateTime.Now;
                        entityInhealthDao.Update(entityInhealth);
                    }
                }

                Registration regis = BusinessLayer.GetRegistration(RegistrationID);
                if (regis != null)
                {
                    RegistrationDao registrationDao = new RegistrationDao(ctx);
                    regis.LastUpdatedBy = AppSession.UserLogin.UserID;
                    registrationDao.Update(regis);
                }

                ConsultVisit visit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
                if (visit != null)
                {
                    PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
                    PatientDiagnosis diffDx = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}'", visit.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS)).FirstOrDefault();
                    bool flagAdd = false;
                    if (diffDx == null)
                    {
                        diffDx = new PatientDiagnosis();
                        flagAdd = true;
                    }
                    diffDx.VisitID = visit.VisitID;
                    diffDx.ParamedicID = (int)visit.ParamedicID;
                    diffDx.DifferentialDate = DateTime.Now;
                    diffDx.DifferentialTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    diffDx.GCDiagnoseType = Constant.DiagnoseType.EARLY_DIAGNOSIS;
                    diffDx.DiagnoseID = txtDiagnoseCode.Text;
                    diffDx.DiagnosisText = Request.Form[txtDiagnoseName.UniqueID];
                    diffDx.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                    diffDx.CreatedBy = AppSession.UserLogin.UserID;
                    diffDx.CreatedDate = DateTime.Now;
                    if (flagAdd)
                        patientDiagnosisDao.Insert(diffDx);
                    else patientDiagnosisDao.Update(diffDx);

                    ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
                    visitDao.Update(visit);
                }
                entityPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                patientDao.Update(entityPatient);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnUpdatePrintNumber(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao entityDao = new RegistrationBPJSDao(ctx);
            try
            {
                RegistrationBPJS entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                entity.GCSEPStatus = Constant.SEP_Status.DICETAK;
                entity.PrintNumber += 1;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnProposeSEP(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao entityDao = new RegistrationBPJSDao(ctx);
            try
            {
                RegistrationBPJS entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                entity.GCSEPStatus = Constant.SEP_Status.PENGAJUAN;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void OnUpdateTanggalPulang(int registrationID, ref bool isSuccess, ref string errMessage)
        {
            Registration entityReg = BusinessLayer.GetRegistration(registrationID);
            RegistrationInhealth entity = BusinessLayer.GetRegistrationInhealth(registrationID);
            if (entity != null)
            {
                InhealthService oService = new InhealthService();
                string service = oService.UpdateTanggalPulang_API(entity.IDAkomodasi, entity.NoSJP, entityReg.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2), Helper.GetDatePickerValue(txtTanggalPulang.Text).ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
                string[] serviceInfo = service.Split('|');
                if (serviceInfo[0] == "1")
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    RegistrationInhealthDao entityDao = new RegistrationInhealthDao(ctx);
                    try
                    {
                        entity.TanggalPulang = Helper.GetDatePickerValue(txtTanggalPulang.Text);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.LastUpdatedDate = DateTime.Now;
                        entityDao.Update(entity);

                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        errMessage = ex.Message;
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }
                }
                else
                {
                    isSuccess = false;
                    errMessage = serviceInfo[1];
                }
            }
        }
    }
}