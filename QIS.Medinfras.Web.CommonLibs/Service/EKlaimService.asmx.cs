using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for EKlaimService
    /// </summary>
    [WebService(Namespace = "http://tempuri2.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class EKlaimService : System.Web.Services.WebService
    {

        /* Constant.SettingParameter.EKLAIM_WEB_SERVICE_URL,
              Constant.SettingParameter.EKLAIM_HOSPITAL_CODE,
              Constant.SettingParameter.EKLAIM_ENCRYPTION_KEY
         * */
        private string url = GetSetPar(Constant.SettingParameter.EKLAIM_WEB_SERVICE_URL); ///AppSession.EKlaim_WebService_URL;
        private string hospitalCode = GetSetPar(Constant.SettingParameter.EKLAIM_HOSPITAL_CODE); //AppSession.EKlaim_HospitalCode;
        private string encryptionKey = GetSetPar(Constant.SettingParameter.EKLAIM_ENCRYPTION_KEY); ///AppSession.EKlaim_EncryptionKey;

        private const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        private const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

        private static string GetSetPar(string parametercode)
        {
            SettingParameterDt lstSetpar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN('{1}')",
                                                                                        AppSession.UserLogin.HealthcareID, //0
                                                                                        parametercode // 1
                                                                                    )).FirstOrDefault();
            return lstSetpar.ParameterValue;
        }

        #region 1. New Claim [Membuat klaim baru (dan registrasi pasien jika belum ada)]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string NewClaim(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //NewClaimMethod new_claim_method = new NewClaimMethod()
            //{
            //    metadata = new NewClaimMetadata()
            //    {
            //        method = "new_claim"
            //    },
            //    data = new NewClaimData()
            //    {
            //        nomor_kartu = nomor_kartu,
            //        nomor_sep = nomor_sep,
            //        nomor_rm = nomor_rm,
            //        nama_pasien = nama_pasien,
            //        tgl_lahir = tgl_lahir,
            //        gender = gender
            //    }
            //};
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest;  //// JsonConvert.SerializeObject(new_claim_method);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                NewClaimResponse respInfo = JsonConvert.DeserializeObject<NewClaimResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "New Claim Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 2. Update Patient [Update data pasien]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdatePatient(string nomor_kartu, string nomor_rm, string nama_pasien, string tgl_lahir, string gender)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            UpdatePatientMethod update_patient_method = new UpdatePatientMethod()
            {
                metadata = new UpdatePatientMetadata()
                {
                    method = "update_patient",
                    nomor_rm = nomor_rm
                },
                data = new UpdatePatientData()
                {
                    nomor_kartu = nomor_kartu,
                    nomor_rm = nomor_rm,
                    nama_pasien = nama_pasien,
                    tgl_lahir = tgl_lahir,
                    gender = gender
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(update_patient_method);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                UpdatePatientResponse respInfo = JsonConvert.DeserializeObject<UpdatePatientResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Update Patient Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 3. Delete Patient [Hapus data pasien]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object DeletePatient(string nomor_rm, string coder_nik)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            DeletePatientMethod delete_patient_method = new DeletePatientMethod()
            {
                metadata = new DeletePatientMetadata()
                {
                    method = "delete_patient"
                },
                data = new DeletePatientData()
                {
                    nomor_rm = nomor_rm,
                    coder_nik = coder_nik
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(delete_patient_method);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                DeletePatientResponse respInfo = JsonConvert.DeserializeObject<DeletePatientResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Delete Patient Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 4. Set Claim [Untuk mengisi/update data klaim]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SetClaimData (
                string nomor_sep 
                ,string nomor_kartu 
                ,string tgl_masuk 
                ,string tgl_pulang 
                ,string cara_masuk 
                ,string jenis_rawat
                ,string kelas_rawat
                ,string adl_sub_acute 
                ,string adl_chronic 
                ,string icu_indikator 
                ,string icu_los 
                ,string ventilator_hour 
                ,string use_ind
                ,string start_dttm
                ,string stop_dttm
                ,string upgrade_class_ind 
                ,string upgrade_class_class 
                ,string upgrade_class_los 
                ,string upgrade_class_payor 
                ,string add_payment_pct 
                ,string birth_weight 
                ,int sistole 
                ,int diastole 
                ,string discharge_status 
                ,string diagnosa 
                ,string procedure 
                ,string diagnosa_inagrouper 
                ,string procedure_inagrouper
                ,string prosedur_non_bedah 
                ,string prosedur_bedah 
                ,string konsultasi 
                ,string tenaga_ahli 
                ,string keperawatan 
                ,string penunjang 
                ,string radiologi 
                ,string laboratorium 
                ,string pelayanan_darah 
                ,string rehabilitasi 
                ,string kamar 
                ,string rawat_intensif 
                ,string obat 
                ,string obat_kronis 
                ,string obat_kemoterapi 
                ,string alkes 
                ,string bmhp 
                ,string sewa_alat 
                ,string pemulasaraan_jenazah 
                ,string kantong_jenazah 
                ,string peti_jenazah 
                ,string plastik_erat 
                ,string desinfektan_jenazah 
                ,string mobil_jenazah 
                ,string desinfektan_mobil_jenazah 
                ,string covid19_status_cd 
                ,string nomor_kartu_t 
                ,string episodes 
                ,string covid19_cc_ind 
                ,string covid19_rs_darurat_ind 
                ,string covid19_co_insidense_ind 
                ,string lab_asam_laktat 
                ,string lab_procalcitonin 
                ,string lab_crp 
                ,string lab_kultur 
                ,string lab_d_dimer 
                ,string lab_pt 
                ,string lab_aptt 
                ,string lab_waktu_pendarahan 
                ,string lab_anti_hiv 
                ,string lab_albumin 
                ,string lab_analisa_gas 
                ,string rad_thorax_ap_pa 
                ,string terapi_konvalesen 
                ,string akses_naat 
                ,string isoman_ind 
                ,string bayi_lahir_status_cd 
                ,string dializer_single_use 
                ,int kantong_darah 
                ,int apgar_menit1_appearance 
                ,int apgar_menit1_pulse 
                ,int apgar_menit1_grimace 
                ,int apgar_menit1_activity 
                ,int apgar_menit1_respiration 
                ,int apgar_menit5_appearance 
                ,int apgar_menit5_pulse 
                ,int apgar_menit5_grimace 
                ,int apgar_menit5_activity 
                ,int apgar_menit5_respiration
                ,int usia_kehamilan
                ,int gravida
                ,int partus
                ,int abortus 
                ,string onset_kontraksi 
                ,string delivery_sequence 
                ,string delivery_method 
                ,string delivery_dttm 
                ,string letak_janin 
                ,string kondisi 
                ,string use_manual 
                ,string use_forcep 
                ,string use_vacuum 
                ,string tarif_poli_eks 
                ,string nama_dokter 
                ,string kode_tarif 
                ,string payor_id 
                ,string payor_cd 
                ,string cob_cd 
                ,string coder_nik
            )
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };
                       

            SetClaimMethod set_claim_method = new SetClaimMethod()
            {
                metadata = new SetClaimMetadata()
                {
                    method = "set_claim_data",
                    nomor_sep = nomor_sep
                },
                data = new SetClaimData()
                {
                    nomor_sep = nomor_sep,
                    nomor_kartu = nomor_kartu,
                    tgl_masuk = tgl_masuk,
                    tgl_pulang = tgl_pulang,
                    cara_masuk = cara_masuk,
                    jenis_rawat = jenis_rawat,
                    kelas_rawat = kelas_rawat,
                    adl_sub_acute = adl_sub_acute,
                    adl_chronic = adl_chronic,
                    icu_indikator = icu_indikator,
                    icu_los = icu_los,
                    ventilator_hour = ventilator_hour,
                    ventilator = new ventilator()
                    {
                        use_ind = use_ind,
                        start_dttm = start_dttm,
                        stop_dttm = stop_dttm
                    },
                    upgrade_class_ind = upgrade_class_ind,
                    upgrade_class_class = upgrade_class_class,
                    upgrade_class_los = upgrade_class_los,
                    upgrade_class_payor = upgrade_class_payor,
                    add_payment_pct = add_payment_pct,
                    birth_weight = birth_weight,
                    sistole = sistole,
                    diastole = diastole,
                    discharge_status = discharge_status,
                    diagnosa = diagnosa,
                    procedure = procedure,
                    diagnosa_inagrouper = diagnosa_inagrouper,
                    procedure_inagrouper = procedure_inagrouper,
                    tarif_rs = new tarif_rs()
                    {
                        prosedur_non_bedah = prosedur_non_bedah,
                        prosedur_bedah = prosedur_bedah,
                        konsultasi = konsultasi,
                        tenaga_ahli = tenaga_ahli,
                        keperawatan = keperawatan,
                        penunjang = penunjang,
                        radiologi = radiologi,
                        laboratorium = laboratorium,
                        pelayanan_darah = pelayanan_darah,
                        rehabilitasi = rehabilitasi,
                        kamar = kamar,
                        rawat_intensif = rawat_intensif,
                        obat = obat,
                        obat_kronis = obat_kronis,
                        obat_kemoterapi = obat_kemoterapi,
                        alkes = alkes,
                        bmhp = bmhp,
                        sewa_alat = sewa_alat
                    },
                    pemulasaraan_jenazah = pemulasaraan_jenazah,
                    kantong_jenazah = kantong_jenazah,
                    peti_jenazah = peti_jenazah,
                    plastik_erat = plastik_erat,
                    desinfektan_jenazah = desinfektan_jenazah,
                    mobil_jenazah = mobil_jenazah,
                    desinfektan_mobil_jenazah = desinfektan_mobil_jenazah,
                    covid19_status_cd = covid19_status_cd,
                    nomor_kartu_t = nomor_kartu_t,
                    episodes = episodes,
                    covid19_cc_ind = covid19_cc_ind,
                    covid19_rs_darurat_ind = covid19_rs_darurat_ind, 
                    covid19_co_insidense_ind = covid19_co_insidense_ind, 
                    covid19_penunjang_pengurang = new covid19_penunjang_pengurang()
                    {
                        lab_asam_laktat = lab_asam_laktat,
                        lab_procalcitonin = lab_procalcitonin,
                        lab_crp = lab_crp,
                        lab_kultur = lab_kultur,
                        lab_d_dimer = lab_d_dimer,
                        lab_pt = lab_pt,
                        lab_aptt = lab_aptt,
                        lab_waktu_pendarahan = lab_waktu_pendarahan,
                        lab_anti_hiv = lab_anti_hiv,
                        lab_analisa_gas = lab_analisa_gas,
                        lab_albumin = lab_albumin,
                        rad_thorax_ap_pa = rad_thorax_ap_pa
                    },
                    terapi_konvalesen = terapi_konvalesen,
                    akses_naat = akses_naat, 
                    isoman_ind = isoman_ind,
                    bayi_lahir_status_cd = bayi_lahir_status_cd,
                    dializer_single_use = dializer_single_use,
                    kantong_darah = kantong_darah,
                    apgar = new apgar()
                    {
                        menit_1 = new menit_1()
                        {
                            appearance = apgar_menit1_appearance,
                            pulse = apgar_menit1_pulse,
                            grimace = apgar_menit1_grimace,
                            activity = apgar_menit1_activity,
                            respiration = apgar_menit1_respiration
                        },
                        menit_5 = new menit_5()
                        {
                            appearance = apgar_menit5_appearance,
                            pulse = apgar_menit5_pulse,
                            grimace = apgar_menit5_grimace,
                            activity = apgar_menit5_activity,
                            respiration = apgar_menit5_respiration
                        }
                    },
                    persalinan = new persalinan()
                    {
                        usia_kehamilan = usia_kehamilan,
                        gravida = gravida,
                        partus = partus,
                        onset_kontraksi = onset_kontraksi
                        //delivery = new delivery()
                        //{
                        //    delivery_sequence = delivery_sequence,
                        //    delivery_method = delivery_method,
                        //    delivery_dttm = delivery_dttm,
                        //    letak_janin = letak_janin,
                        //    kondisi = kondisi,
                        //    use_manual = use_manual,
                        //    use_forcep = use_forcep,
                        //    use_vacuum = use_vacuum
                        //}
                    },
                    tarif_poli_eks = tarif_poli_eks,
                    nama_dokter = nama_dokter,
                    kode_tarif = kode_tarif,
                    payor_id = payor_id,
                    payor_cd = payor_cd,
                    cob_cd = cob_cd,
                    coder_nik = coder_nik,
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(set_claim_method);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SetClaimResponse respInfo = JsonConvert.DeserializeObject<SetClaimResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Send Claim Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 4. Set Claim [Untuk mengisi/update data klaim]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SetClaimData2(String paramRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            }; 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = paramRequest;  /////JsonConvert.SerializeObject(set_claim_method);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
               
                result = paramReceive;
            }
            entityAPILog.Response = "Send Claim Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 5. Grouping Stage 1 [Grouping Stage 1]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GroupingStage1(string jsonParam)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //GroupingStage1Method grouping_stage_1 = new GroupingStage1Method()
            //{
            //    metadata = new GroupingStage1Metadata()
            //    {
            //        method = "grouper",
            //        stage = "1"
            //    },
            //    data = new GroupingStage1Data()
            //    {
            //        nomor_sep = nomor_sep
            //    }
            //};

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonParam; /// JsonConvert.SerializeObject(grouping_stage_1);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                GroupingStage1Response respInfo = JsonConvert.DeserializeObject<GroupingStage1Response>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Grouping Stage 1 Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)] //dipanggil di belakang bukan dari javascript
        public string svGroupingStage1(string jsonRequest)
        {
            
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest; //JsonConvert.SerializeObject(grouping_stage_1);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                GroupingStage1Response respInfo    = JsonConvert.DeserializeObject<GroupingStage1Response>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Grouping Stage 1 Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
       

        #endregion

        #region 6. Grouping Stage 2 [Grouping Stage 2]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GroupingStage2(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //GroupingStage2Method grouping_stage_2 = new GroupingStage2Method()
            //{
            //    metadata = new GroupingStage2Metadata()
            //    {
            //        method = "grouper",
            //        stage = "2"
            //    },
            //    data = new GroupingStage2Data()
            //    {
            //        nomor_sep = nomor_sep,
            //        special_cmg = special_cmg
            //    }
            //};

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest; /// JsonConvert.SerializeObject(grouping_stage_2);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                GroupingStage2Response respInfo = JsonConvert.DeserializeObject<GroupingStage2Response>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Grouping Stage 2 Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 7. Claim Final [Untuk finalisasi klaim]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ClaimFinal(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //ClaimFinalMethod claim_final = new ClaimFinalMethod()
            //{
            //    metadata = new ClaimFinalMetadata()
            //    {
            //        method = "claim_final"
            //    },
            //    data = new ClaimFinalData()
            //    {
            //        nomor_sep = nomor_sep,
            //        coder_nik = coder_nik
            //    }
            //};

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest; ///// JsonConvert.SerializeObject(claim_final);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                ClaimFinalResponse respInfo = JsonConvert.DeserializeObject<ClaimFinalResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Final Claim Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 8. Reedit Claim [Untuk mengedit ulang klaim]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ReeditClaim(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //ReeditClaimMethod reedit_claim = new ReeditClaimMethod()
            //{
            //    metadata = new ReeditClaimMetadata()
            //    {
            //        method = "reedit_claim"
            //    },
            //    data = new ReeditClaimData()
            //    {
            //        nomor_sep = nomor_sep
            //    }
            //};

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest; ///// JsonConvert.SerializeObject(reedit_claim);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                ReeditClaimResponse respInfo = JsonConvert.DeserializeObject<ReeditClaimResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Reedit Claim Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 9. Send Claim [Untuk mengirim klaim ke data center (kolektif per hari)]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendClaim(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //SendClaimMethod send_claim = new SendClaimMethod()
            //{
            //    metadata = new SendClaimMetadata()
            //    {
            //        method = "send_claim"
            //    },
            //    data = new SendClaimData()
            //    {
            //        start_dt = start_dt,
            //        stop_dt = stop_dt,
            //        jenis_rawat = jenis_rawat,
            //        date_type = date_type
            //    }
            //};

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest;  //// JsonConvert.SerializeObject(send_claim);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SendClaimResponse respInfo = JsonConvert.DeserializeObject<SendClaimResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Send Claim Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 10. Send Claim Individual [Untuk mengirim klaim individual ke data center]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendClaimIndividual(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //SendClaimIndividualMethod send_claim_individual = new SendClaimIndividualMethod()
            //{
            //    metadata = new SendClaimIndividualMetadata()
            //    {
            //        method = "send_claim_individual"
            //    },
            //    data = new SendClaimIndividualData()
            //    {
            //        nomor_sep = nomor_sep
            //    }
            //};

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest; //// JsonConvert.SerializeObject(send_claim_individual);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SendClaimIndividualResponse respInfo = JsonConvert.DeserializeObject<SendClaimIndividualResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Send Claim Individual Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 11. Pull Claim [Untuk menarik data klaim dari E-Klaim (method sudah ditutup)]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PullClaim(string start_dt, string stop_dt, string jenis_rawat)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            PullClaimMethod pull_claim = new PullClaimMethod()
            {
                metadata = new PullClaimMetadata()
                {
                    method = "pull_claim"
                },
                data = new PullClaimData()
                {
                    start_dt = start_dt,
                    stop_dt = stop_dt,
                    jenis_rawat = jenis_rawat
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(pull_claim);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                PullClaimResponse respInfo = JsonConvert.DeserializeObject<PullClaimResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Pull Claim Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 12. Get Claim [Untuk mengambil data detail per klaim]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetClaim(string dataRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //GetClaimMethod get_claim_data = new GetClaimMethod()
            //{
            //    metadata = new GetClaimMetadata()
            //    {
            //        method = "get_claim_data"
            //    },
            //    data = new GetClaimData()
            //    {
            //        nomor_sep = nomor_sep
            //    }
            //};

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = dataRequest;  ///JsonConvert.SerializeObject(get_claim_data);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                GetClaimResponse respInfo = JsonConvert.DeserializeObject<GetClaimResponse>(paramReceive);
                 result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Get Claim Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 13. Get Claim Status [Untuk mengambil status per klaim]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetClaimStatus(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //GetClaimStatusMethod get_claim_status = new GetClaimStatusMethod()
            //{
            //    metadata = new GetClaimStatusMetadata()
            //    {
            //        method = "get_claim_status"
            //    },
            //    data = new GetClaimStatusData()
            //    {
            //        nomor_sep = nomor_sep
            //    }
            //};

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest; ////JsonConvert.SerializeObject(get_claim_status);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                GetClaimStatusResponse respInfo = JsonConvert.DeserializeObject<GetClaimStatusResponse>(paramReceive);

                //if (respInfo.metadata.code == "200")
                //{
                //    result = respInfo.metadata.code + "|" + respInfo.metadata.message + "|" + respInfo.response.kdStatusSep + "|" + respInfo.response.nmStatusSep;
                //}
                //else
                //{
                //    result = respInfo.metadata.code + "|" + respInfo.metadata.message;
                //}
            }
            entityAPILog.Response = "Get Claim Status Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 14. Delete Claim [Untuk menghapus klaim]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DeleteClaim(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //DeleteClaimMethod delete_claim = new DeleteClaimMethod()
            //{
            //    metadata = new DeleteClaimMetadata()
            //    {
            //        method = "delete_claim"
            //    },
            //    data = new DeleteClaimData()
            //    {
            //        nomor_sep = nomor_sep,
            //        coder_nik = coder_nik
            //    }
            //};

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest;  //JsonConvert.SerializeObject(delete_claim);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                DeleteClaimResponse respInfo = JsonConvert.DeserializeObject<DeleteClaimResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Delete Claim Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 15. Claim Print [Cetak klaim]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ClaimPrint(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //ClaimPrintMethod claim_print = new ClaimPrintMethod()
            //{
            //    metadata = new ClaimPrintMetadata()
            //    {
            //        method = "claim_print"
            //    },
            //    data = new ClaimPrintData()
            //    {
            //        nomor_sep = nomor_sep
            //    }
            //};

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest;  //// JsonConvert.SerializeObject(claim_print);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                ClaimPrintResponse respInfo = JsonConvert.DeserializeObject<ClaimPrintResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Claim Print Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 16. Search Diagnosis Multipe [Pencarian diagnosa custome]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SearchDiagnosisFindMultiple(string jsonRequst, string typeMethod)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequst;
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SearchDiagnosisResponse respInfo = JsonConvert.DeserializeObject<SearchDiagnosisResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Search Diagnose (" + typeMethod + ") Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 16. Search Diagnosis [Pencarian diagnosa]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SearchDiagnosis(string keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            SearchDiagnosisMethod search_diagnosis = new SearchDiagnosisMethod()
            {
                metadata = new SearchDiagnosisMetadata()
                {
                    method = "search_diagnosis"
                },
                data = new SearchDiagnosisData()
                {
                    keyword = keyword
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(search_diagnosis);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SearchDiagnosisResponse respInfo = JsonConvert.DeserializeObject<SearchDiagnosisResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Search Diagnose Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 16A. Search Diagnosis [Pencarian diagnosa] -> RE-INSERT TABLE BPJSReference
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SearchDiagnosisReinsertBPJSReference(string keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            SearchDiagnosisMethod search_diagnosis = new SearchDiagnosisMethod()
            {
                metadata = new SearchDiagnosisMetadata()
                {
                    method = "search_diagnosis"
                },
                data = new SearchDiagnosisData()
                {
                    keyword = keyword
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(search_diagnosis);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);

                if (!paramReceive.Contains("EMPTY"))
                {
                    SearchDiagnosisResponse respInfo = JsonConvert.DeserializeObject<SearchDiagnosisResponse>(paramReceive);

                    IDbContext ctx = DbFactory.Configure(true);
                    BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                    try
                    {

                        List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}'", Constant.BPJSObjectType.EKLAIM_DIAGNOSE), ctx);
                        if (lstReference != null)
                        {

                            foreach (BPJSReference entity in lstReference)
                            {
                                entityDao.Delete(Constant.BPJSObjectType.EKLAIM_DIAGNOSE, entity.BPJSCode);
                            }
                        }

                        BPJSReference entityInsert = new BPJSReference();

                        for (int i = 0; i < Convert.ToInt32(respInfo.response.count); i++)
                        {
                            entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.EKLAIM_DIAGNOSE;
                            entityInsert.BPJSCode = respInfo.response.data[i][1];
                            entityInsert.BPJSName = respInfo.response.data[i][0];
                            entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entityInsert);
                        }

                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }

                    result = respInfo.metadata.message;
                }
                else
                {
                    result = "Data tidak ditemukan.";
                }
            }
            entityAPILog.Response = "Re-insert Diagnose to BPJS Reference : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 17. Search Procedures [Pencarian prosedur]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SearchProcedures(string keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            SearchProceduresMethod search_procedures = new SearchProceduresMethod()
            {
                metadata = new SearchProceduresMetadata()
                {
                    method = "search_procedures"
                },
                data = new SearchProceduresData()
                {
                    keyword = keyword
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(search_procedures);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SearchProceduresResponse respInfo = JsonConvert.DeserializeObject<SearchProceduresResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Search Procedure Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 17A. Search Procedures [Pencarian prosedur] -> RE-INSERT TABLE BPJSReference
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SearchProceduresReinsertBPJSReference(string keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            SearchProceduresMethod search_procedures = new SearchProceduresMethod()
            {
                metadata = new SearchProceduresMetadata()
                {
                    method = "search_procedures"
                },
                data = new SearchProceduresData()
                {
                    keyword = keyword
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(search_procedures);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);

                if (!paramReceive.Contains("EMPTY"))
                {
                    SearchProceduresResponse respInfo = JsonConvert.DeserializeObject<SearchProceduresResponse>(paramReceive);

                    IDbContext ctx = DbFactory.Configure(true);
                    BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                    try
                    {
                        List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}'", Constant.BPJSObjectType.EKLAIM_PROCEDURE), ctx);
                        if (lstReference != null)
                        {
                            foreach (BPJSReference entity in lstReference)
                            {
                                entityDao.Delete(Constant.BPJSObjectType.EKLAIM_PROCEDURE, entity.BPJSCode);
                            }
                        }

                        BPJSReference entityInsert = new BPJSReference();

                        for (int i = 0; i < Convert.ToInt32(respInfo.response.count); i++)
                        {
                            entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.EKLAIM_PROCEDURE;
                            entityInsert.BPJSCode = respInfo.response.data[i][1];
                            entityInsert.BPJSName = respInfo.response.data[i][0];
                            entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entityInsert);
                        }

                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }

                    result = respInfo.metadata.message;
                }
                else
                {
                    result = "Data tidak ditemukan.";
                }
            }
            entityAPILog.Response = "Re-insert Procedure to BPJS Reference : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 18. Search Diagnosis INA Grouper [Pencarian diagnosa INA Grouper]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SearchDiagnosisINAGrouper(string keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            SearchDiagnosisINAGrouperMethod search_diagnosis_INAGrouper = new SearchDiagnosisINAGrouperMethod()
            {
                metadata = new SearchDiagnosisINAGrouperMetadata()
                {
                    method = "search_diagnosis_inagrouper"
                },
                data = new SearchDiagnosisINAGrouperData()
                {
                    keyword = keyword
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(search_diagnosis_INAGrouper);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SearchDiagnosisINAGrouperResponse respInfo = JsonConvert.DeserializeObject<SearchDiagnosisINAGrouperResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Search Diagnose INAGrouper Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 18A. Search Diagnosis INAGrouper [Pencarian diagnosa] -> RE-INSERT TABLE BPJSReference
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SearchDiagnosisINAGrouperReinsertBPJSReference(string keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            SearchDiagnosisINAGrouperMethod search_diagnosis_inagrouper = new SearchDiagnosisINAGrouperMethod()
            {
                metadata = new SearchDiagnosisINAGrouperMetadata()
                {
                    method = "search_diagnosis_inagrouper"
                },
                data = new SearchDiagnosisINAGrouperData()
                {
                    keyword = keyword
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(search_diagnosis_inagrouper);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);

                if (!paramReceive.Contains("EMPTY"))
                {
                    SearchDiagnosisINAGrouperResponse respInfo = JsonConvert.DeserializeObject<SearchDiagnosisINAGrouperResponse>(paramReceive);

                    IDbContext ctx = DbFactory.Configure(true);
                    BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                    try
                    {
                        List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}'", Constant.BPJSObjectType.EKLAIM_INA_DIAGNOSE), ctx);
                        if (lstReference != null)
                        {
                            foreach (BPJSReference entity in lstReference)
                            {
                                entityDao.Delete(Constant.BPJSObjectType.EKLAIM_INA_DIAGNOSE, entity.BPJSCode);
                            }
                        }

                        BPJSReference entityInsert = new BPJSReference();

                        for (int i = 0; i < Convert.ToInt32(respInfo.response.count); i++)
                        {
                            entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.EKLAIM_INA_DIAGNOSE;
                            entityInsert.BPJSCode = respInfo.response.data[i].code;
                            entityInsert.BPJSName = respInfo.response.data[i].description;
                            entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entityInsert);
                        }

                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }

                    result = respInfo.metadata.message;
                }
                else
                {
                    result = "Data tidak ditemukan.";
                }
            }
            entityAPILog.Response = "Re-insert INA Diagnose to BPJS Reference : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 19. Search Procedures INAGrouper [Pencarian prosedur]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SearchProceduresINAGrouper(string keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            SearchProceduresINAGrouperMethod search_procedures_inagrouper = new SearchProceduresINAGrouperMethod()
            {
                metadata = new SearchProceduresINAGrouperMetadata()
                {
                    method = "search_procedures_inagrouper"
                },
                data = new SearchProceduresINAGrouperData()
                {
                    keyword = keyword
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(search_procedures_inagrouper);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SearchProceduresINAGrouperResponse respInfo = JsonConvert.DeserializeObject<SearchProceduresINAGrouperResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Search Procedure INAGrouper Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 19A. Search Procedures INAGrouper [Pencarian prosedur] -> RE-INSERT TABLE BPJSReference
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SearchProceduresINAGrouperReinsertBPJSReference(string keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            SearchProceduresINAGrouperMethod search_procedures_inagrouper = new SearchProceduresINAGrouperMethod()
            {
                metadata = new SearchProceduresINAGrouperMetadata()
                {
                    method = "search_procedures_inagrouper"
                },
                data = new SearchProceduresINAGrouperData()
                {
                    keyword = keyword
                }
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = JsonConvert.SerializeObject(search_procedures_inagrouper);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);

                if (!paramReceive.Contains("EMPTY"))
                {
                    SearchProceduresINAGrouperResponse respInfo = JsonConvert.DeserializeObject<SearchProceduresINAGrouperResponse>(paramReceive);

                    IDbContext ctx = DbFactory.Configure(true);
                    BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                    try
                    {
                        List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}'", Constant.BPJSObjectType.EKLAIM_INA_PROCEDURE), ctx);
                        if (lstReference != null)
                        {
                            foreach (BPJSReference entity in lstReference)
                            {
                                entityDao.Delete(Constant.BPJSObjectType.EKLAIM_INA_PROCEDURE, entity.BPJSCode);
                            }
                        }

                        BPJSReference entityInsert = new BPJSReference();

                        for (int i = 0; i < Convert.ToInt32(respInfo.response.count); i++)
                        {
                            entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.EKLAIM_INA_PROCEDURE;
                            entityInsert.BPJSCode = respInfo.response.data[i].code;
                            entityInsert.BPJSName = respInfo.response.data[i].description;
                            entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entityInsert);
                        }

                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }

                    result = respInfo.metadata.message;
                }
                else
                {
                    result = "Data tidak ditemukan.";
                }
            }
            entityAPILog.Response = "Re-insert INA Procedure to BPJS Reference : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return result;
        }
        #endregion

        #region 20. Upload File
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UploadFile(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";
            string data = jsonRequest;
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SearchProceduresINAGrouperResponse respInfo = JsonConvert.DeserializeObject<SearchProceduresINAGrouperResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = "Upload File Response : " + result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }

        #endregion

        #region 23. Validasi Nomor Register SITB
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SITBValidate(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest;  //JsonConvert.SerializeObject(delete_claim);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SITBValidateResponse respInfo = JsonConvert.DeserializeObject<SITBValidateResponse>(paramReceive);
                result = respInfo.response.detail;
            }
            entityAPILog.Response =  result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region 24. Membatalkan Validasi Nomor Register SITB
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SITBInvalidate(string jsonRequest)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.EKLAIM,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url));
            request.Method = "POST";

            string data = jsonRequest;  //JsonConvert.SerializeObject(delete_claim);
            entityAPILog.MessageText = data;

            string paramSend = inacbg_encrypt(data, encryptionKey);

            byte[] bytes = Encoding.UTF8.GetBytes(paramSend);
            request.ContentLength = bytes.Length;
            Stream putStream = request.GetRequestStream();
            putStream.Write(bytes, 0, bytes.Length);
            putStream.Close();
            WebResponse response = (WebResponse)request.GetResponse();
            string result = "", paramReceive = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                string cutResultLeft = result.Substring(30);
                string cutResultRight = cutResultLeft.Substring(0, cutResultLeft.Length - 30);

                paramReceive = inacbg_decrypt(cutResultRight, encryptionKey);
                SITBInvalidateResponse respInfo = JsonConvert.DeserializeObject<SITBInvalidateResponse>(paramReceive);
                result = respInfo.metadata.message;
            }
            entityAPILog.Response = result;
            BusinessLayer.InsertAPIMessageLog(entityAPILog);
            return paramReceive;
        }
        #endregion

        #region Utility Function

        // ENCRYPT
        public string inacbg_encrypt(string text, string key)
        {
            var keys = Encoding.Default.GetBytes(hex2bin(key));
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.GenerateIV();
            var iv = aes.IV;
            aes.Key = keys;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            byte[] src = Encoding.Default.GetBytes(text);
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] data = encrypt.TransformFinalBlock(src, 0, src.Length);
                HMACSHA256 hashObject = new HMACSHA256(keys);
                var hash_sign = hashObject.ComputeHash(data);
                byte[] signature = new byte[10];
                Array.Copy(hash_sign, 0, signature, 0, 10);
                byte[] ret = new byte[signature.Length + iv.Length + data.Length];
                Array.Copy(signature, 0, ret, 0, signature.Length);
                Array.Copy(iv, 0, ret, signature.Length, iv.Length);
                Array.Copy(data, 0, ret, signature.Length + iv.Length, data.Length);
                return Convert.ToBase64String(ret);
            }
        }

        // DECRYPT
        public string inacbg_decrypt(string strencrypt, string key)
        {
            string encoded_str = strencrypt;
            byte[] chiper = Convert.FromBase64String(encoded_str);
            var length = chiper.Length;
            byte[] new_byte_iv = new byte[16];
            byte[] new_byte_msg = new byte[length - 26];
            Array.Copy(chiper, 10, new_byte_iv, 0, 16);
            Array.Copy(chiper, 26, new_byte_msg, 0, length - 26);
            byte[] byte_key = Encoding.Default.GetBytes(hex2bin(key));
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.Key = byte_key;
            aes.IV = new_byte_iv;
            ICryptoTransform AESDecrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            return Encoding.Default.GetString(AESDecrypt.TransformFinalBlock(new_byte_msg,
            0,
            new_byte_msg.Length));
        }

        private static string hex2bin(string input)
        {
            input = input.Replace("-", "");
            byte[] raw = new byte[input.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(input.Substring(i * 2, 2), 16);
            }
            return Encoding.Default.GetString(raw);
        }


        //private void SetRequestHeader(HttpWebRequest Request)
        //{
        //    TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        //    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        //    string consID = AppSession.APLICARES_Consumer_ID;
        //    string pass = AppSession.APLICARES_Consumer_Pwd;

        //    Request.Headers.Add("X-Cons-ID", consID);
        //    Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
        //    Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}&{1}", consID, unixTimestamp), pass));
        //}

        //private string GenerateSignature(string data, string secretKey)
        //{
        //    // Initialize the keyed hash object using the secret key as the key
        //    HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

        //    // Computes the signature by hashing the salt with the secret key as the key
        //    var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

        //    // Base 64 Encode
        //    var encodedSignature = Convert.ToBase64String(signature);

        //    // URLEncode
        //    // encodedSignature = System.Web.HttpUtility.UrlEncode(encodedSignature);
        //    return encodedSignature;
        //}
        #endregion
    }
}
