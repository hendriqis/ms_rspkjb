using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RegistrationBPJSUpdateCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramSplit = param.Split('|');
                SetControlProperties();
                hdnRegistrationIDCtl.Value = paramSplit[0];
                txtRegistrationNo.Text = paramSplit[1];
                IsAdd = false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao entityBPJSDao = new RegistrationBPJSDao(ctx);
            ParamedicMasterDao paramedicDao = new ParamedicMasterDao(ctx);
            try
            {
                RegistrationBPJS regBPJS = entityBPJSDao.Get(Convert.ToInt32(hdnRegistrationIDCtl.Value));
                if (regBPJS != null)
                {
                    if (!String.IsNullOrEmpty(regBPJS.NoSEP))
                    {
                        result = false;
                        errMessage = "Maaf, registrasi ini sudah memiliki no SEP, Tidak dapat melanjutkan proses.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        if (regBPJS.NoPeserta != "0" && regBPJS.NoPeserta != "")
                        {
                            if (AppSession.IsBridgingToBPJS)
                            {
                                string filterReg = string.Format("RegistrationID = '{0}' AND GCRegistrationStatus != '{1}'", hdnRegistrationIDCtl.Value, Constant.VisitStatus.CANCELLED);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                vRegistration entityReg = BusinessLayer.GetvRegistrationList(filterReg, ctx).FirstOrDefault();

                                string filterVisit = string.Format("RegistrationID = '{0}' AND IsMainVisit = 1", entityReg.RegistrationID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(filterVisit, ctx).FirstOrDefault();

                                ParamedicMaster pm = paramedicDao.Get(entityReg.ParamedicID);

                                string filterDiagnose = string.Format("VisitID = '{0}' AND GCDiagnoseType = '{1}' AND IsDeleted = 0", entityReg.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                vPatientDiagnosis diagnose = BusinessLayer.GetvPatientDiagnosisList(filterDiagnose, ctx).FirstOrDefault();                               

                                BPJSService bpjsService = new BPJSService();

                                string dateReg = entityReg.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                                string apiResult = "";
                                if (AppSession.SA0167 == Constant.BPJS_Version_Release.v1_0)
                                {
                                    apiResult = bpjsService.GetPeserta(regBPJS.NoPeserta, dateReg).ToString();
                                }
                                else
                                {
                                    apiResult = bpjsService.GetPeserta_MEDINFRASAPI(regBPJS.NoPeserta, dateReg).ToString();
                                }

                                string[] apiResultSplit = apiResult.Split('|');
                                if (apiResultSplit[0] == "1")
                                {
                                    BPJSPesertaAPI respInfo = JsonConvert.DeserializeObject<BPJSPesertaAPI>(apiResultSplit[1]);
                                    if (respInfo.response.peserta != null)
                                    {
                                        regBPJS.NamaPeserta = respInfo.response.peserta.nama;
                                        regBPJS.KodePPK = respInfo.response.peserta.provUmum.kdProvider;
                                        regBPJS.NamaPPK = string.Format("{0} - {1}", respInfo.response.peserta.provUmum.kdProvider, respInfo.response.peserta.provUmum.nmProvider.Trim());
                                        regBPJS.JenisPeserta = respInfo.response.peserta.jenisPeserta.keterangan;
                                        regBPJS.JenisPelayanan = entityReg.DepartmentID == Constant.Facility.INPATIENT ? "1" : "2";
                                        regBPJS.NamaKelasTanggungan = string.Format("{0} - {1}", respInfo.response.peserta.hakKelas.kode, respInfo.response.peserta.hakKelas.keterangan);
                                        regBPJS.KelasTanggungan = respInfo.response.peserta.hakKelas.kode;
                                        regBPJS.Dinsos = respInfo.response.peserta.informasi.dinsos;
                                        regBPJS.NoSKTM = respInfo.response.peserta.informasi.dinsos;
                                        regBPJS.ProlanisPRB = respInfo.response.peserta.informasi.prolanisPRB;

                                        if (!string.IsNullOrEmpty(pm.BPJSReferenceInfo))
                                        {
                                            string[] bpjsInfo = pm.BPJSReferenceInfo.Split(';');
                                            string[] hfisInfo = bpjsInfo[1].Split('|');
                                            regBPJS.KodeDPJP = hfisInfo[0];
                                        }

                                        if (!String.IsNullOrEmpty(entityReg.BPJSPoli))
                                        {
                                            string BPJSPoliName = entityReg.BPJSPoli;
                                            regBPJS.KodePoliklinik = BPJSPoliName.Split('|')[0];
                                            regBPJS.NamaPoliklinik = BPJSPoliName;
                                        }

                                        string BPJSDiagnoseCode = "";
                                        if (diagnose != null)
                                        {
                                            BPJSDiagnoseCode = string.Format("{0}|{1}|{2}", diagnose.INACBGLabel, diagnose.DiagnoseName, diagnose.DiagnosisText);
                                        }

                                        if (!string.IsNullOrEmpty(BPJSDiagnoseCode))
                                        {
                                            string diagnoseSplit = BPJSDiagnoseCode;
                                            regBPJS.KodeDiagnosa = diagnoseSplit.Split('|')[0];
                                            regBPJS.NamaDiagnosa = diagnoseSplit.Split('|')[1];
                                            regBPJS.Keluhan = diagnoseSplit.Split('|')[2];
                                        }

                                        regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityBPJSDao.Update(regBPJS);
                                        ctx.CommitTransaction();
                                    }
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = "Maaf, Bridging BPJS Belum Di Konfigurasi.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Maaf, Pasien ini belum memiliki no peserta BPJS.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, registrasi ini tidak ditanggung BPJS, Harap pastikan penjamin bayar adalah BPJS.";
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
            return result;
        }
    }
}