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
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class GetSEPCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            List<SettingParameterDt> lstSetpar = BusinessLayer.GetSettingParameterDtList(string.Format(" ParameterCode IN('{0}', '{1}') AND HealthcareID='001'", 
                                        Constant.SettingParameter.IS_BPJS_BRIDGING, 
                                        Constant.SettingParameter.IS_BRIDGING_TO_EKLAIM));
            hdnIsBridgingEklaim.Value = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_EKLAIM).FirstOrDefault().ParameterValue;
            hdnIsBridgingVclaim.Value = lstSetpar.Where(p => p.ParameterCode ==     Constant.SettingParameter.IS_BPJS_BRIDGING).FirstOrDefault().ParameterValue; 

            vRegistration1 entity = BusinessLayer.GetvRegistration1List(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
            hdnRegistrationIDCtl.Value = entity.RegistrationID.ToString();
           

            Patient oPatient = BusinessLayer.GetPatient(entity.MRN);
            if (oPatient != null) {
                hdnDOB.Value = oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                hdnGender.Value = oPatient.GCGender;
                hdnMedicalNo.Value = oPatient.MedicalNo;
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
                RegistrationBPJS entity = BusinessLayer.GetRegistrationBPJSList(string.Format("RegistrationID = {0}", hdnRegistrationIDCtl.Value)).FirstOrDefault();
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
                txtDiagnosaName.Text = entity.NamaDiagnosa;
                txtCatatan.Text = entity.Catatan;
               
               
            }
            
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNoSEP, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTglSEP, new ControlEntrySetting(true, true, true));
           /// SetControlEntrySetting(txtJamSEP, new ControlEntrySetting(true, true, true));
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
        //private bool isSepValid() {

        //    bool result = false;
        //    BPJSService bpjsService = new BPJSService();
        //    bpjsService.FindSEPInfo();

        //    return result;
        //}
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
                    onSaveEditData(ref errMessage, ref retval, ref result);
                }
                else
                {
                    errMessage = "Maaf, nomor sep tidak ditemukan.";
                    result = false;
                }
            }
            else {
                onSaveEditData(ref errMessage, ref retval, ref result);
            }
           
            return result;
        }

        private void onSaveEditData(ref string errMessage, ref string retval, ref bool result)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            RegistrationBPJSDao entityRegistrationBPJSDao = new RegistrationBPJSDao(ctx);
            try
            {
                if (hdnIsGetPeserta.Value == "1") {
                    RegistrationBPJS regBPJS = entityRegistrationBPJSDao.Get(Convert.ToInt32(hdnRegistrationIDCtl.Value));
                    if (regBPJS != null)
                    {
                        regBPJS.NamaDiagnosa = Request.Form[txtDiagnosaName.UniqueID]  ;
                        regBPJS.Catatan = Request.Form[txtCatatan.UniqueID];
                        entityRegistrationBPJSDao.Update(regBPJS);
                        result = true;
                        ctx.CommitTransaction();
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
                string tgl_lahir =hdnDOB.Value;
                string nomor_rm = hdnMedicalNo.Value;
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

        protected void cbpGetDataPesertaView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string respEklaim = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "getDataPeserta")
            {
                SetControlProperties(); 
            }
        }
        private void SetControlProperties()
        { 
                BPJSService oBpjs = new BPJSService();
                string resp = oBpjs.FindSEPInfo_MEDINFRASAPI(txtNoSEP.Text).ToString();
                string[] data = resp.Split('|');

                if (data[0] == "1")
                {
                    FindSepApiResponse oData = JsonConvert.DeserializeObject<FindSepApiResponse>(data[1]);
                    if (oData != null)
                    {
                        txtCatatan.Text = oData.response.catatan;
                        txtDiagnosaName.Text = oData.response.diagnosa;
                        txtTglSEP.Text = oData.response.tglSep;
                        hdnIsGetPeserta.Value = "1";
                    }
                }
        }
    }
}