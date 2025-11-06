using System;
using System.Collections.Generic;
using System.Data;
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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BPJSChangeDischargeDateCtl : BaseViewPopupCtl
    {
        String departmentID;

        protected string GetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }

        public override void InitializeDataControl(string param)
        {
            Healthcare healthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            hdnHealthcareInitial.Value = healthcare.Initial;

            hdnDepartmentID.Value = AppSession.UserLogin.DepartmentID;

            List<SettingParameterDt> lstSetpar = BusinessLayer.GetSettingParameterDtList(string.Format(
                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}', '{3}', '{4}')",
                                        AppSession.UserLogin.HealthcareID, //0
                                        Constant.SettingParameter.SA0215, //1 
                                        Constant.SettingParameter.IS_BRIDGING_TO_EKLAIM, //2
                                        Constant.SettingParameter.RM_EKLAIM_SEND_EKLAIM_MEDICALNO, //3
                                        Constant.SettingParameter.SA_CREATE_APPOINTMENT_AFTER_CREATE_SURKON //4
                                        ));

            hdnIsUsedReferenceQueueNo.Value = lstSetpar.Where(t => t.ParameterCode == Constant.SettingParameter.SA0215).FirstOrDefault().ParameterValue;

            hdnIsBridgingEklaim.Value = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_EKLAIM).FirstOrDefault().ParameterValue;
            
            hdnIsSendEKlaimMedicalNo.Value = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.RM_EKLAIM_SEND_EKLAIM_MEDICALNO).FirstOrDefault().ParameterValue;

            hdnIsCreateAppointmentAfterCreateNoSurkon.Value = lstSetpar.Where(t => t.ParameterCode == Constant.SettingParameter.SA_CREATE_APPOINTMENT_AFTER_CREATE_SURKON).FirstOrDefault().ParameterValue; 

            txtNoSEP.Attributes.Add("readonly", "readonly");
             hdnIsBridgingBPJSVClaimVersion.Value = AppSession.SA0167;

            txtDischargeDateCtl.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            
            SetControlProperties();

            if (param != "")
            {
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param))[0];
                hdnVisitID.Value = entity.VisitID.ToString();
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                hdnVisitTypeID.Value = entity.VisitTypeID.ToString();
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                hdnGCCostumerType.Value = entity.GCCustomerType;
                hdnPayerID.Value = entity.BusinessPartnerID.ToString();
                hdnContractID.Value = entity.ContractID.ToString();
                hdnCoverageTypeID.Value = entity.CoverageTypeID.ToString();
                hdnParticipantNo.Value = entity.CorporateAccountNo.ToString();
                hdnIsCoverageLimitPerDay.Value = entity.IsCoverageLimitPerDay.ToString();
                hdnIsControlClassCare.Value = entity.IsControlClassCare.ToString();
                hdnControlClassCare.Value = entity.ControlClassID.ToString();
                hdnEmployeeID.Value = entity.EmployeeID.ToString();
                hdnGCTariffScheme.Value = entity.GCTariffScheme;
                hdnRegistrationNo.Value = entity.RegistrationNo;
                if (entity.GCReferrerGroup == Constant.Referrer.RUMAH_SAKIT)
                    hdnAsalRujukan.Value = "2";
                else
                    hdnAsalRujukan.Value = "1";

                hdnIsPoliExecutive.Value = "0";
                if (!string.IsNullOrEmpty(entity.GCClinicGroup))
                {
                    if (entity.GCClinicGroup == Constant.ClinicGroup.CLINIC_GROUP_NON_BPJS)
                    {
                        hdnIsPoliExecutive.Value = "1";
                    }
                }

                int rowCount = BusinessLayer.GetRegistrationBPJSRowCount(string.Format("RegistrationID = {0}", param));
                hdnID.Value = param;
                hdnMRN.Value = entity.MRN.ToString();
                string BPJSPoliName = entity.BPJSPoli;
               
                if (rowCount > 0)
                {
                    hdnIsBpjsRegistrationCtl.Value = "1";
                    hdnIsAdd.Value = "0";
                    vRegistrationBPJS entityBPJS = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
                    EntityToControl(entityBPJS);
                }
                else
                {
                    hdnIsBpjsRegistrationCtl.Value = "0";
                    hdnIsAdd.Value = "1";
                    departmentID = entity.DepartmentID;
                    txtTglSEP.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtJamSEP.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }

                if (string.IsNullOrEmpty(txtNoSEP.Text)) txtNoSEP.Attributes.Remove("readonly");

                
                vRegistrationBPJS entitySpecialty = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
                SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.OP_IS_DEFAULT_SPECIALTY_CATARACT);
               
                Appointment oAppointment = BusinessLayer.GetAppointmentList(string.Format("FromVisitID  = {0}", hdnVisitID.Value)).FirstOrDefault();
                if (oAppointment != null)
                {
                    hdnAppointmentID.Value = oAppointment.AppointmentID.ToString();
                }

                hdnKelasNaikBPJS.Value = entitySpecialty.BPJSKelasNaik;

                if (hdnIsCreateAppointmentAfterCreateNoSurkon.Value == "1")
                {
                    divBtnSuratRujukan.InnerText = "Appointment & Surat Kontrol";
                }
                else
                {
                    divBtnSuratRujukan.InnerText = "Surat Kontrol";
                }
            }
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string respEklaim = "";
            int RegistrationID = 0;

            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0].Contains("discharge"))
            {
                RegistrationID = Convert.ToInt32(hdnID.Value);
                if (OnDischargePatient(ref errMessage, RegistrationID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            } 
            
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void EntityToControl(vRegistrationBPJS entity)
        {
            hdnBPJSReferenceInfoKodeUnit.Value = entity.KodePoliklinik;
            if (entity.DepartmentID == Constant.Facility.OUTPATIENT)
            {
                hdnBPJSReferenceInfoNamaUnit.Value = entity.NamaPoliklinik.Split('|').ToList()[1];
            }
            else
            {
                hdnBPJSReferenceInfoNamaUnit.Value = entity.NamaPoliklinik;
            }
            txtNoSEP.Text = entity.NoSEP;
          

            if (entity.TanggalSEP != Helper.InitializeDateTimeNull())
            {
                txtTglSEP.Text = entity.TanggalSEP.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            else txtTglSEP.Text = entity.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (entity.TanggalPulang != Helper.InitializeDateTimeNull())
            {
                txtDischargeDateCtl.Text = entity.TanggalPulang.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeDateCtl.Attributes.Add("readonly", "readonly");
            }
            else txtDischargeDateCtl.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (!String.IsNullOrEmpty(entity.JamSEP))
            {
                txtJamSEP.Text = entity.JamSEP;
            }
            else txtJamSEP.Text = entity.RegistrationTime;
            
            hdnNoSKDP.Value = entity.NoSuratKontrol;
            hdnNoSKDManual.Value = entity.NoSuratKontrol;
            hdnKodeDPJP.Value = entity.KodeDPJP;
            hdnStatusPulang.Value = entity.KodeStatusPulang;
            txtStatusPulangCode.Text = entity.KodeStatusPulang;
            txtStatusPulangName.Text = entity.NamaStatusPulang;

            if (entity.TanggalMeninggal.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == "01-01-1900")
            {
                txtDateOfDeath.Text = "";
            }
            else
            {
                txtDateOfDeath.Text = entity.TanggalMeninggal.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
             
            txtNoSuratMeninggal.Text = entity.NoSuratMeninggal;
            txtNoLPManual.Text = entity.NoLPManual;

         
        }

        private void ControlToEntity(RegistrationBPJS entity, IDbContext ctx)
        {
           
            entity.NoSEP = Request.Form[txtNoSEP.UniqueID];
            bool flagSEPNull = false;
            if (string.IsNullOrEmpty(entity.NoSEP))
            {
                flagSEPNull = true;
            }
            if (!string.IsNullOrEmpty(entity.NoSEP))
            {
                entity.GCSEPStatus = Constant.SEP_Status.DISETUJUI;
                entity.JamSEP = txtJamSEP.Text;
                entity.TanggalSEP = Helper.GetDatePickerValue(txtTglSEP.Text);
                 

                if (flagSEPNull)
                {
                    if (entity.SequenceNumber == null)
                    {
                        int currentNo = BusinessLayer.GetRegistrationBPJSMaxQueueNo(string.Format("TanggalSEP = '{0}'", entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                        entity.SequenceNumber = ++currentNo;
                    }
                    else
                    {
                        if (entity.SequenceNumber == 0)
                        {
                            int currentNo = BusinessLayer.GetRegistrationBPJSMaxQueueNo(string.Format("TanggalSEP = '{0}'", entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                            entity.SequenceNumber = ++currentNo;
                        }
                    }
                }
            } 
        }

        private void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0",
                                             Constant.StandardCode.TUJUAN_KUNJUNGAN_BPJS,
                                             Constant.StandardCode.PROSEDUR_BPJS,
                                             Constant.StandardCode.PROSEDUR_PENUNJANG_BPJS,
                                             Constant.StandardCode.ASESMEN_PELAYANAN_BPJS
                                         );

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            List<StandardCode> lstTujuanKunjungan = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.TUJUAN_KUNJUNGAN_BPJS).ToList();
            List<StandardCode> lstProsedur = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PROSEDUR_BPJS).ToList();
            List<StandardCode> lstProsedurPenunjang = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PROSEDUR_PENUNJANG_BPJS).ToList();
            List<StandardCode> lstAsesmenPelayanan = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ASESMEN_PELAYANAN_BPJS).ToList();

            //lstTujuanKunjungan.Insert(0, new StandardCode { TagProperty = "", StandardCodeName = "" });
            lstProsedur.Insert(0, new StandardCode { TagProperty = "", StandardCodeName = "" });
            lstProsedurPenunjang.Insert(0, new StandardCode { TagProperty = "", StandardCodeName = "" });
            lstAsesmenPelayanan.Insert(0, new StandardCode { TagProperty = "", StandardCodeName = "" });

            if (lstTujuanKunjungan.Count > 0)
            {
                string lst = string.Empty;
                for (int i = 0; i < lstTujuanKunjungan.Count; i++)
                {
                    lst += string.Format("{0};{1}|", lstTujuanKunjungan[i].StandardCodeID, lstTujuanKunjungan[i].TagProperty);
                }
                hdnLstTujuanKunjungan.Value = lst;
            }

            if (lstProsedur.Count > 0)
            {
                string lst = string.Empty;
                for (int i = 0; i < lstProsedur.Count; i++)
                {
                    lst += string.Format("{0};{1}|", lstProsedur[i].StandardCodeID, lstProsedur[i].TagProperty);
                }
                hdnLstProsedur.Value = lst;
            }

            if (lstProsedurPenunjang.Count > 0)
            {
                string lst = string.Empty;
                for (int i = 0; i < lstProsedurPenunjang.Count; i++)
                {
                    lst += string.Format("{0};{1}|", lstProsedurPenunjang[i].StandardCodeID, lstProsedurPenunjang[i].TagProperty);
                }
                hdnLstProsedurPenunjang.Value = lst;
            }

            if (lstAsesmenPelayanan.Count > 0)
            {
                string lst = string.Empty;
                for (int i = 0; i < lstAsesmenPelayanan.Count; i++)
                {
                    lst += string.Format("{0};{1}|", lstAsesmenPelayanan[i].StandardCodeID, lstAsesmenPelayanan[i].TagProperty);
                }
                hdnLstAsesmenPelayanan.Value = lst;
            }
        }

        private bool onNewClaim(ref string errMessage, ref string respEklaim, int RegistrationID)
        {
            bool result = true;

            try
            {
                string filterRegBPJS = string.Format("RegistrationID = {0}", RegistrationID);
                vRegistrationBPJS1 entity = BusinessLayer.GetvRegistrationBPJS1List(filterRegBPJS).FirstOrDefault();
                string gender = string.Empty;
                if (entity.GCGender == Constant.Gender.MALE)
                {
                    gender = "1";
                }
                else if (entity.GCGender == Constant.Gender.FEMALE)
                {
                    gender = "2";
                }

                string nama_pasien = entity.PatientName;
                string nomor_kartu = entity.NoPeserta;
                string nomor_sep = txtNoSEP.Text;
                string tgl_lahir = entity.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);

                string nomor_rm = hdnIsSendEKlaimMedicalNo.Value == "1" ? (entity.EKlaimMedicalNo != null && entity.EKlaimMedicalNo != "" ? entity.EKlaimMedicalNo : entity.MedicalNo) : entity.MedicalNo;

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
            }
            catch (Exception ex)
            {
                result = false;
                respEklaim = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage, int RegistrationID)
        {
            bool result = true;
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage, int RegistrationID)
        {
            bool result = true;
            return result;
        }

        private bool OnDischargePatient(ref string errMessage, int RegistrationID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao entityDtDao = new RegistrationBPJSDao(ctx);
            try
            {
                RegistrationBPJS entity = entityDtDao.Get(RegistrationID);
                entity.NoSEP = Request.Form[txtNoSEP.UniqueID];
                entity.TanggalPulang = Helper.GetDatePickerValue(txtDischargeDateCtl.Text);
                entity.KodeStatusPulang = Request.Form[txtStatusPulangCode.UniqueID].Trim();
                entity.NamaStatusPulang = Request.Form[txtStatusPulangName.UniqueID].Trim();
                entity.TanggalMeninggal = Helper.GetDatePickerValue(txtDateOfDeath.Text);
                entity.NoSuratMeninggal = Request.Form[txtNoSuratMeninggal.UniqueID];
                entity.NoLPManual = Request.Form[txtNoLPManual.UniqueID];
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);
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
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            var result = param[0] + "|";
            //if (param[0] == "search")
            //{
            //    BPJSService oService = new BPJSService();
            //    //result = GetBPJSPatientReferralList(txtNoPeserta.Text, hdnAsalRujukan.Value);
            //    result = oService.GetRencanaKontrolByNoPeserta_MEDINFRASAPI(param[1], param[2], txtNoPeserta.Text, param[3]);
            //    string[] resultInfo = result.Split('|');
            //    if (resultInfo[0] == "1")
            //    {
            //        GetSuratKontrolByNoPeserta data = JsonConvert.DeserializeObject<GetSuratKontrolByNoPeserta>(resultInfo[1]);
            //        if (data.metaData.code == "200")
            //        {
            //            grdView.DataSource = data.response.list;
            //            grdView.DataBind();
            //        }
            //    }
            //}
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ValidateParamedicScSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate)
        {
            //Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            //DateTime selectedDate = Helper.GetDatePickerValue(txtTglRencanaKontrol.Text);
            //List<GetParamedicLeaveScheduleCompare> objLeave = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), ParamedicID);

            //#region validate time slot
            //#region if leave in period
            //if (objLeave.FirstOrDefault().DayNumber != 0 && objLeave.Count() > 1)
            //{
            //    #region set time slot Paramedic Schedule
            //    if (obj != null)
            //    {
            //        if (obj.DayNumber == objLeave.FirstOrDefault().DayNumber && objLeave.FirstOrDefault().Date == selectedDate)
            //        {
            //            DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);

            //            DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
            //            DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
            //            DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
            //            DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
            //            DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

            //            DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
            //            DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
            //            DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
            //            DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
            //            DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

            //            if (obj.StartTime5 != "")
            //            {

            //                if (startTimeDefault.TimeOfDay >= objStart5.TimeOfDay)
            //                {
            //                    obj.EndTime5 = startTimeDefault.ToString("HH:mm");
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //                {
            //                    obj.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime5 = "";
            //                    obj.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay <= objStart5.TimeOfDay)
            //                {
            //                    obj.StartTime5 = "";
            //                    obj.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
            //                {
            //                    obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                    obj.StartTime5 = "";
            //                    obj.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objStart4.TimeOfDay)
            //                {
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                    obj.StartTime5 = "";
            //                    obj.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
            //                {
            //                    obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                    obj.StartTime5 = "";
            //                    obj.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay)
            //                {
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                    obj.StartTime5 = "";
            //                    obj.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
            //                {
            //                    obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                    obj.StartTime5 = "";
            //                    obj.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objStart2.TimeOfDay)
            //                {
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                    obj.StartTime5 = "";
            //                    obj.EndTime5 = "";
            //                }
            //            }
            //            else if (obj.StartTime4 != "" && obj.StartTime5 == "")
            //            {
            //                if (startTimeDefault.TimeOfDay >= objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //                {
            //                    obj.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
            //                {
            //                    obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objStart4.TimeOfDay)
            //                {
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
            //                {
            //                    obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay)
            //                {
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
            //                {
            //                    obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objStart2.TimeOfDay)
            //                {
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                }
            //            }
            //            else if (obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
            //            {
            //                if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
            //                {
            //                    obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
            //                {
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay)
            //                {
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
            //                {
            //                    obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay)
            //                {
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
            //                {
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
            //                {
            //                    obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                }
            //            }
            //            else if (obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
            //            {
            //                if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
            //                {
            //                    obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
            //                {
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
            //                {
            //                    obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart1.TimeOfDay)
            //                {
            //                    obj.StartTime1 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
            //                {
            //                    obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                }
            //            }
            //            else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
            //            {
            //                if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
            //                {
            //                    obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay)
            //                {
            //                    obj.StartTime1 = "";
            //                }
            //            }
            //        }
            //        else if (obj.DayNumber == objLeave.LastOrDefault().DayNumber && objLeave.LastOrDefault().Date == selectedDate)
            //        {
            //            DateTime endTime = DateTime.Parse(String.Format("2012-01-28 {0}:15", objLeave.LastOrDefault().EndTime));
            //            endTime = endTime.AddMinutes(15);

            //            DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.LastOrDefault().EndTime);
            //            DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
            //            DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
            //            DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
            //            DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
            //            DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

            //            DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
            //            DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
            //            DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
            //            DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
            //            DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

            //            if (obj.StartTime5 != "")
            //            {
            //                if (endTimeDefault.TimeOfDay >= objStart5.TimeOfDay)
            //                {
            //                    obj.StartTime5 = endTime.ToString("HH:mm");
            //                    obj.StartTime4 = "";
            //                    obj.EndTime4 = "";
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart4.TimeOfDay)
            //                {
            //                    obj.StartTime4 = endTime.ToString("HH:mm");
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
            //                {
            //                    obj.StartTime3 = endTime.ToString("HH:mm");
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
            //                {
            //                    obj.StartTime2 = endTime.ToString("HH:mm");
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
            //                {
            //                    obj.StartTime1 = endTime.ToString("HH:mm");
            //                }
            //            }
            //            else if (obj.StartTime4 != "" && obj.StartTime5 == "")
            //            {
            //                if (endTimeDefault.TimeOfDay >= objStart4.TimeOfDay)
            //                {
            //                    obj.StartTime4 = endTime.ToString("HH:mm");
            //                    obj.StartTime3 = "";
            //                    obj.EndTime3 = "";
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
            //                {
            //                    obj.StartTime3 = endTime.ToString("HH:mm");
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
            //                {
            //                    obj.StartTime2 = endTime.ToString("HH:mm");
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
            //                {
            //                    obj.StartTime1 = endTime.ToString("HH:mm");
            //                }
            //            }
            //            else if (obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
            //            {
            //                if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
            //                {
            //                    obj.StartTime3 = endTime.ToString("HH:mm");
            //                    obj.StartTime2 = "";
            //                    obj.EndTime2 = "";
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
            //                {
            //                    obj.StartTime2 = endTime.ToString("HH:mm");
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
            //                {
            //                    obj.StartTime1 = endTime.ToString("HH:mm");
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
            //                {
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //            }
            //            else if (obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
            //            {
            //                if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
            //                {
            //                    obj.StartTime2 = endTime.ToString("HH:mm");
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
            //                {
            //                    obj.StartTime1 = endTime.ToString("HH:mm");
            //                }
            //            }
            //            else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
            //            {
            //                if (objStart1.TimeOfDay <= endTimeDefault.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
            //                {
            //                    obj.StartTime1 = endTime.ToString("HH:mm");
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
            //                {
            //                    obj.StartTime1 = "";
            //                    obj.EndTime1 = "";
            //                }
            //            }
            //        }
            //        else
            //        {
            //            List<GetParamedicLeaveScheduleCompare> c = objLeave.Where(t => t.Date == selectedDate).ToList();
            //            if (c.Count > 0)
            //            {
            //                obj.StartTime1 = "";
            //                obj.StartTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.StartTime5 = "";

            //                obj.EndTime1 = "";
            //                obj.EndTime2 = "";
            //                obj.EndTime3 = "";
            //                obj.EndTime4 = "";
            //                obj.EndTime5 = "";
            //            }
            //        }
            //    }
            //    #endregion

            //    #region set time slot Paramedic Schedule Date
            //    if (objSchDate != null)
            //    {
            //        DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
            //        DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
            //        DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
            //        DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
            //        DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

            //        DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
            //        DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
            //        DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
            //        DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
            //        DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

            //        if (objSchDate.ScheduleDate == objLeave.FirstOrDefault().Date)
            //        {
            //            DateTime startTimeDefault = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.FirstOrDefault().StartTime));
            //            if (objSchDate.StartTime5 != "")
            //            {

            //                if (startTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay)
            //                {
            //                    objSchDate.EndTime5 = startTimeDefault.ToString("HH:mm");
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //                {
            //                    objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime5 = "";
            //                    objSchDate.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart5.TimeOfDay)
            //                {
            //                    objSchDate.StartTime5 = "";
            //                    objSchDate.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
            //                {
            //                    objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                    objSchDate.StartTime5 = "";
            //                    objSchDate.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart4.TimeOfDay)
            //                {
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                    objSchDate.StartTime5 = "";
            //                    objSchDate.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
            //                {
            //                    objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                    objSchDate.StartTime5 = "";
            //                    objSchDate.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay)
            //                {
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                    objSchDate.StartTime5 = "";
            //                    objSchDate.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
            //                {
            //                    objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                    objSchDate.StartTime5 = "";
            //                    objSchDate.EndTime5 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay)
            //                {
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                    objSchDate.StartTime5 = "";
            //                    objSchDate.EndTime5 = "";
            //                }
            //            }
            //            else if (objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
            //            {
            //                if (startTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //                {
            //                    objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
            //                {
            //                    objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart4.TimeOfDay)
            //                {
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
            //                {
            //                    objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay)
            //                {
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
            //                {
            //                    objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay)
            //                {
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                }
            //            }
            //            else if (objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
            //            {
            //                if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
            //                {
            //                    objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
            //                {
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay)
            //                {
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
            //                {
            //                    objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay)
            //                {
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
            //                {
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
            //                {
            //                    objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                }
            //            }
            //            else if (objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
            //            {
            //                if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
            //                {
            //                    objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
            //                {
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
            //                {
            //                    objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay)
            //                {
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
            //                {
            //                    objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                }
            //            }
            //            else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
            //            {
            //                if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
            //                {
            //                    objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                }
            //                else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay)
            //                {
            //                    objSchDate.StartTime1 = "";
            //                }
            //            }
            //        }
            //        else if (objSchDate.ScheduleDate == objLeave.LastOrDefault().Date)
            //        {
            //            DateTime endTime = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.LastOrDefault().EndTime));
            //            endTime = endTime.AddMinutes(15);

            //            DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.LastOrDefault().EndTime);

            //            if (objSchDate.StartTime5 != "")
            //            {

            //                if (endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay)
            //                {
            //                    objSchDate.StartTime5 = endTime.ToString("HH:mm");
            //                    objSchDate.StartTime4 = "";
            //                    objSchDate.EndTime4 = "";
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay)
            //                {
            //                    objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
            //                {
            //                    objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
            //                {
            //                    objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
            //                {
            //                    objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                }
            //            }
            //            else if (objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
            //            {
            //                if (endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay)
            //                {
            //                    objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //                    objSchDate.StartTime3 = "";
            //                    objSchDate.EndTime3 = "";
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
            //                {
            //                    objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
            //                {
            //                    objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
            //                {
            //                    objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                }
            //            }
            //            else if (objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
            //            {
            //                if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
            //                {
            //                    objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                    objSchDate.StartTime2 = "";
            //                    objSchDate.EndTime2 = "";
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
            //                {
            //                    objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
            //                {
            //                    objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                }
            //            }
            //            else if (objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
            //            {
            //                if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
            //                {
            //                    objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
            //                {
            //                    objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                }
            //            }
            //            else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
            //            {
            //                if (objSchStart1.TimeOfDay <= endTimeDefault.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
            //                {
            //                    objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                }
            //                else if (endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
            //                {
            //                    objSchDate.StartTime1 = "";
            //                    objSchDate.EndTime1 = "";
            //                }
            //            }
            //        }
            //        else
            //        {
            //            List<GetParamedicLeaveScheduleCompare> c = objLeave.Where(t => t.Date == selectedDate).ToList();
            //            if (c.Count > 0)
            //            {
            //                objSchDate.StartTime1 = "";
            //                objSchDate.StartTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.StartTime5 = "";

            //                objSchDate.EndTime1 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //        }
            //    }
            //    #endregion
            //}
            //#endregion
            //#region if leave only in one day
            //else if (objLeave.FirstOrDefault().DayNumber != 0 && objLeave.Count() == 1)
            //{
            //    #region set time slot Paramedic Schedule
            //    if (obj != null)
            //    {
            //        DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);
            //        DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().EndTime);

            //        DateTime startTime = startTimeDefault.AddMinutes(15);
            //        DateTime endTime = endTimeDefault.AddMinutes(15);

            //        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
            //        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
            //        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
            //        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
            //        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

            //        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
            //        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
            //        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
            //        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
            //        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

            //        if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
            //        {
            //            if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
            //            {
            //                obj.StartTime1 = startTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay) //1/2
            //            {
            //                //obj = null;
            //                obj.StartTime1 = "";
            //                obj.EndTime1 = "";
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay) //1/2
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
            //            {
            //                obj.EndTime2 = obj.EndTime1;
            //                obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //                obj.StartTime2 = endTime.ToString("HH:mm");

            //                obj.IsAllowWaitingList2 = obj.IsAllowWaitingList1;
            //                obj.MaximumWaitingList2 = obj.MaximumWaitingList1;

            //                obj.IsAppointmentByTimeSlot2 = obj.IsAppointmentByTimeSlot1;
            //                obj.MaximumAppointment2 = obj.MaximumAppointment1;
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay) //4
            //            {
            //                obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //            }
            //        }
            //        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
            //        {
            //            if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
            //            {
            //                obj.StartTime1 = startTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //2 modif
            //            {
            //                //obj = null;
            //                obj.StartTime1 = "";
            //                obj.EndTime1 = "";
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //2 modif
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //2 modif
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
            //            {
            //                obj.StartTime3 = obj.StartTime2;
            //                obj.EndTime3 = obj.EndTime2;
            //                obj.EndTime2 = obj.EndTime1;
            //                obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //                obj.StartTime2 = endTime.ToString("HH:mm");

            //                obj.IsAllowWaitingList3 = obj.IsAllowWaitingList2;
            //                obj.MaximumWaitingList3 = obj.MaximumWaitingList2;

            //                obj.IsAppointmentByTimeSlot3 = obj.IsAppointmentByTimeSlot2;
            //                obj.MaximumAppointment3 = obj.MaximumAppointment2;
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //4
            //            {
            //                obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
            //            {
            //                DateTime start2 = objStart2.AddMinutes(15);
            //                obj.StartTime1 = start2.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //9
            //            {
            //                //obj = null;
            //                obj.StartTime1 = "";
            //                obj.EndTime1 = "";
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //13
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime2;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //16
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
            //            {
            //                obj.EndTime3 = obj.EndTime2;
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = endTime.ToString("HH:mm");

            //                obj.IsAllowWaitingList3 = obj.IsAllowWaitingList2;
            //                obj.MaximumWaitingList3 = obj.MaximumWaitingList2;

            //                obj.IsAppointmentByTimeSlot3 = obj.IsAppointmentByTimeSlot2;
            //                obj.MaximumAppointment3 = obj.MaximumAppointment2;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //19
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //20
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //        }
            //        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
            //        {
            //            if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
            //            {
            //                obj.StartTime1 = startTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = obj.StartTime3;
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
            //            {
            //                //obj = null;
            //                obj.StartTime1 = "";
            //                obj.EndTime1 = "";
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
            //            {
            //                obj.StartTime4 = obj.StartTime3;
            //                obj.EndTime4 = obj.EndTime3;
            //                obj.StartTime3 = obj.StartTime2;
            //                obj.EndTime3 = obj.EndTime2;
            //                obj.EndTime2 = obj.EndTime1;
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");

            //                obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
            //                obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

            //                obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
            //                obj.MaximumAppointment4 = obj.MaximumAppointment3;
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
            //            {
            //                obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
            //            {
            //                DateTime start2 = objStart2.AddMinutes(15);
            //                obj.StartTime1 = start2.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
            //            {
            //                obj.StartTime1 = obj.StartTime3;
            //                obj.EndTime1 = obj.EndTime3;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime2;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
            //            {
            //                obj.StartTime4 = obj.StartTime3;
            //                obj.EndTime4 = obj.EndTime3;
            //                obj.EndTime3 = obj.EndTime2;
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = endTime.ToString("HH:mm");

            //                obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
            //                obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

            //                obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
            //                obj.MaximumAppointment4 = obj.MaximumAppointment3;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //21
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime3;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //22
            //            {
            //                //obj = null;
            //                obj.StartTime1 = "";
            //                obj.EndTime1 = "";
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay) //24
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay) //26
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //28
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //30
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
            //            {
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
            //            {
            //                obj.EndTime4 = obj.EndTime3;
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime4 = endTime.ToString("HH:mm");

            //                obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
            //                obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

            //                obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
            //                obj.MaximumAppointment4 = obj.MaximumAppointment3;
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //33
            //            {
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //34
            //            {
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
            //            {
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //        }
            //        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
            //        {
            //            if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
            //            {
            //                obj.StartTime1 = startTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = obj.StartTime3;
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
            //            {
            //                //obj = null;
            //                obj.StartTime1 = "";
            //                obj.EndTime1 = "";
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd3.TimeOfDay)
            //            {
            //                obj.StartTime1 = obj.StartTime4;
            //                obj.EndTime1 = obj.EndTime4;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime1 = obj.StartTime4;
            //                obj.EndTime1 = obj.EndTime4;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime4;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
            //            {
            //                obj.StartTime4 = obj.StartTime3;
            //                obj.EndTime4 = obj.EndTime3;
            //                obj.StartTime3 = obj.StartTime2;
            //                obj.EndTime3 = obj.EndTime2;
            //                obj.EndTime2 = obj.EndTime1;
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
            //            {
            //                obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
            //            {
            //                DateTime start2 = objStart2.AddMinutes(15);
            //                obj.StartTime1 = start2.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
            //            {
            //                obj.StartTime1 = obj.StartTime3;
            //                obj.EndTime1 = obj.EndTime3;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime2;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
            //            {
            //                obj.StartTime4 = obj.StartTime3;
            //                obj.EndTime4 = obj.EndTime3;
            //                obj.EndTime3 = obj.EndTime2;
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //21
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime3;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //22
            //            {
            //                //obj = null;
            //                obj.StartTime1 = "";
            //                obj.EndTime1 = "";
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //24
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = obj.StartTime4;
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = obj.StartTime4;
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay) //26
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay) //28
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //30
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = obj.StartTime4;
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
            //            {
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
            //            {
            //                obj.StartTime5 = obj.StartTime4;
            //                obj.EndTime5 = obj.EndTime4;
            //                obj.EndTime4 = obj.EndTime3;
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime4 = endTime.ToString("HH:mm");

            //                obj.IsAllowWaitingList5 = obj.IsAllowWaitingList4;
            //                obj.MaximumWaitingList5 = obj.MaximumWaitingList4;

            //                obj.IsAppointmentByTimeSlot5 = obj.IsAppointmentByTimeSlot4;
            //                obj.MaximumAppointment5 = obj.MaximumAppointment4;
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay) //33
            //            {
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //34
            //            {
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime2 = obj.StartTime4;
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime3 = obj.StartTime4;
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objStart4.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime5 = obj.EndTime4;
            //                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime5 = endTime.ToString("HH:mm");

            //                obj.IsAllowWaitingList5 = obj.IsAllowWaitingList4;
            //                obj.MaximumWaitingList5 = obj.MaximumWaitingList4;

            //                obj.IsAppointmentByTimeSlot5 = obj.IsAppointmentByTimeSlot4;
            //                obj.MaximumAppointment5 = obj.MaximumAppointment4;
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime2 = obj.StartTime4;
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime3 = obj.StartTime4;
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime3 = obj.StartTime4;
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //        }
            //        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
            //        {
            //            if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
            //            {
            //                obj.StartTime1 = startTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = obj.StartTime3;
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                //obj = null;
            //                obj.StartTime1 = "";
            //                obj.EndTime1 = "";
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
            //            {
            //                obj.StartTime1 = obj.StartTime5;
            //                obj.EndTime1 = obj.EndTime5;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd3.TimeOfDay)
            //            {
            //                obj.StartTime1 = obj.StartTime4;
            //                obj.EndTime1 = obj.EndTime4;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime1 = obj.StartTime4;
            //                obj.EndTime1 = obj.EndTime4;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime4;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
            //            {
            //                obj.StartTime4 = obj.StartTime3;
            //                obj.EndTime4 = obj.EndTime3;
            //                obj.StartTime3 = obj.StartTime2;
            //                obj.EndTime3 = obj.EndTime2;
            //                obj.EndTime2 = obj.EndTime1;
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
            //            {
            //                obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
            //            {
            //                DateTime start2 = objStart2.AddMinutes(15);
            //                obj.StartTime1 = start2.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
            //            {
            //                obj.StartTime1 = obj.StartTime3;
            //                obj.EndTime1 = obj.EndTime3;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
            //            {
            //                obj.StartTime1 = obj.StartTime2;
            //                obj.EndTime1 = obj.EndTime2;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime2;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
            //            {
            //                obj.StartTime4 = obj.StartTime3;
            //                obj.EndTime4 = obj.EndTime3;
            //                obj.EndTime3 = obj.EndTime2;
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //21
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime3;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //24
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = obj.StartTime4;
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = obj.StartTime4;
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //26
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime3;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //28
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //30
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = obj.StartTime4;
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
            //            {
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
            //            {
            //                obj.StartTime5 = obj.StartTime4;
            //                obj.EndTime5 = obj.EndTime4;
            //                obj.EndTime4 = obj.EndTime3;
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //33
            //            {
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //34
            //            {
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime2 = obj.StartTime4;
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime3 = obj.StartTime4;
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objStart4.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
            //            {
            //                obj.EndTime5 = obj.EndTime4;
            //                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime5 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime2 = obj.StartTime4;
            //                obj.EndTime2 = obj.EndTime4;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime3 = obj.StartTime4;
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
            //            {
            //                obj.StartTime3 = obj.StartTime4;
            //                obj.EndTime3 = obj.EndTime4;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay == objStart5.TimeOfDay)
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime5;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime1 = endTime.ToString("HH:mm");
            //                obj.EndTime1 = obj.EndTime5;
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime5;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
            //            {
            //                obj.StartTime2 = obj.StartTime5;
            //                obj.EndTime2 = obj.EndTime5;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime5;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime2 = endTime.ToString("HH:mm");
            //                obj.EndTime2 = obj.EndTime5;
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime2 = "";
            //                obj.EndTime2 = "";
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //                obj.EndTime3 = obj.EndTime5;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime3 = "";
            //                obj.EndTime3 = "";
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime3 = endTime.ToString("HH:mm");
            //                obj.EndTime3 = obj.EndTime5;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
            //            {
            //                obj.StartTime3 = obj.StartTime5;
            //                obj.EndTime3 = obj.EndTime5;
            //                obj.StartTime4 = "";
            //                obj.EndTime4 = "";
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
            //            {
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime4 = obj.StartTime5;
            //                obj.EndTime4 = obj.EndTime5;
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //                obj.EndTime4 = obj.EndTime5;
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
            //            {
            //                obj.StartTime4 = obj.StartTime5;
            //                obj.EndTime4 = obj.EndTime5;
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //                obj.EndTime4 = obj.EndTime5;
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
            //            {
            //                obj.StartTime4 = obj.StartTime5;
            //                obj.EndTime4 = obj.EndTime5;
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //                obj.EndTime4 = obj.EndTime5;
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
            //            {
            //                obj.StartTime4 = obj.StartTime5;
            //                obj.EndTime4 = obj.EndTime5;
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime4 = endTime.ToString("HH:mm");
            //                obj.EndTime4 = obj.EndTime5;
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
            //            {
            //                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                obj.StartTime5 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime5 = endTime.ToString("HH:mm");
            //                obj.EndTime5 = obj.EndTime5;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart5.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime5 = startTime.ToString("HH:mm");
            //                obj.EndTime5 = endTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
            //            {
            //                obj.StartTime5 = "";
            //                obj.EndTime5 = "";
            //            }
            //        }
            //    }
            //    #endregion

            //    #region set time slot Paramedic Schedule Date
            //    if (objSchDate != null)
            //    {
            //        DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);
            //        DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().EndTime);

            //        DateTime startTime = startTimeDefault.AddMinutes(15);
            //        DateTime endTime = endTimeDefault.AddMinutes(15);

            //        DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
            //        DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
            //        DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
            //        DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
            //        DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

            //        DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
            //        DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
            //        DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
            //        DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
            //        DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

            //        if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
            //        {
            //            if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
            //            {
            //                objSchDate.StartTime1 = startTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay) //1/2
            //            {
            //                //objSchDate = null;
            //                objSchDate.StartTime1 = "";
            //                objSchDate.EndTime1 = "";
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay) //1/2
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
            //            {
            //                objSchDate.EndTime2 = objSchDate.EndTime1;
            //                objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");

            //                objSchDate.IsAllowWaitingList2 = objSchDate.IsAllowWaitingList1;
            //                objSchDate.MaximumWaitingList2 = objSchDate.MaximumWaitingList1;

            //                objSchDate.IsAppointmentByTimeSlot2 = objSchDate.IsAppointmentByTimeSlot1;
            //                objSchDate.MaximumAppointment2 = objSchDate.MaximumAppointment1;
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay) //4
            //            {
            //                objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //            }
            //        }
            //        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
            //        {
            //            if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
            //            {
            //                objSchDate.StartTime1 = startTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //2 modif
            //            {
            //                //objSchDate = null;
            //                objSchDate.StartTime1 = "";
            //                objSchDate.EndTime1 = "";
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //2 modif
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //2 modif
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
            //            {
            //                objSchDate.StartTime3 = objSchDate.StartTime2;
            //                objSchDate.EndTime3 = objSchDate.EndTime2;
            //                objSchDate.EndTime2 = objSchDate.EndTime1;
            //                objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");

            //                objSchDate.IsAllowWaitingList3 = objSchDate.IsAllowWaitingList2;
            //                objSchDate.MaximumWaitingList3 = objSchDate.MaximumWaitingList2;

            //                objSchDate.IsAppointmentByTimeSlot3 = objSchDate.IsAppointmentByTimeSlot2;
            //                objSchDate.MaximumAppointment3 = objSchDate.MaximumAppointment2;
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //4
            //            {
            //                objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
            //            {
            //                DateTime start2 = objSchStart2.AddMinutes(15);
            //                objSchDate.StartTime1 = start2.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //9
            //            {
            //                //objSchDate = null;
            //                objSchDate.StartTime1 = "";
            //                objSchDate.EndTime1 = "";
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //13
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime2;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //16
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
            //            {
            //                objSchDate.EndTime3 = objSchDate.EndTime2;
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");

            //                objSchDate.IsAllowWaitingList3 = objSchDate.IsAllowWaitingList2;
            //                objSchDate.MaximumWaitingList3 = objSchDate.MaximumWaitingList2;

            //                objSchDate.IsAppointmentByTimeSlot3 = objSchDate.IsAppointmentByTimeSlot2;
            //                objSchDate.MaximumAppointment3 = objSchDate.MaximumAppointment2;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //19
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //20
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //        }
            //        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
            //        {
            //            if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
            //            {
            //                objSchDate.StartTime1 = startTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = objSchDate.StartTime3;
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
            //            {
            //                //objSchDate = null;
            //                objSchDate.StartTime1 = "";
            //                objSchDate.EndTime1 = "";
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
            //            {
            //                objSchDate.StartTime4 = objSchDate.StartTime3;
            //                objSchDate.EndTime4 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = objSchDate.StartTime2;
            //                objSchDate.EndTime3 = objSchDate.EndTime2;
            //                objSchDate.EndTime2 = objSchDate.EndTime1;
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");

            //                objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
            //                objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

            //                objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
            //                objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
            //            {
            //                objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
            //            {
            //                DateTime start2 = objSchStart2.AddMinutes(15);
            //                objSchDate.StartTime1 = start2.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime3;
            //                objSchDate.EndTime1 = objSchDate.EndTime3;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime2;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
            //            {
            //                objSchDate.StartTime4 = objSchDate.StartTime3;
            //                objSchDate.EndTime4 = objSchDate.EndTime3;
            //                objSchDate.EndTime3 = objSchDate.EndTime2;
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");

            //                objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
            //                objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

            //                objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
            //                objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //21
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime3;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //22
            //            {
            //                //objSchDate = null;
            //                objSchDate.StartTime1 = "";
            //                objSchDate.EndTime1 = "";
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //24
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //28
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //30
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
            //            {
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
            //            {
            //                objSchDate.EndTime4 = objSchDate.EndTime3;
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");

            //                objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
            //                objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

            //                objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
            //                objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //33
            //            {
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //34
            //            {
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //        }
            //        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
            //        {
            //            if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
            //            {
            //                objSchDate.StartTime1 = startTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = objSchDate.StartTime3;
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
            //            {
            //                //objSchDate = null;
            //                objSchDate.StartTime1 = "";
            //                objSchDate.EndTime1 = "";
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime4;
            //                objSchDate.EndTime1 = objSchDate.EndTime4;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime4;
            //                objSchDate.EndTime1 = objSchDate.EndTime4;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime4;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
            //            {
            //                objSchDate.StartTime4 = objSchDate.StartTime3;
            //                objSchDate.EndTime4 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = objSchDate.StartTime2;
            //                objSchDate.EndTime3 = objSchDate.EndTime2;
            //                objSchDate.EndTime2 = objSchDate.EndTime1;
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
            //            {
            //                objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
            //            {
            //                DateTime start2 = objSchStart2.AddMinutes(15);
            //                objSchDate.StartTime1 = start2.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime3;
            //                objSchDate.EndTime1 = objSchDate.EndTime3;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime2;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
            //            {
            //                objSchDate.StartTime4 = objSchDate.StartTime3;
            //                objSchDate.EndTime4 = objSchDate.EndTime3;
            //                objSchDate.EndTime3 = objSchDate.EndTime2;
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //21
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime3;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //22
            //            {
            //                //objSchDate = null;
            //                objSchDate.StartTime1 = "";
            //                objSchDate.EndTime1 = "";
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //24
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = objSchDate.StartTime4;
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = objSchDate.StartTime4;
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay) //26
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay) //28
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //30
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = objSchDate.StartTime4;
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
            //            {
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
            //            {
            //                objSchDate.StartTime5 = objSchDate.StartTime4;
            //                objSchDate.EndTime5 = objSchDate.EndTime4;
            //                objSchDate.EndTime4 = objSchDate.EndTime3;
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");

            //                objSchDate.IsAllowWaitingList5 = objSchDate.IsAllowWaitingList4;
            //                objSchDate.MaximumWaitingList5 = objSchDate.MaximumWaitingList4;

            //                objSchDate.IsAppointmentByTimeSlot5 = objSchDate.IsAppointmentByTimeSlot4;
            //                objSchDate.MaximumAppointment5 = objSchDate.MaximumAppointment4;
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay) //33
            //            {
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //34
            //            {
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = objSchDate.StartTime4;
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = objSchDate.StartTime4;
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime5 = objSchDate.EndTime4;
            //                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime5 = endTime.ToString("HH:mm");

            //                objSchDate.IsAllowWaitingList5 = objSchDate.IsAllowWaitingList4;
            //                objSchDate.MaximumWaitingList5 = objSchDate.MaximumWaitingList4;

            //                objSchDate.IsAppointmentByTimeSlot5 = objSchDate.IsAppointmentByTimeSlot4;
            //                objSchDate.MaximumAppointment5 = objSchDate.MaximumAppointment4;
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = objSchDate.StartTime4;
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = objSchDate.StartTime4;
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = objSchDate.StartTime4;
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //        }
            //        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
            //        {
            //            if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
            //            {
            //                objSchDate.StartTime1 = startTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = objSchDate.StartTime3;
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                //objSchDate = null;
            //                objSchDate.StartTime1 = "";
            //                objSchDate.EndTime1 = "";
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime5;
            //                objSchDate.EndTime1 = objSchDate.EndTime5;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime4;
            //                objSchDate.EndTime1 = objSchDate.EndTime4;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime4;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
            //            {
            //                objSchDate.StartTime4 = objSchDate.StartTime3;
            //                objSchDate.EndTime4 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = objSchDate.StartTime2;
            //                objSchDate.EndTime3 = objSchDate.EndTime2;
            //                objSchDate.EndTime2 = objSchDate.EndTime1;
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
            //            {
            //                objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
            //            {
            //                DateTime start2 = objSchStart2.AddMinutes(15);
            //                objSchDate.StartTime1 = start2.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime3;
            //                objSchDate.EndTime1 = objSchDate.EndTime3;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
            //            {
            //                objSchDate.StartTime1 = objSchDate.StartTime2;
            //                objSchDate.EndTime1 = objSchDate.EndTime2;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime2;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
            //            {
            //                objSchDate.StartTime4 = objSchDate.StartTime3;
            //                objSchDate.EndTime4 = objSchDate.EndTime3;
            //                objSchDate.EndTime3 = objSchDate.EndTime2;
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //21
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime3;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //24
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = objSchDate.StartTime4;
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = objSchDate.StartTime4;
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //26
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime3;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //28
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //30
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = objSchDate.StartTime4;
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
            //            {
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
            //            {
            //                objSchDate.StartTime5 = objSchDate.StartTime4;
            //                objSchDate.EndTime5 = objSchDate.EndTime4;
            //                objSchDate.EndTime4 = objSchDate.EndTime3;
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //33
            //            {
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //34
            //            {
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = objSchDate.StartTime4;
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = objSchDate.StartTime4;
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
            //            {
            //                objSchDate.EndTime5 = objSchDate.EndTime4;
            //                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime5 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = objSchDate.StartTime4;
            //                objSchDate.EndTime2 = objSchDate.EndTime4;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = objSchDate.StartTime4;
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = objSchDate.StartTime4;
            //                objSchDate.EndTime3 = objSchDate.EndTime4;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart5.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime5;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime1 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime1 = objSchDate.EndTime5;
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime5;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = objSchDate.StartTime5;
            //                objSchDate.EndTime2 = objSchDate.EndTime5;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime5;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime2 = objSchDate.EndTime5;
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime2 = "";
            //                objSchDate.EndTime2 = "";
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime3 = objSchDate.EndTime5;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime3 = "";
            //                objSchDate.EndTime3 = "";
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime3 = objSchDate.EndTime5;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
            //            {
            //                objSchDate.StartTime3 = objSchDate.StartTime5;
            //                objSchDate.EndTime3 = objSchDate.EndTime5;
            //                objSchDate.StartTime4 = "";
            //                objSchDate.EndTime4 = "";
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
            //            {
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime4 = objSchDate.StartTime5;
            //                objSchDate.EndTime4 = objSchDate.EndTime5;
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime4 = objSchDate.EndTime5;
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = objSchDate.StartTime5;
            //                objSchDate.EndTime4 = objSchDate.EndTime5;
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime4 = objSchDate.EndTime5;
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = objSchDate.StartTime5;
            //                objSchDate.EndTime4 = objSchDate.EndTime5;
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime4 = objSchDate.EndTime5;
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = objSchDate.StartTime5;
            //                objSchDate.EndTime4 = objSchDate.EndTime5;
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime4 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime4 = objSchDate.EndTime5;
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
            //            {
            //                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
            //                objSchDate.StartTime5 = endTime.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime5 = endTime.ToString("HH:mm");
            //                objSchDate.EndTime5 = objSchDate.EndTime5;
            //            }
            //            else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime5 = startTime.ToString("HH:mm");
            //                objSchDate.EndTime5 = endTimeDefault.ToString("HH:mm");
            //            }
            //            else if (startTimeDefault.TimeOfDay == objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
            //            {
            //                objSchDate.StartTime5 = "";
            //                objSchDate.EndTime5 = "";
            //            }
            //        }
            //    }
            //    #endregion
            //}
            //#endregion
            //#endregion
        }

     }
}