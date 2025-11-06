using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryGenerateBill : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";
        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected string GetCoverageLabel()
        {
            if (hdnBPJSMenggunakanCaraCoverageBPJS.Value == "1")
            {
                switch (hdnIsControlBPJSCoverage.Value)
                {
                    case "1": return GetLabel("INACBG's Grouper");
                    default: return GetLabel("Batas Tanggungan");
                }
            }
            else
            {
                return GetLabel("Batas Tanggungan");
            }
        }

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_GENERATE_BILL;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_GENERATE_BILL;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_GENERATE_BILL;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_GENERATE_BILL;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_GENERATE_BILL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_GENERATE_BILL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_GENERATE_BILL;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_GENERATE_BILL;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_GENERATE_BILL;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetDepartmentPharmacy()
        {
            return Constant.Facility.PHARMACY;
        }

        protected string GetDepartmentMedicalCheckup()
        {
            return Constant.Facility.MEDICAL_CHECKUP;
        }

        protected override void InitializeDataControl()
        {
            GetSettingParameter();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;

            hdnIsUsedCalculateCoveragePerBillingGroup.Value = AppSession.IsUsedCalculateCoveragePerBillingGroup;

            vRegistration4 entity = BusinessLayer.GetvRegistration4List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();

            hdnIsBPJSClass.Value = entity.IsBPJSClass ? "1" : "0";
            hdnIsLocked.Value = entity.IsLockDown ? "1" : "0";
            EntityToControl(entity);

            string filterExpressionOutstandingOrder = string.Format("RegistrationID = {0}", entity.RegistrationID);
            vRegistrationOutstandingInfo lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpressionOutstandingOrder).FirstOrDefault();
            bool outstanding = (lstInfo.ServiceOrder + lstInfo.PrescriptionOrder + lstInfo.PrescriptionReturnOrder + lstInfo.LaboratoriumOrder + lstInfo.RadiologiOrder + lstInfo.OtherOrder > 0);

            string filterExpression = "";
            hdnPatientBPJSAmount.Value = "0";
            txtPatientBPJSAmount.Text = 0.ToString();
            if (entity.IsControlCoverageLimit)
            {
                decimal coverageLimit = entity.CoverageLimitAmount;
                filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);
                if (entity.IsCoverageLimitPerDay)
                    filterExpression += string.Format(" AND BillingDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(filterExpression);
                coverageLimit -= lstPatientBill.Sum(p => p.TotalPayerAmount);
                hdnOldRemainingCoverageAmount.Value = coverageLimit.ToString();
                hdnRemainingCoverageAmount.Value = coverageLimit.ToString();
                txtCoverageLimit.Text = coverageLimit.ToString();
            }
            else
            {
                hdnRemainingCoverageAmount.Value = "0";
                hdnDiffCoverageAmount.Value = "0";
                txtCoverageLimit.Text = 0.ToString("N");
            }

            string filterHSU = string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM PatientChargesHd WHERE VisitID IN (SELECT VisitID FROM ConsultVisit WHERE RegistrationID IN (SELECT RegistrationID FROM Registration WHERE RegistrationID = {0})) AND GCTransactionStatus != '{1}' UNION ALL SELECT HealthcareServiceUnitID FROM PatientChargesHd WHERE IsChargesTransfered = 1 AND VisitID IN (SELECT VisitID FROM ConsultVisit WHERE RegistrationID IN (SELECT RegistrationID FROM Registration WHERE LinkedToRegistrationID = {0})) AND GCTransactionStatus != '{1}') AND IsDeleted = 0", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);
            List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(filterHSU);
            lstHSU.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            List<Variable> lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "0", Value = "" });
            lstVariable.Add(new Variable { Code = "1", Value = "Belum Diverifikasi" });
            lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diverifikasi" });
            Methods.SetComboBoxField<Variable>(cboDisplayVerification, lstVariable, "Value", "Code");
            cboDisplayVerification.Value = "0";

            txtFilterTransactionDateFrom.Text = AppSession.RegisteredPatient.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtFilterTransactionDateTo.Text = DateTime.Now.AddMonths(3).ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            txtPatientAmount.Text = "0";
            hdnPatientAmount.Value = "0";
            txtPatientAmount.Attributes.Add("readonly", "readonly");

            BindGridDetail();


            int count = 0;
            int transOpen = 0;
            int regBedChargesOpen = 0;

            string filterExpressionLinked = string.Format("(LinkedToRegistrationID = {0} AND IsChargesTransfered = 0) AND GCTransactionStatus NOT IN ('{1}','{2}') AND IsDeleted = 0", hdnRegistrationID.Value, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            count = BusinessLayer.GetvPatientChargesDt11RowCount(filterExpressionLinked);

            string filterChargesOpen = string.Format("(RegistrationID = {0} AND GCTransactionStatus = '{1}') OR ((LinkedToRegistrationID = {0} AND IsChargesTransfered = 1) AND GCTransactionStatus = '{1}')", hdnRegistrationID.Value, Constant.TransactionStatus.OPEN);
            transOpen = BusinessLayer.GetvPatientChargesHd1RowCount(filterChargesOpen);

            string filterRegBedChargesOpen = string.Format("RegistrationID = {0} AND IsDeleted = 0 AND TransactionID IS NULL", hdnRegistrationID.Value);
            regBedChargesOpen = BusinessLayer.GetRegistrationBedChargesRowCount(filterRegBedChargesOpen);

            if (hdnIsAllowOPEN.Value == "1")
            {
                if (transOpen > 0)
                {
                    transOpen = 0;
                }
            }

            if (hdnPembuatanTagihanTidakAdaOutstandingOrder.Value == "0")
            {
                outstanding = false;
            }

            if (count < 1 && !outstanding && transOpen < 1 && hdnIsHasTotalDifferent.Value == "0")
            {
                tblInfoWarning.Style.Add("display", "none");
            }
            else
            {
                if (count < 1)
                {
                    lblInfoOutstandingBill.Style.Add("display", "none");
                }

                if (!outstanding)
                {
                    lblInfoOutstandingOrder.Style.Add("display", "none");
                }

                if (transOpen < 1)
                {
                    lblInfoTransactionOpen.Style.Add("display", "none");
                }

                if (hdnIsHasTotalDifferent.Value == "0")
                {
                    lblInfoChargesNotBalance.Style.Add("display", "none");
                }
            }

            if (hdnIsAllowOPEN.Value == "1")
            {
                if (count < 1 && !outstanding)
                {
                    tblInfoWarning.Style.Add("display", "none");
                }
                lblInfoTransactionOpen.Style.Add("display", "none");
            }

            if (regBedChargesOpen == 0)
            {
                lblInfoOutstandingRegistrationBedCharges.Style.Add("display", "none");
            }

            if (transOpen > 0)
            {
                if (outstanding)
                {
                    hdnIsHasTransactionAndOrderOutstanding.Value = "1";
                }
            }
            else
            {
                if (outstanding)
                {
                    hdnIsHasTransactionAndOrderOutstanding.Value = "1";
                }
            }
        }

        private void GetSettingParameter()
        {
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                                            Constant.SettingParameter.FN_KONTROL_PEMBUATAN_TAGIHAN, //0
                                                                            Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_JIKA_TIDAK_ADA_OUTSTANDING_ORDER, //1
                                                                            Constant.SettingParameter.FN_BATAS_TANGGUNGAN_LEBIH_BESAR_DARI_TAGIHAN, //2
                                                                            Constant.SettingParameter.FN_IS_ALLOW_ROUNDING_AMOUNT, //3
                                                                            Constant.SettingParameter.FN_NILAI_PEMBULATAN_TAGIHAN, //4
                                                                            Constant.SettingParameter.FN_PEMBULATAN_TAGIHAN_KE_ATAS, //5
                                                                            Constant.SettingParameter.FN_BLOK_PEMBUATAN_TAGIHAN_SAAT_ADA_TRANSAKSI_MASIH_OPEN, //6
                                                                            Constant.SettingParameter.FILTER_PREVIOUS_TRANSACTION_DATE_INTERVAL //7
            ));

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}')",
                                                                        AppSession.UserLogin.HealthcareID, //0
                                                                        Constant.SettingParameter.FN_BIAYA_ADM_RI_DALAM_PERSENTASE, //1
                                                                        Constant.SettingParameter.FN_NILAI_BIAYA_ADM_RI, //2
                                                                        Constant.SettingParameter.FN_NILAI_MIN_BIAYA_ADM_RI, //3
                                                                        Constant.SettingParameter.FN_NILAI_MAX_BIAYA_ADM_RI, //4
                                                                        Constant.SettingParameter.FN_BIAYA_SERVICE_RI_DALAM_PERSENTASE, //5
                                                                        Constant.SettingParameter.FN_NILAI_BIAYA_SERVICE_RI, //6
                                                                        Constant.SettingParameter.FN_NILAI_MIN_BIAYA_SERVICE_RI, //7
                                                                        Constant.SettingParameter.FN_NILAI_MAX_BIAYA_SERVICE_RI, //8
                                                                        Constant.SettingParameter.FN_SELISIH_PASIEN_BPJS_NAIK_KELAS, //9
                                                                        Constant.SettingParameter.FN_BIAYA_ADM_KELAS_TERTINGGI, //10
                                                                        Constant.SettingParameter.FN_ADMIN_HANYA_RAWAT_INAP, //11
                                                                        Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_BPJS_MENGGUNAKAN_CARA_BPJS, //12
                                                                        Constant.SettingParameter.FN_IS_PAYER_ADMIN_FORMULA_VERSION, //13
                                                                        Constant.SettingParameter.FN_IS_PAYER_AMOUNT_FORMULA_VERSION, //14
                                                                        Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_MENGGUNAKAN_FILTER_COVERAGE, //15
                                                                        Constant.SettingParameter.FN0182, //16
                                                                        Constant.SettingParameter.FN_IS_ALLOW_PROCESS_BILL_WHEN_PENDING_RECALCULATED, //17
                                                                        Constant.SettingParameter.FN_NOTIFICATION_REGISTRATION_IS_LINKED_TO_INPATIENT_REGISTRATION //18
            ));

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                if (lstSettingParameterDt.Count > 0)
                {
                    hdnAdministrationFee.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_BIAYA_ADM_RI).FirstOrDefault().ParameterValue;
                    hdnAdministrationFee.Attributes.Add("ispercentage", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_BIAYA_ADM_RI_DALAM_PERSENTASE).FirstOrDefault().ParameterValue == "1" ? "1" : "0");
                    hdnAdministrationFee.Attributes.Add("minamount", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_MIN_BIAYA_ADM_RI).FirstOrDefault().ParameterValue);
                    hdnAdministrationFee.Attributes.Add("maxamount", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_MAX_BIAYA_ADM_RI).FirstOrDefault().ParameterValue);

                    hdnPatientAdminFee.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_BIAYA_ADM_RI).FirstOrDefault().ParameterValue;
                    hdnPatientAdminFee.Attributes.Add("ispercentage", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_BIAYA_ADM_RI_DALAM_PERSENTASE).FirstOrDefault().ParameterValue == "1" ? "1" : "0");
                    hdnPatientAdminFee.Attributes.Add("minamount", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_MIN_BIAYA_ADM_RI).FirstOrDefault().ParameterValue);
                    hdnPatientAdminFee.Attributes.Add("maxamount", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_MAX_BIAYA_ADM_RI).FirstOrDefault().ParameterValue);

                    hdnServiceFee.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_BIAYA_SERVICE_RI).FirstOrDefault().ParameterValue;
                    hdnServiceFee.Attributes.Add("ispercentage", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_BIAYA_SERVICE_RI_DALAM_PERSENTASE).FirstOrDefault().ParameterValue == "1" ? "1" : "0");
                    hdnServiceFee.Attributes.Add("minamount", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_MIN_BIAYA_SERVICE_RI).FirstOrDefault().ParameterValue);
                    hdnServiceFee.Attributes.Add("maxamount", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_MAX_BIAYA_SERVICE_RI).FirstOrDefault().ParameterValue);

                    hdnPatientServiceFee.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_BIAYA_SERVICE_RI).FirstOrDefault().ParameterValue;
                    hdnPatientServiceFee.Attributes.Add("ispercentage", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_BIAYA_SERVICE_RI_DALAM_PERSENTASE).FirstOrDefault().ParameterValue == "1" ? "1" : "0");
                    hdnPatientServiceFee.Attributes.Add("minamount", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_MIN_BIAYA_SERVICE_RI).FirstOrDefault().ParameterValue);
                    hdnPatientServiceFee.Attributes.Add("maxamount", lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_MAX_BIAYA_SERVICE_RI).FirstOrDefault().ParameterValue);

                    hdnIsApplyBPJSClassCareVarianceToPatient.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_SELISIH_PASIEN_BPJS_NAIK_KELAS).FirstOrDefault().ParameterValue;
                    hdnIsAdministrationFeeUseHigherClass.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_BIAYA_ADM_KELAS_TERTINGGI).FirstOrDefault().ParameterValue;
                    hdnIsAdministrationFeeOnlyForInpatient.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_ADMIN_HANYA_RAWAT_INAP).FirstOrDefault().ParameterValue;
                }
            }
            else
            {
                hdnAdministrationFee.Value = "0";
                hdnAdministrationFee.Attributes.Add("ispercentage", "0");
                hdnAdministrationFee.Attributes.Add("minamount", "0");
                hdnAdministrationFee.Attributes.Add("maxamount", "0");

                hdnPatientAdminFee.Value = "0";
                hdnPatientAdminFee.Attributes.Add("ispercentage", "0");
                hdnPatientAdminFee.Attributes.Add("minamount", "0");
                hdnPatientAdminFee.Attributes.Add("maxamount", "0");

                hdnServiceFee.Value = "0";
                hdnServiceFee.Attributes.Add("ispercentage", "0");
                hdnServiceFee.Attributes.Add("minamount", "0");
                hdnServiceFee.Attributes.Add("maxamount", "0");

                hdnPatientServiceFee.Value = "0";
                hdnPatientServiceFee.Attributes.Add("ispercentage", "0");
                hdnPatientServiceFee.Attributes.Add("minamount", "0");
                hdnPatientServiceFee.Attributes.Add("maxamount", "0");

                hdnIsAdministrationFeeUseHigherClass.Value = "0";
                hdnIsAdministrationFeeOnlyForInpatient.Value = "0";
            }

            hdnIsAllowOPEN.Value = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_KONTROL_PEMBUATAN_TAGIHAN).FirstOrDefault().ParameterValue == "1" ? "0" : "1";
            hdnIsAllowOPENForValidation.Value = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_KONTROL_PEMBUATAN_TAGIHAN).FirstOrDefault().ParameterValue;
            hdnPembuatanTagihanTidakAdaOutstandingOrder.Value = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_JIKA_TIDAK_ADA_OUTSTANDING_ORDER).FirstOrDefault().ParameterValue;

            hdnIsCoverageAllowBiggerThanBillingAmount.Value = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_BATAS_TANGGUNGAN_LEBIH_BESAR_DARI_TAGIHAN).FirstOrDefault().ParameterValue;

            hdnBlokPembuatanTagihanSaatAdaTransaksiOpen.Value = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_BLOK_PEMBUATAN_TAGIHAN_SAAT_ADA_TRANSAKSI_MASIH_OPEN).FirstOrDefault().ParameterValue;

            hdnMenggunakanPembulatan.Value = lstSettingParameter.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ALLOW_ROUNDING_AMOUNT).FirstOrDefault().ParameterValue;
            hdnNilaiPembulatan.Value = lstSettingParameter.Where(a => a.ParameterCode == Constant.SettingParameter.FN_NILAI_PEMBULATAN_TAGIHAN).FirstOrDefault().ParameterValue;
            hdnPembulatanKemana.Value = lstSettingParameter.Where(a => a.ParameterCode == Constant.SettingParameter.FN_PEMBULATAN_TAGIHAN_KE_ATAS).FirstOrDefault().ParameterValue;

            hdnBPJSMenggunakanCaraCoverageBPJS.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_BPJS_MENGGUNAKAN_CARA_BPJS).FirstOrDefault().ParameterValue;

            hdnPayerAdminFeeFormulaVersion.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_IS_PAYER_ADMIN_FORMULA_VERSION).FirstOrDefault().ParameterValue;
            hdnPayerAmountFormulaVersion.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_IS_PAYER_AMOUNT_FORMULA_VERSION).FirstOrDefault().ParameterValue;
            hdnIsPembuatanTagihanMenggunakanFilterCoverage.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_MENGGUNAKAN_FILTER_COVERAGE).FirstOrDefault().ParameterValue;

            //hdnFN0182.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN0182).FirstOrDefault().ParameterValue;
            hdnIsAllowProcessBillWhenPendingRecalculated.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_IS_ALLOW_PROCESS_BILL_WHEN_PENDING_RECALCULATED).FirstOrDefault().ParameterValue;
            hdnNotificationRegistrationIsLinkedToRegistrationInpatient.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NOTIFICATION_REGISTRATION_IS_LINKED_TO_INPATIENT_REGISTRATION).FirstOrDefault().ParameterValue;
        }

        protected string GetRemainingCoverageAmount()
        {
            if (hdnRemainingCoverageAmount.Value == "" || hdnRemainingCoverageAmount.Value == null || hdnRemainingCoverageAmount.Value == "NaN")
            {
                hdnRemainingCoverageAmount.Value = "0";
            }

            return Convert.ToDecimal(hdnRemainingCoverageAmount.Value).ToString(Constant.FormatString.NUMERIC_2);
        }

        protected string GetDiffCoverageAmount()
        {
            if (hdnDiffCoverageAmount.Value == "" || hdnDiffCoverageAmount.Value == null || hdnDiffCoverageAmount.Value == "NaN")
            {
                hdnDiffCoverageAmount.Value = "0";
            }

            return Convert.ToDecimal(hdnDiffCoverageAmount.Value).ToString(Constant.FormatString.NUMERIC_2);
        }

        private void BindGridDetail()
        {
            hdnAllowPrint.Value = string.Empty;

            int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            int oHealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            string oTransDateFrom, oTransDateTo;

            if (rblFilterDate.SelectedValue.ToLower() == "true")
            {
                oTransDateFrom = Helper.GetDatePickerValue(txtFilterTransactionDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112);
                oTransDateTo = Helper.GetDatePickerValue(txtFilterTransactionDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112);
            }
            else
            {
                DateTime tempTransDateFrom = AppSession.RegisteredPatient.RegistrationDate;
                string filterRegFrom = string.Format("LinkedToRegistrationID = {0} AND GCRegistrationStatus != '{1}'", oRegistrationID, Constant.VisitStatus.CANCELLED);
                List<Registration> lstRegFrom = BusinessLayer.GetRegistrationList(filterRegFrom);
                foreach (Registration regFrom in lstRegFrom)
                {
                    if (regFrom.RegistrationDate < tempTransDateFrom)
                    {
                        tempTransDateFrom = regFrom.RegistrationDate;
                    }
                }

                oTransDateFrom = tempTransDateFrom.ToString(Constant.FormatString.DATE_FORMAT_112);
                oTransDateTo = DateTime.Now.AddMonths(1).ToString(Constant.FormatString.DATE_FORMAT_112);
            }

            List<GetPatientChargesHdChargeClass> lstTemp = BusinessLayer.GetPatientChargesHdChargeClassList(oRegistrationID, oHealthcareServiceUnitID, oTransDateFrom, oTransDateTo);
            if (cboDisplayVerification.Value.ToString() == "1")
            {
                lstTemp = lstTemp.Where(t => t.IsVerified == false).ToList();
            }
            else if (cboDisplayVerification.Value.ToString() == "2")
            {
                lstTemp = lstTemp.Where(t => t.IsVerified == true).ToList();
            }

            int totalDifferent = lstTemp.Where(a => a.IsTotalDifferent).ToList().Count();
            hdnIsHasTotalDifferent.Value = totalDifferent > 0 ? "1" : "0";
            hdnIsPendingRecalculated.Value = lstTemp.Where(a => a.IsPendingRecalculated).ToList().Count() > 0 ? "1" : "0";

            //List<GetPatientChargesHdChargeClass> lst = lstTemp.Where(t => t.GCTransactionStatus != Constant.TransactionStatus.PROCESSED && t.GCTransactionStatus != Constant.TransactionStatus.CLOSED).ToList();

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                if (lstTemp.Count(t => t.DepartmentID == Constant.Facility.OUTPATIENT || t.DepartmentID == Constant.Facility.EMERGENCY) > 0)
                {
                    hdnAllowPrint.Value += "PTR";
                }
            }
            string tempPatientChargedHd = string.Join(",", lstTemp.Select(p => p.TransactionID));

            tempPatientChargedHd = "'" + tempPatientChargedHd.Replace(",", "','") + "'";
            List<vPatientChargesDt11> lstDt = BusinessLayer.GetvPatientChargesDt11List(
                                                string.Format("TransactionID IN ({0}) AND GCItemType IN ('{1}','{2}')",
                                                tempPatientChargedHd, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS));
            if (lstDt.Count > 0)
            {
                hdnAllowPrint.Value += "DRUG";
            }

            List<GetPatientChargesHdChargeClass> lstDiagnostic = lstTemp.Where(t => t.DepartmentID == Constant.Facility.DIAGNOSTIC).ToList();
            if (lstDiagnostic.Count > 0)
            {
                List<SettingParameter> entitySetPar = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')",
                    Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID));
                string temp = entitySetPar.Where(t => t.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).First().ParameterValue;
                List<HealthcareServiceUnit> entityHSURadioLab = BusinessLayer.GetHealthcareServiceUnitList(string.Format("ServiceUnitID IN ({0},{1})",
                    entitySetPar.Where(t => t.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).First().ParameterValue,
                    entitySetPar.Where(t => t.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).First().ParameterValue));
                int HSULab = entityHSURadioLab.Where(t => t.ServiceUnitID == Convert.ToInt32(entitySetPar.Where(a => a.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).First().ParameterValue)).FirstOrDefault().HealthcareServiceUnitID;
                int HSURadio = entityHSURadioLab.Where(t => t.ServiceUnitID == Convert.ToInt32(entitySetPar.Where(a => a.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).First().ParameterValue)).FirstOrDefault().HealthcareServiceUnitID;
                if (lstDiagnostic.Count(t => t.HealthcareServiceUnitID == HSULab) > 0)
                {
                    hdnAllowPrint.Value += "LAB";
                }
                if (lstDiagnostic.Count(t => t.HealthcareServiceUnitID == HSURadio) > 0)
                {
                    hdnAllowPrint.Value += "IMA";
                }
                if (lstDiagnostic.Count > lstDiagnostic.Count(t => t.HealthcareServiceUnitID == HSURadio || t.HealthcareServiceUnitID == HSULab))
                {
                    hdnAllowPrint.Value += "MDO";
                }
            }

            lvwView.DataSource = lstTemp;
            lvwView.DataBind();

            hdnIsHasTotalChargesDifferent.Value = lstTemp.Where(a => a.IsTotalDifferent).ToList().Count() > 0 ? "1" : "0";

            if (hdnIsControlCoverageLimit.Value == "0")
            {
                HtmlTableRow trFooterRemainingCoverageLimit = lvwView.FindControl("trFooterRemainingCoverageLimit") as HtmlTableRow;
                HtmlTableRow trFooterRemainingTotalBill = lvwView.FindControl("trFooterRemainingTotalBill") as HtmlTableRow;
                HtmlTableRow trDiffCoverageAmount = lvwView.FindControl("trFooterDiffCoverageLimit") as HtmlTableRow;

                if (trFooterRemainingCoverageLimit != null)
                {
                    trFooterRemainingCoverageLimit.Style.Add("display", "none");
                }

                if (trDiffCoverageAmount != null)
                    trDiffCoverageAmount.Style.Add("display", "none");
            }
            else
            {
                HtmlTableRow trDiffCoverageAmount = lvwView.FindControl("trFooterDiffCoverageLimit") as HtmlTableRow;

                #region BPJS
                if (hdnIsControlBPJSCoverage.Value == "1")
                {
                    if (hdnBPJSMenggunakanCaraCoverageBPJS.Value == "1")
                    {
                        if (trDiffCoverageAmount != null)
                            trDiffCoverageAmount.Style.Add("display", "table-row");
                    }
                    else
                    {
                        if (trDiffCoverageAmount != null)
                            trDiffCoverageAmount.Style.Add("display", "none");
                    }
                }
                else
                {
                    if (trDiffCoverageAmount != null)
                        trDiffCoverageAmount.Style.Add("display", "none");
                }
                #endregion
            }

            #region Administration and Service Fee
            if (hdnDepartmentID.Value != Constant.Facility.INPATIENT)
            {
                HtmlTableRow trFooterAdministrationFee = lvwView.FindControl("trFooterAdministrationFee") as HtmlTableRow;
                HtmlTableRow trFooterServiceFee = lvwView.FindControl("trFooterServiceFee") as HtmlTableRow;
                if (trFooterAdministrationFee != null)
                    trFooterAdministrationFee.Style.Add("display", "none");
                if (trFooterServiceFee != null)
                    trFooterServiceFee.Style.Add("display", "none");
            }
            else
            {
                TextBox txtAdministrationFee = lvwView.FindControl("txtAdministrationFee") as TextBox;
                TextBox txtServiceFee = lvwView.FindControl("txtServiceFee") as TextBox;
                if (txtAdministrationFee != null)
                {
                    if (hdnAdministrationFee.Attributes["ispercentage"] == "0")
                    {
                        decimal administrationFee = Convert.ToDecimal(hdnAdministrationFee.Value);
                        decimal maxAmount = Convert.ToDecimal(hdnAdministrationFee.Attributes["maxamount"]);
                        decimal minAmount = Convert.ToDecimal(hdnAdministrationFee.Attributes["minamount"]);
                        if (administrationFee < minAmount && (minAmount > 0))
                            administrationFee = minAmount;
                        if (administrationFee > maxAmount && (maxAmount > 0))
                            administrationFee = maxAmount;
                        txtAdministrationFee.Text = administrationFee.ToString();
                    }

                    if (hdnServiceFee.Attributes["ispercentage"] == "0")
                    {
                        decimal serviceFee = Convert.ToDecimal(hdnServiceFee.Value);
                        decimal maxAmount = Convert.ToDecimal(hdnServiceFee.Attributes["maxamount"]);
                        decimal minAmount = Convert.ToDecimal(hdnServiceFee.Attributes["minamount"]);
                        if (serviceFee < minAmount && (minAmount > 0))
                            serviceFee = minAmount;
                        if (serviceFee > maxAmount && (maxAmount > 0))
                            serviceFee = maxAmount;
                        txtServiceFee.Text = serviceFee.ToString();
                    }
                }
            }
            #endregion
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem || e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetPatientChargesHdChargeClass entity = e.Item.DataItem as GetPatientChargesHdChargeClass;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                chkIsSelected.Visible = entity.IsAllowProcessToBill;
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            if (String.IsNullOrEmpty(param[0].ToString()))
            {
                BindGridDetail();
            }
            else if (param[0] == "recalchargeshd")
            {
                int transactionID = Convert.ToInt32(param[1]);
                if (ProcessRecalculateChargesHd(ref errMessage, transactionID))
                    result += "success|recalchargeshd";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "recaladminservice")
            {
                vRegistration4 entity = BusinessLayer.GetvRegistration4List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID))[0];
                GetSettingParameter();
                GetAdministrationServiceFee(entity);
                result += "success|recaladminservice";
            }
            else
            {
                setTotalInformation();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool ProcessRecalculateChargesHd(ref string errMessage, int oTransactionID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                if (oTransactionID != 0)
                {
                    PatientChargesHd entityHd = entityHdDao.Get(oTransactionID);

                    string filterChargesDt = string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", oTransactionID, Constant.TransactionStatus.VOID);
                    List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(filterChargesDt, ctx);

                    entityHd.TotalPatientAmount = lstEntityDt.Sum(pt => pt.PatientAmount);
                    entityHd.TotalPayerAmount = lstEntityDt.Sum(py => py.PayerAmount);
                    entityHd.TotalAmount = lstEntityDt.Sum(ttl => ttl.LineAmount);
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);
                }
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

        private void setTotalInformation()
        {
        }

        private void EntityToControl(vRegistration4 entity)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

            if (entity.IsControlCoverageLimit)
            {
                hdnIsControlCoverageLimit.Value = "1";

                #region BPJS
                if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                {
                    hdnIsBPJS.Value = "1";

                    if (hdnBPJSMenggunakanCaraCoverageBPJS.Value == "1")
                    {
                        trFilterCoverage.Style.Add("display", "none");
                        trPatientAmount.Style.Add("display", "none");
                        //trCoverageLimit.Style.Remove("display");
                        trPatientBPJSAmount.Style.Remove("display");

                        chkIsEditCoverageBPJSManual.Checked = false;
                        chkIsEditCoverageBPJSManual.Style.Remove("display");
                        txtCoverageLimit.Attributes.Add("readonly", "readonly");
                    }
                    else
                    {
                        if (hdnIsPembuatanTagihanMenggunakanFilterCoverage.Value == "0")
                        {
                            trFilterCoverage.Style.Add("display", "none");
                        }
                        else
                        {
                            trFilterCoverage.Style.Remove("display");
                        }

                        trPatientAmount.Style.Remove("display");
                        //trCoverageLimit.Style.Add("display", "none");
                        trPatientBPJSAmount.Style.Add("display", "none");

                        chkIsEditCoverageBPJSManual.Checked = false;
                        chkIsEditCoverageBPJSManual.Style.Add("display", "none");
                    }

                    hdnIsControlBPJSCoverage.Value = "1";


                    txtPatientAmount.Text = "0";
                    hdnPatientAmount.Value = "0";
                    txtPatientAmount.Attributes.Add("readonly", "readonly");

                    if (AppSession.IsBridgingToBPJS)
                    {
                        RegistrationBPJS entityRegistrationBPJS = BusinessLayer.GetRegistrationBPJS(entity.RegistrationID);
                        if (entityRegistrationBPJS != null)
                        {
                            txtINACBGSCode.Text = entityRegistrationBPJS.GrouperCode;
                            txtINACBGSName.Text = entityRegistrationBPJS.GrouperName;
                        }
                        else
                        {
                            txtCoverageLimit.Attributes.Add("readonly", "readonly");
                            txtINACBGSCode.Attributes.Add("readonly", "readonly");
                            txtINACBGSName.Attributes.Add("readonly", "readonly");
                        }
                    }
                    else
                    {
                        txtINACBGSCode.Attributes.Add("readonly", "readonly");
                        txtINACBGSName.Attributes.Add("readonly", "readonly");
                    }
                }
                else
                {
                    if (hdnIsPembuatanTagihanMenggunakanFilterCoverage.Value == "0")
                    {
                        trFilterCoverage.Style.Add("display", "none");
                    }
                    else
                    {
                        trFilterCoverage.Style.Remove("display");
                    }

                    trPatientBPJSAmount.Style.Add("display", "none");
                    hdnIsControlBPJSCoverage.Value = "0";

                    chkIsEditCoverageBPJSManual.Checked = false;
                    chkIsEditCoverageBPJSManual.Style.Add("display", "none");

                    hdnPatientBPJSAmount.Value = "0";
                    txtPatientBPJSAmount.Text = "0";
                    txtPatientBPJSAmount.Style.Add("display", "none");

                    txtINACBGSCode.Style.Add("display", "none");
                    txtINACBGSName.Style.Add("display", "none");
                }
                #endregion
            }
            else
            {
                trFilterCoverage.Style.Add("display", "none");
                hdnIsControlCoverageLimit.Value = "0";
                hdnIsControlBPJSCoverage.Value = "0";
                trCoverageLimit.Style.Add("display", "none");
                trPatientBPJSAmount.Style.Add("display", "none");

                chkIsEditCoverageBPJSManual.Checked = false;
                chkIsEditCoverageBPJSManual.Style.Add("display", "none");

                txtCoverageLimit.Text = "0";
                txtCoverageLimit.Attributes.Remove("readonly");

                trPatientAmount.Style.Add("display", "none");
                txtPatientAmount.Text = "0";
                hdnPatientAmount.Value = "0";
                txtPatientAmount.Attributes.Add("readonly", "readonly");
            }

            hdnIsCustomerPersonal.Value = entity.GCCustomerType == Constant.CustomerType.PERSONAL ? "1" : "0";

            #region Administration and Service Fee
            GetAdministrationServiceFee(entity);
            #endregion

        }

        private void GetAdministrationServiceFee(vRegistration4 entity)
        {
            bool isFound = false;
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                if (entity.ContractID > 0)
                {
                    CustomerContract contract = BusinessLayer.GetCustomerContract(entity.ContractID);

                    #region old
                    //if (contract.AdministrationFeeAmount > 0)
                    //{
                    //    hdnAdministrationFee.Value = contract.AdministrationFeeAmount.ToString();
                    //    hdnAdministrationFee.Attributes.Add("ispercentage", contract.IsAdministrationFeeInPct ? "1" : "0");
                    //    hdnAdministrationFee.Attributes.Add("maxamount", contract.MaxAdministrationFeeAmount.ToString());
                    //    hdnAdministrationFee.Attributes.Add("minamount", contract.MinAdministrationFeeAmount.ToString());
                    //}

                    //if (contract.PatientAdmFeeAmount > 0)
                    //{
                    //    hdnPatientAdminFee.Value = contract.PatientAdmFeeAmount.ToString();
                    //    hdnPatientAdminFee.Attributes.Add("ispercentage", contract.IsPatientAdmFeeInPct ? "1" : "0");
                    //    hdnPatientAdminFee.Attributes.Add("minamount", contract.MinPatientAdmFeeAmount.ToString());
                    //    hdnPatientAdminFee.Attributes.Add("maxamount", contract.MaxPatientAdmFeeAmount.ToString());
                    //}

                    //if (contract.ServiceFeeAmount > 0)
                    //{
                    //    hdnServiceFee.Value = contract.ServiceFeeAmount.ToString();
                    //    hdnServiceFee.Attributes.Add("ispercentage", contract.IsServiceFeeInPct ? "1" : "0");
                    //    hdnServiceFee.Attributes.Add("maxamount", contract.MaxServiceFeeAmount.ToString());
                    //    hdnServiceFee.Attributes.Add("minamount", contract.MinServiceFeeAmount.ToString());
                    //}

                    //if (contract.PatientServiceFeeAmount > 0)
                    //{
                    //    hdnPatientServiceFee.Value = contract.PatientServiceFeeAmount.ToString();
                    //    hdnPatientServiceFee.Attributes.Add("ispercentage", contract.IsPatientServiceFeeInPct ? "1" : "0");
                    //    hdnPatientServiceFee.Attributes.Add("minamount", contract.MinPatientServiceFeeAmount.ToString());
                    //    hdnPatientServiceFee.Attributes.Add("maxamount", contract.MaxPatientServiceFeeAmount.ToString());
                    //}
                    #endregion

                    #region new
                    hdnAdministrationFee.Value = contract.AdministrationFeeAmount.ToString();
                    hdnAdministrationFee.Attributes.Add("ispercentage", contract.IsAdministrationFeeInPct ? "1" : "0");
                    hdnAdministrationFee.Attributes.Add("maxamount", contract.MaxAdministrationFeeAmount.ToString());
                    hdnAdministrationFee.Attributes.Add("minamount", contract.MinAdministrationFeeAmount.ToString());

                    hdnPatientAdminFee.Value = contract.PatientAdmFeeAmount.ToString();
                    hdnPatientAdminFee.Attributes.Add("ispercentage", contract.IsPatientAdmFeeInPct ? "1" : "0");
                    hdnPatientAdminFee.Attributes.Add("minamount", contract.MinPatientAdmFeeAmount.ToString());
                    hdnPatientAdminFee.Attributes.Add("maxamount", contract.MaxPatientAdmFeeAmount.ToString());

                    hdnServiceFee.Value = contract.ServiceFeeAmount.ToString();
                    hdnServiceFee.Attributes.Add("ispercentage", contract.IsServiceFeeInPct ? "1" : "0");
                    hdnServiceFee.Attributes.Add("maxamount", contract.MaxServiceFeeAmount.ToString());
                    hdnServiceFee.Attributes.Add("minamount", contract.MinServiceFeeAmount.ToString());

                    hdnPatientServiceFee.Value = contract.PatientServiceFeeAmount.ToString();
                    hdnPatientServiceFee.Attributes.Add("ispercentage", contract.IsPatientServiceFeeInPct ? "1" : "0");
                    hdnPatientServiceFee.Attributes.Add("minamount", contract.MinPatientServiceFeeAmount.ToString());
                    hdnPatientServiceFee.Attributes.Add("maxamount", contract.MaxPatientServiceFeeAmount.ToString());
                    #endregion

                    isFound = ((contract.AdministrationFeeAmount > 0) || (contract.PatientAdmFeeAmount > 0) || (contract.ServiceFeeAmount > 0) || (contract.PatientServiceFeeAmount > 0));
                }

                if (!isFound)
                {
                    //Configuration in class
                    ClassCare oClass;
                    if (hdnIsAdministrationFeeUseHigherClass.Value.ToString() == "0")
                    {
                        oClass = BusinessLayer.GetClassCare(entity.ChargeClassID);
                    }
                    else
                    {
                        string filterExp = string.Format("RegistrationID = {0}", entity.RegistrationID);
                        List<vPatientTransfer> oTransferList = BusinessLayer.GetvPatientTransferList(filterExp);
                        int chargeClassID = 0;
                        if (oTransferList.Count > 0)
                        {
                            var result = oTransferList.OrderByDescending(lst => lst.ToChargeClassPriority).FirstOrDefault();
                            var resultFromMax = oTransferList.OrderByDescending(lst => lst.FromChargeClassPriority).FirstOrDefault();
                            if (resultFromMax.FromChargeClassPriority > result.ToChargeClassPriority)
                            {
                                chargeClassID = resultFromMax.FromChargeClassID;
                            }
                            else chargeClassID = result.ToChargeClassID;
                        }
                        else
                        {
                            chargeClassID = entity.ChargeClassID;
                        }
                        oClass = BusinessLayer.GetClassCare(chargeClassID);
                    }

                    if (oClass != null)
                    {
                        if (oClass.AdministrationFeeAmount > 0)
                        {
                            hdnAdministrationFee.Value = oClass.AdministrationFeeAmount.ToString();
                            hdnAdministrationFee.Attributes.Add("ispercentage", oClass.IsAdministrationFeeInPct ? "1" : "0");
                            hdnAdministrationFee.Attributes.Add("maxamount", oClass.MaxAdministrationFeeAmount.ToString());
                            hdnAdministrationFee.Attributes.Add("minamount", oClass.MinAdministrationFeeAmount.ToString());
                        }

                        if (oClass.PatientAdmFeeAmount > 0)
                        {
                            hdnPatientAdminFee.Value = oClass.PatientAdmFeeAmount.ToString();
                            hdnPatientAdminFee.Attributes.Add("ispercentage", oClass.IsPatientAdmFeeInPct ? "1" : "0");
                            hdnPatientAdminFee.Attributes.Add("minamount", oClass.MinPatientAdmFeeAmount.ToString());
                            hdnPatientAdminFee.Attributes.Add("maxamount", oClass.MaxPatientAdmFeeAmount.ToString());
                        }

                        if (oClass.ServiceFeeAmount > 0)
                        {
                            hdnServiceFee.Value = oClass.ServiceFeeAmount.ToString();
                            hdnServiceFee.Attributes.Add("ispercentage", oClass.IsServiceFeeInPct ? "1" : "0");
                            hdnServiceFee.Attributes.Add("maxamount", oClass.MaxServiceFeeAmount.ToString());
                            hdnServiceFee.Attributes.Add("minamount", oClass.MinServiceFeeAmount.ToString());
                        }

                        if (oClass.PatientServiceFeeAmount > 0)
                        {
                            hdnPatientServiceFee.Value = oClass.PatientServiceFeeAmount.ToString();
                            hdnPatientServiceFee.Attributes.Add("ispercentage", oClass.IsPatientServiceFeeInPct ? "1" : "0");
                            hdnPatientServiceFee.Attributes.Add("minamount", oClass.MinPatientServiceFeeAmount.ToString());
                            hdnPatientServiceFee.Attributes.Add("maxamount", oClass.MaxPatientServiceFeeAmount.ToString());
                        }
                    }
                }
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "generatebill")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao entityDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                PatientBillDao patientBillDao = new PatientBillDao(ctx);
                RegistrationDao registrationDao = new RegistrationDao(ctx);
                RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
                AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
                string[] listParam = hdnParam.Value.Split('|');
                string lstTransactionID = "";
                foreach (string param in listParam)
                {
                    if (lstTransactionID != "")
                        lstTransactionID += ",";
                    lstTransactionID += param;
                }

                try
                {
                    if (hdnPembuatanTagihanTidakAdaOutstandingOrder.Value == "0")
                    {
                        #region Tanpa Cek Outstanding Order

                        string statusRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value)).GCRegistrationStatus;
                        string ChargesNoCheck = "";
                        if ((statusRegistration != Constant.VisitStatus.CLOSED)
                            || (!AppSession.IsUsedReopenBilling && !AppSession.RegisteredPatient.IsBillingReopen && statusRegistration != Constant.VisitStatus.CLOSED)
                            || (AppSession.IsUsedReopenBilling && AppSession.RegisteredPatient.IsBillingReopen && statusRegistration == Constant.VisitStatus.CLOSED))
                        {
                            #region Process

                            List<PatientChargesHd> lstPatientChargesHdMain = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID IN ({0})", lstTransactionID), ctx);
                            List<PatientChargesHd> lstPatientChargesHd = lstPatientChargesHdMain.Where(t => t.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL || t.GCTransactionStatus == Constant.TransactionStatus.OPEN).ToList();
                            if (lstPatientChargesHd.Count > 0)
                            {
                                List<PatientChargesDt> lstAllPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID IN ({0}) AND LocationID IS NOT NULL AND IsApproved = 0 AND IsDeleted = 0", lstTransactionID), ctx);

                                PatientBill patientBill = new PatientBill();
                                patientBill.BillingDate = DateTime.Now;
                                patientBill.BillingTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                patientBill.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                                string transactionCode = "";
                                switch (hdnDepartmentID.Value)
                                {
                                    case Constant.Facility.INPATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_BILL; break;
                                    case Constant.Facility.MEDICAL_CHECKUP: transactionCode = Constant.TransactionCode.MCU_PATIENT_BILL; break;
                                    case Constant.Facility.EMERGENCY: transactionCode = Constant.TransactionCode.ER_PATIENT_BILL; break;
                                    case Constant.Facility.PHARMACY: transactionCode = Constant.TransactionCode.PH_PATIENT_BILL; break;
                                    case Constant.Facility.DIAGNOSTIC:
                                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                            transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_BILL;
                                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                            transactionCode = Constant.TransactionCode.IMAGING_PATIENT_BILL;
                                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                                            transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_BILL;
                                        else
                                            transactionCode = Constant.TransactionCode.OTHER_PATIENT_BILL; break;
                                    default: transactionCode = Constant.TransactionCode.OP_PATIENT_BILL; break;
                                }

                                if (hdnIsControlCoverageLimit.Value == "1" && txtCoverageLimit.Text != "")
                                    patientBill.CoverageAmount = Convert.ToDecimal(txtCoverageLimit.Text);
                                else
                                    patientBill.CoverageAmount = 0;

                                patientBill.AdministrationFeeAmount = Convert.ToDecimal(hdnAdministrationFeeAmount.Value);
                                patientBill.PatientAdminFeeAmount = Convert.ToDecimal(hdnPatientAdministrationFeeAmount.Value);
                                patientBill.ServiceFeeAmount = Convert.ToDecimal(hdnServiceFeeAmount.Value);
                                patientBill.PatientServiceFeeAmount = Convert.ToDecimal(hdnPatientServiceFeeAmount.Value);
                                patientBill.DiffCoverageAmount = Convert.ToDecimal(hdnDiffCoverageAmount.Value);
                                patientBill.PatientBillingNo = BusinessLayer.GenerateTransactionNo(transactionCode, patientBill.BillingDate, ctx);
                                patientBill.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                                patientBill.TotalPatientAmount = Convert.ToDecimal(hdnTotalPatientAmount.Value);
                                patientBill.TotalPayerAmount = Convert.ToDecimal(hdnTotalPayerAmount.Value);
                                patientBill.TotalAmount = Convert.ToDecimal(hdnTotalAmount.Value);

                                if (hdnBPJSMenggunakanCaraCoverageBPJS.Value == "0")
                                {
                                    if (patientBill.TotalAmount != (
                                                    lstPatientChargesHd.Sum(t => t.TotalAmount)
                                                    + patientBill.ServiceFeeAmount + patientBill.PatientServiceFeeAmount
                                                    + patientBill.AdministrationFeeAmount + patientBill.PatientAdminFeeAmount
                                                )
                                        )
                                    {
                                        result = false;
                                        errMessage = "Harap Refresh halaman ini.";
                                    }
                                }

                                patientBill.GCVoidReason = null;
                                patientBill.VoidReason = null;
                                patientBill.CreatedBy = AppSession.UserLogin.UserID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                patientBill.PatientBillingID = patientBillDao.InsertReturnPrimaryKeyID(patientBill);

                                DateTime maxProposedDate = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                                Int32 maxProposedBy = 0;

                                foreach (PatientChargesHd patientChargesHd in lstPatientChargesHdMain)
                                {
                                    PatientChargesHd entityCheck = entityDao.Get(patientChargesHd.TransactionID);
                                    if (hdnIsAllowOPENForValidation.Value == "1")
                                    {
                                        if (entityCheck.GCTransactionStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL && entityCheck.GCTransactionStatus != Constant.TransactionStatus.APPROVED && entityCheck.GCTransactionStatus != Constant.TransactionStatus.PROCESSED)
                                        {
                                            if (String.IsNullOrEmpty(ChargesNoCheck))
                                            {
                                                ChargesNoCheck = entityCheck.TransactionNo;
                                            }
                                            else
                                            {
                                                ChargesNoCheck += string.Format(" ,{0}", entityCheck.TransactionNo);
                                            }
                                        }
                                    }
                                }

                                foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                                {
                                    if (patientChargesHd.ProposedDate != null)
                                    {
                                        if (maxProposedDate == Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL))
                                        {
                                            maxProposedDate = patientChargesHd.ProposedDate;
                                            maxProposedBy = Convert.ToInt32(patientChargesHd.ProposedBy);
                                        }
                                        else
                                        {
                                            if (patientChargesHd.ProposedDate > maxProposedDate)
                                            {
                                                maxProposedDate = patientChargesHd.ProposedDate;
                                                maxProposedBy = Convert.ToInt32(patientChargesHd.ProposedBy);
                                            }
                                        }
                                    }

                                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                    patientChargesHd.PatientBillingID = patientBill.PatientBillingID;
                                    patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDao.Update(patientChargesHd);

                                    List<PatientChargesDt> lstPatientChargesDt = lstAllPatientChargesDt.Where(p => p.TransactionID == patientChargesHd.TransactionID).ToList();
                                    foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                                    {
                                        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED; // dibuka comment nya oleh RN di 20200531
                                        patientChargesDt.IsApproved = true;
                                        patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityDtDao.Update(patientChargesDt);
                                    }

                                    if (maxProposedBy != 0)
                                    {
                                        patientBill.LastChargesProposedBy = maxProposedBy;
                                        patientBill.LastChargesProposedDate = maxProposedDate;
                                    }
                                    //patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientBillDao.Update(patientBill);

                                    //Update Registration : Coverage Limit : if has been changed
                                    decimal coverageLimit = Convert.ToDecimal(hdnRemainingCoverageAmount.Value);
                                    decimal oldCoverageLimit = Convert.ToDecimal(hdnOldRemainingCoverageAmount.Value);

                                    if (oldCoverageLimit != coverageLimit)
                                    {
                                        AuditLog entityAuditLog = new AuditLog();

                                        Registration oRegistration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                                        entityAuditLog.OldValues = JsonConvert.SerializeObject(oRegistration);

                                        oRegistration.CoverageLimitAmount += coverageLimit;

                                        entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                                        entityAuditLog.NewValues = JsonConvert.SerializeObject(oRegistration);
                                        entityAuditLog.UserID = AppSession.UserLogin.UserID;
                                        entityAuditLog.LogDate = DateTime.Now;
                                        entityAuditLog.TransactionID = oRegistration.RegistrationID;
                                        entityAuditLogDao.Insert(entityAuditLog);

                                        if (hdnIsControlBPJSCoverage.Value == "1") oRegistration.BPJSAmount += coverageLimit;
                                        oRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        registrationDao.Update(oRegistration);

                                        RegistrationPayer oRegistrationPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationID.Value), ctx).FirstOrDefault();
                                        if (oRegistrationPayer != null)
                                        {
                                            oRegistrationPayer.CoverageLimitAmount += coverageLimit;
                                            oRegistrationPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            registrationPayerDao.Update(oRegistrationPayer);
                                        }
                                    }
                                }

                            }

                            if (String.IsNullOrEmpty(ChargesNoCheck))
                            {
                                if (result)
                                {
                                    ctx.CommitTransaction();
                                }
                                else
                                {
                                    result = false;
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                    ctx.RollBackTransaction();
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = string.Format("Nomor transaksi {0} Tidak dapat diproses. Harap Refresh Halaman ini.", ChargesNoCheck);
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                            #endregion
                        }
                        else
                        {
                            result = false;
                            errMessage = "Registrasi untuk pasien ini sudah di tutup/batal, tagihan sudah di transfer ke rawat inap";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        #endregion
                    }
                    else
                    {
                        #region Cek Outstanding Order

                        List<TestOrderHd> lstPendingTestOrderHd = BusinessLayer.GetTestOrderHdList(string.Format("LinkedChargesID IN ({0}) AND GCTransactionStatus = '{1}'", lstTransactionID, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                        if (lstPendingTestOrderHd.Count > 0)
                        {
                            result = false;
                            errMessage = "Masih Ada Order Penunjang Medis Yang Belum Direalisasi.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        else
                        {
                            List<ServiceOrderHd> lstPendingServiceOrderHd = BusinessLayer.GetServiceOrderHdList(string.Format("LinkedChargesID IN ({0}) AND GCTransactionStatus = '{1}'", lstTransactionID, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                            if (lstPendingServiceOrderHd.Count > 0)
                            {
                                result = false;
                                errMessage = "Masih Ada Order Pelayanan Yang Belum Direalisasi.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                            else
                            {
                                string statusRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value)).GCRegistrationStatus;
                                string ChargesNoCheck = "";
                                if ((statusRegistration != Constant.VisitStatus.CLOSED)
                                    || (!AppSession.IsUsedReopenBilling && !AppSession.RegisteredPatient.IsBillingReopen && statusRegistration != Constant.VisitStatus.CLOSED)
                                    || (AppSession.IsUsedReopenBilling && AppSession.RegisteredPatient.IsBillingReopen && statusRegistration == Constant.VisitStatus.CLOSED))
                                {
                                    #region Process

                                    List<PatientChargesHd> lstPatientChargesHdMain = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID IN ({0})", lstTransactionID), ctx);
                                    List<PatientChargesHd> lstPatientChargesHd = lstPatientChargesHdMain.Where(t => t.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL || t.GCTransactionStatus == Constant.TransactionStatus.OPEN).ToList();

                                    if (lstPatientChargesHd.Count > 0)
                                    {
                                        List<PatientChargesDt> lstAllPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID IN ({0}) AND LocationID IS NOT NULL AND IsApproved = 0 AND IsDeleted = 0", lstTransactionID), ctx);
                                        PatientBill patientBill = new PatientBill();
                                        patientBill.BillingDate = DateTime.Now;
                                        patientBill.BillingTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                        patientBill.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                                        string transactionCode = "";
                                        switch (hdnDepartmentID.Value)
                                        {
                                            case Constant.Facility.INPATIENT: transactionCode = Constant.TransactionCode.IP_PATIENT_BILL; break;
                                            case Constant.Facility.MEDICAL_CHECKUP: transactionCode = Constant.TransactionCode.MCU_PATIENT_BILL; break;
                                            case Constant.Facility.EMERGENCY: transactionCode = Constant.TransactionCode.ER_PATIENT_BILL; break;
                                            case Constant.Facility.PHARMACY: transactionCode = Constant.TransactionCode.PH_PATIENT_BILL; break;
                                            case Constant.Facility.DIAGNOSTIC:
                                                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                                    transactionCode = Constant.TransactionCode.LABORATORY_PATIENT_BILL;
                                                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                                    transactionCode = Constant.TransactionCode.IMAGING_PATIENT_BILL;
                                                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                                                    transactionCode = Constant.TransactionCode.RADIOTHERAPHY_PATIENT_BILL;
                                                else
                                                    transactionCode = Constant.TransactionCode.OTHER_PATIENT_BILL; break;
                                            default: transactionCode = Constant.TransactionCode.OP_PATIENT_BILL; break;
                                        }

                                        if (hdnIsControlCoverageLimit.Value == "1" && txtCoverageLimit.Text != "")
                                            patientBill.CoverageAmount = Convert.ToDecimal(txtCoverageLimit.Text);
                                        else
                                            patientBill.CoverageAmount = 0;

                                        patientBill.AdministrationFeeAmount = Convert.ToDecimal(hdnAdministrationFeeAmount.Value);
                                        patientBill.PatientAdminFeeAmount = Convert.ToDecimal(hdnPatientAdministrationFeeAmount.Value);
                                        patientBill.ServiceFeeAmount = Convert.ToDecimal(hdnServiceFeeAmount.Value);
                                        patientBill.PatientServiceFeeAmount = Convert.ToDecimal(hdnPatientServiceFeeAmount.Value);
                                        patientBill.DiffCoverageAmount = Convert.ToDecimal(hdnDiffCoverageAmount.Value);
                                        patientBill.PatientBillingNo = BusinessLayer.GenerateTransactionNo(transactionCode, patientBill.BillingDate, ctx);
                                        patientBill.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                                        patientBill.TotalPatientAmount = Convert.ToDecimal(hdnTotalPatientAmount.Value);
                                        patientBill.TotalPayerAmount = Convert.ToDecimal(hdnTotalPayerAmount.Value);
                                        patientBill.TotalAmount = Convert.ToDecimal(hdnTotalAmount.Value);

                                        if (hdnBPJSMenggunakanCaraCoverageBPJS.Value == "0")
                                        {
                                            if (patientBill.TotalAmount != (
                                                            lstPatientChargesHd.Sum(t => t.TotalAmount)
                                                            + patientBill.ServiceFeeAmount + patientBill.PatientServiceFeeAmount
                                                            + patientBill.AdministrationFeeAmount + patientBill.PatientAdminFeeAmount
                                                        )
                                                )
                                            {
                                                result = false;
                                                errMessage = "Harap Refresh halaman ini.";
                                            }
                                        }

                                        patientBill.GCVoidReason = null;
                                        patientBill.VoidReason = null;
                                        patientBill.CreatedBy = AppSession.UserLogin.UserID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        patientBill.PatientBillingID = patientBillDao.InsertReturnPrimaryKeyID(patientBill);

                                        DateTime maxProposedDate = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                                        Int32 maxProposedBy = 0;

                                        foreach (PatientChargesHd patientChargesHd in lstPatientChargesHdMain)
                                        {
                                            PatientChargesHd entityCheck = entityDao.Get(patientChargesHd.TransactionID);
                                            if (hdnIsAllowOPENForValidation.Value == "1")
                                            {
                                                if (entityCheck.GCTransactionStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL && entityCheck.GCTransactionStatus != Constant.TransactionStatus.APPROVED && entityCheck.GCTransactionStatus != Constant.TransactionStatus.PROCESSED)
                                                {
                                                    if (String.IsNullOrEmpty(ChargesNoCheck))
                                                    {
                                                        ChargesNoCheck = entityCheck.TransactionNo;
                                                    }
                                                    else
                                                    {
                                                        ChargesNoCheck += string.Format(" ,{0}", entityCheck.TransactionNo);
                                                    }
                                                }
                                            }
                                        }

                                        foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                                        {
                                            if (patientChargesHd.ProposedDate != null)
                                            {
                                                if (maxProposedDate == Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL))
                                                {
                                                    maxProposedDate = patientChargesHd.ProposedDate;
                                                    maxProposedBy = Convert.ToInt32(patientChargesHd.ProposedBy);
                                                }
                                                else
                                                {
                                                    if (patientChargesHd.ProposedDate > maxProposedDate)
                                                    {
                                                        maxProposedDate = patientChargesHd.ProposedDate;
                                                        maxProposedBy = Convert.ToInt32(patientChargesHd.ProposedBy);
                                                    }
                                                }
                                            }

                                            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                            patientChargesHd.PatientBillingID = patientBill.PatientBillingID;
                                            patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityDao.Update(patientChargesHd);

                                            List<PatientChargesDt> lstPatientChargesDt = lstAllPatientChargesDt.Where(p => p.TransactionID == patientChargesHd.TransactionID).ToList();
                                            foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                                            {
                                                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED; // dibuka comment nya oleh RN di 20200531
                                                patientChargesDt.IsApproved = true;
                                                patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                entityDtDao.Update(patientChargesDt);
                                            }
                                        }

                                        if (maxProposedBy != 0)
                                        {
                                            patientBill.LastChargesProposedBy = maxProposedBy;
                                            patientBill.LastChargesProposedDate = maxProposedDate;
                                        }
                                        //patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        patientBillDao.Update(patientBill);

                                        //Update Registration : Coverage Limit : if has been changed
                                        decimal coverageLimit = Convert.ToDecimal(hdnRemainingCoverageAmount.Value);
                                        decimal oldCoverageLimit = Convert.ToDecimal(hdnOldRemainingCoverageAmount.Value);

                                        if (oldCoverageLimit != coverageLimit)
                                        {
                                            AuditLog entityAuditLog = new AuditLog();
                                            Registration oRegistration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                                            entityAuditLog.OldValues = JsonConvert.SerializeObject(oRegistration);
                                            RegistrationPayer oRegistrationPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationID.Value), ctx).FirstOrDefault();
                                            oRegistration.CoverageLimitAmount += coverageLimit;
                                            entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                                            entityAuditLog.NewValues = JsonConvert.SerializeObject(oRegistration);
                                            entityAuditLog.UserID = AppSession.UserLogin.UserID;
                                            entityAuditLog.LogDate = DateTime.Now;
                                            entityAuditLog.TransactionID = oRegistration.RegistrationID;
                                            entityAuditLogDao.Insert(entityAuditLog);
                                            if (hdnIsControlBPJSCoverage.Value == "1") oRegistration.BPJSAmount += coverageLimit;
                                            oRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            registrationDao.Update(oRegistration);
                                            if (oRegistrationPayer != null)
                                            {
                                                oRegistrationPayer.CoverageLimitAmount += coverageLimit;
                                                oRegistrationPayer.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                registrationPayerDao.Update(oRegistrationPayer);
                                            }
                                        }
                                    }

                                    if (String.IsNullOrEmpty(ChargesNoCheck))
                                    {
                                        if (result)
                                        {
                                            ctx.CommitTransaction();
                                        }
                                        else
                                        {
                                            result = false;
                                            Exception ex = new Exception(errMessage);
                                            Helper.InsertErrorLog(ex);
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                        errMessage = string.Format("Nomor transaksi {0} Tidak dapat diproses. Harap Refresh Halaman ini.", ChargesNoCheck);
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        ctx.RollBackTransaction();
                                    }

                                    #endregion
                                }
                                else
                                {
                                    result = false;
                                    errMessage = "Registrasi untuk pasien ini sudah di tutup/batal, tagihan sudah di transfer ke rawat inap";
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                    ctx.RollBackTransaction();
                                }
                            }
                        }

                        #endregion
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
            return true;
        }
    }
}