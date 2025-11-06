using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionEntryDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PRESCRIPTION_ENTRY;
        }

        public String IsEditable()
        {
            return hdnIsEditable.Value;
        }

        public String IsShowSwitchIcon()
        {
            return hdnIsShowSwitchIcon.Value;
        }

        protected string OnGetUrlReferrer()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param[0] == "ptp")
                return ResolveUrl("~/Program/PatientList/VisitList.aspx?id=ptp");
            return ResolveUrl("~/Program/Prescription/PrescriptionEntry/PrescriptionEntryList.aspx");
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}','{36}','{37}','{38}','{39}','{40}')",
                            AppSession.UserLogin.HealthcareID, //0
                            Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //1
                            Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //2
                            Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT, //3
                            Constant.SettingParameter.FM_IS_ALLOW_NON_MASTER_ITEM, //4
                            Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT, //5
                            Constant.SettingParameter.FM_KONTROL_PEMBERIAN_OBAT_KRONIS_BPJS, //6
                            Constant.SettingParameter.FM_JANGKA_WAKTU_PEMBERIAN_OBAT_KRONIS_BPJS, //7
                            Constant.SettingParameter.IM_TRANSAKSI_OBAT_HANYA_TIPE_DISTRIBUSI, //8
                            Constant.SettingParameter.PH_DEFAULT_EMBALACE_CODE_PRESCRIPTION, //9
                            Constant.SettingParameter.PH_AUTO_INSERT_EMBALACE_PRESCRIPTION, //10
                            Constant.SettingParameter.OP_CREATE_BILL_AFTER_PROPOSED_TRANSACTION, //11
                            Constant.SettingParameter.ER_CREATE_BILL_AFTER_PROPOSED_TRANSACTION, //12
                            Constant.SettingParameter.IP_CREATE_BILL_AFTER_PROPOSED_TRANSACTION, //13
                            Constant.SettingParameter.IS_CREATE_BILL_AFTER_PROPOSED_TRANSACTION, //14
                            Constant.SettingParameter.LB_CREATE_BILL_AFTER_PROPOSED_TRANSACTION, //15
                            Constant.SettingParameter.MD_CREATE_BILL_AFTER_PROPOSED_TRANSACTION, //16
                            Constant.SettingParameter.PH_CREATE_BILL_AFTER_PROPOSED_TRANSACTION, //17
                            Constant.SettingParameter.PH0037, //18
                            Constant.SettingParameter.PH_CREATE_QUEUE_LABEL, //19
                            Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX, //20
                            Constant.SettingParameter.PH0083, //21
                            Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN, //22
                            Constant.SettingParameter.FN_PENJAMIN_INHEALTH, //23
                            Constant.SettingParameter.FN_PEMBATASAN_CPOE_INHEALTH, //24
                            Constant.SettingParameter.FN_IS_ALLOW_ROUNDING_AMOUNT, //25
                            Constant.SettingParameter.FN_NILAI_PEMBULATAN_TAGIHAN, //26
                            Constant.SettingParameter.FN_PEMBULATAN_TAGIHAN_KE_ATAS, //27
                            Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, //28
                            Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, //29
                            Constant.SettingParameter.IS_DEFAULT_TRANSAKSI_BPJS, //30
                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //31
                            Constant.SettingParameter.PH_IS_REVIEW_PRESCRIPTION_MANDATORY_FOR_PROPOSED_TRANSACTION, //32
                            Constant.SettingParameter.FN_BIAYA_ADM_RI_DALAM_PERSENTASE, //33
                            Constant.SettingParameter.FN_NILAI_BIAYA_ADM_RI, //34
                            Constant.SettingParameter.PH_IS_QPHISTORY_FOR_NEW_TRANSACTION, //35
                            Constant.SettingParameter.PH_IS_RIGHTPANELPRINT_MUST_PROPOSED_CHARGES, //36
                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //37
                            Constant.SettingParameter.IS_USING_DRUG_ALERT, //38
                            Constant.SettingParameter.SA0138, //39
                            Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS //40
                        ));

                hdnImagingServiceUnitID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
                hdnPrescriptionFeeAmount.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
                hdnIsAllowEntryNonMaster.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_IS_ALLOW_NON_MASTER_ITEM).ParameterValue;
                hdnIsUsingUDD.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT).ParameterValue;
                hdnIsControlChronicDrug.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_KONTROL_PEMBERIAN_OBAT_KRONIS_BPJS).ParameterValue;
                hdnChronicDrugDuration.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FM_JANGKA_WAKTU_PEMBERIAN_OBAT_KRONIS_BPJS).ParameterValue;
                hdnIsDrugChargesJustDistribution.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_TRANSAKSI_OBAT_HANYA_TIPE_DISTRIBUSI).ParameterValue;
                hdnDefaultEmbalaceID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_DEFAULT_EMBALACE_CODE_PRESCRIPTION).ParameterValue;
                hdnIsAutoInsertEmbalace.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_AUTO_INSERT_EMBALACE_PRESCRIPTION).ParameterValue;

                hdnIsAutoCreateBillAfterProposedOP.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP_CREATE_BILL_AFTER_PROPOSED_TRANSACTION).ParameterValue;
                hdnIsAutoCreateBillAfterProposedER.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.ER_CREATE_BILL_AFTER_PROPOSED_TRANSACTION).ParameterValue;
                hdnIsAutoCreateBillAfterProposedIP.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP_CREATE_BILL_AFTER_PROPOSED_TRANSACTION).ParameterValue;
                hdnIsAutoCreateBillAfterProposedIS.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_CREATE_BILL_AFTER_PROPOSED_TRANSACTION).ParameterValue;
                hdnIsAutoCreateBillAfterProposedLB.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_CREATE_BILL_AFTER_PROPOSED_TRANSACTION).ParameterValue;
                hdnIsAutoCreateBillAfterProposedMD.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_CREATE_BILL_AFTER_PROPOSED_TRANSACTION).ParameterValue;
                hdnIsAutoCreateBillAfterProposedPH.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_CREATE_BILL_AFTER_PROPOSED_TRANSACTION).ParameterValue;

                hdnIsAutoGenerateReferenceNo.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH0037).ParameterValue;
                hdnIsGenerateQueueLabel.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_CREATE_QUEUE_LABEL).ParameterValue;
                hdnItemQtyWithSpecialQueuePrefix.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX).ParameterValue;

                hdnMenggunakanPembulatan.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ALLOW_ROUNDING_AMOUNT).FirstOrDefault().ParameterValue;
                hdnNilaiPembulatan.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_NILAI_PEMBULATAN_TAGIHAN).FirstOrDefault().ParameterValue;
                hdnPembulatanKemana.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_PEMBULATAN_TAGIHAN_KE_ATAS).FirstOrDefault().ParameterValue;
                hdnDefaultJenisTransaksiBPJS.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.IS_DEFAULT_TRANSAKSI_BPJS).FirstOrDefault().ParameterValue;

                hdnIsEndingAmountRoundingTo100.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
                hdnIsEndingAmountRoundingTo1.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
                hdnIsReviewPrescriptionMandatoryForProposedTransaction.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.PH_IS_REVIEW_PRESCRIPTION_MANDATORY_FOR_PROPOSED_TRANSACTION).FirstOrDefault().ParameterValue;
                hdnIsQPHistoryForNewTransaction.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_IS_QPHISTORY_FOR_NEW_TRANSACTION).ParameterValue;
                hdnIsRightPanelPrintMustProposedCharges.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_IS_RIGHTPANELPRINT_MUST_PROPOSED_CHARGES).ParameterValue;
                hdnIsUsingDrugAlertMain.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_USING_DRUG_ALERT).ParameterValue;
                hdnIsBridgingToMobileJKN.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA0138).FirstOrDefault().ParameterValue;
                
                string[] param = Page.Request.QueryString["id"].Split('|');

                String filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_TYPE, Constant.StandardCode.TIPE_TRANSAKSI_BPJS);
                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

                List<StandardCode> lstPrescriptionType = lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE).ToList();
                Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstPrescriptionType, "StandardCodeName", "StandardCodeID");
                cboPrescriptionType.SelectedIndex = 0;

                string transactionNo = string.Empty;
                if (param[0] == "to")
                {
                    hdnVisitID.Value = param[1];
                    hdnDefaultPrescriptionOrderID.Value = param[2];
                    PrescriptionOrderHd entityHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnDefaultPrescriptionOrderID.Value));
                    hdnDispensaryServiceUnitID.Value = entityHd.DispensaryServiceUnitID.ToString();
                    if (entityHd.GCOrderStatus == Constant.TestOrderStatus.COMPLETED)
                        btnClinicTransactionTestOrder.Style.Add("display", "none");
                    cboPrescriptionType.Value = entityHd.GCPrescriptionType;
                    PatientChargesHd entityPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PrescriptionOrderID = '{0}' AND GCTransactionStatus <> '{1}'", hdnDefaultPrescriptionOrderID.Value, Constant.TransactionStatus.VOID)).FirstOrDefault();
                    if (entityPatientChargesHd != null) transactionNo = entityPatientChargesHd.TransactionNo;
                }
                else
                {
                    hdnVisitID.Value = param[1];
                    hdnDispensaryServiceUnitID.Value = param[2];
                    btnClinicTransactionTestOrder.Style.Add("display", "none");
                }

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();

                List<PatientVisitNote> lstEntity = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value));
                if (lstEntity.Count == 0)
                {
                    divVisitNote.Attributes.Add("style", "display:none");
                }

                SettingParameterDt oParam1 = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.PH0083).FirstOrDefault();
                SettingParameterDt oParam2 = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();
                SettingParameterDt oParam3 = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PEMBATASAN_CPOE_INHEALTH).FirstOrDefault();
                SettingParameterDt oParam4 = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_INHEALTH).FirstOrDefault();
                SettingParameterDt oParam5 = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).FirstOrDefault();


                hdnIsLimitedCPOEItemForBPJS.Value = oParam1 != null ? (oParam1.ParameterValue == "1" ? "1" : "0") : "0";
                int businnessPartnerID1 = oParam2 != null ? Convert.ToInt32(oParam2.ParameterValue) : 0;
                string bpjsType = oParam5 != null ? oParam5.ParameterValue : string.Empty;

                bool isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealth;

                if (!isLimitedCPOEItemForInhealth)
                {
                    switch (AppSession.RegisteredPatient.DepartmentID)
                    {
                        case Constant.Facility.INPATIENT:
                            isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthIP;
                            break;
                        case Constant.Facility.OUTPATIENT:
                            isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthOP;
                            break;
                        case Constant.Facility.EMERGENCY:
                            isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthER;
                            break;
                        case Constant.Facility.DIAGNOSTIC:
                            isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthMD;
                            break;
                        case Constant.Facility.PHARMACY:
                            isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthPH;
                            break;
                        case Constant.Facility.MEDICAL_CHECKUP:
                            isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthMC;
                            break;
                        default:
                            break;
                    }
                }

                hdnIsLimitedCPOEItemForInHealth.Value = isLimitedCPOEItemForInhealth ? "1" : "0";

                hdnIsBPJS.Value = entity.GCCustomerType == bpjsType ? "1" : "0";
                hdnIsInHealth.Value = entity.GCCustomerType == oParam4.ParameterValue ? "1" : "0";

                if (entity.DischargeDate != null && entity.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
                {
                    hdnIsDischarges.Value = "1";
                }
                else
                {
                    hdnIsDischarges.Value = "0";
                }

                if (entity.DepartmentID == Constant.Facility.INPATIENT)
                {
                    hdnPatientAdminFee.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_NILAI_BIAYA_ADM_RI).FirstOrDefault().ParameterValue;
                    hdnIsPatientAdminFeeInPercentage.Value = (lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_BIAYA_ADM_RI_DALAM_PERSENTASE).FirstOrDefault().ParameterValue == "1" ? "1" : "0");
                }
                else
                {
                    hdnPatientAdminFee.Value = "0";
                    hdnIsPatientAdminFeeInPercentage.Value = "0";
                }

                hdnChargeClassID.Value = entity.ChargeClassID.ToString();
                hdnDepartmentID.Value = entity.DepartmentID;
                hdnDefaultParamedicID.Value = entity.ParamedicID.ToString();
                hdnDefaultParamedicCode.Value = entity.ParamedicCode;
                hdnDefaultParamedicName.Value = entity.ParamedicName;
                hdnMRN.Value = entity.MRN.ToString();
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();

                hdnIsBPJSPatient.Value = entity.GCCustomerType == Constant.CustomerType.BPJS ? "1" : "0";

                List<StandardCode> lstTipeTransaksiBPJS = lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.TIPE_TRANSAKSI_BPJS).ToList();
                Methods.SetComboBoxField<StandardCode>(cboBPJSTransType, lstTipeTransaksiBPJS, "StandardCodeName", "StandardCodeID");
                if (hdnDefaultJenisTransaksiBPJS.Value == "0")
                {
                    if (hdnBusinessPartnerID.Value == "1")
                    {
                        cboBPJSTransType.Value = Constant.BPJSTransactionType.DIBAYAR_PASIEN;
                    }
                    else
                    {
                        cboBPJSTransType.Value = Constant.BPJSTransactionType.DITAGIHKAN;
                    }
                }
                else
                {
                    if (hdnBusinessPartnerID.Value == "1")
                    {
                        cboBPJSTransType.Value = Constant.BPJSTransactionType.DIBAYAR_PASIEN;
                    }
                    else
                    {
                        string filterCustomer = string.Format("BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
                        Customer entityCustomer = BusinessLayer.GetCustomerList(filterCustomer).FirstOrDefault();

                        if (entityCustomer.GCCustomerType == Constant.CustomerType.BPJS)
                        {
                            cboBPJSTransType.Value = Constant.BPJSTransactionType.PAKET;
                        }
                        else
                        {
                            cboBPJSTransType.Value = Constant.BPJSTransactionType.DITAGIHKAN;
                        }
                    }
                }

                hdnImagingServiceUnitID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;

                int locationID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnDispensaryServiceUnitID.Value)).FirstOrDefault().LocationID;
                hdnLocationID.Value = locationID.ToString();
                if (locationID > 0)
                {
                    Location loc = BusinessLayer.GetLocation(locationID);
                    List<Location> lstLocation = null;
                    if (loc.IsHeader)
                        lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                    else
                    {
                        lstLocation = new List<Location>();
                        lstLocation.Add(loc);
                    }
                    Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                    cboLocation.SelectedIndex = 0;
                    if (lstLocation.Count == 1)
                        hdnDefaultLocationID.Value = cboLocation.Value.ToString();
                }

                ctlPatientBanner.InitializePatientBanner(entity);
                hdnDepartmentID.Value = entity.DepartmentID;
                hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnClassID.Value = entity.ChargeClassID.ToString();

                hdnPhysicianID.Value = entity.ParamedicID.ToString();
                txtPhysicianCode.Text = entity.ParamedicCode;
                txtPhysicianName.Text = entity.ParamedicName;

                if (!string.IsNullOrEmpty(transactionNo))
                {
                    IsLoadFirstRecord = true;
                    filterExpression = OnGetFilterExpression();
                    pageIndexFirstLoad = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, transactionNo, "TransactionID DESC");
                }
                else
                {
                    if (param[0] != "to") IsLoadFirstRecord = (OnGetRowCount() > 0);
                }

                BindGridView();

                Helper.SetControlEntrySetting(txtGenericName, new ControlEntrySetting(false, false, false), "mpTrx");
                Helper.SetControlEntrySetting(txtDrugCode, new ControlEntrySetting(true, false, true), "mpTrx");
                Helper.SetControlEntrySetting(cboForm, new ControlEntrySetting(true, true, true), "mpTrx");

                Helper.SetControlEntrySetting(cboLocation, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtStrengthAmount, new ControlEntrySetting(false, false, false), "mpTrx");
                Helper.SetControlEntrySetting(txtStrengthUnit, new ControlEntrySetting(false, false, false), "mpTrx");
                Helper.SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtExpiredDate, new ControlEntrySetting(true, true, false), "mpTrx");
                Helper.SetControlEntrySetting(txtPurposeOfMedication, new ControlEntrySetting(true, true, false), "mpTrx");
                Helper.SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false), "mpTrx");
                Helper.SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true), "mpTrx");
            }

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

        }

        //protected bool IsEditable = true;
        public override void OnAddRecord()
        {
            tdImageDrugAlertInfo.Style.Add("display", "none");
            hdnIsEditable.Value = "1";
            hdnTransactionStatus.Value = "";
            cboLocation.SelectedIndex = 0;
            cboPrescriptionType.SelectedIndex = 0;
            if (hdnDefaultJenisTransaksiBPJS.Value == "0")
            {
                if (hdnBusinessPartnerID.Value == "1")
                {
                    cboBPJSTransType.Value = Constant.BPJSTransactionType.DIBAYAR_PASIEN;
                }
                else
                {
                    cboBPJSTransType.Value = Constant.BPJSTransactionType.DITAGIHKAN;
                }
            }
            else
            {
                if (hdnBusinessPartnerID.Value == "1")
                {
                    cboBPJSTransType.Value = Constant.BPJSTransactionType.DIBAYAR_PASIEN;
                }
                else
                {
                    string filterCustomer = string.Format("BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
                    Customer entityCustomer = BusinessLayer.GetCustomerList(filterCustomer).FirstOrDefault();

                    if (entityCustomer.GCCustomerType == Constant.CustomerType.BPJS)
                    {
                        cboBPJSTransType.Value = Constant.BPJSTransactionType.PAKET;
                    }
                    else
                    {
                        cboBPJSTransType.Value = Constant.BPJSTransactionType.DITAGIHKAN;
                    }
                }
            }
            txtNotes.Text = "";
            txtReferenceNo.Text = string.Empty;

            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divProposedBy.InnerHtml = string.Empty;
            divProposedDate.InnerHtml = string.Empty;
            divVoidBy.InnerHtml = string.Empty;
            divVoidDate.InnerHtml = string.Empty;
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
            divVoidReason.InnerHtml = string.Empty;
            trProposedBy.Style.Add("display", "none");
            trProposedDate.Style.Add("display", "none");
            trVoidBy.Style.Add("display", "none");
            trVoidDate.Style.Add("display", "none");
            trVoidReason.Style.Add("display", "none");

            BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstToBePerformed = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TO_BE_PERFORMED));
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.DRUG_FORM, Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.COENAM_RULE);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            List<StandardCode> lstMedicationRoute = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList();

            Methods.SetComboBoxField<StandardCode>(cboForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstMedicationRoute, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<ClassCare>(cboChargeClass, lstClassCare, "ClassName", "ClassID");

            StandardCode defaultMedicationRoute = lstMedicationRoute.FirstOrDefault(p => p.IsDefault);
            if (defaultMedicationRoute == null)
                defaultMedicationRoute = lstMedicationRoute[0];
            hdnDefaultGCMedicationRoute.Value = defaultMedicationRoute.StandardCodeID;

            cboForm.SelectedIndex = 1;
            cboFrequencyTimeline.SelectedIndex = 1;
            txtDosingDurationTimeline.Text = cboFrequencyTimeline.Text;
            cboMedicationRoute.SelectedIndex = 0;
            cboChargeClass.SelectedIndex = 1;

            txtStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString("HH:mm");
        }

        protected void cboDosingUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (hdnDrugID.Value != null && hdnDrugID.Value.ToString() != "")
            {
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (TagProperty LIKE '%1%' OR TagProperty LIKE '%PRE%')", Constant.StandardCode.ITEM_UNIT));
                Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lst, "StandardCodeName", "StandardCodeID");
                cboDosingUnit.SelectedIndex = 0;

                string result = "";
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "edit")
                    {
                        result = "edit";
                    }
                }
                cboDosingUnit.JSProperties["cpResult"] = result;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnPrescriptionOrderID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            if (hdnDefaultJenisTransaksiBPJS.Value == "0")
            {
                if (hdnBusinessPartnerID.Value == "1")
                {
                    SetControlEntrySetting(cboBPJSTransType, new ControlEntrySetting(true, true, true, Constant.BPJSTransactionType.DIBAYAR_PASIEN));
                }
                else
                {
                    SetControlEntrySetting(cboBPJSTransType, new ControlEntrySetting(true, true, true, Constant.BPJSTransactionType.DITAGIHKAN));
                }
            }
            else
            {
                if (hdnBusinessPartnerID.Value == "1")
                {
                    SetControlEntrySetting(cboBPJSTransType, new ControlEntrySetting(true, true, true, Constant.BPJSTransactionType.DIBAYAR_PASIEN));
                }
                else
                {
                    string filterCustomer = string.Format("BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
                    Customer entityCustomer = BusinessLayer.GetCustomerList(filterCustomer).FirstOrDefault();

                    if (entityCustomer.GCCustomerType == Constant.CustomerType.BPJS)
                    {
                        SetControlEntrySetting(cboBPJSTransType, new ControlEntrySetting(true, true, true, Constant.BPJSTransactionType.PAKET));
                    }
                    else
                    {
                        SetControlEntrySetting(cboBPJSTransType, new ControlEntrySetting(true, true, true, Constant.BPJSTransactionType.DITAGIHKAN));
                    }
                }
            }
            SetControlEntrySetting(txtPrescriptionOrderInfo, new ControlEntrySetting(false, false, false, string.Empty));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(hdnPhysicianID, new ControlEntrySetting(true, false, true, hdnDefaultParamedicID.Value));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnDefaultParamedicCode.Value));
            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, false, hdnDefaultParamedicName.Value));
            SetControlEntrySetting(cboPrescriptionType, new ControlEntrySetting(true, false, true));

            txtReferenceNo.Enabled = hdnIsAutoGenerateReferenceNo.Value != "1";
        }

        #region Load Entity
        protected string OnGetFilterExpression()
        {
            String filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            filterExpression += string.Format(" AND HealthcareServiceUnitID = {0} AND PrescriptionOrderID IS NOT NULL", hdnDispensaryServiceUnitID.Value);
            //filterExpression += string.Format(" AND GCChargesTransactionStatus != '{0}' AND GCChargesTransactionStatus != '' AND GCTransactionStatus != '{0}'", Constant.TransactionStatus.VOID);
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = OnGetFilterExpression();
            return BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = OnGetFilterExpression();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = OnGetFilterExpression();
            PageIndex = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, "TransactionID DESC");

            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPatientChargesHd entityCharges, ref bool isShowWatermark, ref string watermarkText)
        {
            vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0}", entityCharges.PrescriptionOrderID))[0];
            if (entityCharges.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entityCharges.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";
            }
            else
                hdnIsEditable.Value = "1";
            hdnPrescriptionOrderID.Value = entity.PrescriptionOrderID.ToString();
            hdnTransactionID.Value = entityCharges.TransactionID.ToString();
            txtTransactionNo.Text = entityCharges.TransactionNo;
            hdnGCTransactionStatus.Value = entityCharges.GCTransactionStatus;
            txtPrescriptionDate.Text = entityCharges.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entityCharges.TransactionTime;
            cboPrescriptionType.Value = entity.GCPrescriptionType;
            if (!string.IsNullOrEmpty(entityCharges.GCBPJSTransactionType))
            {
                cboBPJSTransType.Value = entityCharges.GCBPJSTransactionType;
            }
            txtReferenceNo.Text = entityCharges.ReferenceNo;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            txtPrescriptionOrderInfo.Text = string.Format("{0}|{1}|{2}", entity.PrescriptionOrderNo, entity.LastUpdatedDateInString, entity.CreatedByName);
            txtNotes.Text = entity.Remarks;
            hdnTransactionStatus.Value = entityCharges.GCTransactionStatus;

            if (entity.GCOrderStatus == Constant.TestOrderStatus.COMPLETED)
            {
                btnClinicTransactionTestOrder.Style.Add("display", "none");
            }
            else
            {
                btnClinicTransactionTestOrder.Style.Remove("display");
            }

            //if (entityCharges.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            //{
            //    cboPrescriptionType.Enabled = true;
            //    cboBPJSTransType.Enabled = true;
            //}
            //else
            //{
            //    cboPrescriptionType.Enabled = false;
            //    cboBPJSTransType.Enabled = false;
            //}

            if (hdnIsUsingDrugAlertMain.Value == "1")
            {
                PrescriptionOrderHdInfo info = BusinessLayer.GetPrescriptionOrderHdInfo(entity.PrescriptionOrderID);
                if (info != null)
                {
                    if (!String.IsNullOrEmpty(info.DrugAlertResultInfo) || !String.IsNullOrEmpty(info.DrugAlertResultInfo1))
                    {
                        string toFound = "No results found. Absence of interaction result should in no way be interpreted as safe. Clinical judgment should be exercised";
                        if (info.DrugAlertResultInfo.Contains(toFound) && info.DrugAlertResultInfo1.Contains(toFound))
                        {
                            tdImageDrugAlertInfo.Style.Add("display", "none");
                        }
                        else
                        {
                            tdImageDrugAlertInfo.Style.Remove("display");
                        }
                    }
                    else
                    {
                        tdImageDrugAlertInfo.Style.Add("display", "none");
                    }
                }
                else
                {
                    tdImageDrugAlertInfo.Style.Add("display", "none");
                }
            }
            else
            {
                tdImageDrugAlertInfo.Style.Add("display", "none");
            }

            divCreatedBy.InnerHtml = entityCharges.CreatedByName;
            divCreatedDate.InnerHtml = entityCharges.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entityCharges.LastUpdatedByName;
            if (entityCharges.LastUpdatedDate != null && entityCharges.LastUpdatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT) != "01 January 1900 00:00:00")
            {
                divLastUpdatedDate.InnerHtml = entityCharges.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }
            else
            {
                divLastUpdatedDate.InnerHtml = string.Empty;
            }

            if (entityCharges.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            {
                divProposedBy.InnerHtml = entityCharges.ProposedByName;
                if (entityCharges.ProposedDate != null && entityCharges.ProposedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divProposedDate.InnerHtml = entityCharges.ProposedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                trProposedBy.Style.Remove("display");
                trProposedDate.Style.Remove("display");
            }
            else
            {
                trProposedBy.Style.Add("display", "none");
                trProposedDate.Style.Add("display", "none");
            }

            if (entityCharges.GCTransactionStatus == Constant.TransactionStatus.VOID)
            {
                divVoidBy.InnerHtml = entityCharges.VoidByName;
                if (entityCharges.VoidDate != null && entityCharges.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    divVoidDate.InnerHtml = entityCharges.VoidDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }

                string voidReason = "";

                if (entityCharges.GCVoidReason == Constant.DeleteReason.OTHER)
                {
                    voidReason = entityCharges.VoidReasonWatermark + " ( " + entityCharges.VoidReason + " )";
                }
                else
                {
                    voidReason = entityCharges.VoidReasonWatermark;
                }

                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");
                divVoidReason.InnerHtml = voidReason;
                trVoidReason.Style.Remove("display");
            }
            else
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");
                trVoidReason.Style.Add("display", "none");
            }

            BindGridView();
        }

        private void BindGridView()
        {
            List<GetPrescriptionPrice> lstEntity = new List<GetPrescriptionPrice>();
            if (hdnPrescriptionOrderID.Value != "" && hdnPrescriptionOrderID.Value != "0" && hdnTransactionID.Value != "0")
            {
                lstEntity = BusinessLayer.GetPrescriptionPrice(Convert.ToInt32(hdnTransactionID.Value), Convert.ToInt32(hdnPrescriptionOrderID.Value));
            }
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
            if (lstEntity.Count > 0)
            {
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAllPayer")).InnerHtml = lstEntity.Sum(x => x.PayerAmount).ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAllPatient")).InnerHtml = lstEntity.Sum(x => x.PatientAmount).ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAll")).InnerHtml = lstEntity.Sum(x => x.LineAmount).ToString("N");
            }

            vConsultVisit2 oVisit = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
            if (oVisit != null)
            {
                hdnIsShowSwitchIcon.Value = oVisit.GCCustomerType != Constant.CustomerType.PERSONAL ? "1" : "0";
            }
            else
            {
                hdnIsShowSwitchIcon.Value = "0";
            }
        }
        #endregion

        #region Save Entity
        private void ControlToEntity(PrescriptionOrderDt entity, PatientChargesDt entityChargesDt)
        {
            #region PrescriptionOrderDt
            entity.IsRFlag = true;
            if (hdnDrugID.Value.ToString() != "")
                entity.ItemID = Convert.ToInt32(hdnDrugID.Value);
            else
                entity.ItemID = null;
            entity.DrugName = Request.Form[txtDrugName.UniqueID];
            entity.GenericName = Request.Form[txtGenericName.UniqueID];
            entity.GCDrugForm = cboForm.Value.ToString();
            if (hdnSignaID.Value != "")
                entity.SignaID = Convert.ToInt32(hdnSignaID.Value);
            else
                entity.SignaID = null;
            if (hdnGCDoseUnit.Value != null && hdnGCDoseUnit.Value.ToString() != "")
            {
                entity.Dose = Convert.ToDecimal(Request.Form[txtStrengthAmount.UniqueID]);
                entity.GCDoseUnit = hdnGCDoseUnit.Value.ToString();
            }
            else
            {
                entity.Dose = null;
                entity.GCDoseUnit = null;
            }
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.GCRoute = cboMedicationRoute.Value.ToString();
            entity.StartDate = Helper.GetDatePickerValue(Request.Form[txtStartDate.UniqueID]);
            entity.StartTime = Request.Form[txtStartTime.UniqueID];
            entity.IsMorning = chkIsMorning.Checked;
            entity.IsNoon = chkIsNoon.Checked;
            entity.IsEvening = chkIsEvening.Checked;
            entity.IsNight = chkIsNight.Checked;
            entity.IsAsRequired = chkIsAsRequired.Checked;
            entity.IsIMM = chkIsIMM.Checked;
            entity.MedicationPurpose = txtPurposeOfMedication.Text;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text);
            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.TakenQty = Convert.ToDecimal(txtTakenQty.Text);
            entity.ChargeQty = entity.TakenQty;
            entity.ResultQty = entity.TakenQty;
            if (!string.IsNullOrEmpty(txtExpiredDate.Text))
                entity.ExpiredDate = Helper.GetDatePickerValue(Request.Form[txtExpiredDate.UniqueID]);
            if (hdnEmbalaceID.Value != "" && hdnEmbalaceID.Value != "0")
                entity.EmbalaceID = Convert.ToInt32(hdnEmbalaceID.Value);
            else
                entity.EmbalaceID = null;
            if (Request.Form[txtEmbalaceQty.UniqueID] != "")
                entity.EmbalaceQty = Convert.ToDecimal(Request.Form[txtEmbalaceQty.UniqueID]);
            else
                entity.EmbalaceQty = 0;

            if (cboCoenamRule.Value != null && cboCoenamRule.Value.ToString() != "")
                entity.GCCoenamRule = cboCoenamRule.Value.ToString();
            else
                entity.GCCoenamRule = null;

            entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
            #endregion

            #region PatientChargesDt
            entityChargesDt.LocationID = Convert.ToInt32(cboLocation.Value);
            entityChargesDt.ItemID = Convert.ToInt32(hdnDrugID.Value);
            entityChargesDt.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);

            entityChargesDt.BaseTariff = Convert.ToDecimal(hdnBaseTariff.Value);
            entityChargesDt.BaseComp1 = Convert.ToDecimal(hdnBaseComp1.Value);
            entityChargesDt.BaseComp2 = Convert.ToDecimal(hdnBaseComp2.Value);
            entityChargesDt.BaseComp3 = Convert.ToDecimal(hdnBaseComp3.Value);

            entityChargesDt.Tariff = Convert.ToDecimal(hdnTariff.Value);
            entityChargesDt.TariffComp1 = Convert.ToDecimal(hdnTariffComp1.Value);
            entityChargesDt.TariffComp2 = Convert.ToDecimal(hdnTariffComp2.Value);
            entityChargesDt.TariffComp3 = Convert.ToDecimal(hdnTariffComp3.Value);

            decimal qtyStock = entity.TakenQty;
            if (entity.TakenQty > 0)
                if (hdnGCConsumptionDeductionType.Value == Constant.QuantityDeductionType.DIBULATKAN) qtyStock = Math.Ceiling(qtyStock);
            entityChargesDt.UsedQuantity = qtyStock;
            entityChargesDt.ChargedQuantity = entity.ChargeQty;
            entityChargesDt.BaseQuantity = qtyStock;
            entityChargesDt.GCItemUnit = hdnGCBaseUnit.Value;
            entityChargesDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityChargesDt.ConversionFactor = 1;
            if (entity.ChargeQty > 0)
            {
                if (!string.IsNullOrEmpty(hdnEmbalaceID.Value) && hdnEmbalaceID.Value != "0")
                {
                    entityChargesDt.EmbalaceAmount = Convert.ToDecimal(Request.Form[txtEmbalaceAmount.UniqueID]);
                }
                else
                {
                    entityChargesDt.EmbalaceAmount = 0;
                }

                entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);
            }
            else
            {
                entityChargesDt.EmbalaceAmount = 0;
                entityChargesDt.PrescriptionFeeAmount = 0;
            }

            entityChargesDt.ChargeClassID = Convert.ToInt32(cboChargeClass.Value);
            entityChargesDt.DiscountAmount = Convert.ToDecimal(Request.Form[txtDiscountAmount.UniqueID]);
            if (entityChargesDt.ChargedQuantity > 0)
            {
                entityChargesDt.DiscountComp1 = entityChargesDt.DiscountAmount / entityChargesDt.ChargedQuantity;
            }
            entityChargesDt.IsDiscount = entityChargesDt.DiscountAmount != 0 ? true : false;

            decimal oPatientAmount = Convert.ToDecimal(Request.Form[txtPatientAmount.UniqueID]);
            decimal oPayerAmount = Convert.ToDecimal(Request.Form[txtPayerAmount.UniqueID]);
            decimal oLineAmount = Convert.ToDecimal(Request.Form[txtLineAmount.UniqueID]);

            if (hdnIsEndingAmountRoundingTo1.Value == "1")
            {
                decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                {
                    oPatientAmount = Math.Floor(oPatientAmount);
                }
                else
                {
                    oPatientAmount = Math.Ceiling(oPatientAmount);
                }

                decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                {
                    oPayerAmount = Math.Floor(oPayerAmount);
                }
                else
                {
                    oPayerAmount = Math.Ceiling(oPayerAmount);
                }

                oLineAmount = oPatientAmount + oPayerAmount;
            }

            if (hdnIsEndingAmountRoundingTo100.Value == "1")
            {
                oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                oLineAmount = oPatientAmount + oPayerAmount;
            }

            entityChargesDt.PatientAmount = oPatientAmount;
            entityChargesDt.PayerAmount = oPayerAmount;
            entityChargesDt.LineAmount = oLineAmount;

            entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;

            #endregion
        }

        public void SavePrescriptionHd(IDbContext ctx, ref int prescriptionID, ref int transactionID, ref string transactionNo)
        {
            PrescriptionOrderHdDao entityOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao entityChargesHdInfoDao = new PatientChargesHdInfoDao(ctx);

            if (hdnPrescriptionOrderID.Value == "0" || hdnTransactionID.Value == "0")
            {
                #region PrescriptionOrderHd
                PrescriptionOrderHd entityOrderHd = new PrescriptionOrderHd();
                entityOrderHd.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                entityOrderHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityOrderHd.VisitHealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entityOrderHd.PrescriptionDate = Helper.GetDatePickerValue(Request.Form[txtPrescriptionDate.UniqueID]);
                entityOrderHd.PrescriptionTime = Request.Form[txtPrescriptionTime.UniqueID];
                entityOrderHd.ClassID = Convert.ToInt32(hdnClassID.Value);
                entityOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                entityOrderHd.LocationID = Convert.ToInt32(cboLocation.Value);
                entityOrderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                entityOrderHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.EMERGENCY:
                        entityOrderHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.OUTPATIENT:
                        entityOrderHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.INPATIENT:
                        entityOrderHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.PHARMACY:
                        entityOrderHd.TransactionCode = Constant.TransactionCode.PH_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.MEDICAL_CHECKUP:
                        entityOrderHd.TransactionCode = Constant.TransactionCode.MCU_MEDICATION_ORDER;
                        break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (hdnHealthcareServiceUnitID.Value == hdnImagingServiceUnitID.Value)
                            entityOrderHd.TransactionCode = Constant.TransactionCode.IMAGING_MEDICATION_ORDER;
                        else if (hdnHealthcareServiceUnitID.Value == hdnLaboratoryServiceUnitID.Value)
                            entityOrderHd.TransactionCode = Constant.TransactionCode.LABORATORY_MEDICATION_ORDER;
                        else
                            entityOrderHd.TransactionCode = Constant.TransactionCode.OTHER_MEDICATION_ORDER;
                        break;
                }

                entityOrderHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityOrderHd.TransactionCode, entityOrderHd.PrescriptionDate, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                if (hdnDispensaryServiceUnitID.Value != "" && hdnDispensaryServiceUnitID.Value != null)
                    entityOrderHd.DispensaryServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                else
                    entityOrderHd.DispensaryServiceUnitID = 0;
                //entityOrderHd.LocationID = Convert.ToInt32(cboLocation.Value);
                entityOrderHd.CreatedBy = AppSession.UserLogin.UserID;
                entityOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityOrderHd.LastUpdatedDate = DateTime.Now;
                entityOrderHd.StartDate = DateTime.Now.Date;
                entityOrderHd.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                entityOrderHd.IsCreatedBySystem = true;
                entityOrderHd.IsOrderedByPhysician = false;
                prescriptionID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityOrderHd);
                #endregion

                #region PatientChargesHd + PatientChargesHdInfo
                PatientChargesHd entityPatientChargesHd = new PatientChargesHd();

                entityPatientChargesHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entityPatientChargesHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtPrescriptionDate.UniqueID]);
                entityPatientChargesHd.TransactionTime = Request.Form[txtPrescriptionTime.UniqueID];
                entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityPatientChargesHd.PrescriptionOrderID = prescriptionID;
                entityPatientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                entityPatientChargesHd.ReferenceNo = txtReferenceNo.Text;
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.EMERGENCY:
                        entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_EMERGENCY;
                        break;
                    case Constant.Facility.OUTPATIENT:
                        entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OUTPATIENT;
                        break;
                    case Constant.Facility.INPATIENT:
                        entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_INPATIENT;
                        break;
                    case Constant.Facility.PHARMACY:
                        entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PH_CHARGES;
                        break;
                    case Constant.Facility.MEDICAL_CHECKUP:
                        entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OTHER;
                        break;
                    case Constant.Facility.DIAGNOSTIC:
                        if (hdnHealthcareServiceUnitID.Value == hdnImagingServiceUnitID.Value)
                            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_IMAGING;
                        else if (hdnHealthcareServiceUnitID.Value == hdnLaboratoryServiceUnitID.Value)
                            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_LABORATORY;
                        else
                            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OTHER;
                        break;
                }
                entityPatientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityPatientChargesHd.TransactionCode, entityPatientChargesHd.TransactionDate, ctx);

                if (hdnIsAutoGenerateReferenceNo.Value == "1")
                {
                    entityPatientChargesHd.ReferenceNo = BusinessLayer.GeneratePrescriptionReferenceNo(entityPatientChargesHd.HealthcareServiceUnitID, entityPatientChargesHd.TransactionDate, ctx);
                }

                entityPatientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                transactionID = entityChargesHdDao.InsertReturnPrimaryKeyID(entityPatientChargesHd);
                transactionNo = entityPatientChargesHd.TransactionNo;

                PatientChargesHdInfo oChargesHdInfo = entityChargesHdInfoDao.Get(transactionID);
                if (oChargesHdInfo != null)
                {
                    oChargesHdInfo.GCBPJSTransactionType = cboBPJSTransType.Value.ToString();
                    entityChargesHdInfoDao.Update(oChargesHdInfo);
                }

                if (entityPatientChargesHd.IsEntryByPhysician == false)
                {
                    #region Log PrescriptionTaskOrder
                    Helper.InsertPrescriptionOrderTaskLog(ctx, prescriptionID, Constant.PrescriptionTaskLogStatus.Sent, AppSession.UserLogin.UserID, false);
                    Helper.InsertPrescriptionOrderTaskLog(ctx, prescriptionID, Constant.PrescriptionTaskLogStatus.Received, AppSession.UserLogin.UserID, false);
                    #endregion
                }



                #endregion


            }
            else
            {
                prescriptionID = Convert.ToInt32(hdnPrescriptionOrderID.Value);
                transactionID = Convert.ToInt32(hdnTransactionID.Value);
                transactionNo = txtTransactionNo.Text;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int PrescriptionID = 0;
                int TransactionID = 0;
                string TransactionNo = "";
                SavePrescriptionHd(ctx, ref PrescriptionID, ref TransactionID, ref TransactionNo);

                retval = PrescriptionID.ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PrescriptionOrderHd entityOrderHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                    entityOrderHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                    entityOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePrescriptionOrderHd(entityOrderHd);

                    PatientChargesHdInfo oChargesHdInfo = BusinessLayer.GetPatientChargesHdInfo(Convert.ToInt32(hdnTransactionID.Value));
                    if (oChargesHdInfo != null)
                    {
                        //PatientChargesHd entityCharges = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionID.Value));
                        //entityCharges.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //BusinessLayer.UpdatePatientChargesHd(entityCharges);

                        oChargesHdInfo.GCBPJSTransactionType = cboBPJSTransType.Value.ToString();
                        BusinessLayer.UpdatePatientChargesHdInfo(oChargesHdInfo);
                    }

                    //PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnTransactionHdID.Value));
                    //entity.ReferenceNo = txtReferenceNo.Text;
                    //entity.TransactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
                    //entity.TransactionTime = txtTransactionTime.Text;
                    //BusinessLayer.UpdatePatientChargesHd(entity);
                    return true;
                }
                else
                {
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entity.TransactionNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            if (param[0] == "void")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
                PatientChargesHdDao chargesDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
                ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
                try
                {
                    ChargesStatusLog log = new ChargesStatusLog();
                    string statusOld = "", statusNew = "";
                    Int32 TransactionID = Convert.ToInt32(hdnTransactionID.Value);
                    PatientChargesHd entity = chargesDao.Get(TransactionID);
                    statusOld = entity.GCTransactionStatus;
                    if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCVoidReason = gcDeleteReason;
                        if (gcDeleteReason == Constant.DeleteReason.OTHER)
                        {
                            entity.VoidReason = reason;
                        }
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.VoidDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        chargesDao.Update(entity);

                        statusNew = entity.GCTransactionStatus;

                        log.VisitID = entity.VisitID;
                        log.TransactionID = entity.TransactionID;
                        log.LogDate = DateTime.Now;
                        log.UserID = AppSession.UserLogin.UserID;
                        log.GCTransactionStatusOLD = statusOld;
                        log.GCTransactionStatusNEW = statusNew;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        logDao.Insert(log);

                        PrescriptionOrderHd orderHd = orderHdDao.Get((int)entity.PrescriptionOrderID);

                        List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value), ctx);
                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                        {
                            patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            patientChargesDt.IsApproved = false;
                            //patientChargesDt.IsDeleted = true;
                            patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            chargesDtDao.Update(patientChargesDt);

                            PrescriptionOrderDt orderDt = orderDtDao.Get((int)patientChargesDt.PrescriptionOrderDetailID);
                            if (orderDt != null)
                            {
                                if (!orderDt.IsDeleted)
                                {
                                    if (orderDt.ItemID.ToString() == BusinessLayer.GetSettingParameter(Constant.SettingParameter.NON_MASTER_ITEM).ParameterValue)
                                    {
                                        orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                        orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                        orderDt.VoidReason = "Linked transaction was deleted";
                                    }
                                    else
                                    {
                                        if (orderHd.IsCreatedBySystem)
                                        {
                                            if (AppSession.IsAutoVoidPrescriptionOrderCreatedBySystem == "1")
                                            {
                                                orderDt.IsDeleted = true;
                                                orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                                orderDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                                orderDt.VoidReason = "Linked transaction was deleted";
                                            }
                                            else
                                            {
                                                orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                            }
                                        }
                                        else
                                        {
                                            orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                        }
                                    }
                                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    orderDtDao.Update(orderDt);
                                }
                            }
                        }

                        //Update Status PrescriptionOrderHd
                        if (orderHd != null)
                        {
                            if (orderHd.IsCreatedBySystem)
                            {
                                if (AppSession.IsAutoVoidPrescriptionOrderCreatedBySystem == "1")
                                {
                                    orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                    orderHd.VoidReason = "Linked transaction was deleted";
                                    orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                }
                                else
                                {
                                    orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    orderHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                    orderHd.ProposedBy = AppSession.UserLogin.UserID;
                                    orderHd.ProposedDate = DateTime.Now;
                                }
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                orderHdDao.Update(orderHd);
                            }
                            else
                            {
                                int dtAllCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND IsDeleted = 0", hdnPrescriptionOrderID.Value), ctx);
                                int dtOpenCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.RECEIVED), ctx);
                                int dtProcessedCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.IN_PROGRESS), ctx);
                                int dtVoidCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.CANCELLED), ctx);

                                if (dtVoidCount == dtAllCount)
                                {
                                    orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    orderHdDao.Update(orderHd);
                                }
                                else
                                {
                                    orderHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                    orderHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                    orderHd.StartDate = DateTime.Now.Date;
                                    orderHd.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    orderHdDao.Update(orderHd);
                                }
                            }

                            if (orderHd.PrescriptionOrderScheduledFromID != null && orderHd.PrescriptionOrderScheduledFromID != 0)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                PrescriptionOrderHd orderHdFrom = orderHdDao.Get((int)orderHd.PrescriptionOrderScheduledFromID);
                                orderHdFrom.IsScheduledTaken = false;
                                orderHdFrom.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                orderHdDao.Update(orderHdFrom);
                            }



                            #region Log PrescriptionTaskOrder
                            int prescriptionOrderID = Convert.ToInt32(entity.PrescriptionOrderID);
                            Helper.InsertPrescriptionOrderTaskLog(ctx, prescriptionOrderID, Constant.PrescriptionTaskLogStatus.Void, AppSession.UserLogin.UserID, false);
                            #endregion
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entity.TransactionNo);
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
            return result;
        }

        #region Proposed Entity
        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            ChargesStatusLogDao logDao = new ChargesStatusLogDao(ctx);
            PatientChargesHdInfoDao entityHdInfoDao = new PatientChargesHdInfoDao(ctx);

            bool isValid = false;

            try
            {
                ChargesStatusLog log = new ChargesStatusLog();
                string statusOld = "", statusNew = "";

                PatientChargesHd entity = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                PrescriptionOrderHd orderHd = orderHdDao.Get(Convert.ToInt32(entity.PrescriptionOrderID));
                statusOld = entity.GCTransactionStatus;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (hdnIsReviewPrescriptionMandatoryForProposedTransaction.Value == "1")
                    {
                        if (orderHd.IsCorrectPatient || orderHd.IsCorrectMedication || orderHd.IsCorrectStrength || orderHd.IsCorrectFrequency || orderHd.IsCorrectDosage || orderHd.IsCorrectRoute || orderHd.IsHasDrugInteraction || orderHd.IsHasDuplication || orderHd.IsCorrectTimeOfGiving || orderHd.IsADChecked || orderHd.IsFARChecked || orderHd.IsKLNChecked)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }

                    if (result)
                    {
                        string validationResult = ValidateTransaction();
                        string[] resultInfo = validationResult.Split('|');

                        if (resultInfo[0] == "0")
                        {
                            errMessage = resultInfo[1];
                            return false;
                        }

                        entity.ReferenceNo = txtReferenceNo.Text;
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.ProposedBy = AppSession.UserLogin.UserID;
                        entity.ProposedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);

                        PatientChargesHdInfo orderHdInfo = entityHdInfoDao.Get(Convert.ToInt32(entity.TransactionID));
                        if (orderHdInfo != null)
                        {
                            orderHdInfo.GCBPJSTransactionType = cboBPJSTransType.Value.ToString();
                            entityHdInfoDao.Update(orderHdInfo);
                        }

                        //Update Order : Start Time
                        if (orderHd != null)
                        {
                            orderHd.GCPrescriptionType = cboPrescriptionType.Value.ToString();
                            orderHd.GCOrderStatus = Constant.TestOrderStatus.COMPLETED;
                            orderHd.CompleteDate = DateTime.Now.Date;
                            orderHd.CompleteTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            orderHdDao.Update(orderHd);
                        }

                        statusNew = entity.GCTransactionStatus;

                        List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsApproved = 0 AND IsDeleted = 0", hdnTransactionID.Value), ctx);
                        foreach (PatientChargesDt entityDt in lstEntityDt)
                        {
                            entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entityDt.IsApproved = true;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);

                            PrescriptionOrderDt orderDt = orderDtDao.Get((int)entityDt.PrescriptionOrderDetailID);
                            if (orderDt != null)
                            {
                                if (!orderDt.IsDeleted && orderDt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                                {
                                    orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.COMPLETED;
                                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    orderDtDao.Update(orderDt);
                                }
                            }
                        }

                        log.VisitID = entity.VisitID;
                        log.TransactionID = entity.TransactionID;
                        log.LogDate = DateTime.Now;
                        log.UserID = AppSession.UserLogin.UserID;
                        log.GCTransactionStatusOLD = statusOld;
                        log.GCTransactionStatusNEW = statusNew;
                        logDao.Insert(log);

                        isValid = true;
                    }
                    else
                    {
                        errMessage = string.Format("Harap isi telaah resep terlebih dahulu.");
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }
                else
                {
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat di-proposed lagi.", entity.TransactionNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
                ctx.CommitTransaction();
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                if (hdnIsBridgingToMobileJKN.Value == "1")
                {
                    ConsultVisit oCV = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", entity.VisitID)).FirstOrDefault();
                    BusinessLayer.OnInsertBPJSTaskLog(oCV.RegistrationID, 7, AppSession.UserLogin.UserID, DateTime.Now);
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

            if (isValid)
            {
                if (hdnBusinessPartnerID.Value == "1")
                {
                    if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    {
                        if (hdnIsAutoCreateBillAfterProposedOP.Value == "1")
                        {
                            OnCreatePatientBill(Convert.ToString(hdnTransactionID.Value), Convert.ToString(hdnGCRegistrationStatus.Value), ref errMessage);
                        }
                    }
                    else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    {
                        if (hdnIsAutoCreateBillAfterProposedER.Value == "1")
                        {
                            OnCreatePatientBill(Convert.ToString(hdnTransactionID.Value), Convert.ToString(hdnGCRegistrationStatus.Value), ref errMessage);
                        }
                    }
                    else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    {
                        if (hdnIsAutoCreateBillAfterProposedIP.Value == "1")
                        {
                            OnCreatePatientBill(Convert.ToString(hdnTransactionID.Value), Convert.ToString(hdnGCRegistrationStatus.Value), ref errMessage);
                        }
                    }
                    else if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                    {
                        if (hdnIsAutoCreateBillAfterProposedPH.Value == "1")
                        {
                            OnCreatePatientBill(Convert.ToString(hdnTransactionID.Value), Convert.ToString(hdnGCRegistrationStatus.Value), ref errMessage);
                        }
                    }
                    else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && hdnHealthcareServiceUnitID.Value != hdnImagingServiceUnitID.Value && hdnHealthcareServiceUnitID.Value != hdnLaboratoryServiceUnitID.Value)
                    {
                        if (hdnIsAutoCreateBillAfterProposedMD.Value == "1")
                        {
                            OnCreatePatientBill(Convert.ToString(hdnTransactionID.Value), Convert.ToString(hdnGCRegistrationStatus.Value), ref errMessage);
                        }
                    }
                    else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && hdnHealthcareServiceUnitID.Value == hdnImagingServiceUnitID.Value)
                    {
                        if (hdnIsAutoCreateBillAfterProposedIS.Value == "1")
                        {
                            OnCreatePatientBill(Convert.ToString(hdnTransactionID.Value), Convert.ToString(hdnGCRegistrationStatus.Value), ref errMessage);
                        }
                    }
                    else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && hdnHealthcareServiceUnitID.Value == hdnLaboratoryServiceUnitID.Value)
                    {
                        if (hdnIsAutoCreateBillAfterProposedLB.Value == "1")
                        {
                            OnCreatePatientBill(Convert.ToString(hdnTransactionID.Value), Convert.ToString(hdnGCRegistrationStatus.Value), ref errMessage);
                        }
                    }
                }
            }

            return result;
        }

        #region CreateBill
        protected bool OnCreatePatientBill(string chargesID, string statusRegistration, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            string Param = chargesID;

            try
            {
                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')",
                    Constant.SettingParameter.FN_KONTROL_PEMBUATAN_TAGIHAN, Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_JIKA_TIDAK_ADA_OUTSTANDING_ORDER));
                hdnPembuatanTagihanTidakAdaOutstandingOrder.Value = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_JIKA_TIDAK_ADA_OUTSTANDING_ORDER).FirstOrDefault().ParameterValue;

                if (hdnPembuatanTagihanTidakAdaOutstandingOrder.Value == "0")
                {
                    #region Tanpa Cek Outstanding Order
                    if (statusRegistration == Constant.VisitStatus.CLOSED || statusRegistration == Constant.VisitStatus.CANCELLED)
                    {
                        result = false;
                        errMessage = "Registrasi untuk pasien ini sudah di tutup/batal, tagihan sudah di transfer ke rawat inap";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        string filterCharges = string.Format("TransactionID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", chargesID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN);
                        List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(filterCharges, ctx);
                        string filter = string.Format("TransactionID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", chargesID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN);
                        List<vPatientChargesHdChargeClass> lstTemp = BusinessLayer.GetvPatientChargesHdChargeClassList(filter, ctx);
                        List<vPatientChargesHdChargeClass> lst = lstTemp.Where(t => t.GCTransactionStatus != Constant.TransactionStatus.PROCESSED && t.GCTransactionStatus != Constant.TransactionStatus.CLOSED).ToList();
                        decimal total = lst.Sum(t => t.TotalPatientAmount);

                        decimal num2 = Convert.ToDecimal(hdnPatientAdminFee.Value);
                        decimal patientFee = Convert.ToDecimal(0);

                        if (hdnIsPatientAdminFeeInPercentage.Value == "1")
                        {
                            patientFee = total * num2 / 100;
                        }
                        else
                        {
                            patientFee = num2;
                        }


                        if (lstPatientChargesHd.Count > 0)
                        {
                            List<PatientChargesDt> lstAllPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID IN ({0}) AND LocationID IS NOT NULL AND IsApproved = 0 AND IsDeleted = 0", chargesID), ctx);
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

                            patientBill.CoverageAmount = 0;
                            patientBill.AdministrationFeeAmount = 0;
                            patientBill.PatientAdminFeeAmount = patientFee;
                            patientBill.ServiceFeeAmount = 0;
                            patientBill.PatientServiceFeeAmount = 0;
                            patientBill.DiffCoverageAmount = 0;
                            patientBill.PatientBillingNo = BusinessLayer.GenerateTransactionNo(transactionCode, patientBill.BillingDate, ctx);
                            patientBill.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                            patientBill.TotalPatientAmount = total + patientFee;
                            patientBill.TotalPayerAmount = 0;
                            patientBill.TotalAmount = total + patientFee;

                            if (hdnMenggunakanPembulatan.Value == "1")
                            {
                                decimal TotalPatientAmount = patientBill.TotalPatientAmount;
                                decimal TotalPayerAmount = patientBill.TotalPayerAmount;

                                if (hdnPembulatanKemana.Value == "1")
                                {
                                    #region SET YANG DIBAYAT PASIEN
                                    if (TotalPatientAmount > 0)
                                    {
                                        Decimal selisihPasien = 0;
                                        Decimal sisaBagiPasien = TotalPatientAmount % Convert.ToDecimal(hdnNilaiPembulatan.Value);
                                        if (sisaBagiPasien > 0)
                                        {
                                            selisihPasien = Convert.ToDecimal(hdnNilaiPembulatan.Value) - sisaBagiPasien;
                                        }
                                        TotalPatientAmount = TotalPatientAmount + selisihPasien;

                                        patientBill.TotalPatientAmount = TotalPatientAmount;
                                    }
                                    #endregion

                                    #region SET YANG DIBAYAR INSTANSI
                                    if (TotalPayerAmount > 0)
                                    {
                                        Decimal selisihInstansi = 0;
                                        Decimal sisaBagiInstansi = TotalPayerAmount % Convert.ToDecimal(hdnNilaiPembulatan.Value);
                                        if (sisaBagiInstansi > 0)
                                        {
                                            selisihInstansi = Convert.ToDecimal(hdnNilaiPembulatan.Value) - sisaBagiInstansi;
                                        }
                                        TotalPayerAmount = TotalPayerAmount + selisihInstansi;

                                        patientBill.TotalPayerAmount = TotalPayerAmount;
                                    }
                                    #endregion

                                    patientBill.TotalAmount = patientBill.TotalPatientAmount + patientBill.TotalPayerAmount;
                                }
                                else
                                {
                                    if (!String.IsNullOrEmpty(hdnNilaiPembulatan.Value))
                                    {
                                        #region SET YANG DIBAYAT PASIEN
                                        if (TotalPatientAmount > 0)
                                        {
                                            Decimal sisaBagiPasien = TotalPatientAmount % Convert.ToDecimal(hdnNilaiPembulatan.Value);
                                            TotalPatientAmount = TotalPatientAmount - sisaBagiPasien;

                                            patientBill.TotalPatientAmount = TotalPatientAmount;
                                        }
                                        #endregion

                                        #region SET YANG DIBAYAR INSTANSI
                                        if (TotalPayerAmount > 0)
                                        {
                                            Decimal sisaBagiInstansi = TotalPayerAmount % Convert.ToDecimal(hdnNilaiPembulatan.Value);
                                            TotalPayerAmount = TotalPayerAmount - sisaBagiInstansi;

                                            patientBill.TotalPayerAmount = TotalPayerAmount;
                                        }
                                        #endregion

                                        patientBill.TotalAmount = patientBill.TotalPatientAmount + patientBill.TotalPayerAmount;
                                    }
                                }
                            }

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
                        }
                        ctx.CommitTransaction();
                    }

                    #endregion
                }
                else
                {
                    #region Cek Outstanding Order

                    List<TestOrderHd> lstPendingTestOrderHd = BusinessLayer.GetTestOrderHdList(string.Format("LinkedChargesID IN ({0}) AND GCTransactionStatus = '{1}'", chargesID, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
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
                        List<ServiceOrderHd> lstPendingServiceOrderHd = BusinessLayer.GetServiceOrderHdList(string.Format("LinkedChargesID IN ({0}) AND GCTransactionStatus = '{1}'", chargesID, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
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
                            if (statusRegistration == Constant.VisitStatus.CLOSED || statusRegistration == Constant.VisitStatus.CANCELLED)
                            {
                                result = false;
                                errMessage = "Registrasi untuk pasien ini sudah di tutup/batal, tagihan sudah di transfer ke rawat inap";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                            else
                            {
                                List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", chargesID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN), ctx);
                                string filter = string.Format("TransactionID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", chargesID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN);
                                List<vPatientChargesHdChargeClass> lstTemp = BusinessLayer.GetvPatientChargesHdChargeClassList(filter, ctx);
                                List<vPatientChargesHdChargeClass> lst = lstTemp.Where(t => t.GCTransactionStatus != Constant.TransactionStatus.PROCESSED && t.GCTransactionStatus != Constant.TransactionStatus.CLOSED).ToList();
                                decimal total = lst.Sum(t => t.TotalPatientAmount);

                                decimal num2 = Convert.ToDecimal(hdnPatientAdminFee.Value);
                                decimal patientFee = Convert.ToDecimal(0);

                                if (hdnIsPatientAdminFeeInPercentage.Value == "1")
                                {
                                    patientFee = total * num2 / 100;
                                }
                                else
                                {
                                    patientFee = num2;
                                }

                                if (lstPatientChargesHd.Count > 0)
                                {
                                    List<PatientChargesDt> lstAllPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID IN ({0}) AND LocationID IS NOT NULL AND IsApproved = 0 AND IsDeleted = 0", chargesID), ctx);
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

                                    patientBill.CoverageAmount = 0;
                                    patientBill.AdministrationFeeAmount = 0;
                                    patientBill.PatientAdminFeeAmount = patientFee;
                                    patientBill.ServiceFeeAmount = 0;
                                    patientBill.PatientServiceFeeAmount = 0;
                                    patientBill.DiffCoverageAmount = 0;
                                    patientBill.PatientBillingNo = BusinessLayer.GenerateTransactionNo(transactionCode, patientBill.BillingDate, ctx);
                                    patientBill.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                                    patientBill.TotalPatientAmount = total + patientFee;
                                    patientBill.TotalPayerAmount = 0;
                                    patientBill.TotalAmount = total + patientFee;

                                    if (hdnMenggunakanPembulatan.Value == "1")
                                    {
                                        decimal TotalPatientAmount = patientBill.TotalPatientAmount;
                                        decimal TotalPayerAmount = patientBill.TotalPayerAmount;

                                        if (hdnPembulatanKemana.Value == "1")
                                        {
                                            #region SET YANG DIBAYAT PASIEN
                                            if (TotalPatientAmount > 0)
                                            {
                                                Decimal selisihPasien = 0;
                                                Decimal sisaBagiPasien = TotalPatientAmount % Convert.ToDecimal(hdnNilaiPembulatan.Value);
                                                if (sisaBagiPasien > 0)
                                                {
                                                    selisihPasien = Convert.ToDecimal(hdnNilaiPembulatan.Value) - sisaBagiPasien;
                                                }
                                                TotalPatientAmount = TotalPatientAmount + selisihPasien;

                                                patientBill.TotalPatientAmount = TotalPatientAmount;
                                            }
                                            #endregion

                                            #region SET YANG DIBAYAR INSTANSI
                                            if (TotalPayerAmount > 0)
                                            {
                                                Decimal selisihInstansi = 0;
                                                Decimal sisaBagiInstansi = TotalPayerAmount % Convert.ToDecimal(hdnNilaiPembulatan.Value);
                                                if (sisaBagiInstansi > 0)
                                                {
                                                    selisihInstansi = Convert.ToDecimal(hdnNilaiPembulatan.Value) - sisaBagiInstansi;
                                                }
                                                TotalPayerAmount = TotalPayerAmount + selisihInstansi;

                                                patientBill.TotalPayerAmount = TotalPayerAmount;
                                            }
                                            #endregion

                                            patientBill.TotalAmount = patientBill.TotalPatientAmount + patientBill.TotalPayerAmount;
                                        }
                                        else
                                        {
                                            if (!String.IsNullOrEmpty(hdnNilaiPembulatan.Value))
                                            {
                                                #region SET YANG DIBAYAT PASIEN
                                                if (TotalPatientAmount > 0)
                                                {
                                                    Decimal sisaBagiPasien = TotalPatientAmount % Convert.ToDecimal(hdnNilaiPembulatan.Value);
                                                    TotalPatientAmount = TotalPatientAmount - sisaBagiPasien;

                                                    patientBill.TotalPatientAmount = TotalPatientAmount;
                                                }
                                                #endregion

                                                #region SET YANG DIBAYAR INSTANSI
                                                if (TotalPayerAmount > 0)
                                                {
                                                    Decimal sisaBagiInstansi = TotalPayerAmount % Convert.ToDecimal(hdnNilaiPembulatan.Value);
                                                    TotalPayerAmount = TotalPayerAmount - sisaBagiInstansi;

                                                    patientBill.TotalPayerAmount = TotalPayerAmount;
                                                }
                                                #endregion

                                                patientBill.TotalAmount = patientBill.TotalPatientAmount + patientBill.TotalPayerAmount;
                                            }
                                        }
                                    }

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
                                }
                                ctx.CommitTransaction();
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
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
        #endregion

        private string ValidateTransaction()
        {
            string result;
            string lstSelectedID = "";
            List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsApproved = 0 AND IsDeleted = 0", hdnTransactionID.Value));
            if (lstEntityDt.Count > 0)
            {
                foreach (PatientChargesDt itm in lstEntityDt)
                    lstSelectedID += "," + itm.ItemID;
            }

            if (!String.IsNullOrEmpty(lstSelectedID))
            {
                string filterExpression = string.Format(" LocationID = {0} AND ItemID IN({1}) AND IsDeleted = 0", hdnLocationID.Value, lstSelectedID.Substring(1));
                List<vItemBalanceForCheckStock> lstBalance = BusinessLayer.GetvItemBalanceForCheckStockList(filterExpression);
                StringBuilder errMessage = new StringBuilder();
                foreach (PatientChargesDt item in lstEntityDt)
                {
                    vItemBalanceForCheckStock oBalance = lstBalance.Where(lst => lst.ItemID == item.ItemID).FirstOrDefault();
                    if (oBalance != null)
                    {
                        if (item.BaseQuantity > oBalance.QuantityEND)
                            errMessage.AppendLine(string.Format("Quantity Item {0} tidak mencukupi di Lokasi ini.", oBalance.ItemName1));
                    }
                }

                filterExpression = string.Format("PrescriptionOrderID = '{0}' AND GCPrescriptionOrderStatus = '{1}' AND OrderIsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.TestOrderStatus.RECEIVED);
                List<vPrescriptionOrderDt1> lstOpenOrder = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
                if (lstOpenOrder.Count > 0)
                {
                    errMessage.AppendLine("Masih ada item order yang belum diproses.");
                }

                if (!string.IsNullOrEmpty(errMessage.ToString()))
                    result = string.Format("{0}|{1}", "0", errMessage.ToString());
                else
                    result = string.Format("{0}|{1}", "1", "success");
            }
            else
            {
                result = string.Format("{0}|Tidak ada item yang bisa diproses", "0");
            }
            return result;
        }
        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string isAllow = "notConfirm";
            int TransactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    TransactionID = Convert.ToInt32(hdnEntryID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage, ref isAllow))
                        result += "success";
                    else
                        result += string.Format("fail|{0}|{1}", isAllow, errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref TransactionID, ref isAllow))
                        result += "success";
                    else
                        result += string.Format("fail|{0}|{1}", isAllow, errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                TransactionID = Convert.ToInt32(hdnEntryID.Value);
                if (OnDeleteEntityDt(ref errMessage, param[1]))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "switch")
            {
                TransactionID = Convert.ToInt32(hdnEntryID.Value);
                if (OnSwitchEntityDt(ref errMessage, TransactionID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = TransactionID.ToString();
        }

        private bool IsPrescriptionItemValid(ref string errMessage, ref string isAllow)
        {
            #region Check For Allergy
            string filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND IsDeleted = 0", hdnMRN.Value, Constant.AllergenType.DRUG);
            string allergenName = string.Empty;
            vDrugInfo entityItem = null;

            List<PatientAllergy> lstPatientAllergy = null;
            if (hdnDrugID.Value != "")
            {
                entityItem = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", hdnDrugID.Value))[0];
                if ((entityItem.ItemID != Convert.ToInt32(hdnDrugID.Value)) && (entityItem.GenericName != string.Empty))
                {
                    filterExpression += string.Format(" AND (Allergen LIKE '%{0}%' OR Allergen LIKE '%{1}%')", entityItem.GenericName, entityItem.ItemName1);
                }
                else filterExpression += string.Format(" AND Allergen LIKE '%{0}%'", entityItem.ItemName1);
                lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);
            }

            if (lstPatientAllergy.Count > 0)
            {
                errMessage = string.Format("Pasien memiliki alergi {0} ({1})", txtGenericName.Text, txtDrugName.Text).Replace("()", "");
                return false;
            }
            else
            {
                filterExpression = string.Format("ItemID = {0}", hdnDrugID.Value);
                List<DrugContent> contents = BusinessLayer.GetDrugContentList(filterExpression);
                foreach (DrugContent item in contents)
                {
                    if (item.Keyword != "" && item.Keyword != null)
                    {
                        filterExpression = string.Format("MRN = {0} AND GCAllergenType = '{1}' AND Allergen LIKE '%{2}%' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, Constant.AllergenType.DRUG, item.Keyword);
                        lstPatientAllergy = BusinessLayer.GetPatientAllergyList(filterExpression);
                        if (lstPatientAllergy.Count > 0)
                        {
                            errMessage = string.Format("Pasien memiliki alergi {0} ({1})", item.ContentText, txtDrugName.Text).Replace("()", "");
                            return false;
                        }
                    }
                }
            }
            #endregion

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION, Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA, Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI));
            SettingParameter setParMaxDurasiNarkotika = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_MAKSIMUM_DURASI_NARKOTIKA).FirstOrDefault();
            SettingParameter setParControlAdverseReaction = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_ADVERSE_REACTION).FirstOrDefault();
            bool isControlTheraphyDuplication = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.FM_KONTROL_DUPLIKASI_TERAPI).FirstOrDefault().ParameterValue == "0" ? false : true;

            string prescriptionOrderId = hdnPrescriptionOrderID.Value;

            if (isControlTheraphyDuplication)
            {
                #region Duplicate Theraphy
                if (prescriptionOrderId != "0" && prescriptionOrderId != "")
                {
                    string filterExp = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", prescriptionOrderId);
                    List<vPrescriptionOrderDt> itemlist = BusinessLayer.GetvPrescriptionOrderDtList(filterExp);
                    foreach (var item in itemlist)
                    {
                        //Generic Name
                        if ((item.ItemID.ToString() != hdnDrugID.Value) && (item.GenericName.ToLower().TrimEnd() == txtGenericName.Text.ToLower().TrimEnd()) && !item.GenericName.Equals(string.Empty))
                        {
                            errMessage = string.Format("Duplikasi obat dengan nama generik {0} yang sama ({1})", item.GenericName.TrimEnd(), item.ItemName1.TrimEnd());
                            return false;
                        }
                        //vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                        //if (drugInfo != null)
                        //{
                        //ATC Class
                        //di comment berkaitan dengan issue RSMD Jika ada order resep farmasi dengan kelompok/Kelas ATC yang sama di satu order tetap bisa dilakukan
                        //if ((item.ItemID.ToString() != hdnDrugID.Value) && ((drugInfo.ATCClassCode == entityItem.ATCClassCode) && (!String.IsNullOrEmpty(entityItem.ATCClassCode))))
                        //{
                        //    errMessage = string.Format("Duplikasi obat dengan Kelompok/Kelas ATC {0} yang sama ({1})", drugInfo.ATCClassName.TrimEnd(), item.ItemName1.TrimEnd());
                        //    return false;
                        //}
                        //Kelompok Theraphy
                        //di comment berkaitan dengan issue RSMD Jika ada order resep farmasi dengan kelompok terapi yang sama di satu order tetap bisa dilakukan
                        //if ((item.ItemID.ToString() != hdnDrugID.Value) && (drugInfo.MIMSClassCode.ToLower().TrimEnd() == entityItem.MIMSClassCode.ToLower().TrimEnd()) && (!String.IsNullOrEmpty(entityItem.MIMSClassCode)))
                        //{
                        //    errMessage = string.Format("Duplikasi obat dengan Kelompok Terapi {0} yang sama ({1})", drugInfo.MIMSClassName.TrimEnd(), item.ItemName1.TrimEnd());
                        //    return false;
                        //}
                        //}
                    }
                }
                #endregion
            }

            #region psikotropika & narkotika
            if (entityItem.GCDrugClass == Constant.DrugClass.MORPHIN || entityItem.GCDrugClass == Constant.DrugClass.NARKOTIKA || entityItem.GCDrugClass == Constant.DrugClass.PSIKOTROPIKA)
            {
                int frekuensiDay = 0;
                frekuensiDay = Convert.ToInt32(txtFrequencyNumber.Text);
                if (cboFrequencyTimeline.Value.ToString() == Constant.DosingFrequency.WEEK) frekuensiDay = frekuensiDay * 7;
                else if (cboFrequencyTimeline.Value.ToString() == Constant.DosingFrequency.HOUR) frekuensiDay = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(frekuensiDay) / 24.0));
                if (Convert.ToInt32(setParMaxDurasiNarkotika.ParameterValue) > 0)
                {
                    if (Convert.ToInt32(frekuensiDay) > Convert.ToInt32(setParMaxDurasiNarkotika.ParameterValue))
                    {
                        errMessage = string.Format("Obat {0} Mengandung Narkotika, pemakaian tidak boleh lebih dari {1} hari", entityItem.ItemName1.TrimEnd(), setParMaxDurasiNarkotika.ParameterValue);
                        return false;
                    }
                }
            }
            #endregion

            if (setParControlAdverseReaction.ParameterValue == "0")
            {
                isAllow = "confirm";
            }

            #region Adverse Reaction
            prescriptionOrderId = hdnPrescriptionOrderID.Value;
            if (prescriptionOrderId != "0")
            {
                filterExpression = string.Format("ItemID = {0}", hdnDrugID.Value);
                List<DrugReaction> reactions = BusinessLayer.GetDrugReactionList(filterExpression);
                foreach (DrugReaction advReaction in reactions)
                {
                    string filterExp = string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", prescriptionOrderId);
                    List<vPrescriptionOrderDt> itemlist = BusinessLayer.GetvPrescriptionOrderDtList(filterExp);
                    foreach (var item in itemlist)
                    {
                        vDrugInfo drugInfo = BusinessLayer.GetvDrugInfoList(string.Format("ItemCode = '{0}'", item.ItemCode)).FirstOrDefault();
                        if (drugInfo != null)
                        {
                            //Generic Name
                            if (drugInfo.GenericName.ToLower().TrimEnd().Contains(advReaction.AdverseReactionText1.ToLower().TrimEnd())
                                || advReaction.AdverseReactionText1.ToLower().TrimEnd().Contains(drugInfo.GenericName.ToLower().TrimEnd()))
                            {
                                errMessage = string.Format("Terjadi interaksi obat dengan {0} ({1}) \n Catatan Interaksi Obat: \n {2}", item.ItemName1.TrimEnd(), drugInfo.GenericName, advReaction.AdverseReactionText2);
                                return false;
                            }
                        }
                    }
                }
            }
            #endregion

            #region Kontrol Pemberian Obat Kronis - BPJS
            if ((AppSession.RegisteredPatient.DepartmentID != Constant.Facility.INPATIENT && AppSession.RegisteredPatient.DepartmentID != Constant.Facility.EMERGENCY) && hdnIsControlChronicDrug.Value == "1" && hdnIsBPJSPatient.Value == "1")
            {
                if (entityItem != null)
                {
                    if (entityItem.IsChronic)
                    {
                        List<BPJSPatientChronicMedication> lstTrx = BusinessLayer.GetBPJSPatientChronicMedicationList(AppSession.RegisteredPatient.MRN, entityItem.ItemID, Convert.ToInt16(hdnChronicDrugDuration.Value));
                        if (lstTrx.Count > 0)
                        {
                            BPJSPatientChronicMedication trxInfo = lstTrx.FirstOrDefault();
                            errMessage = string.Format("{0} termasuk obat kronis, terakhir kali diberikan pada tanggal {1} sebanyak {2}", entityItem.ItemName1.TrimEnd(), trxInfo.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT), trxInfo.ChargedQuantity.ToString("G29"));
                            return false;
                        }
                    }
                }
            }
            #endregion

            return (errMessage == string.Empty);
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int TransactionID, ref string isAllow)
        {
            bool result = true;
            result = IsPrescriptionItemValid(ref errMessage, ref isAllow);
            if (result || isAllow.Equals("confirm"))
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);
                PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
                ItemProductDao iProductDao = new ItemProductDao(ctx);
                try
                {
                    int PrescriptionID = 0, PrescriptionOrderDtID = 0;
                    string TransactionNo = "";
                    SavePrescriptionHd(ctx, ref PrescriptionID, ref TransactionID, ref TransactionNo);

                    PatientChargesHd entityHd = entityHdDao.Get(TransactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PrescriptionOrderDt entityDt = new PrescriptionOrderDt();
                        PatientChargesDt entityChargesDt = new PatientChargesDt();
                        ControlToEntity(entityDt, entityChargesDt);

                        entityDt.PrescriptionOrderID = PrescriptionID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        PrescriptionOrderDtID = entityOrderDtDao.InsertReturnPrimaryKeyID(entityDt);

                        ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityChargesDt.ItemID), ctx).FirstOrDefault();
                        entityChargesDt.AveragePrice = iPlanning.AveragePrice;
                        entityChargesDt.CostAmount = iPlanning.UnitPrice;

                        if (entityChargesDt.ItemID != null && entityChargesDt.ItemID != 0)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            ItemProduct iProduct = iProductDao.Get(entityChargesDt.ItemID);
                            entityChargesDt.HETAmount = iProduct.HETAmount;
                        }

                        entityChargesDt.PrescriptionOrderDetailID = PrescriptionOrderDtID;
                        entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                        entityChargesDt.TransactionID = TransactionID;
                        entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityChargesDtDao.Insert(entityChargesDt);
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entityHd.TransactionNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
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
            return result;
        }

        private bool OnSaveEditRecordEntityDt(ref string errMessage, ref string isAllow)
        {
            bool result = true;
            errMessage = string.Empty;
            result = IsPrescriptionItemValid(ref errMessage, ref isAllow);
            if (result || isAllow.Equals("confirm"))
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                PatientChargesDtDao entityChargesDao = new PatientChargesDtDao(ctx);
                PrescriptionOrderDtLogDao orderLogDao = new PrescriptionOrderDtLogDao(ctx);

                try
                {
                    int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                    if (transactionID > 0)
                    {
                        PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                            hdnSelectedItem.Value = JsonConvert.SerializeObject(entityDt);
                            PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(String.Format("PrescriptionOrderDetailID = {0} AND GCTransactionDetailStatus != '{1}'", hdnEntryID.Value, Constant.TransactionStatus.VOID), ctx)[0];
                            if (!entityChargesDt.IsDeleted)
                            {
                                if (hdnDrugID.Value.ToString() != "" && hdnDrugID.Value.ToString() != "0" && hdnDrugID.Value.ToString() != entityDt.ItemID.ToString())
                                {
                                    result = false;
                                    errMessage = "Maaf, transaksi tidak diperbolehkan mengubah itemnya. Jika ingin diubah itemnya, silahkan hapus detail ini, lalu tambah detail item baru.";
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                }
                                else
                                {
                                    ControlToEntity(entityDt, entityChargesDt);
                                    entityChargesDt.LastUpdatedBy = entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDtDao.Update(entityDt);
                                    entityChargesDao.Update(entityChargesDt);

                                    //PrescriptionOrderDt oOldItem = JsonConvert.DeserializeObject<PrescriptionOrderDt>(hdnSelectedItem.Value);
                                    PrescriptionOrderDtLog orderDtLog = new PrescriptionOrderDtLog();
                                    orderDtLog.LogDate = DateTime.Now;
                                    orderDtLog.PrescriptionOrderDetailID = entityDt.PrescriptionOrderDetailID;
                                    orderDtLog.OldValues = hdnSelectedItem.Value;
                                    orderDtLog.NewValues = JsonConvert.SerializeObject(entityDt);
                                    orderDtLog.UserID = AppSession.UserLogin.UserID;
                                    orderLogDao.Insert(orderDtLog);
                                }
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entityHd.TransactionNo);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
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
            }
            return result;
        }

        private bool OnDeleteEntityDt(ref string errMessage, string param)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderDtLogDao orderLogDao = new PrescriptionOrderDtLogDao(ctx);
            string[] paramDelete = param.Split(';');
            int ID = Convert.ToInt32(paramDelete[0]);
            string gcDeleteReason = paramDelete[1];
            string reason = paramDelete[2];
            try
            {
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                if (transactionID > 0)
                {
                    PatientChargesHd entityHd = entityChargesHdDao.Get(transactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PrescriptionOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                        hdnSelectedItem.Value = JsonConvert.SerializeObject(entityDt);
                        if (!entityDt.IsCompound)
                        {
                            PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDtList(String.Format("TransactionID = {0} AND ID = {1} AND PrescriptionOrderDetailID = {2}", hdnTransactionID.Value, hdnChargeDtID.Value, hdnEntryID.Value), ctx)[0];
                            entityChargesDt.PrescriptionOrderDetailID = null;
                            entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            entityChargesDt.GCDeleteReason = gcDeleteReason;
                            entityChargesDt.DeleteReason = reason;
                            entityChargesDt.DeleteDate = DateTime.Now;
                            entityChargesDt.DeleteBy = AppSession.UserLogin.UserID;
                            entityChargesDt.IsDeleted = true;
                            entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityChargesDtDao.Update(entityChargesDt);

                            if (!entityDt.IsCreatedFromOrder)
                            {
                                if (AppSession.IsAutoVoidPrescriptionOrderCreatedBySystem == "1")
                                {
                                    entityDt.IsDeleted = true;
                                    entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                    entityDt.GCVoidReason = Constant.DeleteReason.OTHER;
                                    entityDt.VoidReason = "Linked transaction was deleted";
                                }
                                else
                                {
                                    entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                }
                            }
                            else
                            {
                                entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                            }

                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }
                        else
                        {
                            List<PrescriptionOrderDt> lstPrescriptionDt = BusinessLayer.GetPrescriptionOrderDtList(String.Format("PrescriptionOrderDetailID = {0} OR ParentID = {0}", hdnEntryID.Value), ctx);

                            String lstOrderDetailID = "";
                            foreach (PrescriptionOrderDt obj in lstPrescriptionDt)
                            {
                                if (lstOrderDetailID == "") lstOrderDetailID += obj.PrescriptionOrderDetailID;
                                else lstOrderDetailID += "," + obj.PrescriptionOrderDetailID;
                            }

                            List<PatientChargesDt> lstChargesDt = BusinessLayer.GetPatientChargesDtList(String.Format("TransactionID = {0} AND PrescriptionOrderDetailID IN ({1})", hdnTransactionID.Value, lstOrderDetailID), ctx);
                            foreach (PrescriptionOrderDt obj in lstPrescriptionDt)
                            {
                                PatientChargesDt objChargesDt = lstChargesDt.FirstOrDefault(x => x.PrescriptionOrderDetailID == obj.PrescriptionOrderDetailID && x.IsDeleted == false); // && x.ID == Convert.ToInt32(hdnChargeDtID.Value));
                                if (objChargesDt != null)
                                {
                                    objChargesDt.PrescriptionOrderDetailID = null;
                                    objChargesDt.GCDeleteReason = gcDeleteReason;
                                    objChargesDt.DeleteReason = reason;
                                    objChargesDt.DeleteDate = DateTime.Now;
                                    objChargesDt.DeleteBy = AppSession.UserLogin.UserID;
                                    objChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                    objChargesDt.IsDeleted = true;
                                    objChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityChargesDtDao.Update(objChargesDt);
                                }

                                if (!entityDt.IsCreatedFromOrder)
                                {
                                    if (AppSession.IsAutoVoidPrescriptionOrderCreatedBySystem == "1")
                                    {
                                        obj.IsDeleted = true;
                                        obj.GCPrescriptionOrderStatus = Constant.OrderStatus.CANCELLED;
                                        obj.GCVoidReason = Constant.DeleteReason.OTHER;
                                        obj.VoidReason = "Linked transaction was deleted";
                                    }
                                    else
                                    {
                                        obj.ResultQty = obj.DispenseQty;
                                        obj.TakenQty = obj.DispenseQty;
                                        obj.ChargeQty = obj.ChargeQty;
                                        obj.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                    }
                                }
                                else
                                {
                                    obj.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                                }

                                obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(obj);
                            }
                        }

                        PrescriptionOrderDtLog orderDtLog = new PrescriptionOrderDtLog();
                        orderDtLog.LogDate = DateTime.Now;
                        orderDtLog.PrescriptionOrderDetailID = entityDt.PrescriptionOrderDetailID;
                        orderDtLog.OldValues = hdnSelectedItem.Value;
                        orderDtLog.NewValues = JsonConvert.SerializeObject(entityDt);
                        orderDtLog.UserID = AppSession.UserLogin.UserID;
                        orderLogDao.Insert(orderDtLog);
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entityHd.TransactionNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = string.Format("<strong>{0} ({1})</strong><br/><br/><i>{2}</i>", ex.Message, ex.Source, ex.StackTrace);

                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnSwitchEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                if (transactionID > 0)
                {
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = BusinessLayer.GetPatientChargesDtList(String.Format("PrescriptionOrderDetailID = {0}", hdnEntryID.Value), ctx).FirstOrDefault();
                        if (!entity.IsDeleted)
                        {
                            decimal temp = entity.PayerAmount;
                            entity.PayerAmount = entity.PatientAmount;
                            entity.PatientAmount = temp;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterCompound = string.Format("PrescriptionOrderDetailID IN (SELECT PrescriptionOrderDetailID FROM PrescriptionOrderDt WHERE ParentID = {0} AND IsDeleted = 0) AND IsDeleted = 0", hdnEntryID.Value);
                        List<PatientChargesDt> lstCompound = BusinessLayer.GetPatientChargesDtList(filterCompound, ctx);
                        foreach (PatientChargesDt chargesCompound in lstCompound)
                        {
                            decimal temp = chargesCompound.PayerAmount;
                            chargesCompound.PayerAmount = chargesCompound.PatientAmount;
                            chargesCompound.PatientAmount = temp;
                            chargesCompound.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(chargesCompound);
                        }
                    }
                    else
                    {
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entityHd.TransactionNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = string.Format("<strong>{0} ({1})</strong><br/><br/><i>{2}</i>", ex.Message, ex.Source, ex.StackTrace);
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}