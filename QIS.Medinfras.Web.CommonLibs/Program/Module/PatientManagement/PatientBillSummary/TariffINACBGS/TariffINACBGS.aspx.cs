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
    public partial class TariffINACBGS : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";
        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_TARIFF_INACBGS;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_TARIFF_INACBGS;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_TARIFF_INACBGS;
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
            Helper.SetControlEntrySetting(txtINACBGSCode, new ControlEntrySetting(true, true, true, string.Empty), "mpINACBGs");
            Helper.SetControlEntrySetting(txtINACBGSName, new ControlEntrySetting(true, true, true, string.Empty), "mpINACBGs");
            laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
            vRegistration4 entity = BusinessLayer.GetvRegistration4List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID))[0];
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
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
            if (entity.IsControlCoverageLimit)
            {
                decimal coverageLimit = entity.CoverageLimitAmount;
                filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);
                if (entity.IsCoverageLimitPerDay)
                    filterExpression += string.Format(" AND BillingDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(filterExpression);
                coverageLimit -= lstPatientBill.Sum(p => p.TotalPayerAmount);
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
                    hdnIsControlBPJSCoverage.Value = "1";
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
                    hdnIsControlBPJSCoverage.Value = "0";
                    hdnPatientBPJSAmount.Value = "0";
                    txtCoverageLimit.Attributes.Add("readonly", "readonly");
                    txtINACBGSCode.Attributes.Add("readonly", "readonly");
                    txtINACBGSName.Attributes.Add("readonly", "readonly");
                }
                #endregion
            }
            else
            {
                hdnIsControlCoverageLimit.Value = "0";
                hdnIsControlBPJSCoverage.Value = "0";
                txtCoverageLimit.Attributes.Add("readonly", "readonly");
                txtINACBGSCode.Attributes.Add("readonly", "readonly");
                txtINACBGSName.Attributes.Add("readonly", "readonly");
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "saveinacbgs")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                RegistrationDao registrationDao = new RegistrationDao(ctx);
                RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
                RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
                AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
                string[] listParam = hdnParam.Value.Split('|');

                try
                {
                    string statusRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value)).GCRegistrationStatus;
                    if (statusRegistration == Constant.VisitStatus.CLOSED || statusRegistration == Constant.VisitStatus.CANCELLED)
                    {
                        result = false;
                        errMessage = "Registrasi untuk pasien ini sudah di tutup/batal, tagihan sudah di transfer ke rawat inap";
                    }
                    else
                    {
                        //Update Registration : Coverage Limit : if has been changed
                        decimal coverageLimit = Convert.ToDecimal(hdnRemainingCoverageAmount.Value);
                        Registration oRegistration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                        RegistrationBPJS oRegistrationBPJS = registrationBPJSDao.Get(oRegistration.RegistrationID);
                        RegistrationPayer oRegistrationPayer = BusinessLayer.GetRegistrationPayerList(string.Format("RegistrationID = {0} AND IsPrimaryPayer = 1 AND IsDeleted = 0", hdnRegistrationID.Value)).FirstOrDefault();
                        AuditLog entityAuditLog = new AuditLog();
                        entityAuditLog.OldValues = JsonConvert.SerializeObject(oRegistrationBPJS);
                        oRegistration.CoverageLimitAmount = coverageLimit;
                        if (hdnIsControlBPJSCoverage.Value == "1") oRegistration.BPJSAmount = coverageLimit;
                        registrationDao.Update(oRegistration);
                        if (oRegistrationPayer != null)
                        {
                            oRegistrationPayer.CoverageLimitAmount = coverageLimit;
                            registrationPayerDao.Update(oRegistrationPayer);
                        }
                        if (oRegistrationBPJS != null)
                        {
                            oRegistrationBPJS.GrouperCode = txtINACBGSCode.Text;
                            oRegistrationBPJS.GrouperName = txtINACBGSName.Text;
                            registrationBPJSDao.Update(oRegistrationBPJS);
                        }
                        entityAuditLog.ObjectType = Constant.BusinessObjectType.REGISTRATION_BPJS;
                        entityAuditLog.NewValues = JsonConvert.SerializeObject(oRegistrationBPJS);
                        entityAuditLog.UserID = AppSession.UserLogin.UserID;
                        entityAuditLog.LogDate = DateTime.Now;
                        entityAuditLog.TransactionID = oRegistrationBPJS.RegistrationID;
                        entityAuditLogDao.Insert(entityAuditLog);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
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