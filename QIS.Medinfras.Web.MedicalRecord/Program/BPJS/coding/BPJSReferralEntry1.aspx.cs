using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Text;
using System.Net;
using QIS.Medinfras.Web.CommonLibs.Service;


namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class BPJSReferralEntry1 : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.BPJS_PROSES_RUJUKAN;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected string RegistrationDateTime = "";
        protected override void InitializeDataControl()
        {
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            List<StandardCode> lstJenisPelayanan = new List<StandardCode>();
            lstJenisPelayanan.Add(new StandardCode() { StandardCodeID = "1", StandardCodeName = "1 - Rawat Inap" });
            lstJenisPelayanan.Add(new StandardCode() { StandardCodeID = "2", StandardCodeName = "2 - Rawat Jalan" });
            Methods.SetComboBoxField<StandardCode>(cboJenisPelayanan, lstJenisPelayanan, "StandardCodeName", "StandardCodeID");

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}')", Constant.StandardCode.DISCHARGE_ROUTINE, Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.TIPE_RUJUKAN_BPJS);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRefferalType, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.TIPE_RUJUKAN_BPJS || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");

            Helper.SetControlEntrySetting(txtNoPeserta, new ControlEntrySetting(false, false, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtNomorSEP, new ControlEntrySetting(false, false, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeTime, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboDischargeRoutine, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            EntityToControl(entity);
            if (entity.GCDischargeCondition == "" && entity.GCDischargeMethod == "")
            {
                txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
        }

        private void EntityToControl(ConsultVisit entity)
        {
            txtVisitDate.Text = entity.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtVisitTime.Text = entity.ActualVisitTime;
            cboPatientOutcome.Value = entity.GCDischargeCondition;
            cboDischargeRoutine.Value = entity.GCDischargeMethod;
            txtDischargeDate.Text = entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDischargeTime.Text = entity.DischargeTime;
            txtLOS.Text = entity.LOSInDay <= 0 ? "1" : entity.LOSInDay.ToString();

            //Get SEP Information
            string filterExpression = string.Format("RegistrationID = {0}", entity.RegistrationID);
            vRegistrationBPJS regBPJS = BusinessLayer.GetvRegistrationBPJSList(filterExpression).FirstOrDefault();
            if (regBPJS != null)
            {
                txtNoPeserta.Text = regBPJS.NHSRegistrationNo;
                txtNomorSEP.Text = regBPJS.NoSEP;
                txtJenisRawat.Text = AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT ? "1 - Rawat Inap" : "2 - Rawat Jalan";
                hdnJnsRawat.Value = AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT ? "1" : "2";
                hdnKlsRawat.Value = regBPJS.KelasTanggungan;
                txtKelasTanggungan.Text = String.Format("Kelas {0}", regBPJS.KelasTanggungan);

                if (entity.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT || entity.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
                {
                    if (entity.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT)
                    {
                        vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entity.ReferralUnitID)).FirstOrDefault();
                        if (hsu != null)
                        {
                            if (!string.IsNullOrEmpty(hsu.BPJSPoli))
                            {
                                string[] unitInfo = hsu.BPJSPoli.Split('|');
                                txtRefferal.Text = hsu.ServiceUnitCode;
                                txtRefferalName.Text = hsu.ServiceUnitName;
                                txtRefferalDate.Text = entity.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                                txtKodePoli.Text = unitInfo[0];
                                txtNamaPoli.Text = unitInfo[1];

                            }
                        }
                    }
                    else
                    {
                        vReferrer reff = BusinessLayer.GetvReferrerList(string.Format("BusinessPartnerID = {0}", entity.ReferralTo)).FirstOrDefault();
                        if (reff != null)
                        {
                            if (!string.IsNullOrEmpty(reff.CommCode))
                            {
                                txtRefferal.Text = reff.CommCode;
                                txtRefferalName.Text = reff.BusinessPartnerName;
                                txtRefferalDate.Text = entity.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                            }
                        }
                    }
                }
                else
                {
                    txtRefferal.Text = string.Empty;
                    txtRefferalName.Text = string.Empty;
                    txtRefferalDate.Text = string.Empty;
                }

                //txtRefferal.Text = String.IsNullOrEmpty(regBPJS.NoRujukan) ? "Tidak Ada" : string.Format("Ada ({0})", regBPJS.NoRujukan);

                #region Patient Diagnosis
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID);
                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
                if (lstDiagnosis.Count > 0)
                {
                    StringBuilder strComplication = new StringBuilder();
                    string strDiagnosis = string.Empty;
                    int diagnoseCount = 0;
                    bool isMainDiagnosisExists = false;
                    foreach (vPatientDiagnosis diag in lstDiagnosis)
                    {
                        if (diag.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                        {
                            hdnMainDiagnoseCode.Value = diag.INACBGLabel.TrimEnd();
                            txtMainDiagnose.Text = string.Format("{0} ({1})", diag.DiagnoseName, diag.INACBGLabel.TrimEnd());
                            isMainDiagnosisExists = true;

                            txtDiagnoseCode.Text = hdnBPJSDiagnoseCodeCtl.Value = diag.INACBGLabel;
                            txtDiagnoseName.Text = diag.DiagnoseName;
                        }
                        else if (diag.GCDiagnoseType == Constant.DiagnoseType.EARLY_DIAGNOSIS && !isMainDiagnosisExists)
                        {
                            hdnMainDiagnoseCode.Value = diag.INACBGLabel.TrimEnd();
                            txtMainDiagnose.Text = string.Format("{0} ({1})", diag.DiagnoseName, diag.INACBGLabel.TrimEnd());
                            isMainDiagnosisExists = true;

                            txtDiagnoseCode.Text = hdnBPJSDiagnoseCodeCtl.Value = diag.INACBGLabel;
                            txtDiagnoseName.Text = diag.DiagnoseName;
                        }
                        else {
                            strComplication.AppendLine(string.Format("{0} ({1})", diag.DiagnoseName, diag.INACBGLabel.TrimEnd()));
                        }
                        strDiagnosis += diag.INACBGLabel.TrimEnd();
                        strDiagnosis += ";";
                        diagnoseCount += 1;
                    }
                    strDiagnosis = strDiagnosis.Substring(0, strDiagnosis.Length - 1);
                    //if (diagnoseCount < 30)
                    //{
                    //    for (int i = diagnoseCount; i < 30; i++)
                    //    {
                    //        strDiagnosis += ";";
                    //    }
                    //}
                    txtComplication.Text = strComplication.ToString();
                    hdnDiagnosis.Value = strDiagnosis;

                }
                #endregion

                #region Patient Procedures
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID);
                List<vPatientProcedure> lstProcedure = BusinessLayer.GetvPatientProcedureList(filterExpression);
                if (lstProcedure.Count > 0)
                {
                    StringBuilder strProcedures = new StringBuilder();
                    string strproc = string.Empty;
                    int procedureCount = 0;
                    foreach (vPatientProcedure proc in lstProcedure)
                    {
                        strProcedures.AppendLine(string.Format("{0} ({1})", proc.ProcedureName, proc.INACBGLabel.TrimEnd()));
                        strproc += proc.INACBGLabel.TrimEnd();
                        strproc += ";";
                        procedureCount += 1;
                    }
                    strproc = strproc.Substring(0, strproc.Length - 1);
                    //if (procedureCount < 30)
                    //{
                    //    for (int i = procedureCount; i < 30; i++)
                    //    {
                    //        strproc += ";";
                    //    }
                    //}
                    txtProcedures.Text = strProcedures.ToString();
                    hdnProcedures.Value = strproc;
                }
                else
                {
                    txtProcedures.Text = string.Empty;
                    hdnProcedures.Value = "";
                }
                #endregion

                #region Get Hospital Bill
                //filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus <> 'X121^999'", entity.RegistrationID);
                //List<vPatientBill> patientBills = BusinessLayer.GetvPatientBillList(filterExpression);
                //decimal payerAmount = 0;
                //if (patientBills.Count>0)
                //    payerAmount = patientBills.Sum(lst => lst.TotalPayerAmount);
                //if (payerAmount == 0)
                //{
                //   //Calculate from Patient Charges
                //    filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus <> '{1}'", entity.VisitID, Constant.TransactionStatus.VOID);
                //    List<vPatientChargesHd> chargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression);
                //    if (chargesHd.Count > 0)
                //        payerAmount = chargesHd.Sum(lst => lst.TotalPayerAmount);
                //}
                //txtTariffRS.Text = payerAmount.ToString(); 
                #endregion

                hdnGCBPJSClaimStatus.Value = regBPJS.GCBPJSClaimStatus;
            }
        }

        private string GetSpecialCMG_ADL(decimal los)
        {
            string result = string.Format("'{0}^{1}'", Constant.StandardCode.BPJS_SPECIAL_CMG_ADL, "0");

            if (los >= 1 && los <= 42)
            {
                result = string.Format("'{0}^{1}'", Constant.StandardCode.BPJS_SPECIAL_CMG_ADL, "1");
            }
            else if (los >= 43 && los < 102)
            {
                result = string.Format("'{0}^{1}'", Constant.StandardCode.BPJS_SPECIAL_CMG_ADL, "2");
            }
            else
            {
                result = string.Format("'{0}^{1}'", Constant.StandardCode.BPJS_SPECIAL_CMG_ADL, "3");
            }
            return result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string noSep = Request.Form[txtNomorSEP.UniqueID];
            DateTime tglMasuk = DateTime.ParseExact(Request.Form[txtVisitDate.UniqueID], Constant.FormatString.DATE_PICKER_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
            DateTime tglKeluar = DateTime.ParseExact(Request.Form[txtDischargeDate.UniqueID], Constant.FormatString.DATE_PICKER_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
            string jaminan = string.Empty;
            string poli = AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT ? string.Empty : Request.Form[txtKodePoli.UniqueID];
            string ruangRawat = string.Empty;
            string kelasRawat = AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT ? Request.Form[txtKelasTanggungan.UniqueID] : string.Empty;
            string spesialistik = string.Empty;
            string caraKeluar = string.Empty;
            string kondisiPulang = string.Empty;
            string tindakLanjut = string.Empty;
            string poliKontrol = string.Empty;
            DateTime tglKontrol = string.IsNullOrEmpty(Request.Form[txtRefferalDate.UniqueID]) ? DateTime.MinValue.Date : Helper.GetDatePickerValue(Request.Form[txtRefferalDate.UniqueID]);

            string mainDiagnosisCode = hdnMainDiagnoseCode.Value;
            string dpjp = string.Empty;
            string kodePPK = string.Empty;

            if (type == "propose")
            {
                try
                {
                    BPJSService oService = new BPJSService();
                    string apiResult = oService.InsertLPK(noSep, tglMasuk, tglKeluar, jaminan, poli, ruangRawat, kelasRawat, spesialistik, caraKeluar, kondisiPulang, mainDiagnosisCode, hdnDiagnosis.Value, hdnProcedures.Value, tindakLanjut, kodePPK, tglKontrol, poliKontrol, dpjp).ToString();
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        Exception ex = new Exception(apiResultInfo[1]);
                        errMessage = apiResultInfo[2];
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                    else
                    {
                        #region Update Registration BPJS
                        RegistrationBPJS regBPJS = BusinessLayer.GetRegistrationBPJS(AppSession.RegisteredPatient.RegistrationID);

                        if (regBPJS != null)
                        {

                            regBPJS.NoPeserta = txtNoPeserta.Text;

                            string[] lstDiagnosa = hdnDiagnosis.Value.ToString().Split(';');
                            regBPJS.DiagnosaUtama = lstDiagnosa[0];
                            regBPJS.D1 = lstDiagnosa[1];
                            regBPJS.D2 = lstDiagnosa[2];
                            regBPJS.D3 = lstDiagnosa[3];
                            regBPJS.D4 = lstDiagnosa[4];
                            regBPJS.D5 = lstDiagnosa[5];
                            regBPJS.D6 = lstDiagnosa[6];
                            regBPJS.D7 = lstDiagnosa[7];
                            regBPJS.D8 = lstDiagnosa[8];
                            regBPJS.D9 = lstDiagnosa[9];
                            regBPJS.D10 = lstDiagnosa[10];
                            regBPJS.D11 = lstDiagnosa[11];
                            regBPJS.D12 = lstDiagnosa[12];
                            regBPJS.D13 = lstDiagnosa[13];
                            regBPJS.D14 = lstDiagnosa[14];
                            regBPJS.D15 = lstDiagnosa[15];
                            regBPJS.D16 = lstDiagnosa[16];
                            regBPJS.D17 = lstDiagnosa[17];
                            regBPJS.D18 = lstDiagnosa[18];
                            regBPJS.D19 = lstDiagnosa[19];
                            regBPJS.D20 = lstDiagnosa[20];
                            regBPJS.D21 = lstDiagnosa[21];
                            regBPJS.D22 = lstDiagnosa[22];
                            regBPJS.D23 = lstDiagnosa[23];
                            regBPJS.D24 = lstDiagnosa[24];
                            regBPJS.D25 = lstDiagnosa[25];
                            regBPJS.D26 = lstDiagnosa[26];
                            regBPJS.D27 = lstDiagnosa[27];
                            regBPJS.D28 = lstDiagnosa[28];
                            regBPJS.D29 = lstDiagnosa[29];

                            string[] lstProcedure = hdnProcedures.Value.ToString().Split(';');
                            regBPJS.P1 = lstProcedure[0];
                            regBPJS.P2 = lstProcedure[1];
                            regBPJS.P3 = lstProcedure[2];
                            regBPJS.P4 = lstProcedure[3];
                            regBPJS.P5 = lstProcedure[4];
                            regBPJS.P6 = lstProcedure[5];
                            regBPJS.P7 = lstProcedure[6];
                            regBPJS.P8 = lstProcedure[7];
                            regBPJS.P9 = lstProcedure[8];
                            regBPJS.P10 = lstProcedure[9];
                            regBPJS.P11 = lstProcedure[10];
                            regBPJS.P12 = lstProcedure[11];
                            regBPJS.P13 = lstProcedure[12];
                            regBPJS.P14 = lstProcedure[13];
                            regBPJS.P15 = lstProcedure[14];
                            regBPJS.P16 = lstProcedure[15];
                            regBPJS.P17 = lstProcedure[16];
                            regBPJS.P18 = lstProcedure[17];
                            regBPJS.P19 = lstProcedure[18];
                            regBPJS.P20 = lstProcedure[19];
                            regBPJS.P21 = lstProcedure[20];
                            regBPJS.P22 = lstProcedure[21];
                            regBPJS.P23 = lstProcedure[22];
                            regBPJS.P24 = lstProcedure[23];
                            regBPJS.P25 = lstProcedure[24];
                            regBPJS.P26 = lstProcedure[25];
                            regBPJS.P27 = lstProcedure[26];
                            regBPJS.P28 = lstProcedure[27];
                            regBPJS.P29 = lstProcedure[28];
                            regBPJS.P30 = lstProcedure[29];

                            regBPJS.TariffRS = !string.IsNullOrEmpty(txtTariffRS.Text) ? Convert.ToDecimal(txtTariffRS.Text) : 0;

                            regBPJS.GCBPJSClaimStatus = Constant.BPJS_Claim_Status.PROSES_VERIFIKASI;
                            regBPJS.LastUpdatedDate = DateTime.Now;
                            regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;

                            regBPJS.NoRujukan = "";
                            BusinessLayer.UpdateRegistrationBPJS(regBPJS);
                        }
                        #endregion
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
            else if (type == "edit")
            {
                try
                {
                    BPJSService oService = new BPJSService();
                    string apiResult = oService.UpdateLPK(noSep, tglMasuk, tglKeluar, jaminan, poli, ruangRawat, kelasRawat, spesialistik, caraKeluar, kondisiPulang, mainDiagnosisCode, hdnDiagnosis.Value, hdnProcedures.Value, tindakLanjut, kodePPK, tglKontrol, poliKontrol, dpjp).ToString();
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        Exception ex = new Exception(apiResultInfo[1]);
                        errMessage = apiResultInfo[2];
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                    else
                    {
                        #region Update Registration BPJS
                        RegistrationBPJS regBPJS = BusinessLayer.GetRegistrationBPJS(AppSession.RegisteredPatient.RegistrationID);

                        if (regBPJS != null)
                        {

                            regBPJS.NoPeserta = txtNoPeserta.Text;

                            string[] lstDiagnosa = hdnDiagnosis.Value.ToString().Split(';');
                            regBPJS.DiagnosaUtama = lstDiagnosa[0];
                            regBPJS.D1 = lstDiagnosa[1];
                            regBPJS.D2 = lstDiagnosa[2];
                            regBPJS.D3 = lstDiagnosa[3];
                            regBPJS.D4 = lstDiagnosa[4];
                            regBPJS.D5 = lstDiagnosa[5];
                            regBPJS.D6 = lstDiagnosa[6];
                            regBPJS.D7 = lstDiagnosa[7];
                            regBPJS.D8 = lstDiagnosa[8];
                            regBPJS.D9 = lstDiagnosa[9];
                            regBPJS.D10 = lstDiagnosa[10];
                            regBPJS.D11 = lstDiagnosa[11];
                            regBPJS.D12 = lstDiagnosa[12];
                            regBPJS.D13 = lstDiagnosa[13];
                            regBPJS.D14 = lstDiagnosa[14];
                            regBPJS.D15 = lstDiagnosa[15];
                            regBPJS.D16 = lstDiagnosa[16];
                            regBPJS.D17 = lstDiagnosa[17];
                            regBPJS.D18 = lstDiagnosa[18];
                            regBPJS.D19 = lstDiagnosa[19];
                            regBPJS.D20 = lstDiagnosa[20];
                            regBPJS.D21 = lstDiagnosa[21];
                            regBPJS.D22 = lstDiagnosa[22];
                            regBPJS.D23 = lstDiagnosa[23];
                            regBPJS.D24 = lstDiagnosa[24];
                            regBPJS.D25 = lstDiagnosa[25];
                            regBPJS.D26 = lstDiagnosa[26];
                            regBPJS.D27 = lstDiagnosa[27];
                            regBPJS.D28 = lstDiagnosa[28];
                            regBPJS.D29 = lstDiagnosa[29];

                            string[] lstProcedure = hdnProcedures.Value.ToString().Split(';');
                            regBPJS.P1 = lstProcedure[0];
                            regBPJS.P2 = lstProcedure[1];
                            regBPJS.P3 = lstProcedure[2];
                            regBPJS.P4 = lstProcedure[3];
                            regBPJS.P5 = lstProcedure[4];
                            regBPJS.P6 = lstProcedure[5];
                            regBPJS.P7 = lstProcedure[6];
                            regBPJS.P8 = lstProcedure[7];
                            regBPJS.P9 = lstProcedure[8];
                            regBPJS.P10 = lstProcedure[9];
                            regBPJS.P11 = lstProcedure[10];
                            regBPJS.P12 = lstProcedure[11];
                            regBPJS.P13 = lstProcedure[12];
                            regBPJS.P14 = lstProcedure[13];
                            regBPJS.P15 = lstProcedure[14];
                            regBPJS.P16 = lstProcedure[15];
                            regBPJS.P17 = lstProcedure[16];
                            regBPJS.P18 = lstProcedure[17];
                            regBPJS.P19 = lstProcedure[18];
                            regBPJS.P20 = lstProcedure[19];
                            regBPJS.P21 = lstProcedure[20];
                            regBPJS.P22 = lstProcedure[21];
                            regBPJS.P23 = lstProcedure[22];
                            regBPJS.P24 = lstProcedure[23];
                            regBPJS.P25 = lstProcedure[24];
                            regBPJS.P26 = lstProcedure[25];
                            regBPJS.P27 = lstProcedure[26];
                            regBPJS.P28 = lstProcedure[27];
                            regBPJS.P29 = lstProcedure[28];
                            regBPJS.P30 = lstProcedure[29];

                            regBPJS.TariffRS = !string.IsNullOrEmpty(txtTariffRS.Text) ? Convert.ToDecimal(txtTariffRS.Text) : 0;

                            regBPJS.LastUpdatedDate = DateTime.Now;
                            regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateRegistrationBPJS(regBPJS);
                        }
                        #endregion
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }                
            }
            else if (type=="delete")
            {
                try
                {
                    BPJSService oService = new BPJSService();
                    string apiResult = oService.DeleteLPK(noSep).ToString();
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        Exception ex = new Exception(apiResultInfo[1]);
                        errMessage = apiResultInfo[2];
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                    else
                    {
                        #region Update Registration BPJS
                        RegistrationBPJS regBPJS = BusinessLayer.GetRegistrationBPJS(AppSession.RegisteredPatient.RegistrationID);

                        if (regBPJS != null)
                        {

                            regBPJS.NoPeserta = txtNoPeserta.Text;

                            string[] lstDiagnosa = hdnDiagnosis.Value.ToString().Split(';');
                            regBPJS.DiagnosaUtama = lstDiagnosa[0];
                            regBPJS.D1 = lstDiagnosa[1];
                            regBPJS.D2 = lstDiagnosa[2];
                            regBPJS.D3 = lstDiagnosa[3];
                            regBPJS.D4 = lstDiagnosa[4];
                            regBPJS.D5 = lstDiagnosa[5];
                            regBPJS.D6 = lstDiagnosa[6];
                            regBPJS.D7 = lstDiagnosa[7];
                            regBPJS.D8 = lstDiagnosa[8];
                            regBPJS.D9 = lstDiagnosa[9];
                            regBPJS.D10 = lstDiagnosa[10];
                            regBPJS.D11 = lstDiagnosa[11];
                            regBPJS.D12 = lstDiagnosa[12];
                            regBPJS.D13 = lstDiagnosa[13];
                            regBPJS.D14 = lstDiagnosa[14];
                            regBPJS.D15 = lstDiagnosa[15];
                            regBPJS.D16 = lstDiagnosa[16];
                            regBPJS.D17 = lstDiagnosa[17];
                            regBPJS.D18 = lstDiagnosa[18];
                            regBPJS.D19 = lstDiagnosa[19];
                            regBPJS.D20 = lstDiagnosa[20];
                            regBPJS.D21 = lstDiagnosa[21];
                            regBPJS.D22 = lstDiagnosa[22];
                            regBPJS.D23 = lstDiagnosa[23];
                            regBPJS.D24 = lstDiagnosa[24];
                            regBPJS.D25 = lstDiagnosa[25];
                            regBPJS.D26 = lstDiagnosa[26];
                            regBPJS.D27 = lstDiagnosa[27];
                            regBPJS.D28 = lstDiagnosa[28];
                            regBPJS.D29 = lstDiagnosa[29];

                            string[] lstProcedure = hdnProcedures.Value.ToString().Split(';');
                            regBPJS.P1 = lstProcedure[0];
                            regBPJS.P2 = lstProcedure[1];
                            regBPJS.P3 = lstProcedure[2];
                            regBPJS.P4 = lstProcedure[3];
                            regBPJS.P5 = lstProcedure[4];
                            regBPJS.P6 = lstProcedure[5];
                            regBPJS.P7 = lstProcedure[6];
                            regBPJS.P8 = lstProcedure[7];
                            regBPJS.P9 = lstProcedure[8];
                            regBPJS.P10 = lstProcedure[9];
                            regBPJS.P11 = lstProcedure[10];
                            regBPJS.P12 = lstProcedure[11];
                            regBPJS.P13 = lstProcedure[12];
                            regBPJS.P14 = lstProcedure[13];
                            regBPJS.P15 = lstProcedure[14];
                            regBPJS.P16 = lstProcedure[15];
                            regBPJS.P17 = lstProcedure[16];
                            regBPJS.P18 = lstProcedure[17];
                            regBPJS.P19 = lstProcedure[18];
                            regBPJS.P20 = lstProcedure[19];
                            regBPJS.P21 = lstProcedure[20];
                            regBPJS.P22 = lstProcedure[21];
                            regBPJS.P23 = lstProcedure[22];
                            regBPJS.P24 = lstProcedure[23];
                            regBPJS.P25 = lstProcedure[24];
                            regBPJS.P26 = lstProcedure[25];
                            regBPJS.P27 = lstProcedure[26];
                            regBPJS.P28 = lstProcedure[27];
                            regBPJS.P29 = lstProcedure[28];
                            regBPJS.P30 = lstProcedure[29];

                            regBPJS.TariffRS = !string.IsNullOrEmpty(txtTariffRS.Text) ? Convert.ToDecimal(txtTariffRS.Text) : 0;

                            regBPJS.GCBPJSClaimStatus = null;
                            regBPJS.LastUpdatedDate = DateTime.Now;
                            regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateRegistrationBPJS(regBPJS);
                        }
                        #endregion
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }                  
            }
            return false;
        }

        //public object InvokeLPKInsertAPI(int registrationID)
        //{
        //    string result = "";
        //    try
        //    {
        //        BPJSService oService = new BPJSService();
        //        string apiResult = oService.InsertLPK(entity, visitInfo, patientInfo);
        //        string[] apiResultInfo = apiResult.Split('|');
        //        if (apiResultInfo[0] == "0")
        //        {
        //            entityAPILog.IsSuccess = false;
        //            entityAPILog.MessageText = apiResultInfo[1];
        //            entityAPILog.Response = apiResultInfo[1];
        //            Exception ex = new Exception(apiResultInfo[1]);
        //            Helper.InsertErrorLog(ex);
        //        }

        //        LPK_SEP_Request_Param param = new LPK_SEP_Request_Param();

        //        param = GenerateRequestParam();

        //        //string filterExpression = string.Format("TransactionID = {0}", transactionID);
        //        //vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
        //        //if (oHeader != null)
        //        //{
        //        //    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
        //        //    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
        //        //    TestOrderDTO oData = new TestOrderDTO();
        //        //    if (oList.Count > 0)
        //        //    {
        //        //        string orderPriority = "NORMAL";
        //        //        string orderParamedicCode = oVisit.ParamedicCode;
        //        //        string orderParamedicName = oVisit.ParamedicName;
        //        //        DateTime orderDate = DateTime.Now.Date;
        //        //        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        //        //        if (testOrderID > 0)
        //        //        {
        //        //            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
        //        //            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";
        //        //            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : "";
        //        //            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : "";
        //        //            orderDate = oOrderHd.TestOrderDate;
        //        //            orderTime = oOrderHd.TestOrderTime;
        //        //        }

        //        //        oData.placerOrderNumber = oHeader.TransactionNo;
        //        //        oData.visitNumber = oVisit.RegistrationNo;
        //        //        oData.pointOfCare = oHeader.ServiceUnitName;
        //        //        oData.room = oVisit.RoomName;
        //        //        oData.bed = oVisit.BedCode;
        //        //        oData.orderDateTime = string.Format("{0} {1}:00", orderDate.ToString("yyyy-MM-dd"), orderTime);
        //        //        oData.imagingOrderPriority = orderPriority;
        //        //        oData.reportingPriority = orderPriority;

        //        //        List<TestOrderDtDTO> lstDetail = new List<TestOrderDtDTO>();

        //        //        foreach (vPatientChargesDt item in oList)
        //        //        {
        //        //            TestOrderDtDTO oDetail = new TestOrderDtDTO();
        //        //            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
        //        //            procedure oProcedure = new procedure() { procedureCode = item.ItemCode, procedureName = item.ItemName1, modalityCode = modality, procedureFee = 0 };
        //        //            readingPhysician oPhysician = new readingPhysician() { radStaffCode = item.ParamedicCode, radStaffName = item.ParamedicName };
        //        //            List<readingPhysician> lst = new List<readingPhysician>();
        //        //            lst.Add(oPhysician);

        //        //            oDetail.procedure = oProcedure;
        //        //            oDetail.readingPhysician = lst;
        //        //            lstDetail.Add(oDetail);
        //        //        }
        //        //        oData.orderDetail = lstDetail;

        //        //        patient oPatient = new patient();

        //        //        oPatient.patientID = oVisit.MRN.ToString();
        //        //        oPatient.mrn = oVisit.MedicalNo;
        //        //        oPatient.patientName = oVisit.PatientName;
        //        //        oPatient.sex = oVisit.GCGender.Substring(5);
        //        //        oPatient.address = oVisit.HomeAddress;
        //        //        oPatient.dateOfBirth = oVisit.DateOfBirth.ToString("yyyy-MM-dd");
        //        //        oPatient.size = "0";
        //        //        oPatient.weight = "0";
        //        //        oPatient.maritalStatus = string.IsNullOrEmpty(oVisit.GCMaritalStatus) ? "U" : oVisit.GCMaritalStatus.Substring(5);

        //        //        oData.patient = oPatient;

        //        //        List<referringPhysician> lstReferringPhysician = new List<referringPhysician>();

        //        //        if (testOrderID > 0)
        //        //        {
        //        //            lstReferringPhysician.Add(new referringPhysician() { refPhyCode = orderParamedicCode, refPhyName = orderParamedicName });
        //        //        }
        //        //        else
        //        //        {
        //        //            if (!String.IsNullOrEmpty(oVisit.ReferralPhysicianCode))
        //        //                lstReferringPhysician.Add(new referringPhysician() { refPhyCode = oVisit.ReferralPhysicianCode, refPhyName = oVisit.ReferralPhysicianName });
        //        //            else
        //        //                lstReferringPhysician.Add(new referringPhysician() { refPhyCode = oVisit.ParamedicCode, refPhyName = oVisit.ParamedicName });
        //        //        }

        //        //        oData.referringPhysician = lstReferringPhysician;

        //        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inputOrder/", url));
        //        //        request.Method = "POST";
        //        //        request.ContentType = "application/json";
        //        //        Methods.SetRequestHeader(request);

        //        //        var json = JsonConvert.SerializeObject(oData);
        //        //        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //        //        {
        //        //            streamWriter.Write(json);
        //        //        }

        //        //        WebResponse response = (WebResponse)request.GetResponse();
        //        //        string responseMsg = string.Empty;
        //        //        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        //        //        {
        //        //            responseMsg = sr.ReadToEnd();
        //        //        };

        //        //        APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

        //        //        if (!string.IsNullOrEmpty(respInfo.Data))
        //        //        {
        //        //            result = string.Format("{0}|{1}", "1", respInfo.Data);
        //        //        }
        //        //        else
        //        //        {
        //        //            result = string.Format("{0}|{1}", "0", respInfo.Remark);
        //        //        }
        //        //    }
        //        //    else
        //        //    {
        //        //        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
        //        //    }
        //        //}
        //        return result;
        //    }
        //    catch (WebException ex)
        //    {
        //        switch (ex.Status)
        //        {
        //            case WebExceptionStatus.ProtocolError:
        //                result = string.Format("{0}|{1}", "0", "Method not found");
        //                break;
        //            default:
        //                result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
        //                break;
        //        }
        //        return result;
        //    }
        //}

        //private LPK_SEP_Request_Param GenerateRequestParam()
        //{
        //    try
        //    {
        //        string caraKeluar = cboCaraKeluar.Value != null ? cboCaraKeluar.Value.ToString() : string.Empty;
        //        string kondisiPulang = cboKondisiPulang.Value != null ? cboKondisiPulang.Value.ToString() : string.Empty;

        //        string url = AppSession.BPJS_WS_URL;
        //        #region Convert into DTO Objects : request object

        //        t_lpk obj = new t_lpk();
        //        obj.noSep = Request.Form[txtNomorSEP.UniqueID];
        //        obj.tglMasuk = DateTime.ParseExact(Request.Form[txtVisitDate.UniqueID], Constant.FormatString.DATE_PICKER_FORMAT, System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
        //        obj.tglKeluar = DateTime.ParseExact(Request.Form[txtDischargeDate.UniqueID], Constant.FormatString.DATE_PICKER_FORMAT, System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
        //        obj.jaminan = "1";
        //        obj.poli = new t_lpk_poli() { poli = Request.Form[txtKodePoli.UniqueID] };
        //        obj.perawatan = new t_lpk_perawatan()
        //        {
        //            ruangRawat = Request.Form[txtRuangRawat.UniqueID],
        //            kelasRawat = Request.Form[txtKelasTanggungan.UniqueID],
        //            spesialistik = Request.Form[txtKodeSpesialitik.UniqueID],
        //            caraKeluar = caraKeluar,
        //            kondisiPulang = kondisiPulang
        //        };

        //        if (!string.IsNullOrEmpty(hdnDiagnosis.Value))
        //        {
        //            t_lpk_diagnosa diagnosa = new t_lpk_diagnosa();
        //            string mainDiagnosisCode = Request.Form[txtMainDiagnose.UniqueID];
        //            if (!String.IsNullOrEmpty(mainDiagnosisCode))
        //            {
        //                diagnosa.level = "1";
        //                diagnosa.kode = mainDiagnosisCode;

        //                List<t_lpk_diagnosa> listDx = new List<t_lpk_diagnosa>();
        //                listDx.Add(diagnosa);

        //                string[] lstDiagnosa = hdnDiagnosis.Value.ToString().Split(';');
        //                foreach (string dx in lstDiagnosa)
        //                {
        //                    if (dx != mainDiagnosisCode)
        //                    {
        //                        diagnosa = new t_lpk_diagnosa();
        //                        diagnosa.level = "2";
        //                        diagnosa.kode = dx;
        //                        listDx.Add(diagnosa);
        //                    }
        //                }
        //                obj.diagnosa = listDx;
        //            }
        //            else
        //            {
        //                obj.diagnosa = null;
        //            }
        //        }
        //        else
        //        {
        //            obj.diagnosa = null;
        //        }

        //        if (!string.IsNullOrEmpty(hdnProcedures.Value))
        //        {
        //            List<t_lpk_procedure> listPr = new List<t_lpk_procedure>();
        //            string[] lstProcedure = hdnProcedures.Value.ToString().Split(';');
        //            foreach (string pr in lstProcedure)
        //            {
        //                t_lpk_procedure proc = new t_lpk_procedure();
        //                proc.kode = pr;
        //                listPr.Add(proc);
        //            }
        //            obj.procedure = listPr;
        //        }
        //        else
        //        {
        //            obj.procedure = null;
        //        }
        //        #endregion

        //        LPK_SEP_Request request = new LPK_SEP_Request() { t_lpk = obj };
        //        LPK_SEP_Request_Param dto = new LPK_SEP_Request_Param() { request = request };
        //        return dto;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}