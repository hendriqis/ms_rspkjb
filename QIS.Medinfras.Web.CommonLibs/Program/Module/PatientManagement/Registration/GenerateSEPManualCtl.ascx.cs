using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class GenerateSEPManualCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            List<SettingParameterDt> lstSetpar = BusinessLayer.GetSettingParameterDtList(string.Format(
                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}', '{3}')",
                                        AppSession.UserLogin.HealthcareID, //0
                                        Constant.SettingParameter.IS_BPJS_BRIDGING, //1 
                                        Constant.SettingParameter.IS_BRIDGING_TO_EKLAIM, //2
                                        Constant.SettingParameter.RM_EKLAIM_SEND_EKLAIM_MEDICALNO
                                        ));

            hdnIsBridgingEklaim.Value = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_EKLAIM).FirstOrDefault().ParameterValue;
            hdnIsBridgingVclaim.Value = lstSetpar.Where(p => p.ParameterCode ==     Constant.SettingParameter.IS_BPJS_BRIDGING).FirstOrDefault().ParameterValue;

            hdnIsSendEKlaimMedicalNo.Value = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.RM_EKLAIM_SEND_EKLAIM_MEDICALNO).FirstOrDefault().ParameterValue;

            vRegistration1 entity = BusinessLayer.GetvRegistration1List(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
            hdnRegistrationIDCtl.Value = entity.RegistrationID.ToString();
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtPatient.Text = string.Format("({0}) {1}", entity.MedicalNo, entity.PatientName);
            

            Patient oPatient = BusinessLayer.GetPatient(entity.MRN);
            if (oPatient != null) {
                hdnDOB.Value = oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                hdnGender.Value = oPatient.GCGender;
                hdnMedicalNo.Value = oPatient.MedicalNo;
                hdnEKlaimMedicalNo.Value = oPatient.EKlaimMedicalNo;
                hdnPatientName.Value = oPatient.FullName;
                hdnNoPeserta.Value = oPatient.NHSRegistrationNo;
            }

            InitializeControlProperties();
        }
        private void InitializeControlProperties()
        {
            List<vRegistrationBPJS2> entityBPJSList = BusinessLayer.GetvRegistrationBPJS2List(string.Format("RegistrationID = {0}", hdnRegistrationIDCtl.Value));
            if (entityBPJSList.Count() > 0)
            {
                IsAdd = false;
                vRegistrationBPJS2 entityBPJS = entityBPJSList.FirstOrDefault();
                txtNoSEP.Text = entityBPJS.NoSEP;
                hdnoldNoSep.Value = entityBPJS.NoSEP;

                if (entityBPJS.TanggalSEP.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                {
                    txtTglSEP.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }
                else
                {
                    txtTglSEP.Text = entityBPJS.TanggalSEP.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }

                if (entityBPJS.JamSEP == "" || entityBPJS.JamSEP == null)
                {
                    txtJamSEP.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }
                else
                {
                    txtJamSEP.Text = entityBPJS.JamSEP;
                }
            }
            else
            {
                IsAdd = true;
                txtTglSEP.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtJamSEP.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNoSEP, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTglSEP, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtJamSEP, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao entityRegistrationBPJSDao = new RegistrationBPJSDao(ctx);
            try
            {
                if (hdnIsBridgingVclaim.Value == "1")
                {
                    BPJSService oBpjs = new BPJSService();
                    string resp = oBpjs.FindSEPInfo_MEDINFRASAPI(txtNoSEP.Text).ToString();
                    string[] data = resp.Split('|');

                    if (data[0] == "1")
                    {
                        FindSepApiResponse oData = JsonConvert.DeserializeObject<FindSepApiResponse>(data[1]);
                        if (oData != null)
                        {
                            if (oData.response.peserta.noKartu == hdnNoPeserta.Value)
                            {
                                RegistrationBPJS newRegBPJS = new RegistrationBPJS();
                                newRegBPJS.RegistrationID = Convert.ToInt32(hdnRegistrationIDCtl.Value);
                                newRegBPJS.NoSEP = txtNoSEP.Text;
                                newRegBPJS.TanggalSEP = Helper.GetDatePickerValue(txtTglSEP.Text);
                                newRegBPJS.JamSEP = txtJamSEP.Text;
                                newRegBPJS.IsManualSEP = true;
                                newRegBPJS.CreatedBy = AppSession.UserLogin.UserID;
                                newRegBPJS.Catatan = oData.response.catatan;
                                newRegBPJS.NamaDiagnosa = oData.response.diagnosa;
                                newRegBPJS.KelasTanggungan = oData.response.klsRawat.klsRawatHak;
                                newRegBPJS.NamaKelasTanggungan = string.Format("{0} - {1}", oData.response.klsRawat.klsRawatHak, oData.response.peserta.hakKelas);
                                entityRegistrationBPJSDao.Insert(newRegBPJS);
                                ctx.CommitTransaction();

                                #region E-Klaim

                                if (hdnIsBridgingEklaim.Value == "1")
                                {
                                    string errMessageEklaim = string.Empty;
                                    string respEklaim = string.Empty;
                                    onNewClaim(ref errMessageEklaim, ref respEklaim);
                                    if (!string.IsNullOrEmpty(respEklaim))
                                    {
                                        NewClaimResponse respInfo = JsonConvert.DeserializeObject<NewClaimResponse>(respEklaim);
                                    }
                                }

                                #endregion

                            }
                            else 
                            {
                                errMessage = "Maaf, Nomor SEP tidak sesuai dengan nomor kartu peserta.";
                                result = false;
                            }
                          
                        }
                    }
                }
                else {
                    RegistrationBPJS newRegBPJS = new RegistrationBPJS();
                    newRegBPJS.RegistrationID = Convert.ToInt32(hdnRegistrationIDCtl.Value);
                    newRegBPJS.NoSEP = txtNoSEP.Text;
                    newRegBPJS.TanggalSEP = Helper.GetDatePickerValue(txtTglSEP.Text);
                    newRegBPJS.JamSEP = txtJamSEP.Text;
                    newRegBPJS.IsManualSEP = true;
                    newRegBPJS.CreatedBy = AppSession.UserLogin.UserID;
                    entityRegistrationBPJSDao.Insert(newRegBPJS);
                    ctx.CommitTransaction();
                }
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            if (hdnIsBridgingVclaim.Value == "1")
            {
                BPJSService oBpjs = new BPJSService();
                string resp = oBpjs.FindSEPInfo_MEDINFRASAPI(txtNoSEP.Text).ToString();
                string[] data = resp.Split('|');
              
                if (data[0] == "1")
                {
                    FindSepApiResponse oData = JsonConvert.DeserializeObject<FindSepApiResponse>(data[1]);
                    if (oData != null)
                    {
                        if (oData.response.peserta.noKartu == hdnNoPeserta.Value)
                        {
                            onSaveEditData(oData, ref errMessage, ref retval, ref result);
                            if (result) {
                                if (hdnIsBridgingEklaim.Value == "1") 
                                {
                                    string errMessageEklaim = string.Empty;
                                    string respEklaim = string.Empty;
                                    onNewClaim(ref errMessageEklaim, ref respEklaim);
                                    if (!string.IsNullOrEmpty(respEklaim))
                                    {
                                        string respEklaimDeleteResponse = string.Empty;
                                        string errMessageDeleteResponse = string.Empty;

                                        NewClaimResponse respInfo = JsonConvert.DeserializeObject<NewClaimResponse>(respEklaim);
                                        if (respInfo.metadata.code == "200")
                                        {
                                            onDeleteClaim(ref  errMessageDeleteResponse, ref   respEklaimDeleteResponse); 

                                        }
                                        
                                    }
                                }
                            }
                        }
                        else
                        {
                            errMessage = "Maaf, Nomor SEP tidak sesuai dengan nomor kartu peserta.";
                            result = false;
                        }
                    }
                }
                else
                {
                    errMessage = "Maaf, nomor sep tidak ditemukan.";
                    result = false;
                }
            }
            else {
                onSaveEditData(null, ref errMessage, ref retval, ref result);
            }
           
            return result;
        }

        private void onSaveEditData(FindSepApiResponse oData, ref string errMessage, ref string retval, ref bool result)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            RegistrationBPJSDao entityRegistrationBPJSDao = new RegistrationBPJSDao(ctx);
            try
            {
                RegistrationBPJS regBPJS = entityRegistrationBPJSDao.Get(Convert.ToInt32(hdnRegistrationIDCtl.Value));
                if (regBPJS != null)
                {
                    regBPJS.NoSEP = txtNoSEP.Text;
                    regBPJS.TanggalSEP = Helper.GetDatePickerValue(txtTglSEP.Text);
                    regBPJS.JamSEP = txtJamSEP.Text;
                    regBPJS.IsManualSEP = true;
                    regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;

                    if (oData != null)
                    {
                        regBPJS.KodeDPJPKonsulan = oData.response.dpjp.kdDPJP;
                        regBPJS.NamaDPJPKonsulan = oData.response.dpjp.nmDPJP;
                        regBPJS.KelasTanggungan = oData.response.klsRawat.klsRawatHak;
                        regBPJS.NamaKelasTanggungan = string.Format("{0} - {1}", oData.response.klsRawat.klsRawatHak, oData.response.peserta.hakKelas);
                    }

                    entityRegistrationBPJSDao.Update(regBPJS);
                    ctx.CommitTransaction();
                    result = true;
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, data tidak ditemukan.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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
        }

        private bool onNewClaim(ref string errMessage, ref string respEklaim)
        {
            bool result = true;

            try
            {
                string gender = hdnGender.Value == "0003^M" ? "1" : "2";
                string nama_pasien = hdnPatientName.Value;
                string nomor_kartu = hdnNoPeserta.Value;
                string nomor_sep = txtNoSEP.Text;
                string tgl_lahir = hdnDOB.Value;
                
                string nomor_rm = hdnIsSendEKlaimMedicalNo.Value == "1" ? (hdnEKlaimMedicalNo.Value != null && hdnEKlaimMedicalNo.Value != "" ? hdnEKlaimMedicalNo.Value : hdnMedicalNo.Value) : hdnMedicalNo.Value;
                
                NewClaimMethod newClaim = new NewClaimMethod()
                {
                    metadata = new NewClaimMetadata()
                    {
                        method = "new_claim"
                    },
                    data = new NewClaimData()
                    {
                        gender = gender,
                        nama_pasien = nama_pasien,
                        nomor_kartu = nomor_kartu,
                        nomor_sep = nomor_sep,
                        tgl_lahir = tgl_lahir,
                        nomor_rm = nomor_rm
                    }
                };
                string jsonRequest = JsonConvert.SerializeObject(newClaim);
                EKlaimService eklaimService = new EKlaimService();

                string response = eklaimService.NewClaim(jsonRequest);
                respEklaim = response;
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                Helper.InsertErrorLog(ex);
                return result;
            }
        }

        private bool onDeleteClaim(ref string errMessage, ref string respEklaim)
        {
            try
            {
                string coder_nik = string.Empty;
                UserAttribute ua = BusinessLayer.GetUserAttributeList(string.Format("userid='{0}'", AppSession.UserLogin.UserID)).FirstOrDefault();
                if (ua != null)
                {
                    coder_nik = ua.SSN;
                }

                DeleteClaimMethod Claim = new DeleteClaimMethod()
                {
                    metadata = new DeleteClaimMetadata()
                    {
                        method = "delete_claim",
                    },
                    data = new DeleteClaimData()
                    {
                        coder_nik = coder_nik,
                        nomor_sep = hdnoldNoSep.Value,
                    }
                };
                string jsonRequest = JsonConvert.SerializeObject(Claim);
                EKlaimService eklaimService = new EKlaimService();
                string response = eklaimService.DeleteClaim(jsonRequest);
                respEklaim = response.ToString();
                return true;
            }
            catch (Exception er)
            {
                errMessage = er.Message.ToString();
                Helper.InsertErrorLog(er);
                return false;
            }
        }
    }
}