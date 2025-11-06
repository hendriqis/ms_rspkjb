using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryGenerateBillAR : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";
        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected string GetCoverageLabel()
        {
            switch (hdnIsControlBPJSCoverage.Value)
            {
                case "1": return GetLabel("INACBG's Grouper");
                default: return GetLabel("Batas Tanggungan");
            }
        }

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_GENERATE_BILL_AR;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_GENERATE_BILL_AR;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_GENERATE_BILL_AR;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_GENERATE_BILL_AR;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_GENERATE_BILL_AR;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_GENERATE_BILL_AR;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_GENERATE_BILL_AR;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_GENERATE_BILL_AR;
            }
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
            vRegistration4 entity = BusinessLayer.GetvRegistration4List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID))[0];
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            hdnIsBPJSClass.Value = entity.IsBPJSClass ? "1" : "0";
            hdnIsLocked.Value = entity.IsLockDown ? "1" : "0";
            EntityToControl(entity);

            string filterExpressionOutstandingOrder = string.Format("RegistrationID = {0}", entity.RegistrationID);
            vRegistrationOutstandingInfo lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpressionOutstandingOrder).FirstOrDefault();
            bool outstanding = (lstInfo.ServiceOrder + lstInfo.PrescriptionOrder + lstInfo.PrescriptionReturnOrder + lstInfo.LaboratoriumOrder + lstInfo.RadiologiOrder + lstInfo.OtherOrder > 0);
            int count = 0;

            if (entity.LinkedRegistrationID != 0)
            {
                string filterExpressionLinked = string.Format("(RegistrationID = {0} AND IsChargesTransfered = 0) AND GCTransactionStatus NOT IN ('{1}','{2}') AND IsDeleted = 0", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                count = BusinessLayer.GetvPatientChargesDtRowCount(filterExpressionLinked);
            }
            else
                tblInfoOutstandingTransfer.Style.Add("display", "none");

            if (count < 1 && !outstanding)
            {
                tblInfoOutstandingTransfer.Style.Add("display", "none");
                hdnIsAllowGenerateBill.Value = "1";
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
            }

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

            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vPatientChargesHd WHERE {0}) AND IsDeleted = 0", filterExpression));
            lstHSU.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            List<Variable> lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
            lstVariable.Add(new Variable { Code = "1", Value = "Belum Diverifikasi" });
            lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diverifikasi" });
            Methods.SetComboBoxField<Variable>(cboDisplay, lstVariable, "Value", "Code");
            cboDisplay.Value = "0";

            txtPatientAmount.Text = "0";
            hdnPatientAmount.Value = "0";
            txtPatientAmount.Attributes.Add("readonly", "readonly");

            BindGridDetail();
        }

        private void GetSettingParameter()
        {
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}')",
                //HealthcareID = '{0}' AND -- AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.FN_KONTROL_PEMBUATAN_TAGIHAN));

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_BIAYA_ADM_RI_DALAM_PERSENTASE, Constant.SettingParameter.FN_NILAI_BIAYA_ADM_RI, Constant.SettingParameter.FN_NILAI_MIN_BIAYA_ADM_RI,
                    Constant.SettingParameter.FN_NILAI_MAX_BIAYA_ADM_RI, Constant.SettingParameter.FN_BIAYA_SERVICE_RI_DALAM_PERSENTASE, Constant.SettingParameter.FN_NILAI_BIAYA_SERVICE_RI, Constant.SettingParameter.FN_NILAI_MIN_BIAYA_SERVICE_RI,
                    Constant.SettingParameter.FN_NILAI_MAX_BIAYA_SERVICE_RI, Constant.SettingParameter.FN_SELISIH_PASIEN_BPJS_NAIK_KELAS, Constant.SettingParameter.FN_BIAYA_ADM_KELAS_TERTINGGI, Constant.SettingParameter.FN_ADMIN_HANYA_RAWAT_INAP));

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
        }

        protected string GetRemainingCoverageAmount()
        {
            return Convert.ToDecimal(hdnRemainingCoverageAmount.Value).ToString("N2");
        }

        protected string GetDiffCoverageAmount()
        {
            return Convert.ToDecimal(hdnDiffCoverageAmount.Value).ToString("N2");
        }

        private void BindGridDetail()
        {
            string filter;

            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filter = string.Format("((RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1)) AND GCTransactionStatus <> '{2}')", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value, Constant.TransactionStatus.VOID);
            else
                filter = string.Format("RegistrationID = {0} AND GCTransactionStatus <> '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);


            int healthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            if (healthcareServiceUnitID > 0)
                filter += string.Format(" AND HealthcareServiceUnitID = {0}", healthcareServiceUnitID);
            hdnAllowPrint.Value = string.Empty;
            string cboDisplayValue = cboDisplay.Value.ToString();
            if (cboDisplayValue == "1")
                filter += string.Format(" AND IsVerified = 0");
            else if (cboDisplayValue == "2")
                filter += string.Format(" AND IsVerified = 1");
            filter += " ORDER BY ServiceUnitName";
            List<vPatientChargesHdChargeClass> lstTemp = BusinessLayer.GetvPatientChargesHdChargeClassList(filter);
            List<vPatientChargesHdChargeClass> lst = lstTemp.Where(t => t.GCTransactionStatus != Constant.TransactionStatus.PROCESSED && t.GCTransactionStatus != Constant.TransactionStatus.CLOSED).ToList();

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                if (lstTemp.Count(t => t.DepartmentID == Constant.Facility.OUTPATIENT || t.DepartmentID == Constant.Facility.EMERGENCY) > 0)
                {
                    hdnAllowPrint.Value += "PTR";
                }
            }
            string tempPatientChargedHd = string.Join(",", lstTemp.Select(p => p.TransactionID));

            tempPatientChargedHd = "'" + tempPatientChargedHd.Replace(",", "','") + "'";
            List<vPatientChargesDt> lstDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID IN ({0}) AND GCItemType IN ('{1}','{2}')", tempPatientChargedHd, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS));
            if (lstDt.Count > 0) hdnAllowPrint.Value += "DRUG";
            List<vPatientChargesHdChargeClass> lstDiagnostic = lstTemp.Where(t => t.DepartmentID == Constant.Facility.DIAGNOSTIC).ToList();
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
            lvwView.DataSource = lst;
            lvwView.DataBind();

            if (hdnIsControlCoverageLimit.Value == "0")
            {
                HtmlTableRow trFooterRemainingCoverageLimit = lvwView.FindControl("trFooterRemainingCoverageLimit") as HtmlTableRow;
                HtmlTableRow trFooterRemainingTotalBill = lvwView.FindControl("trFooterRemainingTotalBill") as HtmlTableRow;
                HtmlTableRow trDiffCoverageAmount = lvwView.FindControl("trFooterDiffCoverageLimit") as HtmlTableRow;
                if (trFooterRemainingCoverageLimit != null)
                {
                    trFooterRemainingTotalBill.Style.Add("display", "none");
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
                    if (trDiffCoverageAmount != null)
                        trDiffCoverageAmount.Style.Add("display", "table-row");
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

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
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
                    trFilterCoverage.Style.Add("display", "none");

                    hdnIsControlBPJSCoverage.Value = "1";
                    trPatientBPJSAmount.Style.Add("display", "table-row");

                    trPatientAmount.Style.Add("display", "none");
                    txtPatientAmount.Text = "0";
                    hdnPatientAmount.Value = "0";
                    txtPatientAmount.Attributes.Add("readonly", "readonly");

                    if (AppSession.IsBridgingToBPJS)
                    {
                        RegistrationBPJS entityRegistrationBPJS = BusinessLayer.GetRegistrationBPJS(entity.RegistrationID);
                        if (entityRegistrationBPJS != null && hdnDepartmentID.Value != Constant.Facility.PHARMACY)
                        {
                            txtCoverageLimit.Attributes.Add("readonly", "readonly");
                            txtINACBGSCode.Attributes.Add("readonly", "readonly");
                            txtINACBGSName.Attributes.Add("readonly", "readonly");
                            txtINACBGSCode.Text = entityRegistrationBPJS.GrouperCode;
                            txtINACBGSName.Text = entityRegistrationBPJS.GrouperName;
                        }
                        else
                        {
                            //txtPatientBPJSAmount.Attributes.Add("readonly", "false");
                            txtCoverageLimit.Attributes.Add("readonly", "readonly");
                            txtINACBGSCode.Attributes.Add("readonly", "readonly");
                            txtINACBGSName.Attributes.Add("readonly", "readonly");
                        }
                    }
                    else
                    {
                        //txtPatientBPJSAmount.Attributes.Add("readonly", "false");
                        //txtCoverageLimit.Attributes.Add("readonly", "false");
                        txtINACBGSCode.Attributes.Add("readonly", "readonly");
                        txtINACBGSName.Attributes.Add("readonly", "readonly");
                    }
                }
                else
                {
                    trPatientBPJSAmount.Style.Add("display", "none");
                    hdnIsControlBPJSCoverage.Value = "0";
                    hdnPatientBPJSAmount.Value = "0";
                    txtPatientBPJSAmount.Text = "0";
                    txtPatientBPJSAmount.Style.Add("display", "none");
                    //txtCoverageLimit.Attributes.Add("readonly", "false");
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
                    if (contract.AdministrationFeeAmount > 0)
                    {
                        hdnAdministrationFee.Value = contract.AdministrationFeeAmount.ToString();
                        hdnAdministrationFee.Attributes.Add("ispercentage", contract.IsAdministrationFeeInPct ? "1" : "0");
                        hdnAdministrationFee.Attributes.Add("maxamount", contract.MaxAdministrationFeeAmount.ToString());
                        hdnAdministrationFee.Attributes.Add("minamount", contract.MinAdministrationFeeAmount.ToString());
                    }

                    if (contract.PatientAdmFeeAmount > 0)
                    {
                        hdnPatientAdminFee.Value = contract.PatientAdmFeeAmount.ToString();
                        hdnPatientAdminFee.Attributes.Add("ispercentage", contract.IsPatientAdmFeeInPct ? "1" : "0");
                        hdnPatientAdminFee.Attributes.Add("minamount", contract.MinPatientAdmFeeAmount.ToString());
                        hdnPatientAdminFee.Attributes.Add("maxamount", contract.MaxPatientAdmFeeAmount.ToString());
                    }

                    if (contract.ServiceFeeAmount > 0)
                    {
                        hdnServiceFee.Value = contract.ServiceFeeAmount.ToString();
                        hdnServiceFee.Attributes.Add("ispercentage", contract.IsServiceFeeInPct ? "1" : "0");
                        hdnServiceFee.Attributes.Add("maxamount", contract.MaxServiceFeeAmount.ToString());
                        hdnServiceFee.Attributes.Add("minamount", contract.MinServiceFeeAmount.ToString());
                    }

                    if (contract.PatientServiceFeeAmount > 0)
                    {
                        hdnPatientServiceFee.Value = contract.PatientServiceFeeAmount.ToString();
                        hdnPatientServiceFee.Attributes.Add("ispercentage", contract.IsPatientServiceFeeInPct ? "1" : "0");
                        hdnPatientServiceFee.Attributes.Add("minamount", contract.MinPatientServiceFeeAmount.ToString());
                        hdnPatientServiceFee.Attributes.Add("maxamount", contract.MaxPatientServiceFeeAmount.ToString());
                    }

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

        private string SetGCCashierGroupValue()
        {
            string CashierGroup = "";
            List<StandardCode> lstCashierGroup = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CASHIER_GROUP));
            CashierGroup = lstCashierGroup.FirstOrDefault().StandardCodeID;
            return CashierGroup;
        }

        private string SetGCShiftValue()
        {
            string shift = "";
            List<StandardCode> lstShift = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.SHIFT));
            hdnListShift.Value = string.Join("|", lstShift.Select(p => string.Format("{0},{1}", p.StandardCodeID, p.TagProperty)).ToList());
            string timeNow = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            string[] arrListShift = hdnListShift.Value.Split('|');
            foreach (string listShift in arrListShift)
            {
                string[] temp = listShift.Split(',');
                string[] shiftTime = temp[1].Split(';');
                if (string.Compare(shiftTime[0], shiftTime[1]) < 0)
                {
                    if (string.Compare(shiftTime[0], timeNow) <= 0 && string.Compare(shiftTime[1], timeNow) >= 0)
                    {
                        shift = temp[0];
                    }
                }
                else
                {
                    if (string.Compare(shiftTime[0], timeNow) >= 0 || string.Compare(shiftTime[1], timeNow) <= 0)
                    {
                        shift = temp[0];
                    }
                }
            }
            return shift;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            PatientPaymentHdDao entityPaymentHdDao = new PatientPaymentHdDao(ctx);
            PatientPaymentDtDao entityPaymentDtDao = new PatientPaymentDtDao(ctx);
            PatientBillPaymentDao patientBillPaymentDao = new PatientBillPaymentDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);

            if (type == "generatebill")
            {
                #region Generate Bill

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
                    List<TestOrderHd> lstPendingTestOrderHd = BusinessLayer.GetTestOrderHdList(string.Format("LinkedChargesID IN ({0}) AND GCTransactionStatus = '{1}'", lstTransactionID, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                    if (lstPendingTestOrderHd.Count > 0)
                    {
                        result = false;
                        errMessage = "Masih Ada Order Penunjang Medis Yang Belum Direalisasi.";
                    }
                    else
                    {
                        List<ServiceOrderHd> lstPendingServiceOrderHd = BusinessLayer.GetServiceOrderHdList(string.Format("LinkedChargesID IN ({0}) AND GCTransactionStatus = '{1}'", lstTransactionID, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                        if (lstPendingServiceOrderHd.Count > 0)
                        {
                            result = false;
                            errMessage = "Masih Ada Order Pelayanan Yang Belum Direalisasi.";
                        }
                        else
                        {
                            string statusRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value)).GCRegistrationStatus;
                            if (statusRegistration == Constant.VisitStatus.CLOSED || statusRegistration == Constant.VisitStatus.CANCELLED)
                            {
                                result = false;
                                errMessage = "Registrasi untuk pasien ini sudah di tutup/batal, tagihan sudah di transfer ke rawat inap";
                            }
                            else
                            {
                                List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", lstTransactionID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN), ctx);
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

                                    patientBill.GCVoidReason = null;
                                    patientBill.VoidReason = null;
                                    patientBill.LastUpdatedBy = patientBill.CreatedBy = AppSession.UserLogin.UserID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientBill.PatientBillingID = patientBillDao.InsertReturnPrimaryKeyID(patientBill);
                                    foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                                    {
                                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                        patientChargesHd.PatientBillingID = patientBill.PatientBillingID;
                                        patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityDao.Update(patientChargesHd);

                                        List<PatientChargesDt> lstPatientChargesDt = lstAllPatientChargesDt.Where(p => p.TransactionID == patientChargesHd.TransactionID).ToList();
                                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                                        {
                                            //patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED;
                                            patientChargesDt.IsApproved = true;
                                            patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityDtDao.Update(patientChargesDt);
                                        }
                                    }

                                    //Update Registration : Coverage Limit : if has been changed
                                    decimal coverageLimit = Convert.ToDecimal(hdnRemainingCoverageAmount.Value);
                                    decimal oldCoverageLimit = Convert.ToDecimal(hdnOldRemainingCoverageAmount.Value);

                                    if (oldCoverageLimit != coverageLimit)
                                    {
                                        AuditLog entityAuditLog = new AuditLog();
                                        Registration oRegistration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                                        entityAuditLog.OldValues = JsonConvert.SerializeObject(oRegistration);
                                        RegistrationPayer oRegistrationPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationID.Value)).FirstOrDefault();
                                        oRegistration.CoverageLimitAmount = coverageLimit;
                                        entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                                        entityAuditLog.NewValues = JsonConvert.SerializeObject(oRegistration);
                                        entityAuditLog.UserID = AppSession.UserLogin.UserID;
                                        entityAuditLog.LogDate = DateTime.Now;
                                        entityAuditLog.TransactionID = oRegistration.RegistrationID;
                                        entityAuditLogDao.Insert(entityAuditLog);
                                        if (hdnIsControlBPJSCoverage.Value == "1") oRegistration.BPJSAmount = coverageLimit;
                                        registrationDao.Update(oRegistration);
                                        if (oRegistrationPayer != null)
                                        {
                                            oRegistrationPayer.CoverageLimitAmount = coverageLimit;
                                            registrationPayerDao.Update(oRegistrationPayer);
                                        }
                                    }
                                }
                            }
                        }
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

                #endregion
            }
            else if (type == "generatebillwithar")
            {
                #region Generate Bill with Create AR

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
                    List<TestOrderHd> lstPendingTestOrderHd = BusinessLayer.GetTestOrderHdList(string.Format("LinkedChargesID IN ({0}) AND GCTransactionStatus = '{1}'", lstTransactionID, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                    if (lstPendingTestOrderHd.Count > 0)
                    {
                        result = false;
                        errMessage = "Masih Ada Order Penunjang Medis Yang Belum Direalisasi.";
                    }
                    else
                    {
                        List<ServiceOrderHd> lstPendingServiceOrderHd = BusinessLayer.GetServiceOrderHdList(string.Format("LinkedChargesID IN ({0}) AND GCTransactionStatus = '{1}'", lstTransactionID, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                        if (lstPendingServiceOrderHd.Count > 0)
                        {
                            result = false;
                            errMessage = "Masih Ada Order Pelayanan Yang Belum Direalisasi.";
                        }
                        else
                        {
                            string statusRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value)).GCRegistrationStatus;
                            if (statusRegistration == Constant.VisitStatus.CLOSED || statusRegistration == Constant.VisitStatus.CANCELLED)
                            {
                                result = false;
                                errMessage = "Registrasi untuk pasien ini sudah di tutup/batal, tagihan sudah di transfer ke rawat inap";
                            }
                            else
                            {
                                List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", lstTransactionID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN), ctx);
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

                                    patientBill.GCVoidReason = null;
                                    patientBill.VoidReason = null;
                                    patientBill.LastUpdatedBy = patientBill.CreatedBy = AppSession.UserLogin.UserID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    patientBill.PatientBillingID = patientBillDao.InsertReturnPrimaryKeyID(patientBill);
                                    foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                                    {
                                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                        patientChargesHd.PatientBillingID = patientBill.PatientBillingID;
                                        patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityDao.Update(patientChargesHd);

                                        List<PatientChargesDt> lstPatientChargesDt = lstAllPatientChargesDt.Where(p => p.TransactionID == patientChargesHd.TransactionID).ToList();
                                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                                        {
                                            //patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED;
                                            patientChargesDt.IsApproved = true;
                                            patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityDtDao.Update(patientChargesDt);
                                        }
                                    }

                                    //Update Registration : Coverage Limit : if has been changed
                                    decimal coverageLimit = Convert.ToDecimal(hdnRemainingCoverageAmount.Value);
                                    decimal oldCoverageLimit = Convert.ToDecimal(hdnOldRemainingCoverageAmount.Value);

                                    if (oldCoverageLimit != coverageLimit)
                                    {
                                        AuditLog entityAuditLog = new AuditLog();
                                        Registration oRegistration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                                        entityAuditLog.OldValues = JsonConvert.SerializeObject(oRegistration);
                                        RegistrationPayer oRegistrationPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationID.Value)).FirstOrDefault();
                                        oRegistration.CoverageLimitAmount = coverageLimit;
                                        entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION;
                                        entityAuditLog.NewValues = JsonConvert.SerializeObject(oRegistration);
                                        entityAuditLog.UserID = AppSession.UserLogin.UserID;
                                        entityAuditLog.LogDate = DateTime.Now;
                                        entityAuditLog.TransactionID = oRegistration.RegistrationID;
                                        entityAuditLogDao.Insert(entityAuditLog);
                                        if (hdnIsControlBPJSCoverage.Value == "1") oRegistration.BPJSAmount = coverageLimit;
                                        registrationDao.Update(oRegistration);
                                        if (oRegistrationPayer != null)
                                        {
                                            oRegistrationPayer.CoverageLimitAmount = coverageLimit;
                                            registrationPayerDao.Update(oRegistrationPayer);
                                        }
                                    }

                                    #region Payment Hd
                                    PatientPaymentHd entityHd = new PatientPaymentHd();
                                    entityHd.PaymentDate = DateTime.Now;
                                    entityHd.PaymentTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                    entityHd.GCPaymentType = Constant.PaymentType.AR_PAYER;
                                    entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                                    entityHd.GCCashierGroup = SetGCCashierGroupValue();
                                    entityHd.GCShift = SetGCShiftValue();

                                    entityHd.TotalPaymentAmount = patientBill.TotalPayerAmount;
                                    entityHd.TotalFeeAmount = 0;
                                    entityHd.TotalPayerBillAmount = patientBill.TotalPayerAmount;

                                    entityHd.Remarks = "Created Automatically from Generate Bill";

                                    entityHd.CashBackAmount = 0;
                                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                    string transactionCodePayment = "";
                                    switch (hdnDepartmentID.Value)
                                    {
                                        case Constant.Facility.INPATIENT:
                                            switch (entityHd.GCPaymentType)
                                            {
                                                case Constant.PaymentType.DOWN_PAYMENT: transactionCodePayment = Constant.TransactionCode.IP_PATIENT_PAYMENT_DP; break;
                                                case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.IP_PATIENT_PAYMENT_SETTLEMENT; break;
                                                case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                case Constant.PaymentType.CUSTOM: transactionCodePayment = Constant.TransactionCode.IP_PATIENT_PAYMENT_CUSTOM; break;
                                                //case Constant.PaymentType.PAYMENT_RETURN: transactionCodePayment = Constant.TransactionCode.IP_PATIENT_PAYMENT_RETURN; break;
                                                default: transactionCodePayment = Constant.TransactionCode.IP_PATIENT_PAYMENT_AR_PAYER; break;
                                            } break;
                                        case Constant.Facility.MEDICAL_CHECKUP:
                                            switch (entityHd.GCPaymentType)
                                            {
                                                case Constant.PaymentType.DOWN_PAYMENT: transactionCodePayment = Constant.TransactionCode.MCU_PATIENT_PAYMENT_DP; break;
                                                case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.MCU_PATIENT_PAYMENT_SETTLEMENT; break;
                                                case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                                                case Constant.PaymentType.CUSTOM: transactionCodePayment = Constant.TransactionCode.MCU_PATIENT_PAYMENT_CUSTOM; break;
                                                //case Constant.PaymentType.PAYMENT_RETURN: transactionCodePayment = Constant.TransactionCode.MCU_PATIENT_PAYMENT_RETURN; break;
                                                default: transactionCodePayment = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PAYER; break;
                                            } break;
                                        case Constant.Facility.EMERGENCY:
                                            switch (entityHd.GCPaymentType)
                                            {
                                                case Constant.PaymentType.DOWN_PAYMENT: transactionCodePayment = Constant.TransactionCode.ER_PATIENT_PAYMENT_DP; break;
                                                case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.ER_PATIENT_PAYMENT_SETTLEMENT; break;
                                                case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                case Constant.PaymentType.CUSTOM: transactionCodePayment = Constant.TransactionCode.ER_PATIENT_PAYMENT_CUSTOM; break;
                                                //case Constant.PaymentType.PAYMENT_RETURN: transactionCodePayment = Constant.TransactionCode.ER_PATIENT_PAYMENT_RETURN; break;
                                                default: transactionCodePayment = Constant.TransactionCode.ER_PATIENT_PAYMENT_AR_PAYER; break;
                                            } break;
                                        case Constant.Facility.PHARMACY:
                                            switch (entityHd.GCPaymentType)
                                            {
                                                case Constant.PaymentType.DOWN_PAYMENT: transactionCodePayment = Constant.TransactionCode.PH_PATIENT_PAYMENT_DP; break;
                                                case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.PH_PATIENT_PAYMENT_SETTLEMENT; break;
                                                case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PATIENT; break;
                                                case Constant.PaymentType.CUSTOM: transactionCodePayment = Constant.TransactionCode.PH_PATIENT_PAYMENT_CUSTOM; break;
                                                //case Constant.PaymentType.PAYMENT_RETURN: transactionCodePayment = Constant.TransactionCode.PH_PATIENT_PAYMENT_RETURN; break;
                                                default: transactionCodePayment = Constant.TransactionCode.PH_PATIENT_PAYMENT_AR_PAYER; break;
                                            } break;
                                        case Constant.Facility.DIAGNOSTIC:
                                            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                                            {
                                                switch (entityHd.GCPaymentType)
                                                {
                                                    case Constant.PaymentType.DOWN_PAYMENT: transactionCodePayment = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_DP; break;
                                                    case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_SETTLEMENT; break;
                                                    case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PATIENT; break;
                                                    case Constant.PaymentType.CUSTOM: transactionCodePayment = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_CUSTOM; break;
                                                    //case Constant.PaymentType.PAYMENT_RETURN: transactionCodePayment = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_RETURN; break;
                                                    default: transactionCodePayment = Constant.TransactionCode.LABORATORY_PATIENT_PAYMENT_AR_PAYER; break;
                                                }
                                            }
                                            else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                                            {
                                                switch (entityHd.GCPaymentType)
                                                {
                                                    case Constant.PaymentType.DOWN_PAYMENT: transactionCodePayment = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_DP; break;
                                                    case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_SETTLEMENT; break;
                                                    case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PATIENT; break;
                                                    case Constant.PaymentType.CUSTOM: transactionCodePayment = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_CUSTOM; break;
                                                    //case Constant.PaymentType.PAYMENT_RETURN: transactionCodePayment = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_RETURN; break;
                                                    default: transactionCodePayment = Constant.TransactionCode.IMAGING_PATIENT_PAYMENT_AR_PAYER; break;
                                                }
                                            }
                                            else
                                            {
                                                switch (entityHd.GCPaymentType)
                                                {
                                                    case Constant.PaymentType.DOWN_PAYMENT: transactionCodePayment = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_DP; break;
                                                    case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_SETTLEMENT; break;
                                                    case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PATIENT; break;
                                                    case Constant.PaymentType.CUSTOM: transactionCodePayment = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_CUSTOM; break;
                                                    //case Constant.PaymentType.PAYMENT_RETURN: transactionCodePayment = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_RETURN; break;
                                                    default: transactionCodePayment = Constant.TransactionCode.OTHER_PATIENT_PAYMENT_AR_PAYER; break;
                                                }
                                            } break;
                                        default:
                                            switch (entityHd.GCPaymentType)
                                            {
                                                case Constant.PaymentType.DOWN_PAYMENT: transactionCodePayment = Constant.TransactionCode.OP_PATIENT_PAYMENT_DP; break;
                                                case Constant.PaymentType.SETTLEMENT: transactionCodePayment = Constant.TransactionCode.OP_PATIENT_PAYMENT_SETTLEMENT; break;
                                                case Constant.PaymentType.AR_PATIENT: transactionCodePayment = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PATIENT; break;
                                                case Constant.PaymentType.CUSTOM: transactionCodePayment = Constant.TransactionCode.OP_PATIENT_PAYMENT_CUSTOM; break;
                                                //case Constant.PaymentType.PAYMENT_RETURN: transactionCodePayment = Constant.TransactionCode.OP_PATIENT_PAYMENT_RETURN; break;
                                                default: transactionCodePayment = Constant.TransactionCode.OP_PATIENT_PAYMENT_AR_PAYER; break;
                                            } break;
                                    }
                                    entityHd.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCodePayment, entityHd.PaymentDate, ctx);
                                    entityHd.CreatedBy = entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityHd.PaymentID = entityPaymentHdDao.InsertReturnPrimaryKeyID(entityHd);
                                    #endregion

                                    #region Payment Dt
                                    PatientPaymentDt entityDt = new PatientPaymentDt();
                                    entityDt.PaymentID = entityHd.PaymentID;
                                    entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
                                    entityDt.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);
                                    entityDt.PaymentAmount = patientBill.TotalPayerAmount;
                                    entityDt.CardFeeAmount = 0;
                                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                    entityPaymentDtDao.Insert(entityDt);
                                    #endregion

                                    #region Update Billing
                                    List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN ({0})", patientBill.PatientBillingID), ctx);
                                    List<PatientChargesHd> lstpchd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", patientBill.PatientBillingID), ctx);
                                    decimal totalPaymentAmount = entityHd.TotalPaymentAmount;
                                    foreach (PatientBill pbl in lstPatientBill)
                                    {
                                        pbl.PaymentID = entityHd.PaymentID;
                                        PatientBillPayment patientBillPayment = new PatientBillPayment();
                                        patientBillPayment.PaymentID = entityHd.PaymentID;
                                        patientBillPayment.PatientBillingID = pbl.PatientBillingID;

                                        if (entityHd.GCPaymentType == Constant.PaymentType.AR_PAYER)
                                        {
                                            if (pbl.PayerRemainingAmount > totalPaymentAmount)
                                            {
                                                pbl.TotalPayerPaymentAmount += totalPaymentAmount;
                                                patientBillPayment.PayerPaymentAmount = totalPaymentAmount;
                                                totalPaymentAmount = 0;
                                            }
                                            else
                                            {
                                                decimal payerRemainingAmount = pbl.PayerRemainingAmount;
                                                patientBillPayment.PayerPaymentAmount = payerRemainingAmount;
                                                pbl.TotalPayerPaymentAmount += payerRemainingAmount;
                                            }
                                        }
                                        else if (entityHd.GCPaymentType == Constant.PaymentType.AR_PATIENT)
                                            patientBillPayment.PatientPaymentAmount = pbl.TotalPatientPaymentAmount = pbl.PatientRemainingAmount;
                                        //else if (entityHd.GCPaymentType == Constant.PaymentType.PAYMENT_RETURN)
                                        //    patientBillPayment.PatientPaymentAmount = pbl.TotalPatientPaymentAmount = pbl.PatientRemainingAmount;
                                        else
                                        {
                                            if (entityHd.GCPaymentType == Constant.PaymentType.SETTLEMENT)
                                            {
                                                totalPaymentAmount -= pbl.PatientRemainingAmount;
                                                patientBillPayment.PatientPaymentAmount = pbl.PatientRemainingAmount;
                                                pbl.TotalPatientPaymentAmount = (pbl.TotalPatientAmount - pbl.PatientDiscountAmount);
                                            }
                                            else if (pbl.PatientRemainingAmount < totalPaymentAmount)
                                            {
                                                totalPaymentAmount -= pbl.PatientRemainingAmount;
                                                patientBillPayment.PatientPaymentAmount = pbl.PatientRemainingAmount;
                                                pbl.TotalPatientPaymentAmount = (pbl.TotalPatientAmount - pbl.PatientDiscountAmount);
                                            }
                                            else
                                            {
                                                pbl.TotalPatientPaymentAmount += totalPaymentAmount;
                                                patientBillPayment.PatientPaymentAmount = totalPaymentAmount;
                                                totalPaymentAmount = 0;
                                            }
                                        }

                                        if (pbl.RemainingAmount < 1)
                                        {
                                            pbl.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                            List<PatientChargesHd> lstChargesHd = lstpchd.Where(p => p.PatientBillingID == pbl.PatientBillingID).ToList();
                                            foreach (PatientChargesHd patientChargesHd in lstChargesHd)
                                            {
                                                patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                                patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                entityDao.Update(patientChargesHd);
                                            }
                                        }
                                        //pbl.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        patientBillDao.Update(pbl);

                                        patientBillPaymentDao.Insert(patientBillPayment);
                                    }
                                    #endregion
                                }
                            }
                        }
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

                #endregion
            }
            return result;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem || e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPatientChargesHdChargeClass entity = e.Item.DataItem as vPatientChargesHdChargeClass;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                chkIsSelected.Visible = entity.IsAllowProcessToBill;
            }
        }
    }
}