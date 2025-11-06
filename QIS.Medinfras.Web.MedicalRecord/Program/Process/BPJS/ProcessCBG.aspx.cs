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


namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class ProcessCBG : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.INACBGS_BRIDGING;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected string RegistrationDateTime = "";
        protected override void InitializeDataControl()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}')", Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.BPJS_PAYMENT_METHOD, Constant.StandardCode.BPJS_SPECIAL_CMG_ADL);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField<StandardCode>(cboModelBayar, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.BPJS_PAYMENT_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeCondition, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME).ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField<StandardCode>(cboADL, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.BPJS_SPECIAL_CMG_ADL).ToList(), "StandardCodeName", "StandardCodeID");

            RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");

            Helper.SetControlEntrySetting(txtNoPeserta, new ControlEntrySetting(false, false, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtNomorSEP, new ControlEntrySetting(false, false, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeTime, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboDischargeCondition, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtADLScore, new ControlEntrySetting(true, true, false), "mpPatientDischarge");

            ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            EntityToControl(entity);
            if (entity.GCDischargeCondition == "" && entity.GCDischargeMethod == "")
            {
                txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
            txtADLScore.Enabled = (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT && entity.LOSInDay >= 43);
        }

        private void EntityToControl(ConsultVisit entity)
        {
            cboDischargeCondition.Value = entity.GCDischargeCondition;
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
                txtJenisRawat.Text = AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT ? "1 - RITL" : "2 - RJTL";
                hdnJnsRawat.Value = AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT ? "1" : "2";
                txtKelasTanggungan.Text = String.Format("Kelas {0}", regBPJS.KelasTanggungan);
                hdnKlsRawat.Value = regBPJS.KelasTanggungan;
                txtSuratRujukan.Text = String.IsNullOrEmpty(regBPJS.NoRujukan) ? "Tidak Ada" : string.Format("Ada ({0})", regBPJS.NoRujukan);

                #region Patient Diagnosis
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID);
                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
                if (lstDiagnosis.Count > 0)
                {
                    StringBuilder strComplication = new StringBuilder();
                    string strDiagnosis = string.Empty;
                    int diagnoseCount = 0;
                    foreach (vPatientDiagnosis diag in lstDiagnosis)
                    {
                        if (diag.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                        {
                            txtMainDiagnose.Text = string.Format("{0} ({1})", diag.DiagnoseName, diag.DiagnoseID);
                        }
                        else
                        {
                            strComplication.AppendLine(string.Format("{0} ({1})", diag.DiagnoseName, diag.DiagnoseID));
                        }
                        strDiagnosis += diag.INACBGLabel.TrimEnd();
                        strDiagnosis += ";";
                        diagnoseCount += 1;
                    }
                    strDiagnosis = strDiagnosis.Substring(0, strDiagnosis.Length - 1);
                    if (diagnoseCount < 30)
                    {
                        for (int i = diagnoseCount; i < 30; i++)
                        {
                            strDiagnosis += ";";
                        }
                    }
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
                        strProcedures.AppendLine(string.Format("{0} ({1})", proc.ProcedureName, proc.ProcedureID));
                        strproc += proc.INACBGLabel.TrimEnd();
                        strproc += ";";
                        procedureCount += 1;
                    }
                    strproc = strproc.Substring(0, strproc.Length - 1);
                    if (procedureCount < 30)
                    {
                        for (int i = procedureCount; i < 30; i++)
                        {
                            strproc += ";";
                        }
                    }
                    txtProcedures.Text = strProcedures.ToString();
                    hdnProcedures.Value = strproc;
                }
                else
                {
                    txtProcedures.Text = string.Empty;
                    hdnProcedures.Value = "";
                }
                #endregion

                #region Special CMG
                txtADLScore.Text = regBPJS.ADL;
                //cboADL.Value = string.Format("{0}^{1}", Constant.StandardCode.BPJS_SPECIAL_CMG_ADL, regBPJS.ADL);
                #endregion

                #region Get Hospital Bill
                filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus <> 'X121^999'", entity.RegistrationID);
                List<vPatientBill> patientBills = BusinessLayer.GetvPatientBillList(filterExpression);
                decimal payerAmount = 0;
                if (patientBills.Count>0)
                    payerAmount = patientBills.Sum(lst => lst.TotalPayerAmount);
                if (payerAmount == 0)
                {
                   //Calculate from Patient Charges
                    filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus <> '{1}'", entity.VisitID, Constant.TransactionStatus.VOID);
                    List<vPatientChargesHd> chargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression);
                    if (chargesHd.Count > 0)
                        payerAmount = chargesHd.Sum(lst => lst.TotalPayerAmount);
                }
                txtTariffRS.Text = payerAmount.ToString(); 
                #endregion

                txtGrouperCode.Text = regBPJS.GrouperCode;
                txtGrouperName.Text = regBPJS.GrouperName;
                txtGrouperTariff.Text = regBPJS.Tariff.ToString();
                txtTotalTariff.Text = regBPJS.Tariff.ToString();

                /*#region Get INACBGS Local Grouper Tariff
                List<GetINACBGTariff> list = BusinessLayer.GetINACBGGrouperTariff(hdnJnsRawat.Value, hdnKlsRawat.Value, hdnDiagnosis.Value, hdnProcedures.Value);
                if (list.Count > 0)
                {
                    GetINACBGTariff obj = list[0];
                    txtGrouperCode.Text = obj.GrouperCode;
                    txtGrouperName.Text = obj.GrouperName;
                    txtGrouperTariff.Text = obj.Tariff.ToString();

                    txtTotalTariff.Text = obj.Tariff.ToString();
                }
                #endregion*/
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
            if (type == "process")
            {
                try
                {
                    ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);

                    if (entity != null)
                    {
                        RegistrationBPJS regBPJS = BusinessLayer.GetRegistrationBPJS(entity.RegistrationID);
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

                        if(!string.IsNullOrEmpty(hdnProcedures.Value.ToString())){
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
                        }

                        regBPJS.ADL = txtADLScore.Text;
                        regBPJS.GrouperCode = txtGrouperCode.Text;
                        regBPJS.GrouperName = txtGrouperName.Text;
                        regBPJS.Tariff = !string.IsNullOrEmpty(txtGrouperTariff.Text) ? Convert.ToDecimal(txtGrouperTariff.Text) : 0;
                        regBPJS.TariffRS = !string.IsNullOrEmpty(txtTariffRS.Text) ? Convert.ToDecimal(txtTariffRS.Text) : 0;

                        regBPJS.LastUpdatedDate = DateTime.Now;
                        regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdateRegistrationBPJS(regBPJS);
                        
                        //Update Coverage Limit Amount
                        Registration oRegistration = BusinessLayer.GetRegistration(entity.RegistrationID);
                        if (oRegistration != null)
                        {
                            oRegistration.CoverageLimitAmount = !string.IsNullOrEmpty(txtGrouperTariff.Text) ? Convert.ToDecimal(txtGrouperTariff.Text) : 0;
                            oRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oRegistration.LastUpdatedDate = DateTime.Now;
                            BusinessLayer.UpdateRegistration(oRegistration);
                        }
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
    }
}