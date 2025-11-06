using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RegistrationEntry : BasePageTrx
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Status/";
        protected int serviceUnitUserCount = 0;
        protected string oDepartmentID = "";
        protected string maxBackDate = "";
        protected string maxNextDate = "";
        protected string filterExpressionOtherMedicalDiagnostic = "";

        private bool isAdd = false;

        //private const string LABORATORY = "LABORATORY";
        //private const string IMAGING = "IMAGING";
        //private const string PHARMACY = "PHARMACY";

        private const string REGISTRATION_ORDER_CLAUSE = "RegistrationID DESC";

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.REGISTRATION;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.REGISTRATION;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.REGISTRATION;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.REGISTRATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.REGISTRATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.REGISTRATION;
                    return Constant.MenuCode.MedicalDiagnostic.REGISTRATION;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.REGISTRATION;
                default: return Constant.MenuCode.Outpatient.REGISTRATION;
            }
        }

        protected string GetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }

        protected string GetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM CustomerContract WHERE '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0)", DateTime.Now.ToString("yyyyMMdd"));
        }

        protected string GetGenderFemale()
        {
            return Constant.Gender.FEMALE;
        }

        protected string GetItemMasterFilterExpression()
        {
            return string.Format("ItemID IN (SELECT ItemID FROM vItemService WHERE GCItemType = '{0}' AND IsPackageItem = 1) AND IsDeleted = 0 AND GCItemStatus = '{1}'", Constant.ItemType.MEDICAL_CHECKUP, Constant.ItemStatus.ACTIVE);
        }

        protected string GetItemMasterAIOFilterExpression()
        {
            string paramURL = Page.Request.QueryString["id"];

            string filterExpression = string.Format("ItemID IN (SELECT ItemID FROM ItemService WHERE IsPackageAllInOne = 1) AND IsDeleted = 0 AND GCItemStatus = '{0}'", Constant.ItemStatus.ACTIVE);

            if (paramURL == Constant.Facility.LABORATORY)
            {
                filterExpression += string.Format(" AND GCItemType = '{0}'", Constant.ItemType.LABORATORIUM);
            }
            else if (paramURL == Constant.Facility.IMAGING)
            {
                filterExpression += string.Format(" AND GCItemType = '{0}'", Constant.ItemType.RADIOLOGI);
            }
            else if (paramURL == Constant.Facility.DIAGNOSTIC)
            {
                filterExpression += string.Format(" AND GCItemType = '{0}'", Constant.ItemType.PENUNJANG_MEDIS);
            }
            else
            {
                filterExpression += string.Format(" AND GCItemType = '{0}'", Constant.ItemType.PELAYANAN);
            }

            return filterExpression;
        }

        protected string GetCustomerContractFilterExpression()
        {
            return string.Format(" AND EndDate >= CONVERT(DATE,GetDate()) AND IsDeleted = 0", hdnPayerID.Value, DateTime.Now.ToString("yyyyMMdd"));
        }

        protected string GetErrorMessageHasMedicalNo()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_PATIENT_HAS_MEDICAL_NO_VALIDATION);
        }

        protected string GetServiceUnitUserParameter()
        {
            return string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, hdnDepartmentID.Value);
        }

        protected string GetCustomerTypePersonal()
        {
            return Constant.CustomerType.PERSONAL;
        }

        protected string GetCustomerTypeHealthcare()
        {
            return Constant.CustomerType.HEALTHCARE;
        }

        protected override void InitializeDataControl()
        {
            isAdd = true;
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];
                oDepartmentID = hdnDepartmentID.Value;
                if (Page.Request.QueryString["id"] != Constant.Module.RADIOTHERAPHY)
                {
                    hdnIsRadioteraphy.Value = "0";
                    hdnDepartmentIDFilterAppointment.Value = Page.Request.QueryString["id"];
                }
                else
                {
                    hdnIsRadioteraphy.Value = "1";
                    hdnDepartmentIDFilterAppointment.Value = Constant.Facility.DIAGNOSTIC;
                }
                Healthcare oHc = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                hdnDateToday.Value = Helper.GetCurrentDate().ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                hdnIsBridgingToInhealth.Value = AppSession.IsBridgingToInhealth ? "1" : "0";

                if (hdnDepartmentID.Value == Constant.Facility.LABORATORY)
                {
                    //vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList("IsLaboratoryUnit = 1 AND IsUsingRegistration = 1").FirstOrDefault();
                    //hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                    //hdnIsServiceUnitHasParamedic.Value = hsu.IsHasParamedic ? "1" : "0";
                    //hdnIsServiceUnitHasVisitType.Value = hsu.IsHasVisitType ? "1" : "0";

                    if (hdnDepartmentID.Value == Constant.Facility.LABORATORY)
                        hdnDiagnosticType.Value = "0";
                    else
                        hdnDiagnosticType.Value = "1";

                    hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
                    hdnIsPoliExecutive.Value = "0";
                }
                else if (hdnDepartmentID.Value == Constant.Facility.IMAGING)
                {
                    vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID)).FirstOrDefault();
                    hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                    hdnIsServiceUnitHasParamedic.Value = hsu.IsHasParamedic ? "1" : "0";
                    hdnIsServiceUnitHasVisitType.Value = hsu.IsHasVisitType ? "1" : "0";

                    if (hdnDepartmentID.Value == Constant.Facility.LABORATORY)
                        hdnDiagnosticType.Value = "0";
                    else
                        hdnDiagnosticType.Value = "1";

                    hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
                    hdnIsPoliExecutive.Value = "0";
                }
                else if (hdnDepartmentID.Value == Constant.Module.RADIOTHERAPHY)
                {
                    vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID)).FirstOrDefault();
                    hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                    hdnIsServiceUnitHasParamedic.Value = hsu.IsHasParamedic ? "1" : "0";
                    hdnIsServiceUnitHasVisitType.Value = hsu.IsHasVisitType ? "1" : "0";

                    hdnDiagnosticType.Value = "1";

                    hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;
                    hdnIsPoliExecutive.Value = "0";
                }
                else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                {
                    vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.EMERGENCY, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                    hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                    hdnIsServiceUnitHasParamedic.Value = hsu.IsHasParamedic ? "1" : "0";
                    hdnIsServiceUnitHasVisitType.Value = hsu.IsHasVisitType ? "1" : "0";
                    hdnIsPoliExecutive.Value = "0";
                }
                else if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
                {
                    vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                    hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
                    hdnIsServiceUnitHasParamedic.Value = hsu.IsHasParamedic ? "1" : "0";
                    hdnIsServiceUnitHasVisitType.Value = hsu.IsHasVisitType ? "1" : "0";
                    hdnIsPoliExecutive.Value = "0";
                }
            }
            else
                hdnDepartmentID.Value = Constant.Facility.OUTPATIENT;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            hdnIsUsedPatientOwnerStatus.Value = AppSession.IsUsedPatientOwnerStatus;
            hdnIsUsedPatientOwnerStatusInInpatientRegistration.Value = AppSession.IsUsedPatientOwnerStatusInInpatientRegistration;
            hdnIsUsedPatientOwnerStatusInOutpatientRegistration.Value = AppSession.IsUsedPatientOwnerStatusInOutpatientRegistration;
            hdnIsUsedPatientOwnerStatusInEmergencyRegistration.Value = AppSession.IsUsedPatientOwnerStatusInEmergencyRegistration;
            hdnIsUsedPatientOwnerStatusInMCURegistration.Value = AppSession.IsUsedPatientOwnerStatusInMCURegistration;
            hdnIsUsedPatientOwnerStatusInLaboratoryRegistration.Value = AppSession.IsUsedPatientOwnerStatusInLaboratoryRegistration;
            hdnIsUsedPatientOwnerStatusInImagingRegistration.Value = AppSession.IsUsedPatientOwnerStatusInImagingRegistration;
            hdnIsUsedPatientOwnerStatusInDiagnosticRegistration.Value = AppSession.IsUsedPatientOwnerStatusInDiagnosticRegistration;
            hdnIsUsedPatientOwnerStatusInPharmacyRegistration.Value = AppSession.IsUsedPatientOwnerStatusInPharmacyRegistration;

            hdnIsUsedInputAIOPackageInOutpatientRegistration.Value = AppSession.IsUsedInputItemAIOInOutpatientRegistration;
            hdnIsUsedInputAIOPackageInLaboratoryRegistration.Value = AppSession.IsUsedInputItemAIOInLaboratoryRegistration;
            hdnIsUsedInputAIOPackageInImagingRegistration.Value = AppSession.IsUsedInputItemAIOInImagingRegistration;
            hdnIsUsedInputAIOPackageInDiagnosticRegistration.Value = AppSession.IsUsedInputItemAIOInDiagnosticRegistration;
            hdnIsUsedInputAIOPackageInPharmacyRegistration.Value = "0";
            hdnIsUsedInputAIOPackageInInpatientRegistration.Value = AppSession.IsUsedInputItemAIOInInpatientRegistration;
            hdnIsBridgingBPJSVClaimVersion.Value = AppSession.SA0167;

            GetSettingParameter();
            InitializeControlProperties();
        }

        private void GetSettingParameter()
        {
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')",
                Constant.SettingParameter.MAX_BACK_DATE, Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU, Constant.SettingParameter.MAX_NEXT_DATE));
            maxBackDate = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MAX_BACK_DATE).ParameterValue;
            maxNextDate = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MAX_NEXT_DATE).ParameterValue;

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}', '{13}','{14}','{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}','{39}','{40}','{41}','{42}','{43}','{44}','{45}','{46}','{47}','{48}','{49}','{50}','{51}','{52}')",
                AppSession.UserLogin.HealthcareID, //0
                Constant.SettingParameter.IS_BPJS_BRIDGING, //1
                Constant.SettingParameter.BPJS_CODE, //2
                Constant.SettingParameter.BPJS_CONSUMER_ID, //3
                Constant.SettingParameter.BPJS_CONSUMER_PASSWORD, //4
                Constant.SettingParameter.BPJS_SEP_WS_URL, //5
                Constant.SettingParameter.FN_KONTROL_BIAYA_KARTU, //6
                Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU, //7
                Constant.SettingParameter.FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ, //8
                Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI, //9
                Constant.SettingParameter.OP_PENDAFTARAN_DENGAN_RUANG, //10
                Constant.SettingParameter.FN_KONTROL_BIAYA_ADM_PASIEN_RJ, //11
                Constant.SettingParameter.SA_CHECK_DATA_PASIEN, //12
                Constant.SettingParameter.SA_AUTHORIZE_TO_JUMP, //13
                Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI, //14
                Constant.SettingParameter.SA_TIPE_SEARCH_DIALOG_PASIEN, //15
                Constant.SettingParameter.FN_WARNING_PASIEN_ADA_PIUTANG_PRIBADI, //16
                Constant.SettingParameter.OP_IS_QUEUE_NO_USING_APPOINTMENT, //17
                Constant.SettingParameter.RM_DEFAULT_PATIENT_WALKIN, //18
                Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID, //19
                Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID, //20
                Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID, //21
                Constant.SettingParameter.WARNING_PATIENT_OUTSTANDING_REG_DIFF_DAY, //22
                Constant.SettingParameter.IS_PENDAFTARAN_DENGAN_RUANG, //23
                Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //24
                Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //25
                Constant.SettingParameter.CHANGE_DOCTOR_AUTOMATIC_CHANGE_UNIT, //26
                Constant.SettingParameter.IP_REGISTRATION_BLOCK_FROM_REGISTRATION_24JAM, //27
                Constant.SettingParameter.IS_BRIDGING_TO_QUMATIC, //28
                Constant.SettingParameter.OP_IS_BLOCK_PATIENT_ALREADY_HAS_REGISTRATION_ON_DATE_CLINIC_AND_PARAMEDIC, //29
                Constant.SettingParameter.RM_REGISTRASI_SELAIN_RAJAL_MEMPERHATIKAN_CUTI_DOKTER, //30
                Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, //31
                Constant.SettingParameter.FN_PENJAMIN_INHEALTH, //32
                Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS, //33
                Constant.SettingParameter.SA_PARAMEDIC_SCHEDULE_VALIDATION_BEFORE_REGISTRATION, //34
                Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE, //35
                Constant.SettingParameter.IP_IS_PATIENT_NEWBORN_MANDATORY_MOTHER_REGISTRATIONNO, //36
                Constant.SettingParameter.FN_REGISTRATION_BABY_LINK_REGISTRATION_MOTHER, //37
                Constant.SettingParameter.IP_REGISTRATION_BLOCK_OPEN_REGISTRATION, //38
                Constant.SettingParameter.SA0117, //39
                Constant.SettingParameter.DEFAULT_PATIENT_OWNER_STATUS_IN_REGISTRATION, //40
                Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE, //41
                Constant.SettingParameter.IP0012, //42
                Constant.SettingParameter.LB_IS_USING_RESULT_DELIVERY_PLAN, //43
                Constant.SettingParameter.OP0038, //44
                Constant.SettingParameter.SA0215, //45
                Constant.SettingParameter.RM_DEFAULT_LAST_PARAMEDIC, //46
                Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE, //47
                Constant.SettingParameter.WARNING_PATIENT_PERSONAL_OUTSTANDING_REG_DIFF_DAY, //48
                Constant.SettingParameter.SA0138, //49
                Constant.SettingParameter.SA0175, //50
                Constant.SettingParameter.RT0001, //51
                Constant.SettingParameter.IP0040 //52
                ));

            SettingParameterDt oParam = new SettingParameterDt();
            oParam = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID).FirstOrDefault();
            if (oParam != null)
                hdnHealthcareServiceUnitICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;

            oParam = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID).FirstOrDefault();
            if (oParam != null)
                hdnHealthcareServiceUnitNICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;

            oParam = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID).FirstOrDefault();
            if (oParam != null)
                hdnHealthcareServiceUnitPICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;

            hdnIsQueueNoUsingAppointment.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.OP_IS_QUEUE_NO_USING_APPOINTMENT).FirstOrDefault().ParameterValue;
            hdnIsBridgingToBPJS.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.IS_BPJS_BRIDGING).FirstOrDefault().ParameterValue;
            hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";
            hdnItemCardFee.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU).ParameterValue;
            hdnIsControlPatientCardPayment.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_BIAYA_KARTU).ParameterValue;
            hdnIsControlAdministrationCharges.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ).ParameterValue;
            hdnChargeCodeAdministrationForInstansi.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI).ParameterValue;
            hdnIsOutpatientUsingRoom.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP_PENDAFTARAN_DENGAN_RUANG).ParameterValue;
            hdnIsEmergencyUsingRoom.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PENDAFTARAN_DENGAN_RUANG).ParameterValue;
            hdnIsControlAdmCost.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_BIAYA_ADM_PASIEN_RJ).ParameterValue;
            hdnIsCheckNewPatient.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_CHECK_DATA_PASIEN).ParameterValue;
            hdnIsAuthorizedToJump.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_AUTHORIZE_TO_JUMP).ParameterValue;
            hdnAdminID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI).ParameterValue;
            hdnIsWarningPatientHaveAR.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_WARNING_PASIEN_ADA_PIUTANG_PRIBADI).ParameterValue;
            hdnPatientSearchDialogType.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_TIPE_SEARCH_DIALOG_PASIEN).ParameterValue;
            hdnRMPatientWalkin.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RM_DEFAULT_PATIENT_WALKIN).ParameterValue;
            hdnWarningPatientOutstandingRegDiffDay.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.WARNING_PATIENT_OUTSTANDING_REG_DIFF_DAY).ParameterValue;
            hdnHealthcareServiceUnitIDLaboratory.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnHealthcareServiceUnitIDRadiology.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnPilihDokterOtomatisIsiUnit.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.CHANGE_DOCTOR_AUTOMATIC_CHANGE_UNIT).ParameterValue;
            hdnRegistrasiAsalDiblokJikaLebih24Jam.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP_REGISTRATION_BLOCK_FROM_REGISTRATION_24JAM).ParameterValue;
            hdnIsBridgingToQumatic.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_QUMATIC).ParameterValue;
            hndIsBlockPatientAlreadyHasRegistrationDateParamedicClinicSame.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP_IS_BLOCK_PATIENT_ALREADY_HAS_REGISTRATION_ON_DATE_CLINIC_AND_PARAMEDIC).ParameterValue;
            hdnRegistrasiSelainRajalMemperhatikanCutiDokter.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RM_REGISTRASI_SELAIN_RAJAL_MEMPERHATIKAN_CUTI_DOKTER).ParameterValue;
            hdnIsBridgingToGateway.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).ParameterValue;
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).ParameterValue;
            hdnInhealthCustomerType.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_INHEALTH).ParameterValue;
            hdnScheduleValidationBeforeRegistration.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_PARAMEDIC_SCHEDULE_VALIDATION_BEFORE_REGISTRATION).ParameterValue;
            hdnProviderGatewayService.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).ParameterValue;
            hdnIsPatientNewBornMandatoryMotherRegistrationNo.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP_IS_PATIENT_NEWBORN_MANDATORY_MOTHER_REGISTRATIONNO).ParameterValue;
            hdnIsRegistrationBabyLinkRegistrationMother.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_REGISTRATION_BABY_LINK_REGISTRATION_MOTHER).ParameterValue;
            hdnIsInpatientRegistrationBlockOpenRegistration.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP_REGISTRATION_BLOCK_OPEN_REGISTRATION).ParameterValue;
            hdnIsCheckScheduleBeforeRegistrationUsingConfirmation.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0117).ParameterValue;
            hdnDefaultBindingPatientOwnerStatus.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.DEFAULT_PATIENT_OWNER_STATUS_IN_REGISTRATION).ParameterValue;
            hdnIsParamedicInRegistrationUseSchedule.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE).ParameterValue;
            hdnIsBridgingToIPTV.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IP0012).ParameterValue;
            hdnIsUsingResultDeliveryPlan.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.LB_IS_USING_RESULT_DELIVERY_PLAN).FirstOrDefault().ParameterValue;
            hdnIsRegistrationUsingPromotionScheme.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.OP0038).FirstOrDefault().ParameterValue;
            hdnIsUsedReferenceQueueNo.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.SA0215).FirstOrDefault().ParameterValue;
            hdnDefaultLastParamedicID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM_DEFAULT_LAST_PARAMEDIC).FirstOrDefault().ParameterValue;
            hdnIsUsingMultiVisitSchedule.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).FirstOrDefault().ParameterValue;
            hdnWarningPatientPersonalOutstandingRegDiffDay.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.WARNING_PATIENT_PERSONAL_OUTSTANDING_REG_DIFF_DAY).ParameterValue;
            hdnIsBridgingToMobileJKN.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA0138).FirstOrDefault().ParameterValue;
            hdnIsSendNotifToMobileJKN.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.SA0175).FirstOrDefault().ParameterValue;
            hdnRadioteraphyUnitID.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.RT0001).FirstOrDefault().ParameterValue;
            hdnIsAutomaticInputChargeClass.Value = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.IP0040).FirstOrDefault().ParameterValue;
        }

        protected string GetPageTitle()
        {
            string menuCode = "Pendaftaran Pasien";
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.MEDICAL_CHECKUP: menuCode = Constant.MenuCode.MedicalCheckup.REGISTRATION; break;
                case Constant.Facility.INPATIENT: menuCode = Constant.MenuCode.Inpatient.REGISTRATION; break;
                case Constant.Facility.EMERGENCY: menuCode = Constant.MenuCode.EmergencyCare.REGISTRATION; break;
                case Constant.Facility.DIAGNOSTIC:
                    if (Page.Request.QueryString["id"] != null)
                    {
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        {
                            menuCode = Constant.MenuCode.Laboratory.REGISTRATION;
                        }
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        {
                            menuCode = Constant.MenuCode.Imaging.REGISTRATION;
                        }
                        else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        {
                            menuCode = Constant.MenuCode.Radiotheraphy.REGISTRATION;
                        }
                        else
                        {
                            menuCode = Constant.MenuCode.MedicalDiagnostic.REGISTRATION;
                        }
                    }
                    break;
                case Constant.Facility.PHARMACY: menuCode = Constant.MenuCode.Pharmacy.REGISTRATION; break;
                default: menuCode = Constant.MenuCode.Outpatient.REGISTRATION; break;
            }
            return BusinessLayer.GetMenuMasterList(string.Format("MenuCode='{0}'", menuCode)).FirstOrDefault().MenuCaption;
        }

        protected string GetServiceUnitLabel()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return GetLabel("Ruang Perawatan");
                case Constant.Facility.DIAGNOSTIC: return GetLabel("Penunjang Medis");
                case Constant.Facility.PHARMACY: return GetLabel("Farmasi");
                default: return GetLabel("Klinik");
            }
        }

        private void InitializeControlProperties()
        {
            if (hdnIsParamedicInRegistrationUseSchedule.Value == "1")
            {
                if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY || hdnDepartmentID.Value == Constant.Facility.INPATIENT || hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                {
                    trParamedicHasSchedule.Style.Add("display", "none");
                }
                else
                {
                    trParamedicHasSchedule.Style.Remove("display");
                }
            }
            else
            {
                trParamedicHasSchedule.Style.Add("display", "none");
            }

            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                string departmentID = Page.Request.QueryString["id"];
                if (departmentID != Constant.Facility.LABORATORY && departmentID != Constant.Facility.IMAGING && departmentID != Constant.Module.RADIOTHERAPHY)
                {
                    filterExpressionOtherMedicalDiagnostic = string.Format("HealthcareServiceUnitID NOT IN ({0},{1},{2}) AND IsLaboratoryUnit = 0", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID, hdnRadioteraphyUnitID.Value);
                }
            }
            serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", hdnDepartmentID.Value, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0",
                                                        Constant.StandardCode.REFERRAL, //0
                                                        Constant.StandardCode.CUSTOMER_TYPE, //1
                                                        Constant.StandardCode.ADMISSION_SOURCE, //2
                                                        Constant.StandardCode.VISIT_REASON, //3
                                                        Constant.StandardCode.ADMISSION_CONDITION, //4
                                                        Constant.StandardCode.RESULT_DELIVERY_PLAN //5
                                                    );
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            List<StandardCode> lstAdmissionSource = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_SOURCE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboAdmissionSource, lstAdmissionSource, "StandardCodeName", "StandardCodeID");

            StandardCode admissionSource = lstAdmissionSource.FirstOrDefault(p => p.IsDefault);
            if (admissionSource != null)
            {
                hdnDefaultGCAdmissionSource.Value = admissionSource.StandardCodeID;
            }
            else
            {
                hdnDefaultGCAdmissionSource.Value = "";
            }

            Methods.SetComboBoxField<StandardCode>(cboVisitReason, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.VISIT_REASON || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.REFERRAL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRegistrationPayer, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAdmissionCondition, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_CONDITION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            cboVisitReason.SelectedIndex = 0;
            cboRegistrationPayer.SelectedIndex = 0;
            cboAdmissionCondition.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboResultDeliveryPlan, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.RESULT_DELIVERY_PLAN || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            cboResultDeliveryPlan.Value = "";

            List<Specialty> lstSpecialty = BusinessLayer.GetSpecialtyList("IsDeleted = 0");
            Methods.SetComboBoxField<Specialty>(cboSpecialty, lstSpecialty, "SpecialtyName", "SpecialtyID");
            cboSpecialty.SelectedIndex = 0;

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0 AND IsUsedInChargeClass = 1");
            Methods.SetComboBoxField<ClassCare>(cboControlClassCare, lstClassCare, "ClassName", "ClassID");
            cboControlClassCare.SelectedIndex = 0;

            cboRegistrationPayer.Value = Constant.CustomerType.PERSONAL;

            hdnGCTariffSchemePersonal.Value = BusinessLayer.GetCustomer(1).GCTariffScheme;

            if (AppSession.IsBridgingToBPJS == false)
            {
                tdValidateBPJS.Style.Add("display", "none");
                hdBPJSInformation.Attributes.Add("style", "display:none");
                tblBPJSInformation.Attributes.Add("style", "display:none");
                tdReferral.Style.Add("display", "none");
                trIsKontrolBPJS.Attributes.Add("style", "display:none");
            }
            else
            {
                tdValidateBPJS.Style.Add("display", "block");
                hdBPJSInformation.Attributes.Add("style", "display:block");
                tblBPJSInformation.Attributes.Add("style", "display:block");
                tdReferral.Style.Add("display", "block");

                if (hdnDepartmentID.Value != Constant.Facility.OUTPATIENT)
                {
                    trIsKontrolBPJS.Attributes.Add("style", "display:none");
                }
                else
                {
                    trIsKontrolBPJS.Attributes.Remove("style");
                }
            }

            if (AppSession.IsBridgingToInhealth == true)
            {
                hdnTokenInhealth.Value = AppSession.Inheatlh_Access_Token;
                hdnKodeProviderInhealth.Value = AppSession.Inhealth_Provider_Code;
                hdnUsername.Value = AppSession.UserLogin.UserName;
                hdnTodayDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
            }

            List<StandardCode> lstPatientOwnerStatus = BusinessLayer.GetStandardCodeList(string.Format(
                                                                    "ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_OWNER_STATUS));
            lstPatientOwnerStatus.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            if (hdnIsUsedPatientOwnerStatus.Value == "1")
            {
                trPatientOwnerStatus.Style.Remove("display");

                if ((hdnIsUsedPatientOwnerStatusInInpatientRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    || (hdnIsUsedPatientOwnerStatusInOutpatientRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    || (hdnIsUsedPatientOwnerStatusInEmergencyRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    || (hdnIsUsedPatientOwnerStatusInMCURegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
                    || (hdnIsUsedPatientOwnerStatusInLaboratoryRegistration.Value == "1" && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                    || (hdnIsUsedPatientOwnerStatusInImagingRegistration.Value == "1" && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    || (hdnIsUsedPatientOwnerStatusInImagingRegistration.Value == "1" && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                    || (hdnIsUsedPatientOwnerStatusInDiagnosticRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                    || (hdnIsUsedPatientOwnerStatusInPharmacyRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.PHARMACY))
                {
                    Methods.SetComboBoxField<StandardCode>(cboPatientOwnerStatus, lstPatientOwnerStatus.Where(a => a.ParentID == Constant.StandardCode.PATIENT_OWNER_STATUS || a.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
                    cboPatientOwnerStatus.Value = hdnDefaultBindingPatientOwnerStatus.Value != "" ? hdnDefaultBindingPatientOwnerStatus.Value : Constant.PatientOwnerStatus.EXTERNAL;
                }
                else
                {
                    Methods.SetComboBoxField<StandardCode>(cboPatientOwnerStatus, lstPatientOwnerStatus, "StandardCodeName", "StandardCodeID");
                    cboPatientOwnerStatus.SelectedIndex = 0;
                }
            }
            else
            {
                trPatientOwnerStatus.Style.Add("display", "none");
            }

            if ((hdnIsUsedInputAIOPackageInOutpatientRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                || (hdnIsUsedInputAIOPackageInLaboratoryRegistration.Value == "1" && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                || (hdnIsUsedInputAIOPackageInImagingRegistration.Value == "1" && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                || (hdnIsUsedInputAIOPackageInImagingRegistration.Value == "1" && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                || (hdnIsUsedInputAIOPackageInDiagnosticRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                || (hdnIsUsedInputAIOPackageInPharmacyRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                || (hdnIsUsedInputAIOPackageInInpatientRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.INPATIENT))
            {
                trItemAIO.Style.Remove("display");
            }
            else
            {
                trItemAIO.Style.Add("display", "none");
            }

            if (hdnIsUsingResultDeliveryPlan.Value == "1")
            {
                trResultDeliveryPlan.Attributes.Remove("style");
            }
            else
            {
                trResultDeliveryPlan.Attributes.Add("style", "display:none");
            }

            if (hdnIsRegistrationUsingPromotionScheme.Value == "1")
            {
                tblPromotion.Attributes.Remove("style");
            }
            else
            {
                tblPromotion.Attributes.Add("style", "display:none");
            }
        }

        protected override void SetControlProperties()
        {
            string reservationID = Request.QueryString["resid"];
            string appointmentId = Request.QueryString["appid"];
            string departmentID = Page.Request.QueryString["id"];

            if (reservationID != null && !isNewOrRefreshClick)
            {
                vBedReservation reservation = BusinessLayer.GetvBedReservationList(string.Format("ReservationID = {0}", reservationID)).FirstOrDefault();
                hdnReservationID.Value = Convert.ToString(reservation.ReservationID);
                txtReservationNo.Text = reservation.ReservationNo;
                hdnMRN.Value = Convert.ToString(reservation.MRN);
                txtMRN.Text = reservation.MedicalNo;
                txtPatientName.Text = reservation.PatientName;
                txtPreferredName.Text = reservation.PreferredName;
                txtGender.Text = reservation.Gender;
                txtHandphoneNo.Text = reservation.PhoneNo1;
                txtDOB.Text = reservation.cfDateOfBirthInString;
                txtAgeInYear.Text = Convert.ToString(reservation.AgeInYear);
                txtAgeInMonth.Text = Convert.ToString(reservation.AgeInMonth);
                txtAgeInDay.Text = Convert.ToString(reservation.AgeInDay);
                txtAddress.Text = reservation.PatientAddress;
                hdnClassID.Value = Convert.ToString(reservation.ChargeClassID);
                txtClassCode.Text = reservation.ClassCode;
                txtClassName.Text = reservation.ClassName;
                hdnHealthcareServiceUnitID.Value = Convert.ToString(reservation.HealthcareServiceUnitID);
                txtServiceUnitCode.Text = reservation.ServiceUnitCode;
                txtServiceUnitName.Text = reservation.ServiceUnitName;
                if (!String.IsNullOrEmpty(Convert.ToString(reservation.RoomID)))
                {
                    hdnRoomID.Value = Convert.ToString(reservation.RoomID);
                    txtRoomCode.Text = reservation.RoomCode;
                    txtRoomName.Text = reservation.RoomName;
                }
                hdnBedID.Value = Convert.ToString(reservation.BedID);
                txtBedCode.Text = reservation.BedCode;
                hdnChargeClassID.Value = Convert.ToString(reservation.ChargeClassID);
                txtChargeClassCode.Text = reservation.ChargeClassCode;
                txtChargeClassName.Text = reservation.ChargeClassName;
            }


            if (appointmentId != null && !isNewOrRefreshClick)
            {
                vAppointment appointment = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", appointmentId)).FirstOrDefault();
                hdnAppointmentID.Value = appointmentId;
                txtAppointmentNo.Text = appointment.AppointmentNo;

                tdIsAutoAppointment.Style.Remove("display");
                tdIsAutoAppointment.Attributes.Remove("style");

                string filterExpressionDraftCharges = String.Format("AppointmentID = {0} AND GCTransactionStatus = '{1}' AND GCTransactionDetailStatus = '{1}' AND IsDeleted = 0", hdnAppointmentID.Value, Constant.TransactionStatus.OPEN);
                List<vDraftPatientChargesDt> lstDraftCharges = BusinessLayer.GetvDraftPatientChargesDtList(filterExpressionDraftCharges);
                string filterExpressionDraftTestOrderHd = String.Format("AppointmentID = {0} AND GCTransactionStatus = '{1}'", hdnAppointmentID.Value, Constant.TransactionStatus.OPEN, Constant.TestOrderStatus.OPEN);
                List<vDraftTestOrderHd> lstDraftTestOrderHd = BusinessLayer.GetvDraftTestOrderHdList(filterExpressionDraftTestOrderHd);
                string filterExpressionDraftTestOrder = String.Format("AppointmentID = {0} AND GCTransactionStatus = '{1}' AND GCDraftTestOrderStatus = '{2}' AND IsDeleted = 0", hdnAppointmentID.Value, Constant.TransactionStatus.OPEN, Constant.TestOrderStatus.OPEN);
                List<vDraftTestOrderDt> lstDraftTestOrder = BusinessLayer.GetvDraftTestOrderDtList(filterExpressionDraftTestOrder);
                string filterExpressionDraftServiceOrder = String.Format("AppointmentID = {0} AND GCTransactionStatus = '{1}' AND GCDraftServiceOrderStatus = '{2}' AND IsDeleted = 0", hdnAppointmentID.Value, Constant.TransactionStatus.OPEN, Constant.TestOrderStatus.OPEN);
                List<vDraftServiceOrderDt> lstDraftServiceOrder = BusinessLayer.GetvDraftServiceOrderDtList(filterExpressionDraftServiceOrder);
                string filterExpressionDraftPrescriptionOrder = String.Format("AppointmentID = {0} AND GCTransactionStatus = '{1}' AND GCDraftPrescriptionOrderStatus = '{2}' AND IsDeleted = 0", hdnAppointmentID.Value, Constant.TransactionStatus.OPEN, Constant.TestOrderStatus.OPEN);
                List<vDraftPrescriptionOrderDt> lstDraftPrescriptionOrder = BusinessLayer.GetvDraftPrescriptionOrderDtList(filterExpressionDraftPrescriptionOrder);

                if (lstDraftCharges.Count > 0 || lstDraftTestOrderHd.Count > 0 || lstDraftTestOrder.Count > 0 || lstDraftServiceOrder.Count > 0 || lstDraftPrescriptionOrder.Count > 0)
                {
                    hdnIsAppointmentHaveDraft.Value = "1";
                }
                else
                {
                    hdnIsAppointmentHaveDraft.Value = "0";
                }

                if (!String.IsNullOrEmpty(Convert.ToString(appointment.RoomID)))
                {
                    hdnRoomID.Value = Convert.ToString(appointment.RoomID);
                    txtRoomCode.Text = appointment.RoomCode;
                    txtRoomName.Text = appointment.RoomName;
                }

                cboRegistrationPayer.Text = appointment.GCCustomerType;
                cboSpecialty.Value = appointment.SpecialtyID.ToString();

                cboReferral.Value = appointment.GCReferrerGroup;
                if (appointment.ReferrerParamedicID != null && appointment.ReferrerParamedicID != 0)
                {
                    hdnReferrerID.Value = "";
                    hdnReferrerParamedicID.Value = appointment.ReferrerParamedicID.ToString();
                    txtReferralDescriptionCode.Text = appointment.ReferrerParamedicCode;
                    txtReferralDescriptionName.Text = appointment.ReferrerParamedicName;
                }
                else
                {
                    hdnReferrerID.Value = appointment.ReferrerID.ToString();
                    hdnReferrerParamedicID.Value = "";
                    txtReferralDescriptionCode.Text = appointment.ReferrerCode;
                    txtReferralDescriptionName.Text = appointment.ReferrerName;
                }
                txtReferralNo.Text = appointment.ReferenceNo.Contains('|') ? appointment.ReferenceNo.Split('|')[0] : appointment.ReferenceNo;
                hdnIsReferralVisit.Value = appointment.IsReferralVisit ? "1" : "0";

                if (appointment.GCCustomerType != Constant.CustomerType.PERSONAL)
                {
                    hdnPayerID.Value = Convert.ToString(appointment.BusinessPartnerID);
                    txtPayerCompanyCode.Text = appointment.BusinessPartnerCode;
                    txtPayerCompanyName.Text = appointment.BusinessPartnerName;
                    hdnContractID.Value = Convert.ToString(appointment.ContractID);
                    txtContractNo.Text = appointment.ContractNo;
                    txtContractPeriod.Text = appointment.ContractEndDate.ToString(Constant.FormatString.DATE_FORMAT);
                    hdnCoverageTypeID.Value = Convert.ToString(appointment.CoverageTypeID);
                    txtCoverageTypeCode.Text = appointment.CoverageTypeCode;
                    txtCoverageTypeName.Text = appointment.CoverageTypeName;
                    //                    cboControlClassCare.Text = appointment.ControlClassID;
                    txtCoverageLimit.Text = appointment.CoverageLimitAmount.ToString();
                    chkIsCoverageLimitPerDay.Checked = appointment.IsCoverageLimitPerDay;
                }

                chkIsUsingCOB.Checked = appointment.IsUsingCOB;

                if (appointment.GCResultDeliveryPlan != null && appointment.GCResultDeliveryPlan != "")
                {
                    cboResultDeliveryPlan.Value = appointment.GCResultDeliveryPlan;
                    if (appointment.GCResultDeliveryPlan == Constant.ResultDeliveryPlan.OTHERS)
                    {
                        txtResultDeliveryPlanOthers.Text = appointment.ResultDeliveryPlanOthers;
                    }
                    else
                    {
                        txtResultDeliveryPlanOthers.Text = "";
                    }
                }
                else
                {
                    cboResultDeliveryPlan.Value = "";
                    txtResultDeliveryPlanOthers.Text = "";
                }

            }

            if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                SettingParameterDt isAlwaysUseDefaultEmergencyParamedic = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.ER_DOKTER_UGD_SELALU_DEFAULT_SETVAR_DOKTER_UGD);
                SettingParameterDt SetParIgnoreEmergencyParamedic = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_IGNORE_EMERGENCY_PARAMEDIC);
                SettingParameterDt SetParDefaultParamedic = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.ER_DEFAULT_PHYSICIAN);

                if (SetParIgnoreEmergencyParamedic.ParameterValue == "1")
                {
                    if (SetParDefaultParamedic.ParameterValue != "" && SetParDefaultParamedic.ParameterValue != "0")
                    {
                        string filterLeave = string.Format("IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicID = {0}", SetParDefaultParamedic.ParameterValue);
                        List<vParamedicLeaveSchedule> lstLeave = BusinessLayer.GetvParamedicLeaveScheduleList(filterLeave);
                        if (lstLeave.Count() <= 0)
                        {
                            vParamedicMaster entityParamedic = BusinessLayer.GetvParamedicMasterList(String.Format("ParamedicID = {0}", SetParDefaultParamedic.ParameterValue.ToString()))[0];
                            hdnParamedicID.Value = entityParamedic.ParamedicID.ToString();
                            txtPhysicianCode.Text = entityParamedic.ParamedicCode;
                            txtPhysicianName.Text = entityParamedic.ParamedicName;
                            hdnPhysicianBPJSReferenceInfo.Value = entityParamedic.BPJSReferenceInfo;
                            cboSpecialty.Text = entityParamedic.SpecialtyID;
                        }
                    }
                    else
                    {
                        string filterExpression = OnGetRegistrationNoFilterExpression();
                        filterExpression += string.Format(" AND RegistrationDate = '{0}' ORDER BY CreatedDate DESC", DateTime.Now.ToString("yyyy-MM-dd"));
                        vRegistration2 entity = BusinessLayer.GetvRegistration2List(filterExpression).FirstOrDefault();
                        if (entity != null)
                        {
                            string filterLeave = string.Format("IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicID = {0}", entity.ParamedicID);
                            List<vParamedicLeaveSchedule> lstLeave = BusinessLayer.GetvParamedicLeaveScheduleList(filterLeave);
                            if (lstLeave.Count() <= 0)
                            {
                                hdnParamedicID.Value = entity.ParamedicID.ToString();
                                txtPhysicianCode.Text = entity.ParamedicCode;
                                txtPhysicianName.Text = entity.ParamedicName;
                                cboSpecialty.Value = entity.SpecialtyID;
                            }
                        }
                    }
                }
                else
                {
                    if (isAlwaysUseDefaultEmergencyParamedic.ParameterValue == "1")
                    {
                        if (SetParDefaultParamedic.ParameterValue != "" && SetParDefaultParamedic.ParameterValue != "0")
                        {
                            vParamedicMaster entityParamedic = BusinessLayer.GetvParamedicMasterList(String.Format("ParamedicID = {0}", SetParDefaultParamedic.ParameterValue.ToString()))[0];
                            string filterLeave = string.Format("IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicID = {0}", entityParamedic.ParamedicID);
                            List<vParamedicLeaveSchedule> lstLeave = BusinessLayer.GetvParamedicLeaveScheduleList(filterLeave);
                            if (lstLeave.Count() <= 0)
                            {
                                hdnParamedicID.Value = entityParamedic.ParamedicID.ToString();
                                txtPhysicianCode.Text = entityParamedic.ParamedicCode;
                                txtPhysicianName.Text = entityParamedic.ParamedicName;
                                cboSpecialty.Text = entityParamedic.SpecialtyID;
                            }
                        }
                        else
                        {
                            string filterExpression = OnGetRegistrationNoFilterExpression();
                            filterExpression += string.Format(" AND RegistrationDate = '{0}' ORDER BY CreatedDate DESC", DateTime.Now.ToString("yyyy-MM-dd"));
                            vRegistration2 entity = BusinessLayer.GetvRegistration2List(filterExpression).FirstOrDefault();
                            if (entity != null)
                            {
                                string filterLeave = string.Format("IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicID = {0}", entity.ParamedicID);
                                List<vParamedicLeaveSchedule> lstLeave = BusinessLayer.GetvParamedicLeaveScheduleList(filterLeave);
                                if (lstLeave.Count() <= 0)
                                {
                                    hdnParamedicID.Value = entity.ParamedicID.ToString();
                                    txtPhysicianCode.Text = entity.ParamedicCode;
                                    txtPhysicianName.Text = entity.ParamedicName;
                                    cboSpecialty.Value = entity.SpecialtyID;
                                }
                            }
                        }
                    }
                    else
                    {
                        string filterExpression = OnGetRegistrationNoFilterExpression();
                        filterExpression += string.Format(" AND RegistrationDate = '{0}' ORDER BY CreatedDate DESC", DateTime.Now.ToString("yyyy-MM-dd"));
                        vRegistration2 entity = BusinessLayer.GetvRegistration2List(filterExpression).FirstOrDefault();
                        if (entity != null)
                        {
                            string filterLeave = string.Format("IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicID = {0}", entity.ParamedicID);
                            List<vParamedicLeaveSchedule> lstLeave = BusinessLayer.GetvParamedicLeaveScheduleList(filterLeave);
                            if (lstLeave.Count() <= 0)
                            {
                                hdnParamedicID.Value = entity.ParamedicID.ToString();
                                txtPhysicianCode.Text = entity.ParamedicCode;
                                txtPhysicianName.Text = entity.ParamedicName;
                                cboSpecialty.Value = entity.SpecialtyID;
                            }
                        }
                    }
                }
            }

            if (isAdd)
            {
                btnSearchPeserta.Attributes.Add("style", "display:block");
                btnReferral.Attributes.Add("style", "display:block");
                btnGenerateSEP.Attributes.Add("style", "display:none");
                txtSurkonBPJS.Text = string.Empty;
            }
            else
            {
                btnSearchPeserta.Attributes.Add("style", "display:none");
                btnReferral.Attributes.Add("style", "display:none");
                Helper.SetControlEntrySetting(txtReferralNo, new ControlEntrySetting(true, false, true, string.Empty), "mpGenerateSEP");
                Helper.SetControlEntrySetting(txtTglRujukan, new ControlEntrySetting(true, false, true, string.Empty), "mpGenerateSEP");
                txtTglRujukan.Attributes.Add("readonly", "readonly");
                //Helper.SetControlEntrySetting(txtDiagnoseCode, new ControlEntrySetting(true, true, true, string.Empty), "mpGenerateSEP");
                //Helper.SetControlEntrySetting(txtDiagnoseText, new ControlEntrySetting(true, true, true, string.Empty), "mpGenerateSEP");
                //Helper.SetControlEntrySetting(txtReferralDescriptionCode, new ControlEntrySetting(true, true, true, string.Empty), "mpGenerateSEP");
                Helper.SetControlAttribute(txtReferralNo, true);
                Helper.SetControlAttribute(chkIsSuplesi, true);
                Helper.SetControlEntrySetting(txtKodeKecamatan, new ControlEntrySetting(true, false, false, string.Empty), "mpGenerateSEP");
                Helper.SetControlEntrySetting(txtKodeKabupaten, new ControlEntrySetting(true, false, false, string.Empty), "mpGenerateSEP");
                Helper.SetControlEntrySetting(txtKodePropinsi, new ControlEntrySetting(true, false, false, string.Empty), "mpGenerateSEP");
                Helper.SetControlEntrySetting(txtNoSEPSuplesi, new ControlEntrySetting(true, false, false, string.Empty), "mpGenerateSEP");
                //Helper.SetControlAttribute(txtDiagnoseText, true);
                //lblDiagnose.Attributes.Remove("class");
                //lblDiagnose.Attributes.Add("class", "lblLink");
                //lblDiagnose.Attributes.Add("IsEditAbleInEditMode", "1");
            }

            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC || hdnDepartmentID.Value == Constant.Facility.PHARMACY || hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
            {
                SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.SA_REGISTRATION_ALLOW_GUEST);
                if (setvarDt.ParameterValue == "0")
                {
                    trIsHasMRN.Style.Add("display", "none");
                }
            }
            else
            {
                trIsHasMRN.Style.Add("display", "none");
            }

            if (hdnDepartmentID.Value != Constant.Facility.MEDICAL_CHECKUP)
            {
                trItemMCU.Style.Add("display", "none");
            }

            if (hdnDepartmentID.Value != Constant.Facility.INPATIENT)
            {
                if (hdnDepartmentID.Value == Constant.Facility.IMAGING)
                {
                    trServiceUnit.Style.Add("display", "none");
                }

                if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                {
                    if (hdnIsEmergencyUsingRoom.Value != "1")
                    {
                        trBedQuickPicks.Style.Add("display", "none");
                        trRoom.Style.Add("display", "none");
                        trBed.Style.Add("display", "none");
                    }
                    else
                    {
                        lblRoom.Attributes.Add("class", "lblLink");
                        lblBed.Attributes.Add("class", "lblLink");
                    }
                    trReservation.Style.Add("display", "none");
                }
                else
                {
                    trBedQuickPicks.Style.Add("display", "none");
                    trRoom.Style.Add("display", "none");
                    trBed.Style.Add("display", "none");
                    trReservation.Style.Add("display", "none");
                }

                trClass.Style.Add("display", "none");
                trChargeClass.Style.Add("display", "none");

                trAdmissionSource.Style.Add("display", "none");
                trAdmissionRegistrationNo.Style.Add("display", "none");
                trClassRequest.Style.Add("display", "none");
                tdTemporaryLocation.Style.Add("display", "none");

            }

            if ((hdnDepartmentID.Value != Constant.Facility.OUTPATIENT && hdnDepartmentID.Value != Constant.Facility.DIAGNOSTIC && hdnDepartmentID.Value != Constant.Facility.MEDICAL_CHECKUP))
            {
                if (hdnDepartmentID.Value != Constant.Facility.PHARMACY && hdnDepartmentID.Value != Constant.Facility.INPATIENT)
                {
                    trServiceUnit.Style.Add("display", "none");
                }
                trAppointment.Style.Add("display", "none");
            }

            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT && hdnIsOutpatientUsingRoom.Value == "1")
            {
                trRoom.Style.Add("display", "table-row");
                trReservation.Style.Add("display", "none");
            }
            if (hdnDepartmentID.Value != Constant.Facility.OUTPATIENT && hdnDepartmentID.Value != Constant.Facility.EMERGENCY)
            {
                h4LastVisitData.Attributes.Add("style", "display:none");
                tblLastVisitData.Attributes.Add("style", "display:none");
            }
            if (hdnDepartmentID.Value != Constant.Facility.EMERGENCY)
            {

                trVisitReason.Style.Add("display", "none");
                trAdmissionCondition.Style.Add("display", "none");
                lblOtherVisitNotesLabel.InnerHtml = GetLabel("Alasan Kunjungan");
            }

            tblPayerCompany.Style.Remove("display");
            chkUsingCOB.Style.Remove("display");
            if (cboRegistrationPayer.Value.ToString() == Constant.CustomerType.PERSONAL)
            {
                tblPayerCompany.Style.Add("display", "none");
                chkUsingCOB.Style.Add("display", "none");
            }

            if (cboAdmissionSource.Value.ToString() == Constant.AdmissionSource.INPATIENT)
            {
                lblFromRegistrationNo.Attributes.Add("class", "lblDisabled");
                txtFromRegistrationNo.Attributes.Add("readonly", "readonly");
            }
            else
            {
                lblFromRegistrationNo.Attributes.Add("class", "lblLink lblMandatory");
                txtFromRegistrationNo.Attributes.Remove("readonly");
            }


            if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
            {
                string filterVT = String.Format("ParameterCode = '{0}'", Constant.SettingParameter.RM_DEFAULT_PHARMACY_VISITTYPE);
                SettingParameterDt setvarVT = BusinessLayer.GetSettingParameterDtList(filterVT).FirstOrDefault();
                if (!string.IsNullOrEmpty(setvarVT.ParameterValue))
                {
                    string filterVTPharmacy = String.Format("VisitTypeCode = '{0}'", setvarVT.ParameterValue);
                    VisitType vt = BusinessLayer.GetVisitTypeList(filterVTPharmacy).FirstOrDefault();
                    string filterSUVTPharmacy = String.Format("VisitTypeID = {0}", vt.VisitTypeID);
                    ServiceUnitVisitType suvt = BusinessLayer.GetServiceUnitVisitTypeList(filterSUVTPharmacy).FirstOrDefault();

                    hdnVisitTypeID.Value = suvt.VisitTypeID.ToString();
                    txtVisitTypeCode.Text = vt.VisitTypeCode;
                }
                trReservation.Style.Add("display", "none");
            }

            if (hdnDepartmentID.Value == Constant.Facility.PHARMACY || hdnDepartmentID.Value == Constant.Facility.LABORATORY || hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                string filterParamWalkin = String.Format("ParameterCode = '{0}'", Constant.SettingParameter.RM_DEFAULT_PATIENT_WALKIN);
                SettingParameterDt setvarWalkin = BusinessLayer.GetSettingParameterDtList(filterParamWalkin).FirstOrDefault();
                if (setvarWalkin.ParameterValue != "0" && setvarWalkin.ParameterValue != "" && setvarWalkin.ParameterValue != null)
                {
                    string filterPatientWalkin = String.Format("MedicalNo = '{0}'", setvarWalkin.ParameterValue);
                    vPatient patientWalkin = BusinessLayer.GetvPatientList(filterPatientWalkin).FirstOrDefault();

                    hdnMRN.Value = patientWalkin.MRN.ToString();
                    txtMRN.Text = patientWalkin.MedicalNo;

                    txtPatientName.Text = patientWalkin.PatientName;
                    txtPreferredName.Text = patientWalkin.PreferredName;
                    txtGender.Text = patientWalkin.Sex;
                    txtNHSRegistrationNo.Text = patientWalkin.NHSRegistrationNo;
                    txtNamaPeserta.Text = patientWalkin.NamaPesertaBPJS;
                    txtJenisPeserta.Text = patientWalkin.JenisPesertaBPJS;
                    txtKelas.Text = patientWalkin.KodeKelasBPJS;
                    txtNamaFaskes.Text = patientWalkin.NamaPPK1BPJS;
                    if (patientWalkin.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                    {
                        txtDOB.Text = patientWalkin.DateOfBirthInString;
                        txtAgeInYear.Text = patientWalkin.AgeInYear.ToString();
                        txtAgeInMonth.Text = patientWalkin.AgeInMonth.ToString();
                        txtAgeInDay.Text = patientWalkin.AgeInDay.ToString();
                    }
                    else
                    {
                        txtDOB.Text = "";
                        txtAgeInYear.Text = "0";
                        txtAgeInMonth.Text = "0";
                        txtAgeInDay.Text = "0";
                    }
                    txtHandphoneNo.Text = patientWalkin.MobilePhoneNo1;
                    txtAddress.Text = patientWalkin.HomeAddress;
                }
                trReservation.Style.Add("display", "none");
            }

            txtMRN.Focus();

            if (hdnDepartmentID.Value != Constant.Facility.PHARMACY && hdnDepartmentID.Value != Constant.Facility.DIAGNOSTIC)
            {
                btnClinicTransaction.Style.Add("display", "none");
            }
            else if (hdnDepartmentID.Value == Constant.Facility.PHARMACY && hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (hdnIsAuthorizedToJump.Value != "1")
                {
                    btnClinicTransaction.Style.Add("display", "none");
                    trReservation.Style.Add("display", "none");
                }
            }

            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (departmentID == Constant.Facility.IMAGING || departmentID == Constant.Facility.LABORATORY)
                {
                    trPartus.Style.Add("display", "none");
                }
            }

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                if (hdnIsPatientNewBornMandatoryMotherRegistrationNo.Value == "1")
                {
                    lblMotherRegNo.Attributes.Add("class", "lblLink lblMandatory");
                }
                else
                {
                    lblMotherRegNo.Attributes.Add("class", "lblLink lblKey");
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnIsBPJSChecked, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnIsNewPatient, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnIsAdd, new ControlEntrySetting(false, false, false, "0"));
            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT) hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_INPATIENT_ALLOW_BACK_DATE).ParameterValue;
            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY) hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_EMERGENCY_ALLOW_BACK_DATE).ParameterValue;
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC) hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_DIAGNOSTIC_ALLOW_BACK_DATE).ParameterValue;
            else if (hdnDepartmentID.Value == Constant.Facility.PHARMACY) hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_DIAGNOSTIC_ALLOW_BACK_DATE).ParameterValue;
            else if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP) hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_CHECKUP_ALLOW_BACK_DATE).ParameterValue;
            else
            {
                hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_OUTPATIENT_ALLOW_BACK_DATE).ParameterValue;
                hdnIsAllowNextDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_OUTPATIENT_ALLOW_NEXT_DATE).ParameterValue;
            }

            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_INPATIENT_ALLOW_BACK_DATE).ParameterValue; break;
                case Constant.Facility.EMERGENCY: hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_EMERGENCY_ALLOW_BACK_DATE).ParameterValue; break;
                case Constant.Facility.DIAGNOSTIC: hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_DIAGNOSTIC_ALLOW_BACK_DATE).ParameterValue; break;
                case Constant.Facility.PHARMACY: hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_DIAGNOSTIC_ALLOW_BACK_DATE).ParameterValue; break;
                case Constant.Facility.MEDICAL_CHECKUP: hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_CHECKUP_ALLOW_BACK_DATE).ParameterValue; break;
                default: hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_OUTPATIENT_ALLOW_BACK_DATE).ParameterValue; break;
            }

            SetControlEntrySetting(hdnRegistrationID, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnRegistrationStatus, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnIsAppointmentHaveDraft, new ControlEntrySetting(true, false, false, "0"));
            SetControlEntrySetting(hdnAppointmentID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtAppointmentNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(hdnReservationID, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtReservationNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting((hdnIsAllowBackDate.Value == "1"), false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtRegistrationHour, new ControlEntrySetting((hdnIsAllowBackDate.Value == "1"), false, true, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(true, false, false));

            SetControlEntrySetting(txtNoTicket, new ControlEntrySetting(true, false, false));

            if (hdnIsPatientNewBornMandatoryMotherRegistrationNo.Value == "1")
            {
                SetControlEntrySetting(txtMotherRegNo, new ControlEntrySetting(false, false, true));
            }
            else
            {
                SetControlEntrySetting(txtMotherRegNo, new ControlEntrySetting(false, false, false));
            }
            SetControlEntrySetting(txtRegistrar, new ControlEntrySetting(false, false, false, AppSession.UserLogin.UserFullName));
            SetControlEntrySetting(chkCardFee, new ControlEntrySetting(true, false, false));

            SetControlEntrySetting(cboAdmissionSource, new ControlEntrySetting(true, false, false, hdnDefaultGCAdmissionSource.Value));
            SetControlEntrySetting(txtFromRegistrationNo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtToRegistrationNo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(chkIsUsingCOB, new ControlEntrySetting(true, false, false, false));
            SetControlEntrySetting(chkIsGuaranteeLetterExists, new ControlEntrySetting(true, false, false, false));
            SetControlEntrySetting(chkIsCoverageLimitPerDay, new ControlEntrySetting(true, false, false, false));

            if (hdnIsUsingResultDeliveryPlan.Value == "1")
            {
                trResultDeliveryPlan.Attributes.Remove("style");
            }
            else
            {
                trResultDeliveryPlan.Attributes.Add("style", "display:none");
            }

            SetControlEntrySetting(cboResultDeliveryPlan, new ControlEntrySetting(true, false, false, ""));
            SetControlEntrySetting(txtResultDeliveryPlanOthers, new ControlEntrySetting(false, false, false, ""));

            SetControlEntrySetting(chkIsHasMRN, new ControlEntrySetting(true, false, false, true));
            SetControlEntrySetting(hdnMRN, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtMRN, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtIdentityCardNo, new ControlEntrySetting(false, false, false, string.Empty));
            SetControlEntrySetting(txtNHSRegistrationNo, new ControlEntrySetting(true, false, false, string.Empty));
            SetControlEntrySetting(txtInhealthParticipantNo, new ControlEntrySetting(false, false, false, string.Empty));
            SetControlEntrySetting(txtCorporateAccountNo, new ControlEntrySetting(false, false, false, string.Empty));
            SetControlEntrySetting(chkIsPregnant, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(chkIsKontrolBPJS, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtPatientName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsVisitorRestriction, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(chkIsTemporaryLocation, new ControlEntrySetting(true, false, false));

            SetControlEntrySetting(txtPreferredName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGender, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDOB, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeInYear, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeInMonth, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAgeInDay, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtHandphoneNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEmailAddress, new ControlEntrySetting(false, false, false, string.Empty));
            SetControlEntrySetting(txtPatientJob, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPatientJobOffice, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPatientNotes, new ControlEntrySetting(true, false, false));

            SetControlEntrySetting(txtAccidentLocation, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtNamaPeserta, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtJenisPeserta, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtKelas, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNamaFaskes, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNoSEP, new ControlEntrySetting(false, false, false, string.Empty));
            SetControlEntrySetting(txtIHSNumber, new ControlEntrySetting(false, false, false));

            if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
                SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MEDICAL_CHECKUP_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.OUTPATIENT_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.ER_CHARGE_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FM_CHARGE_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_CHARGE_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IMAGING_CHARGE_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RT0002).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MD_CHARGE_CLASS).ParameterValue));
            else
                SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, true));

            SetControlEntrySetting(txtClassCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtClassName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(hdnClassRequestID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtClassRequestCode, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtClassRequestName, new ControlEntrySetting(false, false, false));

            if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
                SetControlEntrySetting(hdnChargeClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MEDICAL_CHECKUP_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                SetControlEntrySetting(hdnChargeClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.OUTPATIENT_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                SetControlEntrySetting(hdnChargeClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.ER_CHARGE_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                SetControlEntrySetting(hdnChargeClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FM_CHARGE_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                SetControlEntrySetting(hdnChargeClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_CHARGE_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                SetControlEntrySetting(hdnChargeClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IMAGING_CHARGE_CLASS).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                SetControlEntrySetting(hdnChargeClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RT0002).ParameterValue));
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                SetControlEntrySetting(hdnChargeClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MD_CHARGE_CLASS).ParameterValue));
            else
                SetControlEntrySetting(hdnChargeClassID, new ControlEntrySetting(true, true));

            SetControlEntrySetting(txtChargeClassCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtChargeClassName, new ControlEntrySetting(false, false, true));

            if (hdnDepartmentID.Value != Constant.Facility.EMERGENCY)
            {
                SetControlEntrySetting(hdnRoomID, new ControlEntrySetting(true, true));
                SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtRoomName, new ControlEntrySetting(false, false, true));
                SetControlEntrySetting(hdnBedID, new ControlEntrySetting(true, true));
                SetControlEntrySetting(txtBedCode, new ControlEntrySetting(true, false, true));
            }
            else
            {
                SetControlEntrySetting(hdnRoomID, new ControlEntrySetting(true, true));
                SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtRoomName, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(hdnBedID, new ControlEntrySetting(true, true));
                SetControlEntrySetting(txtBedCode, new ControlEntrySetting(true, false, false));
            }

            if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
            {
                SettingParameter settingParameter = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PHARMACY_PHYSICIAN);
                ParamedicMaster objParamedicMaster = BusinessLayer.GetParamedicMaster(Convert.ToInt32(settingParameter.ParameterValue));

                if (hdnDefaultLastParamedicID.Value == "1")
                {
                    SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, true, true, ""));
                    SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, ""));
                    SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true, ""));
                    SetControlEntrySetting(cboSpecialty, new ControlEntrySetting(true, false, true, ""));
                }
                else
                {
                    SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, true, true, objParamedicMaster.ParamedicID));
                    SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, objParamedicMaster.ParamedicCode));
                    SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true, objParamedicMaster.FullName));
                    SetControlEntrySetting(cboSpecialty, new ControlEntrySetting(true, false, true, objParamedicMaster.SpecialtyID));
                }
            }
            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                String tempHdnParamedicID = "0";
                String tempTxtPhysicianCode = "";
                String tempTxtPhysicianName = "";
                object tempCboSpecialty = "";

                SettingParameterDt SetParDefaultParamedic = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.ER_DEFAULT_PHYSICIAN);
                if (SetParDefaultParamedic.ParameterValue != null && SetParDefaultParamedic.ParameterValue != "")
                {
                    string filterLeave = string.Format("IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicID = {0}", SetParDefaultParamedic.ParameterValue);
                    List<vParamedicLeaveSchedule> lstLeave = BusinessLayer.GetvParamedicLeaveScheduleList(filterLeave);
                    if (lstLeave.Count() <= 0)
                    {
                        vParamedicMaster entityParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", SetParDefaultParamedic.ParameterValue.ToString())).FirstOrDefault();
                        tempHdnParamedicID = entityParamedic.ParamedicID.ToString();
                        tempTxtPhysicianCode = entityParamedic.ParamedicCode;
                        tempTxtPhysicianName = entityParamedic.ParamedicName;
                        tempCboSpecialty = entityParamedic.SpecialtyID;
                    }
                }

                GetParamedicVisitTypeList entityVisitType = BusinessLayer.GetParamedicVisitTypeList(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(tempHdnParamedicID), "").FirstOrDefault();
                if (entityVisitType != null)
                {
                    SetControlEntrySetting(hdnVisitTypeID, new ControlEntrySetting(true, false, true, entityVisitType.VisitTypeID.ToString()));
                    SetControlEntrySetting(txtVisitTypeName, new ControlEntrySetting(false, false, false, entityVisitType.VisitTypeName));
                    SetControlEntrySetting(txtVisitTypeCode, new ControlEntrySetting(true, false, true, entityVisitType.VisitTypeCode));
                }
                else
                {
                    SetControlEntrySetting(txtVisitTypeCode, new ControlEntrySetting(true, false, true));
                    SetControlEntrySetting(txtVisitTypeName, new ControlEntrySetting(false, false, false));
                }

                if (hdnDefaultLastParamedicID.Value == "1")
                {
                    SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, true, true, ""));
                    SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, ""));
                    SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true, ""));
                    SetControlEntrySetting(cboSpecialty, new ControlEntrySetting(true, false, true, ""));
                }
                else
                {
                    SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, true, true, tempHdnParamedicID));
                    SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, tempTxtPhysicianCode));
                    SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true, tempTxtPhysicianName));
                    SetControlEntrySetting(cboSpecialty, new ControlEntrySetting(true, false, true, tempCboSpecialty));
                }

                vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.EMERGENCY, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true, false, hsu.HealthcareServiceUnitID.ToString()));
                SetControlEntrySetting(hdnBPJSPoli, new ControlEntrySetting(true, true, false, hsu.BPJSPoli.ToString()));
                SetControlEntrySetting(hdnIsServiceUnitHasParamedic, new ControlEntrySetting(true, true, false, hsu.IsHasParamedic ? "1" : "0"));
                SetControlEntrySetting(hdnIsServiceUnitHasVisitType, new ControlEntrySetting(true, true, false, hsu.IsHasVisitType ? "1" : "0"));
            }
            else
            {
                if (!String.IsNullOrEmpty(hdnLastParamedicID.Value))
                {
                    if (hdnDefaultLastParamedicID.Value == "1")
                    {
                        SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, true, true, ""));
                        SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, ""));
                        SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true, ""));
                        SetControlEntrySetting(cboSpecialty, new ControlEntrySetting(true, false, true, ""));
                    }
                    else
                    {
                        string filterLeave = string.Format("IsDeleted = 0 AND (CONVERT(DATE,GETDATE()) BETWEEN StartDate AND EndDate ) AND ParamedicID = {0}", hdnLastParamedicID.Value);
                        List<vParamedicLeaveSchedule> lstLeave = BusinessLayer.GetvParamedicLeaveScheduleList(filterLeave);
                        if (lstLeave.Count() <= 0)
                        {
                            SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, true, true, hdnLastParamedicID.Value));
                            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, hdnLastParamedicCode.Value));
                            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true, hdnLastParamedicName.Value));
                            SetControlEntrySetting(cboSpecialty, new ControlEntrySetting(true, false, true, hdnLastSpecialty.Value));
                        }
                        else
                        {
                            SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, true, true, ""));
                            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, ""));
                            SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true, ""));
                            SetControlEntrySetting(cboSpecialty, new ControlEntrySetting(true, false, true, ""));
                        }
                    }
                }
                else
                {
                    SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, true, true, ""));
                    SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true, ""));
                    SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(false, false, true, ""));
                    SetControlEntrySetting(cboSpecialty, new ControlEntrySetting(true, false, true, ""));
                }

                if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
                {
                    vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                    SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true, false, hsu.HealthcareServiceUnitID.ToString()));
                    SetControlEntrySetting(hdnIsServiceUnitHasParamedic, new ControlEntrySetting(true, true, false, hsu.IsHasParamedic ? "1" : "0"));
                    SetControlEntrySetting(hdnIsServiceUnitHasVisitType, new ControlEntrySetting(true, true, false, hsu.IsHasVisitType ? "1" : "0"));
                }
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                {
                    string departmentID = Page.Request.QueryString["id"];
                    //if (departmentID == LABORATORY || departmentID == IMAGING)
                    if (departmentID == Constant.Facility.IMAGING)
                        SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true, false, hdnHealthcareServiceUnitID.Value));
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true, false, hdnHealthcareServiceUnitID.Value));
                    else
                        SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true));
                    SetControlEntrySetting(hdnIsServiceUnitHasParamedic, new ControlEntrySetting(true, true));
                    SetControlEntrySetting(hdnIsServiceUnitHasVisitType, new ControlEntrySetting(true, true));
                }
                else
                {
                    SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true));
                    SetControlEntrySetting(hdnIsServiceUnitHasParamedic, new ControlEntrySetting(true, true));
                    SetControlEntrySetting(hdnIsServiceUnitHasVisitType, new ControlEntrySetting(true, true));
                }
            }

            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtItemName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(txtItemAIOCode, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtItemAIOName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, true));
            if (!(hdnDepartmentID.Value == Constant.Facility.EMERGENCY))
            {
                SetControlEntrySetting(txtVisitTypeCode, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtVisitTypeName, new ControlEntrySetting(false, false, false));
            }
            SetControlEntrySetting(cboReferral, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(hdnReferrerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnReferrerParamedicID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtReferralDescriptionCode, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtReferralDescriptionName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReferralNo, new ControlEntrySetting(true, false, false, string.Empty));
            SetControlEntrySetting(txtTglRujukan, new ControlEntrySetting(true, false, false, string.Empty));
            txtTglRujukan.Attributes.Add("readonly", "readonly");
            SetControlEntrySetting(txtReferrerDate, new ControlEntrySetting(true, false, false, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtReferrerTime, new ControlEntrySetting(true, false, false, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(txtDiagnoseCode, new ControlEntrySetting(true, false, false, string.Empty));
            SetControlEntrySetting(hdnBPJSDiagnoseCode, new ControlEntrySetting(true, false, false, string.Empty));
            SetControlEntrySetting(txtDiagnoseText, new ControlEntrySetting(true, false, false, string.Empty));
            SetControlEntrySetting(txtPatientVisaNumber, new ControlEntrySetting(true, false, false, string.Empty));
            SetControlEntrySetting(lblDiagnose, new ControlEntrySetting(true, false));
            SetControlEntrySetting(cboAdmissionCondition, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboVisitReason, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtVisitNotes, new ControlEntrySetting((hdnDepartmentID.Value != Constant.Facility.EMERGENCY), false, false));
            SetControlEntrySetting(cboRegistrationPayer, new ControlEntrySetting(true, false, false, Constant.CustomerType.PERSONAL));
            SetControlEntrySetting(hdnPayerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPayerCompanyCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPayerCompanyName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnContractID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtContractNo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtContractPeriod, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnCoverageTypeID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCoverageTypeCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtCoverageTypeName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnEmployeeID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtEmployeeCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtEmployeeName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtParticipantNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtCoverageLimit, new ControlEntrySetting(true, false, true, "0"));
            SetControlEntrySetting(hdnIsControlClassCare, new ControlEntrySetting(true, false, true, "0"));
            //SetControlEntrySetting(txtContractSummary, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboControlClassCare, new ControlEntrySetting(true, false, true));

            SetControlEntrySetting(chkIsNewBorn, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(chkIsParturition, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(chkIsNeedPastoralCare, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(chkIsFastTrack, new ControlEntrySetting(true, false, false));

            SetControlEntrySetting(lblItemMCU, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblItemAIO, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblMRN, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblGuestNo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblMotherRegNo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblAppointmentNo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblServiceUnit, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblSurkonBPJS, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblVisitType, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblReferralDescription, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblRoom, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblClass, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblClassRequest, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblChargeClass, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblBed, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblFromRegistrationNo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblToRegistrationNo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblPayerCompany, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblCoverageType, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblContract, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblEmployee, new ControlEntrySetting(true, false));

            SetControlEntrySetting(hdnVisitID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnFromRegistrationID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnToRegistrationID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestGCSalutation, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestGCTitle, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestFirstName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestMiddleName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestLastName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestGCSuffix, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestGCGender, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestDateOfBirth, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestStreetName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestCounty, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestCity, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestDistrict, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestStreetNameDomicile, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestCountyDomicile, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestCityDomicile, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestDistrictDomicile, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestPhoneNo, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestMobilePhoneNo, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestEmailAddress, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestGCIdentityNoType, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnGuestSSN, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGuestNo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnMotherMRN, new ControlEntrySetting(true, true));

            SetControlEntrySetting(lblPromotion, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnPromotionID, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPromotionCode, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtPromotionName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(chkParamedicHasSchedule, new ControlEntrySetting(true, false, false, false));

            if (hdnIsUsedPatientOwnerStatus.Value == "1")
            {
                if ((hdnIsUsedPatientOwnerStatusInInpatientRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    || (hdnIsUsedPatientOwnerStatusInOutpatientRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    || (hdnIsUsedPatientOwnerStatusInEmergencyRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    || (hdnIsUsedPatientOwnerStatusInMCURegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
                    || (hdnIsUsedPatientOwnerStatusInLaboratoryRegistration.Value == "1" && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                    || (hdnIsUsedPatientOwnerStatusInImagingRegistration.Value == "1" && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    || (hdnIsUsedPatientOwnerStatusInImagingRegistration.Value == "1" && AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                    || (hdnIsUsedPatientOwnerStatusInDiagnosticRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                    || (hdnIsUsedPatientOwnerStatusInPharmacyRegistration.Value == "1" && hdnDepartmentID.Value == Constant.Facility.PHARMACY))
                {
                    SetControlEntrySetting(cboPatientOwnerStatus, new ControlEntrySetting(true, false, true, hdnDefaultBindingPatientOwnerStatus.Value != "" ? hdnDefaultBindingPatientOwnerStatus.Value : Constant.PatientOwnerStatus.EXTERNAL));
                }
                else
                {
                    SetControlEntrySetting(cboPatientOwnerStatus, new ControlEntrySetting(true, false, false, hdnDefaultBindingPatientOwnerStatus.Value != "" ? hdnDefaultBindingPatientOwnerStatus.Value : Constant.PatientOwnerStatus.EXTERNAL));
                }
            }
            else
            {
                SetControlEntrySetting(cboPatientOwnerStatus, new ControlEntrySetting(false, false, false, hdnDefaultBindingPatientOwnerStatus.Value != "" ? hdnDefaultBindingPatientOwnerStatus.Value : Constant.PatientOwnerStatus.EXTERNAL));
            }
        }

        public override void OnAddRecord()
        {
            txtMRN.Focus();
            hdnFromRegistrationID.Value = "";
            isAdd = true;
        }

        #region Load Entity
        protected string OnGetRegistrationNoFilterExpression()
        {
            string filterExpression = "";
            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
                {
                    if (hdnDiagnosticType.Value == "0")
                    {
                        filterExpression = string.Format("HealthcareServiceUnitID IN (SELECT a.HealthcareServiceUnitID FROM HealthcareServiceUnit a WHERE a.ServiceUnitID IN (SELECT b.ServiceUnitID FROM ServiceUnitMaster b WHERE b.IsLaboratoryUnit = 1 AND b.IsUsingRegistration = 1))");
                    }
                    else
                    {
                        filterExpression = string.Format("HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(hdnRadioteraphyUnitID.Value))
                    {
                        filterExpression = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1},{2},{3})", hdnDepartmentID.Value, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID, hdnRadioteraphyUnitID.Value);
                    }
                    else
                    {
                        filterExpression = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1},{2})", hdnDepartmentID.Value, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                    }
                    filterExpression += string.Format(" AND HealthcareServiceUnitID NOT IN (SELECT a.HealthcareServiceUnitID FROM HealthcareServiceUnit a WHERE a.ServiceUnitID IN (SELECT b.ServiceUnitID FROM ServiceUnitMaster b WHERE b.IsLaboratoryUnit = 1 AND b.IsUsingRegistration = 1))");
                }
            }
            else
            {
                filterExpression = string.Format("DepartmentID = '{0}'", hdnDepartmentID.Value);
            }
            return filterExpression;
        }

        protected string OnGetMotherRegistrationNoFilterExpression()
        {
            return string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND DepartmentID = '{3}' AND IsParturition = 1", Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, hdnDepartmentID.Value);
        }

        public override int OnGetRowCount()
        {
            string filterExpression = OnGetRegistrationNoFilterExpression();
            return BusinessLayer.GetvRegistration1RowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = OnGetRegistrationNoFilterExpression();
            vRegistration1 entityTemp = BusinessLayer.GetvRegistration1(filterExpression, PageIndex, REGISTRATION_ORDER_CLAUSE);
            if (entityTemp != null)
            {
                if (entityTemp.DepartmentID != Constant.Facility.INPATIENT && entityTemp.DepartmentID != Constant.Facility.LABORATORY &&
                    entityTemp.DepartmentID != Constant.Facility.PHARMACY)
                {
                    Registration entityLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0} AND GCRegistrationStatus != '{1}'", entityTemp.RegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
                    hdnIsLinkedToInpatient.Value = entityLinked != null ? "1" : "0";
                }
            }
            filterExpression += string.Format(" AND RegistrationID = '{0}'", entityTemp.RegistrationID);
            vRegistration entity = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();
            if (entity.AppointmentID != 0)
            {
                if (!entity.IsAutoAppointment)
                {
                    tdIsAutoAppointment.Style.Remove("display");
                    tdIsAutoAppointment.Attributes.Remove("style");
                }
            }

            if (entity.IsVIP == true)
            {
                tdIsVIP.Style.Remove("display");
                tdIsVIP.Attributes.Remove("style");
            }

            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = OnGetRegistrationNoFilterExpression();
            PageIndex = BusinessLayer.GetvRegistration1RowIndex(filterExpression, keyValue, REGISTRATION_ORDER_CLAUSE);
            filterExpression += string.Format(" AND RegistrationNo = '{0}'", keyValue);
            vRegistration entity = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();
            if (entity != null)
            {
                if (entity.DepartmentID != Constant.Facility.INPATIENT && entity.DepartmentID != Constant.Facility.LABORATORY &&
                    entity.DepartmentID != Constant.Facility.PHARMACY)
                {
                    Registration entityLinked = BusinessLayer.GetRegistrationList(string.Format("LinkedRegistrationID = {0} AND GCRegistrationStatus != '{1}'", entity.RegistrationID, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
                    hdnIsLinkedToInpatient.Value = entityLinked != null ? "1" : "0";
                }

                if (entity.AppointmentID != 0)
                {
                    if (!entity.IsAutoAppointment)
                    {
                        tdIsAutoAppointment.Style.Remove("display");
                        tdIsAutoAppointment.Attributes.Remove("style");
                    }
                }

                if (entity.IsVIP == true)
                {
                    tdIsVIP.Style.Remove("display");
                    tdIsVIP.Attributes.Remove("style");
                }

                EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            }
        }

        private void EntityToControl(vRegistration entity, ref bool isShowWatermark, ref string watermarkText)
        {
            trParamedicHasSchedule.Style.Add("display", "none");
            btnParamedicSelection.Style.Add("display", "none");

            cboReferral.Enabled = false;

            bool flagEditable = true;
            hdnRegistrationStatus.Value = entity.GCRegistrationStatus;
            if (entity.GCRegistrationStatus == Constant.VisitStatus.CLOSED || entity.GCRegistrationStatus == Constant.VisitStatus.CANCELLED)
            {
                flagEditable = false;
                isShowWatermark = true;
                watermarkText = entity.RegistrationStatusWatermark;
                btnGenerateSEP.Attributes.Add("style", "display:none");
            }
            if (entity.IsNewBorn)
            {
                trMotherRegNo.Attributes.Remove("style");
                vPatientBirthRecord entityBirthRecord = BusinessLayer.GetvPatientBirthRecordList(string.Format("MRN = '{0}' AND IsDeleted = 0", entity.MRN)).FirstOrDefault();
                if (entityBirthRecord != null)
                {
                    hdnMotherVisitID.Value = entityBirthRecord.MotherVisitID.ToString();
                    hdnMotherMRN.Value = entityBirthRecord.MotherMRN.ToString();
                    hdnMotherName.Value = entityBirthRecord.MotherFirstName;
                    txtMotherRegNo.Text = entityBirthRecord.MotherRegistrationNo;
                }
            }

            if (entity.GuestID != 0)
            {
                Guest entityGuest = BusinessLayer.GetGuestList(string.Format("GuestID = '{0}' AND IsDeleted = 0", entity.GuestID)).FirstOrDefault();
                hdnGuestID.Value = Convert.ToString(entityGuest.GuestID);
                txtGuestNo.Text = entityGuest.GuestNo;
            }

            chkIsHasMRN.Checked = entity.IsHasMRN;
            hdnGuestID.Value = entity.GuestID.ToString();
            hdnMRN.Value = entity.MRN.ToString();
            hdnMobilePhoneNo1.Value = entity.MobilePhoneNo1;
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnVisitID.Value = entity.VisitID.ToString();
            txtReservationNo.Text = entity.ReservationNo;
            txtAppointmentNo.Text = entity.AppointmentNo;
            hdnAppointmentID.Value = entity.AppointmentID.ToString();
            if (!string.IsNullOrEmpty(entity.AppointmentNo))
            {
                trEstimatedService.Attributes.Remove("style");
                txtEstimatedService.Text = string.Format("{0} - {1}", entity.AppointmentStartTime, entity.AppointmentEndTime);
            }
            txtRegistrationDate.Text = entity.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRegistrationHour.Text = entity.RegistrationTime;
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtNoTicket.Text = entity.RegistrationTicketNo;

            if (hdnIsUsedReferenceQueueNo.Value == "1")
            {
                txtQueueNo.Text = entity.cfReferenceQueueNo;
            }
            else
            {
                txtQueueNo.Text = entity.QueueNo == 0 ? "" : entity.cfQueueNo;
            }
            chkCardFee.Checked = entity.IsPrintingPatientCard;
            txtRegistrar.Text = entity.CreatedByName;
            txtDiagnoseCode.Text = entity.DiagnoseID;
            txtDiagnoseName.Text = entity.DiagnoseName;
            txtReferralNo.Text = entity.ReferralNo;
            txtDiagnoseText.Text = entity.DiagnosisText;
            txtNoSEP.Text = entity.NoSEP;
            hdnBPJSPoli.Value = entity.NamaPoliKlinik;
            chkIsPregnant.Checked = entity.IsPregnant;
            txtMRN.Text = entity.MedicalNo;
            txtPatientName.Text = entity.cfPatientNameSalutation;
            txtPreferredName.Text = entity.PreferredName;
            txtGender.Text = entity.Sex;
            if (entity.IdentityNoType != null && entity.IdentityNoType != "" && entity.SSN != null && entity.SSN != "")
            {
                txtIdentityCardNo.Text = string.Format("({0}) {1}", entity.IdentityNoType, entity.SSN);
            }
            else if (entity.SSN != null && entity.SSN != "")
            {
                txtIdentityCardNo.Text = entity.SSN;
            }
            txtNHSRegistrationNo.Text = entity.NHSRegistrationNo;
            txtInhealthParticipantNo.Text = entity.InhealthParticipantNo;
            txtCorporateAccountNo.Text = entity.PatientCorporateAccountNo;
            hdnBPJSDiagnoseCode.Value = entity.KodeDiagnosa;
            txtNamaPeserta.Text = entity.NamaPeserta;
            txtJenisPeserta.Text = entity.JenisPeserta;
            txtKelas.Text = entity.NamaKelasTanggungan;
            txtNamaFaskes.Text = entity.NamaPPK;
            txtDinsos.Text = entity.Dinsos;
            txtNoSKTM.Text = entity.NoSKTM;
            txtPRB.Text = entity.ProlanisPRB;
            txtSubSpesialisCode.Text = entity.KodeSubSpesialis;
            txtSubSpesialisName.Text = entity.NamaSubSpesialis;
            if (AppSession.IsBridgingToBPJS != false)
            {
                if (entity.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    if (!string.IsNullOrEmpty(entity.NoSuratKontrolManual))
                    {
                        txtSurkonBPJS.Text = entity.NoSuratKontrolManual;
                        trIsKontrolBPJS.Attributes.Remove("style");
                        trSurkonBPJS.Attributes.Remove("style");
                        txtSurkonBPJS.Attributes.Add("readonly", "readonly");
                        chkIsKontrolBPJS.Checked = true;
                    }
                    else
                    {
                        trIsKontrolBPJS.Attributes.Add("style", "display:none");
                        trSurkonBPJS.Attributes.Add("style", "display:none");
                        txtSurkonBPJS.Attributes.Remove("readonly");
                        chkIsKontrolBPJS.Checked = false;
                    }
                }
            }
            if (entity.GCVisitReason == Constant.VisitReason.ACCIDENT)
            {
                trAccidentLocation1.Style.Clear();
                trAccidentLocation2.Style.Clear();
                trAccidentLocation3.Style.Clear();
                trAccidentLocation4.Style.Clear();
                txtAccidentLocation.Text = entity.AccidentLocation;
            }

            if (entity.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
            {
                txtDOB.Text = entity.DateOfBirthInString;
                txtAgeInYear.Text = entity.AgeInYear.ToString();
                txtAgeInMonth.Text = entity.AgeInMonth.ToString();
                txtAgeInDay.Text = entity.AgeInDay.ToString();
            }
            else
            {
                txtDOB.Text = "";
                txtAgeInYear.Text = "0";
                txtAgeInMonth.Text = "0";
                txtAgeInDay.Text = "0";
            }
            txtHandphoneNo.Text = entity.MobilePhoneNo1;
            txtAddress.Text = entity.HomeAddress;
            txtEmailAddress.Text = entity.EmailAddress;
            txtPatientNotes.Text = entity.PatientNotes;

            hdnParamedicID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;

            hdnToRegistrationID.Value = entity.LinkedToRegistrationID.ToString();
            txtToRegistrationNo.Text = entity.LinkedToRegistrationNo;
            chkIsParturition.Checked = entity.IsParturition;
            chkIsNeedPastoralCare.Checked = entity.IsNeedPastoralCare;
            chkIsFastTrack.Checked = entity.IsFastTrack;

            hdnChargeClassID.Value = entity.ChargeClassID.ToString();
            txtChargeClassCode.Text = entity.ChargeClassCode;
            txtChargeClassName.Text = entity.ChargeClassName;

            if (hdnIsUsingResultDeliveryPlan.Value == "1")
            {
                trResultDeliveryPlan.Attributes.Remove("style");
            }
            else
            {
                trResultDeliveryPlan.Attributes.Add("style", "display:none");
            }

            if (entity.GCResultDeliveryPlan != null && entity.GCResultDeliveryPlan != "")
            {
                cboResultDeliveryPlan.Value = entity.GCResultDeliveryPlan;
                if (entity.GCResultDeliveryPlan == Constant.ResultDeliveryPlan.OTHERS)
                {
                    txtResultDeliveryPlanOthers.Text = entity.ResultDeliveryPlanOthers;
                }
                else
                {
                    txtResultDeliveryPlanOthers.Text = "";
                }
            }
            else
            {
                cboResultDeliveryPlan.Value = "";
                txtResultDeliveryPlanOthers.Text = "";
            }

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                hdnClassID.Value = entity.ClassID.ToString();
                txtClassCode.Text = entity.ClassCode;
                txtClassName.Text = entity.ClassName;
                chkIsTemporaryLocation.Checked = entity.IsTemporaryLocation;
                txtClassRequestCode.Text = entity.RequestClassCode;
                txtClassRequestName.Text = entity.RequestClassName;
                hdnChargeClassBPJSCode.Value = entity.ChargeClassBPJSCode;
                hdnChargeClassBPJSType.Value = entity.ChargeClassBPJSType;
                hdnRoomID.Value = entity.RoomID.ToString();
                txtRoomCode.Text = entity.RoomCode;
                txtRoomName.Text = entity.RoomName;
                hdnBedID.Value = entity.BedID.ToString();
                txtBedCode.Text = entity.BedCode;
                cboAdmissionSource.Value = entity.GCAdmissionSource;
                hdnFromRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                txtFromRegistrationNo.Text = entity.LinkedRegistrationNo;
                chkIsNewBorn.Checked = entity.IsNewBorn;
                chkIsParturition.Checked = entity.IsParturition;
                chkIsPregnant.Checked = entity.IsPregnant;
                chkIsVisitorRestriction.Checked = entity.IsVisitorRestriction;
                chkIsNeedPastoralCare.Checked = entity.IsNeedPastoralCare;
                chkIsFastTrack.Checked = entity.IsFastTrack;

                if (entity.IsTemporaryLocation)
                {
                    trClassRequest.Attributes.Remove("style");
                }
                else
                {
                    trClassRequest.Attributes.Add("style", "display:none");
                }
            }
            else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                vRegistration2 registration = BusinessLayer.GetvRegistration2(string.Format("RegistrationDate < '{0}' AND MRN = {1} AND GCRegistrationStatus != '{2}'", entity.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT_112), entity.MRN, Constant.VisitStatus.CANCELLED), 0, "RegistrationDate DESC");
                if (registration != null)
                {
                    tdLastPayerName.InnerHtml = HttpUtility.HtmlEncode(registration.BusinessPartnerName);
                    tdLastRegistrationDate.InnerHtml = HttpUtility.HtmlEncode(registration.RegistrationDateInString);
                    tdLastServiceUnitParamedic.InnerHtml = registration.ServiceUnitName + "<br/>" + registration.ParamedicName;
                    trLastVisitData.Attributes.Remove("style");
                    trLastVisitDataEmpty.Attributes.Add("style", "display:none");

                    if (entity.GCCustomerType != Constant.CustomerType.PERSONAL)
                    {
                        string filterPaymentPersonal = string.Format("RegistrationID = '{0}' AND GCPaymentType = '{1}' AND GCTransactionStatus != '{2}' AND IsDeleted = 0",
                                                                        registration.RegistrationID, Constant.PaymentType.AR_PATIENT, Constant.TransactionStatus.VOID);
                        List<PatientPaymentHd> lstPaymentPersonalAR = BusinessLayer.GetPatientPaymentHdList(filterPaymentPersonal);

                        decimal payment = registration.PaymentAmount;
                        decimal totalLineAmount = (registration.ChargesAmount + registration.SourceAmount + registration.AdminAmount - registration.DiscountAmount + registration.RoundingAmount - registration.TransferAmount);
                        decimal coverage = entity.CoverageLimitAmount;
                        decimal payerCoverage = registration.ARInProcessAmount - lstPaymentPersonalAR.Sum(t => t.TotalPaymentAmount);
                        decimal PatientARAmount = lstPaymentPersonalAR.Sum(t => t.TotalPaymentAmount);
                        decimal patientCoverage = totalLineAmount - registration.ARInProcessAmount;
                        decimal remaining = registration.PaymentAmount - totalLineAmount;
                        //decimal remaining = totalLineAmount - (entity.ARInProcessAmount + (totalLineAmount - entity.ARInProcessAmount));

                        if (payment < totalLineAmount)
                        {
                            imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_warning.png");
                        }
                        else
                        {
                            imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_ok.png");
                        }

                        imgCoverage.Attributes.Add("title", String.Format("Coverage Amount = {0}\nPayer Amount = {1}\nPatient AR Amount = {2}\nPatient Amount = {3}\nTotal Amount = {4}\nRemaining Amount = {5}",
                                                            coverage.ToString(Constant.FormatString.NUMERIC_2),
                                                            payerCoverage.ToString(Constant.FormatString.NUMERIC_2),
                                                            PatientARAmount.ToString(Constant.FormatString.NUMERIC_2),
                                                            patientCoverage.ToString(Constant.FormatString.NUMERIC_2),
                                                            totalLineAmount.ToString(Constant.FormatString.NUMERIC_2),
                                                            remaining.ToString(Constant.FormatString.NUMERIC_2)));
                    }
                    else
                    {
                        decimal payment = registration.PaymentAmount;
                        decimal totalLineAmount = (registration.ChargesAmount + registration.SourceAmount + registration.AdminAmount - registration.DiscountAmount + registration.RoundingAmount - registration.TransferAmount);
                        decimal remaining = registration.PaymentAmount - totalLineAmount;

                        if (payment < totalLineAmount)
                        {
                            imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_warning.png");
                        }
                        else
                        {
                            imgCoverage.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "coverage_ok.png");
                        }

                        imgCoverage.Attributes.Add("title", String.Format("Payment Amount = {0}\nTotal Amount = {1}\nRemaining Amount = {2}",
                                                            payment.ToString(Constant.FormatString.NUMERIC_2),
                                                            totalLineAmount.ToString(Constant.FormatString.NUMERIC_2),
                                                            remaining.ToString(Constant.FormatString.NUMERIC_2)));
                    }

                }
                else
                {
                    trLastVisitDataEmpty.Attributes.Remove("style");
                    trLastVisitData.Attributes.Add("style", "display:none");
                }
                hdnClassID.Value = entity.ClassID.ToString();
                hdnRoomID.Value = entity.RoomID.ToString();
                txtRoomCode.Text = entity.RoomCode;
                txtRoomName.Text = entity.RoomName;
            }
            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                cboAdmissionCondition.Value = entity.GCAdmissionCondition;
                txtRoomCode.Text = entity.RoomCode;
                txtRoomName.Text = entity.RoomName;
            }
            //else if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
            //{
            //    vConsultVisitItemPackage chargesDt = BusinessLayer.GetvConsultVisitItemPackageList(string.Format("RegistrationID = {0}", entity.RegistrationID), 1, 1, "ID ASC").FirstOrDefault();
            //    if (chargesDt != null)
            //    {
            //        hdnItemID.Value = chargesDt.ItemID.ToString();
            //        txtItemCode.Text = chargesDt.ItemCode;
            //        txtItemName.Text = chargesDt.ItemName1;
            //    }
            //}

            List<vConsultVisitItemPackage> lstCVIP = BusinessLayer.GetvConsultVisitItemPackageList(string.Format("RegistrationID = {0}", entity.RegistrationID), 1, 1, "ID ASC");
            if (lstCVIP.Count() > 0)
            {
                vConsultVisitItemPackage cvip = lstCVIP.FirstOrDefault();
                if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
                {
                    hdnItemID.Value = cvip.ItemID.ToString();
                    txtItemCode.Text = cvip.ItemCode;
                    txtItemName.Text = cvip.ItemName1;
                }
                else
                {
                    hdnItemAIOID.Value = cvip.ItemID.ToString();
                    txtItemAIOCode.Text = cvip.ItemCode;
                    txtItemAIOName.Text = cvip.ItemName1;
                }
            }

            trBedQuickPicks.Style.Add("display", "none");

            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            cboSpecialty.Value = entity.SpecialtyID;
            hdnVisitTypeID.Value = entity.VisitTypeID.ToString();
            txtVisitTypeCode.Text = entity.VisitTypeCode;
            txtVisitTypeName.Text = entity.VisitTypeName;
            cboReferral.Value = entity.GCReferrerGroup;
            hdnReferrerID.Value = entity.ReferrerID.ToString();
            hdnReferrerParamedicID.Value = entity.ReferrerParamedicID.ToString();
            if (entity.ReferrerID != 0)
            {
                txtReferralDescriptionCode.Text = entity.ReferrerCommCode;
                txtReferralDescriptionName.Text = entity.ReferrerName;
            }
            else if (entity.ReferrerParamedicID != 0)
            {
                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(entity.ReferrerParamedicID);
                txtReferralDescriptionCode.Text = pm.ParamedicCode;
                txtReferralDescriptionName.Text = pm.FullName;
            }
            txtReferrerDate.Text = entity.ReferrerDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReferrerTime.Text = entity.ReferrerTime;
            cboVisitReason.Value = entity.GCVisitReason;
            txtVisitNotes.Text = entity.VisitReason;
            cboRegistrationPayer.Value = entity.GCCustomerType;
            if (!string.IsNullOrEmpty(txtNHSRegistrationNo.Text) && flagEditable && (cboRegistrationPayer.Value.ToString() == Constant.CustomerType.BPJS) && string.IsNullOrEmpty(txtNoSEP.Text))
                btnGenerateSEP.Attributes.Add("style", "display:block");
            else
                btnGenerateSEP.Attributes.Add("style", "display:none");
            hdnPayerID.Value = entity.BusinessPartnerID.ToString();
            txtPayerCompanyCode.Text = entity.BusinessPartnerCode;
            txtPayerCompanyName.Text = entity.BusinessPartnerName;
            hdnContractID.Value = entity.ContractID.ToString();
            txtContractNo.Text = entity.ContractNo;
            chkIsUsingCOB.Checked = entity.IsUsingCOB;
            hdnCoverageTypeID.Value = entity.CoverageTypeID.ToString();
            txtCoverageTypeCode.Text = entity.CoverageTypeCode;
            txtCoverageTypeName.Text = entity.CoverageTypeName;
            hdnEmployeeID.Value = entity.EmployeeID.ToString();
            txtEmployeeCode.Text = entity.EmployeeCode;
            txtEmployeeName.Text = entity.EmployeeName;

            txtParticipantNo.Text = entity.CorporateAccountNo;
            txtCoverageLimit.Text = entity.CoverageLimitAmount.ToString(Constant.FormatString.NUMERIC_2);
            chkIsCoverageLimitPerDay.Checked = entity.IsCoverageLimitPerDay;
            chkIsGuaranteeLetterExists.Checked = entity.IsGuaranteeLetterExists;

            Int32 ContractID = Convert.ToInt32(hdnContractID.Value);
            CustomerContract entityContract = BusinessLayer.GetCustomerContract(Convert.ToInt32(ContractID));
            if (entityContract != null)
            {
                txtContractPeriod.Text = entityContract.EndDate.ToString(Constant.FormatString.DATE_FORMAT);
            }

            if (!entity.IsControlCoverageLimit)
            {
                trCoverageLimit.Attributes.Add("style", "display:none");
                trCoverageLimitPerDay.Attributes.Add("style", "display:none");
            }
            else if (entity.DepartmentID != Constant.Facility.INPATIENT)
                trCoverageLimitPerDay.Attributes.Add("style", "display:none");

            if (!entity.IsControlClassCare)
                trControlClassCare.Attributes.Add("style", "display:none");
            else
                cboControlClassCare.Value = entity.ControlClassID.ToString();

            if (hdnIsBridgingToBPJS.Value == "1")
            {
                //Get BPJS Registration Information
                SetBPJSInformationControlDataBinding(entity.RegistrationID);
            }

            if (hdnIsUsedPatientOwnerStatus.Value == "1")
            {
                cboPatientOwnerStatus.Value = entity.GCPatientOwnerStatus;
            }

            if (entity.PromotionSchemeID != 0)
            {
                hdnPromotionID.Value = entity.PromotionSchemeID.ToString();
                txtPromotionCode.Text = entity.PromotionSchemeCode;
                txtPromotionName.Text = entity.PromotionSchemeName;
            }
            else
            {
                hdnPromotionID.Value = "";
                txtPromotionCode.Text = "";
                txtPromotionName.Text = "";
            }

            divRegistrationStatus.InnerHtml = entity.RegistrationStatus;
            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.IsAlive == false)
            {
                tdIsAlive.Style.Remove("display");
            }

            vPatient entityPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN)).FirstOrDefault();
            if (entityPatient != null)
            {
                if (entityPatient.IsVIP == true)
                {
                    tdIsVIP.Style.Remove("display");
                }
                txtPatientJob.Text = entityPatient.Occupation;
                txtPatientJobOffice.Text = entityPatient.Company;
            }
            txtPatientVisaNumber.Text = entity.PatientVisaNumber;
            //vPatient patientVIP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN)).FirstOrDefault();
            //if (patientVIP.IsVIP == false)
            //{
            //    tdIsVIP.Style.Remove("display");
            //}
            //else
            //{
            //    string VIPGroup = string.IsNullOrEmpty(patientVIP.VIPGroup) ? patientVIP.VIPGroup : patientVIP.VIPGroup;
            //    tdIsVIP.Attributes.Add("title", string.Format("{0}", VIPGroup));
            //}

            if (entity.IsUsingCOB)
            {
                imgCOB.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "cob.PNG");
            }
            else
            {
                imgCOB.Style.Add("display", "none");
            }
        }

        private void SetBPJSInformationControlDataBinding(int registrationID)
        {
            vRegistrationBPJS entityBPJS = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID = {0}", registrationID)).FirstOrDefault();
            if (entityBPJS != null)
            {
                hdnTglRujukan.Value = entityBPJS.TanggalRujukan.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                if (!string.IsNullOrEmpty(entityBPJS.AccidentPayer))
                {
                    chkBPJSAccidentPayer1.Checked = entityBPJS.AccidentPayer.Contains('1');
                    chkBPJSAccidentPayer2.Checked = entityBPJS.AccidentPayer.Contains('2');
                    chkBPJSAccidentPayer3.Checked = entityBPJS.AccidentPayer.Contains('3');
                    chkBPJSAccidentPayer4.Checked = entityBPJS.AccidentPayer.Contains('4');
                }

                if (entityBPJS.KecamatanID != 0)
                {
                    hdnKecamatanID.Value = entityBPJS.KecamatanID.ToString();
                    hdnKodeKecamatanBPJS.Value = entityBPJS.KodeKecamatanBPJS;
                    txtKodeKecamatan.Text = entityBPJS.KodeKecamatan;
                    txtNamaKecamatan.Text = entityBPJS.NamaKecamatan;
                }

                if (entityBPJS.KabupatenID != 0)
                {
                    hdnKabupatenID.Value = entityBPJS.KabupatenID.ToString();
                    hdnKodeKabupatenBPJS.Value = entityBPJS.KodeKabupatenBPJS;
                    txtKodeKabupaten.Text = entityBPJS.KodeKabupaten;
                    txtNamaKabupaten.Text = entityBPJS.NamaKabupaten;
                }

                if (!string.IsNullOrEmpty(entityBPJS.GCState))
                {
                    hdnGCState.Value = entityBPJS.GCState;
                    hdnKodePropinsiBPJS.Value = entityBPJS.KodePropinsi;
                    txtKodePropinsi.Text = entityBPJS.GCState.Substring(5, 3);
                    txtNamaPropinsi.Text = entityBPJS.NamaPropinsi;
                }

                if (entityBPJS.SEPBPJSClassType != "")
                {
                    txtSEPClass.Text = entityBPJS.SEPBPJSClassType + " - (" + entityBPJS.SEPBPJSClassName + ")";
                }

                hdnChargeClassSEP.Value = entityBPJS.KelasSEP.ToString();

                hdnNoSKDP.Value = string.IsNullOrEmpty(entityBPJS.NoRujukanKe) ? entityBPJS.NoSuratKontrol : entityBPJS.NoRujukanKe;
                hdnKodeDPJP.Value = string.IsNullOrEmpty(entityBPJS.KodeDPJPRujukan) ? entityBPJS.KodeDPJP : entityBPJS.KodeDPJPRujukan;
                chkIsSuplesi.Checked = entityBPJS.Suplesi;
                txtNoSEPSuplesi.Text = entityBPJS.NoSEPSuplesi;
                hdnIsCataract.Value = entityBPJS.IsCataract ? "1" : "0";
                txtSurkonBPJS.Text = entityBPJS.NoSuratKontrolManual;

                if (string.IsNullOrEmpty(hdnKodeDPJP.Value))
                {
                    string filterExp = string.Format("ParamedicID = {0}", hdnParamedicID.Value);
                    vParamedicMaster oParamedic = BusinessLayer.GetvParamedicMasterList(filterExp).FirstOrDefault();
                    if (oParamedic != null)
                    {
                        if (!string.IsNullOrEmpty(oParamedic.BPJSReferenceInfo))
                        {
                            string[] bpjsReference = oParamedic.BPJSReferenceInfo.Split(';');
                            string[] hfisInfo = bpjsReference[1].Split('|');
                            hdnKodeDPJP.Value = hfisInfo[0];
                        }
                    }
                }
            }
            else
            {
                hdnNoSKDP.Value = "";
                hdnKodeDPJP.Value = "";
                chkBPJSAccidentPayer1.Checked = false;
                chkBPJSAccidentPayer2.Checked = false;
                chkBPJSAccidentPayer3.Checked = false;
                chkBPJSAccidentPayer4.Checked = false;
                hdnKecamatanID.Value = "0";
                hdnKodeKecamatanBPJS.Value = "";
                txtKodeKecamatan.Text = "";
                txtNamaKecamatan.Text = "";
                hdnKabupatenID.Value = "0";
                hdnKodeKabupatenBPJS.Value = "";
                txtKodeKabupaten.Text = "";
                txtNamaKabupaten.Text = "";
                hdnGCState.Value = "0";
                hdnKodePropinsiBPJS.Value = "";
                txtKodePropinsi.Text = "";
                txtNamaPropinsi.Text = "";
            }
        }

        #endregion

        #region Save Entity
        private void ControlToEntity(Registration entity, RegistrationPayer entityRegistrationPayer, ConsultVisit entityVisit, PatientDiagnosis entityPatientDiagnosis, Guest entityGuest)
        {
            ControlToEntity(entity, entityRegistrationPayer, entityVisit, entityPatientDiagnosis);

            string DOB = Convert.ToString(entityGuest.DateOfBirth);
            //if (Convert.ToString(entityGuest.GuestID) != "0")
            if (hdnGuestID.Value.ToString() == "")
            {
                #region Patient Data
                //entityGuest.RegistrationID = entity.RegistrationID;
                entityGuest.GCSalutation = hdnGuestGCSalutation.Value;
                entityGuest.GCTitle = hdnGuestGCTitle.Value;
                entityGuest.FirstName = hdnGuestFirstName.Value;
                entityGuest.MiddleName = hdnGuestMiddleName.Value;
                entityGuest.LastName = hdnGuestLastName.Value;
                entityGuest.GCSuffix = hdnGuestGCSuffix.Value;
                entityGuest.GCGender = hdnGuestGCGender.Value;
                //entityGuest.DateOfBirth = Helper.GetDatePickerValue(hdnGuestDateOfBirth.Value);
                //entityGuest.DateOfBirth = Convert.ToDateTime(hdnGuestDateOfBirth.Value);
                DOB = hdnGuestDateOfBirth.Value;
                //entityGuest.GuestNo = txtGuestNo.Text;
                #endregion

                #region Patient Address
                entityGuest.StreetName = hdnGuestStreetName.Value;
                entityGuest.County = hdnGuestCounty.Value; // Desa
                entityGuest.District = hdnGuestDistrict.Value; //Kabupaten
                entityGuest.City = hdnGuestCity.Value;
                #endregion

                #region Patient Address Domicile
                entityGuest.StreetNameDomicile = hdnGuestStreetNameDomicile.Value;
                entityGuest.CountyDomicile = hdnGuestCountyDomicile.Value; // Desa
                entityGuest.DistrictDomicile = hdnGuestDistrictDomicile.Value; //Kabupaten
                entityGuest.CityDomicile = hdnGuestCityDomicile.Value;
                #endregion

                #region Patient Contact
                entityGuest.PhoneNo = hdnGuestPhoneNo.Value;
                entityGuest.MobilePhoneNo = hdnGuestMobilePhoneNo.Value;
                entityGuest.EmailAddress = hdnGuestEmailAddress.Value;
                entityGuest.SSN = hdnGuestSSN.Value;
                entityGuest.GCIdentityNoType = hdnGuestGCIdentityNoType.Value;
                #endregion

                entityGuest.Name = Helper.GenerateName(entityGuest.LastName, entityGuest.MiddleName, entityGuest.FirstName);
                entityGuest.FullName = Helper.GenerateFullName(entityGuest.Name, hdnGuestTitle.Value, hdnGuestSuffix.Value);

                if (txtParticipantNo.Text != null && txtParticipantNo.Text != "")
                {
                    entityGuest.CorporateAccountNo = txtParticipantNo.Text;
                }
                else
                {
                    entityGuest.CorporateAccountNo = Request.Form[txtCorporateAccountNo.UniqueID];
                }
            }
        }

        private void ControlToEntity(Registration entity, RegistrationPayer entityRegistrationPayer, ConsultVisit entityVisit, PatientDiagnosis entityPatientDiagnosis, PatientBirthRecord entityPatientBirth)
        {
            ControlToEntity(entity, entityRegistrationPayer, entityVisit, entityPatientDiagnosis);

            #region Patient Birth Data
            #region Data Bayi
            entityPatientBirth.MRN = Convert.ToInt32(hdnMRN.Value);
            #endregion
            #region Data Ibu
            entityPatientBirth.MotherMRN = Convert.ToInt32(hdnMotherMRN.Value);
            entityPatientBirth.MotherVisitID = Convert.ToInt32(hdnMotherVisitID.Value);
            #endregion
            #endregion

        }

        private void ControlToEntity(Registration entity, RegistrationPayer entityRegistrationPayer, ConsultVisit entityVisit, PatientDiagnosis entityPatientDiagnosis)
        {
            #region PatientDiagnosis
            ControlToEntityPatientDiagnosis(entityPatientDiagnosis);
            #endregion
            DateTime registrationDate = DateTime.Now;
            String registrationTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);


            if (hdnIsAllowBackDate.Value == "1")
            {
                if (Helper.GetDatePickerValue(txtRegistrationDate).Date <= registrationDate.Date)
                {
                    registrationDate = Helper.GetDatePickerValue(txtRegistrationDate);
                    registrationTime = txtRegistrationHour.Text;
                }
            }

            if (hdnIsAllowNextDate.Value == "1")
            {
                if (Helper.GetDatePickerValue(txtRegistrationDate).Date >= registrationDate.Date)
                {
                    registrationDate = Helper.GetDatePickerValue(txtRegistrationDate);
                    registrationTime = txtRegistrationHour.Text;
                }
            }

            entityVisit.VisitDate = entityVisit.ActualVisitDate = entity.RegistrationDate = entityPatientDiagnosis.DifferentialDate = registrationDate;
            entityVisit.VisitTime = entityVisit.ActualVisitTime = entity.RegistrationTime = entityPatientDiagnosis.DifferentialTime = registrationTime;

            entity.IsBackDate = (entity.RegistrationDate.Date != DateTime.Now.Date);
            if (cboReferral.Value != null)
            {
                entity.GCReferrerGroup = cboReferral.Value.ToString();
            }
            else entity.GCReferrerGroup = null;
            entity.ReferralNo = txtReferralNo.Text;
            entity.IsPregnant = chkIsPregnant.Checked;
            entity.IsParturition = chkIsParturition.Checked;
            entity.IsNeedPastoralCare = chkIsNeedPastoralCare.Checked;
            entity.IsFastTrack = chkIsFastTrack.Checked;
            entity.RegistrationTicketNo = txtNoTicket.Text;

            if (chkIsHasMRN.Checked)
            {
                entity.MRN = Convert.ToInt32(hdnMRN.Value);
            }
            else
            {
                entity.MRN = null;
            }
            entity.IsNewPatient = (hdnIsNewPatient.Value == "1");
            entity.AgeInYear = Convert.ToInt16(Request.Form[txtAgeInYear.UniqueID]);
            entity.AgeInMonth = Convert.ToInt16(Request.Form[txtAgeInMonth.UniqueID]);
            entity.AgeInDay = Convert.ToInt16(Request.Form[txtAgeInDay.UniqueID]);

            entityVisit.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entityVisit.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            if (cboSpecialty.Value != null && cboSpecialty.Value.ToString() != "")
                entityVisit.SpecialtyID = cboSpecialty.Value.ToString();
            else
                entityVisit.SpecialtyID = null;
            entityVisit.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);

            if (hdnReferrerID.Value != "" && hdnReferrerID.Value != "0")
            {
                entity.ReferrerID = Convert.ToInt32(hdnReferrerID.Value);
                entity.ReferrerParamedicID = null;
            }
            else if (hdnReferrerParamedicID.Value != "" && hdnReferrerParamedicID.Value != "0")
            {
                entity.ReferrerID = null;
                entity.ReferrerParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);
            }
            else
            {
                entity.ReferrerID = null;
                entity.ReferrerParamedicID = null;
            }

            entity.ReferrerDate = Helper.GetDatePickerValue(txtReferrerDate.Text);
            entity.ReferrerTime = txtReferrerTime.Text;

            entity.GCAdmissionSource = cboAdmissionSource.Value.ToString();

            if (cboResultDeliveryPlan.Value != null)
            {
                if (cboResultDeliveryPlan.Value.ToString() != "")
                {
                    entity.GCResultDeliveryPlan = cboResultDeliveryPlan.Value.ToString();
                    if (cboResultDeliveryPlan.Value.ToString() == Constant.ResultDeliveryPlan.OTHERS)
                    {
                        entity.ResultDeliveryPlanOthers = Request.Form[txtResultDeliveryPlanOthers.UniqueID];
                    }
                    else
                    {
                        entity.ResultDeliveryPlanOthers = null;
                    }
                }
                else
                {
                    entity.GCResultDeliveryPlan = null;
                    entity.ResultDeliveryPlanOthers = null;
                }
            }
            else
            {
                entity.GCResultDeliveryPlan = null;
                entity.ResultDeliveryPlanOthers = null;
            }

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                entityVisit.ClassID = Convert.ToInt32(hdnClassID.Value);
                entityVisit.IsTemporaryLocation = chkIsTemporaryLocation.Checked;

                if (!String.IsNullOrEmpty(hdnClassRequestID.Value))
                {
                    entityVisit.RequestClassID = Convert.ToInt32(hdnClassRequestID.Value);
                }

                entityVisit.RoomID = Convert.ToInt32(hdnRoomID.Value);
                entityVisit.BedID = Convert.ToInt32(hdnBedID.Value);
                if (entity.GCAdmissionSource == Constant.AdmissionSource.INPATIENT)
                    entity.LinkedRegistrationID = null;
                else
                {
                    //entity.IsNewPatient = (hdnFromRegistrationIsNewPatient.Value == "1");
                    //entity.IsNewPatient = false;
                    if (!String.IsNullOrEmpty(hdnFromRegistrationIsNewPatient.Value))
                    {
                        entity.LinkedRegistrationID = Convert.ToInt32(hdnFromRegistrationID.Value);
                    }
                }

                entity.IsNewBorn = chkIsNewBorn.Checked;
                entity.IsParturition = chkIsParturition.Checked;
                entity.IsNeedPastoralCare = chkIsNeedPastoralCare.Checked;
                entity.IsVisitorRestriction = chkIsVisitorRestriction.Checked;
                entity.IsFastTrack = chkIsFastTrack.Checked;
            }
            else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                entityVisit.ClassID = Convert.ToInt32(hdnClassID.Value);
                if (hdnRoomID.Value != "0" && hdnRoomID.Value != "")
                {
                    entityVisit.RoomID = Convert.ToInt32(hdnRoomID.Value);
                }
                else
                {
                    entityVisit.RoomID = null;
                }
                entityVisit.BedID = null;
                entity.LinkedRegistrationID = null;

                entity.IsNewBorn = false;
                entity.IsParturition = false;
                entity.IsNeedPastoralCare = false;
                entity.IsVisitorRestriction = false;
                entity.IsFastTrack = chkIsFastTrack.Checked;
            }
            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                entityVisit.ClassID = Convert.ToInt32(hdnClassID.Value);
                entity.LinkedRegistrationID = null;

                entity.IsNewBorn = false;
                entity.IsParturition = chkIsParturition.Checked;
                entity.IsNeedPastoralCare = false;
                entity.IsVisitorRestriction = false;
                entity.IsFastTrack = chkIsFastTrack.Checked;

                if (hdnIsEmergencyUsingRoom.Value == "1")
                {
                    if (hdnRoomID.Value != "" && hdnBedID.Value != "")
                    {
                        entityVisit.RoomID = Convert.ToInt32(hdnRoomID.Value);
                        entityVisit.BedID = Convert.ToInt32(hdnBedID.Value);
                    }
                }
            }
            else
            {
                entityVisit.ClassID = Convert.ToInt32(hdnClassID.Value);

                entityVisit.RoomID = null;
                entityVisit.BedID = null;
                entity.LinkedRegistrationID = null;

                entity.IsNewBorn = false;
                entity.IsParturition = chkIsParturition.Checked;
                entity.IsNeedPastoralCare = false;
                entity.IsVisitorRestriction = false;
                entity.IsFastTrack = chkIsFastTrack.Checked;
            }

            HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(entityVisit.HealthcareServiceUnitID); //pitix
            if (hdnDepartmentID.Value != Constant.Facility.INPATIENT && !hsu.IsChargeClassEditableForNonInpatient)
            {
                if (hsu.ChargeClassID != null && hsu.ChargeClassID != 0)
                {
                    entityVisit.ChargeClassID = hsu.ChargeClassID;
                }
                else
                {
                    entityVisit.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);
                }
            }
            else
            {
                if (hdnChargeClassID.Value != "" && hdnChargeClassID.Value != "0")
                {
                    entityVisit.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);
                }
                else
                {
                    if (hsu.ChargeClassID != null && hsu.ChargeClassID != 0)
                    {
                        entityVisit.ChargeClassID = hsu.ChargeClassID;
                    }
                }
            }

            if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                if (cboVisitReason.Value != null)
                {
                    entityVisit.GCVisitReason = cboVisitReason.Value.ToString();
                    if (entityVisit.GCVisitReason == Constant.VisitReason.OTHER)
                        entityVisit.VisitReason = Request.Form[txtVisitNotes.UniqueID];
                }
                else
                    entityVisit.VisitReason = txtVisitNotes.Text;
                if (cboAdmissionCondition.Value != null)
                    entityVisit.GCAdmissionCondition = cboAdmissionCondition.Value.ToString();
            }
            else
            {
                entityVisit.GCVisitReason = Constant.VisitReason.OTHER;
                entityVisit.VisitReason = txtVisitNotes.Text;
                entityVisit.GCAdmissionCondition = null;
            }

            entityVisit.IsReferralVisit = hdnIsReferralVisit.Value == "1" ? true : false;


            if (hdnToRegistrationID.Value != "" && hdnToRegistrationID.Value != "0")
            {
                entity.LinkedToRegistrationID = Convert.ToInt32(hdnToRegistrationID.Value);
            }

            entity.IsPrintingPatientCard = chkCardFee.Checked;
            entity.GCAdmissionType = Constant.AdmissionType.ROUTINE;

            entity.GCCustomerType = cboRegistrationPayer.Value.ToString();
            entityRegistrationPayer.GCCustomerType = cboRegistrationPayer.Value.ToString();
            //Personal => Business Partner ID 1
            entity.IsUsingCOB = chkIsUsingCOB.Checked;
            if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
            {
                entity.BusinessPartnerID = 1;
                entity.ContractID = null;
                entity.CoverageTypeID = null;
                entity.CoverageLimitAmount = 0;
                entity.IsCoverageLimitPerDay = false;
                entity.GCTariffScheme = hdnGCTariffSchemePersonal.Value;
                entity.IsControlClassCare = false;
                entity.ControlClassID = null;
                entity.EmployeeID = null;
                entity.CorporateAccountNo = Request.Form[txtCorporateAccountNo.UniqueID];
            }
            else
            {
                entity.BusinessPartnerID = Convert.ToInt32(hdnPayerID.Value);
                entity.ContractID = Convert.ToInt32(hdnContractID.Value);
                entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                if (txtParticipantNo.Text != null && txtParticipantNo.Text != "")
                {
                    entity.CorporateAccountNo = txtParticipantNo.Text;
                }
                else
                {
                    entity.CorporateAccountNo = Request.Form[txtCorporateAccountNo.UniqueID];
                }
                entity.CorporateAccountName = Request.Form[txtPatientName.UniqueID];
                entity.CoverageLimitAmount = Convert.ToDecimal(txtCoverageLimit.Text);
                entity.IsCoverageLimitPerDay = chkIsCoverageLimitPerDay.Checked;
                entity.GCTariffScheme = hdnGCTariffScheme.Value;
                entity.IsControlClassCare = (hdnIsControlClassCare.Value == "1");
                if (entity.IsControlClassCare)
                    entity.ControlClassID = Convert.ToInt32(cboControlClassCare.Value);
                else
                    entity.ControlClassID = null;
                if (hdnEmployeeID.Value == "" || hdnEmployeeID.Value == "0")
                    entity.EmployeeID = null;
                else
                    entity.EmployeeID = Convert.ToInt32(hdnEmployeeID.Value);

                entityRegistrationPayer.BusinessPartnerID = Convert.ToInt32(hdnPayerID.Value);
                entityRegistrationPayer.ContractID = Convert.ToInt32(hdnContractID.Value);
                entityRegistrationPayer.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                entityRegistrationPayer.IsGuaranteeLetterExists = chkIsGuaranteeLetterExists.Checked;
                if (txtParticipantNo.Text != null && txtParticipantNo.Text != "")
                {
                    entityRegistrationPayer.CorporateAccountNo = txtParticipantNo.Text;
                }
                else
                {
                    entityRegistrationPayer.CorporateAccountNo = Request.Form[txtCorporateAccountNo.UniqueID];
                }
                entityRegistrationPayer.CorporateAccountName = Request.Form[txtPatientName.UniqueID];
                entityRegistrationPayer.CoverageLimitAmount = Convert.ToDecimal(txtCoverageLimit.Text);
                entityRegistrationPayer.IsCoverageLimitPerDay = chkIsCoverageLimitPerDay.Checked;
                if (entity.IsControlClassCare)
                    entityRegistrationPayer.ControlClassID = Convert.ToInt32(cboControlClassCare.Value);
                else
                    entityRegistrationPayer.ControlClassID = null;
                entityRegistrationPayer.IsPrimaryPayer = true;

            }
            entity.PatientVisaNumber = txtPatientVisaNumber.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            ////if (hdnItemAIOID.Value != null && hdnItemAIOID.Value != "" && hdnItemAIOID.Value != "0" && cboRegistrationPayer.Value.ToString() != Constant.CustomerType.PERSONAL)
            ////{
            ////    errMessage = string.Format("Registrasi dengan pengisian paket all-in-one AIO hanya diperuntukan registrasi dengan penjamin bayar PERSONAL.");
            ////    return false;
            ////}

            if (Convert.ToString(cboRegistrationPayer.Value) == Constant.CustomerType.BPJS)
            {
                if (cboReferral.Value == null || cboReferral.Value.ToString() == "" && (hdnReferrerID.Value == "" || hdnReferrerID.Value == "0" || hdnReferrerID.Value == null || hdnReferrerParamedicID.Value == "" || hdnReferrerParamedicID.Value == "0" || hdnReferrerParamedicID.Value == null))
                {
                    errMessage = string.Format("Penjamin Bayar BPJS. Harap isi Rujukan beserta Deskripsi Rujukan terlebih dahulu.");
                    return false;
                }
            }

            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                if (chkIsHasMRN.Checked)
                {
                    if (hdnIsInpatientRegistrationBlockOpenRegistration.Value == "1")
                    {
                        int count = BusinessLayer.GetvRegistration1RowCount(string.Format("MRN = {0} AND DepartmentID = '{1}' AND GCRegistrationStatus NOT IN ('{2}','{3}')",
                                        hdnMRN.Value, Constant.Facility.INPATIENT, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED));
                        if (count > 0)
                        {
                            errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_PATIENT_OPENED_REGISTRATION_VALIDATION);
                            return false;
                        }
                    }
                    else
                    {
                        int count = BusinessLayer.GetvRegistration1RowCount(string.Format("MRN = {0} AND DepartmentID = '{1}' AND GCRegistrationStatus NOT IN ('{2}','{3}','{4}')",
                                        hdnMRN.Value, Constant.Facility.INPATIENT, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED));
                        if (count > 0)
                        {
                            errMessage = Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_PATIENT_OPENED_REGISTRATION_VALIDATION);
                            return false;
                        }
                    }
                }

                if (hdnChargeClassID.Value != "0" && hdnChargeClassID.Value == "1")
                {
                    ClassCare obj = BusinessLayer.GetClassCare(Convert.ToInt32(hdnChargeClassID.Value));
                    if (obj != null)
                    {
                        if (!obj.IsUsedInChargeClass)
                        {
                            errMessage = string.Format("Kelas Perawatan tidak boleh digunakan sebagai Kelas Tagihan.");
                            return false;
                        }
                    }
                    else
                    {
                        errMessage = string.Format("Kelas Tagihan tidak ditemukan dalam master kelas.");
                        return false;
                    }
                }

                if (chkIsNewBorn.Checked)
                {
                    if (hdnIsPatientNewBornMandatoryMotherRegistrationNo.Value == "1")
                    {
                        if (hdnMotherVisitID.Value == "" || hdnMotherVisitID.Value == "0" || hdnMotherVisitID.Value == null)
                        {
                            errMessage = string.Format("Harap isi No.Registrasi Ibu terlebih dahulu.");
                            return false;
                        }
                    }
                }
            }
            else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("MRN = {0} AND ParamedicID = {1} AND HealthcareServiceUnitID = {2} AND GCRegistrationStatus != '{3}' AND CONVERT(VARCHAR, CreatedDate, 112) = CONVERT(VARCHAR, GETDATE(), 112) ORDER BY RegistrationID DESC", hdnMRN.Value, hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value, Constant.VisitStatus.CANCELLED)).FirstOrDefault();
                if (entityReg != null)
                {
                    string[] timeSplit = txtRegistrationHour.Text.Split(':');
                    string oldHour = entityReg.CreatedDate.Hour.ToString();
                    string oldMinute = entityReg.CreatedDate.Minute.ToString();
                    string newHour = timeSplit[0];
                    string newMinute = timeSplit[1];

                    if (newHour == oldHour && oldMinute == newMinute)
                    {
                        errMessage = string.Format("Sudah ada registrasi untuk pasien <b>{0}</b>ke poli <b>{1}</b> dengan <b>{2}</b> di tanggal <b>{3}</b> dan jam ini", entityReg.PatientName, entityReg.ServiceUnitName, entityReg.ParamedicName, entityReg.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT));
                        return false;
                    }

                    //DateTime timeHourLater = entityReg.CreatedDate.AddHours(1);
                    //string[] timeSplit = txtRegistrationHour.Text.Split(':');
                    //int hour = Convert.ToInt32(timeSplit[0]);
                    //int minute = Convert.ToInt32(timeSplit[1]);
                    //TimeSpan timeSpan = new TimeSpan(hour, minute, 0);
                    //DateTime registrationTime = Helper.GetDatePickerValue(txtRegistrationDate.Text);
                    //registrationTime = registrationTime.Date + timeSpan;

                    //if (registrationTime.Ticks > entityReg.CreatedDate.Ticks && registrationTime.Ticks < timeHourLater.Ticks)
                    //{
                    //    errMessage = string.Format("Sudah ada registasi untuk pasien <b>{0}</b> ke poli <b>{1}</b> dengan <b>{2}</b>, pukul <b>{3}</b>", entityReg.PatientName, entityReg.ServiceUnitName, entityReg.ParamedicName, entityReg.RegistrationTime);
                    //    return false;
                    //}
                }

                #region validation paramedic schedule and max quota
                vParamedicSchedule obj = null;
                vParamedicScheduleDate objSchDate = null;
                if ((hdnParamedicID.Value != null && hdnParamedicID.Value != "" && hdnParamedicID.Value != "0") && ((hdnHealthcareServiceUnitID.Value != null && hdnHealthcareServiceUnitID.Value != "" && hdnHealthcareServiceUnitID.Value != "0")))
                {
                    int dayNumber = (int)DateTime.Now.DayOfWeek;
                    if (dayNumber == 0)
                    {
                        dayNumber = 7;
                    }
                    obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                                        hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value, dayNumber)).FirstOrDefault();

                    objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                            hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value, DateTime.Now)).FirstOrDefault();
                }

                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                {
                    if (hdnScheduleValidationBeforeRegistration.Value == "1")
                    {
                        if (obj == null && objSchDate == null)
                        {
                            if (hdnIsCheckScheduleBeforeRegistrationUsingConfirmation.Value == "1")
                            {
                                errMessage = "confirmation|Pendaftaran Gagal. Tidak ada jadwal dokter. Melanjutkan proses registrasi?";
                            }
                            else
                            {
                                errMessage = "Pendaftaran Gagal. Tidak ada jadwal dokter";
                            }
                            return false;
                        }
                    }
                    return true;
                }
                #endregion
            }

            return true;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            int newQueue = 0;

            if (hdnIsBridgingToGateway.Value == "1")
            {
                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                {
                    //Healthcare entityHSU = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                    if (Constant.HealthcareGatewayProvider.RSDOSOBA == hdnProviderGatewayService.Value)
                    {
                        int session = BusinessLayer.GetRegistrationSession(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Helper.GetDatePickerValue(txtRegistrationDate), txtRegistrationHour.Text);
                        string queue = BridgingToGatewayGetQueueNo(txtMRN.Text, txtPhysicianCode.Text, cboRegistrationPayer.Value.ToString(), Helper.GetDatePickerValue(txtRegistrationDate), txtRegistrationHour.Text, "DT", hdnHealthcareServiceUnitID.Value.ToString(), session);
                        string[] queueSplit = queue.Split('|');
                        if (queueSplit[0] == "1")
                        {
                            newQueue = Convert.ToInt16(queueSplit[1]);
                        }
                        else
                        {
                            errMessage = queueSplit[1];
                            result = false;
                        }
                    }
                }
            }

            if (result)
            {
                IDbContext ctx = DbFactory.Configure(true);
                RegistrationDao entityDao = new RegistrationDao(ctx);
                PatientDao entityPatientDao = new PatientDao(ctx);
                GuestDao entityGuestDao = new GuestDao(ctx);
                ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);
                BedReservationDao entityReservationDao = new BedReservationDao(ctx);
                AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
                AppointmentRequestDao entityApmReqDao = new AppointmentRequestDao(ctx);
                BedDao entityBedDao = new BedDao(ctx);
                SettingParameterDao entitySettingParameterDao = new SettingParameterDao(ctx);
                SettingParameterDtDao entitySettingParameterDtDao = new SettingParameterDtDao(ctx);
                EmergencyContactDao emergencyContactDao = new EmergencyContactDao(ctx);
                PatientDiagnosisDao entityPatientDiagnosisDao = new PatientDiagnosisDao(ctx);
                RegistrationPayerDao entityRegistrationPayerDao = new RegistrationPayerDao(ctx);
                RegistrationBPJSDao entityRegistrationBPJSDao = new RegistrationBPJSDao(ctx);
                PatientBirthRecordDao entityPatientBirthRecordDao = new PatientBirthRecordDao(ctx);
                ParamedicTeamDao entityParamedicTeamDao = new ParamedicTeamDao(ctx);
                PatientFamilyDao entityPatientFamilyDao = new PatientFamilyDao(ctx);
                ServiceUnitParamedicDao entityServiceUnitParamedicDao = new ServiceUnitParamedicDao(ctx);
                ChiefComplaintDao entityChiefComplaintDao = new ChiefComplaintDao(ctx);
                RegistrationInhealthDao entityRegistrationInhealthDao = new RegistrationInhealthDao(ctx);
                PatientReferralDao entityPatientReferralDao = new PatientReferralDao(ctx);

                GeneralConsentDao generalConsentDao = new GeneralConsentDao(ctx);
                DiagnosticVisitScheduleDao diagVisitScheduleDao = new DiagnosticVisitScheduleDao(ctx);

                try
                {
                    Registration entity = new Registration();
                    ConsultVisit entityVisit = new ConsultVisit();
                    RegistrationPayer entityRegistrationPayer = new RegistrationPayer();

                    PatientDiagnosis entityPatientDiagnosis = new PatientDiagnosis();
                    PatientBirthRecord entityPatientBirth = null;

                    Patient entityPatient = null;
                    Guest entityGuest = new Guest();

                    RegistrationBPJS entityRegistrationBPJS = null;
                    RegistrationInhealth entityRegistrationInhealth = null;


                    if (hdnBedID.Value != null && hdnBedID.Value != "" && hdnBedID.Value != "0")
                    {
                        Bed entityBed = entityBedDao.Get(Convert.ToInt32(hdnBedID.Value));
                        if (entityBed.GCBedStatus == Constant.BedStatus.UNOCCUPIED)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = true;
                    }

                    vParamedicSchedule obj = null;
                    vParamedicScheduleDate objSchDate = null;
                    if ((hdnParamedicID.Value != null && hdnParamedicID.Value != "" && hdnParamedicID.Value != "0") && ((hdnHealthcareServiceUnitID.Value != null && hdnHealthcareServiceUnitID.Value != "" && hdnHealthcareServiceUnitID.Value != "0")))
                    {
                        int dayNumber = (int)DateTime.Now.DayOfWeek;
                        if (dayNumber == 0)
                        {
                            dayNumber = 7;
                        }
                        obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                                            hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value, dayNumber), ctx).FirstOrDefault();

                        objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                                "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                                hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value, DateTime.Now), ctx).FirstOrDefault();
                    }

                    if (result)
                    {
                        if (chkIsHasMRN.Checked)
                        {
                            if (hdnMotherMRN.Value != string.Empty)
                            {
                                PatientBirthRecord entityBirthRecord = BusinessLayer.GetPatientBirthRecordList(string.Format("MotherMRN = {0} AND MRN = {1} AND IsDeleted = 0", hdnMotherMRN.Value, hdnMRN.Value), ctx).FirstOrDefault();
                                if (entityBirthRecord != null)
                                {
                                    entityPatientBirth = entityPatientBirthRecordDao.Get(entityBirthRecord.BirthRecordID);
                                    ControlToEntity(entity, entityRegistrationPayer, entityVisit, entityPatientDiagnosis, entityPatientBirth);
                                }
                                else
                                {
                                    entityPatientBirth = new PatientBirthRecord();
                                    ControlToEntity(entity, entityRegistrationPayer, entityVisit, entityPatientDiagnosis, entityPatientBirth);
                                }
                            }
                            else ControlToEntity(entity, entityRegistrationPayer, entityVisit, entityPatientDiagnosis);
                        }
                        else
                        {
                            //entityGuest = new Guest();
                            ControlToEntity(entity, entityRegistrationPayer, entityVisit, entityPatientDiagnosis, entityGuest);
                        }
                        switch (hdnDepartmentID.Value)
                        {
                            case Constant.Facility.MEDICAL_CHECKUP: entity.TransactionCode = Constant.TransactionCode.MCU_REGISTRATION; break;
                            case Constant.Facility.EMERGENCY: entity.TransactionCode = Constant.TransactionCode.ER_REGISTRATION; break;
                            case Constant.Facility.INPATIENT: entity.TransactionCode = Constant.TransactionCode.IP_REGISTRATION; break;
                            case Constant.Facility.DIAGNOSTIC:
                                if (hdnIsRadioteraphy.Value != "1")
                                {
                                    if (hdnDiagnosticType.Value == "0")
                                        entity.TransactionCode = Constant.TransactionCode.LABORATORY_REGISTRATION;
                                    else if (hdnDiagnosticType.Value == "1")
                                        entity.TransactionCode = Constant.TransactionCode.IMAGING_REGISTRATION;
                                    else
                                        entity.TransactionCode = Constant.TransactionCode.OTHER_MEDICAL_DIAGNOSTIC_REGISTRATION;
                                    break;
                                }
                                else
                                {
                                    entity.TransactionCode = Constant.TransactionCode.RADIOTHERAPHY_REGISTRATION;
                                    break;
                                }
                            case Constant.Facility.PHARMACY: entity.TransactionCode = Constant.TransactionCode.PH_REGISTRATION; break;
                            default: entity.TransactionCode = Constant.TransactionCode.OP_REGISTRATION; break;
                        }

                        string registrationStatus = Constant.VisitStatus.CHECKED_IN;
                        if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                        {
                            if (entitySettingParameterDtDao.Get(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IP0012).ParameterValue == "1")
                            {
                                registrationStatus = Constant.VisitStatus.CHECKED_IN;

                                entityVisit.StartServiceDate = entityVisit.ActualVisitDate;
                                entityVisit.StartServiceTime = entityVisit.ActualVisitTime;
                            }
                            else
                            {
                                registrationStatus = Constant.VisitStatus.OPEN;
                            }
                        }
                        else if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP || hdnDepartmentID.Value == Constant.Facility.EMERGENCY || hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC || hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                        {
                            registrationStatus = Constant.VisitStatus.CHECKED_IN;
                        }
                        else
                        {
                            if (entitySettingParameterDao.Get(Constant.SettingParameter.IS_OUTPATIENT_REGISTRATION_AUTOMATICALLY_CHECKED_IN).ParameterValue == "1")
                                registrationStatus = Constant.VisitStatus.CHECKED_IN;
                            else
                                registrationStatus = Constant.VisitStatus.OPEN;
                        }

                        BedReservation entityBedReservation = null;
                        if (hdnReservationID.Value != "")
                        {
                            entityBedReservation = entityReservationDao.Get(Convert.ToInt32(hdnReservationID.Value));
                            entityBedReservation.GCReservationStatus = Constant.Bed_Reservation_Status.COMPLETE;
                            entityBedReservation.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityReservationDao.Update(entityBedReservation);
                            entity.ReservationID = entityBedReservation.ReservationID;
                        }
                        else
                        {
                            entity.ReservationID = null;
                        }

                        Appointment entityAppointment = null;
                        Guest entityGuest1 = null;
                        if (hdnAppointmentID.Value != "")
                        {
                            entityAppointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                            entityAppointment.GCAppointmentStatus = Constant.AppointmentStatus.COMPLETE;
                            entityAppointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityAppointmentDao.Update(entityAppointment);
                            entity.AppointmentID = entityAppointment.AppointmentID;

                            if (entityAppointment.GuestID != null)
                            {
                                entityGuest1 = entityGuestDao.Get(Convert.ToInt32(entityAppointment.GuestID));
                                entityGuest1.IsDeleted = true;
                                entityGuest1.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityGuestDao.Update(entityGuest1);
                            }
                        }
                        else
                        {
                            entity.AppointmentID = null;
                        }

                        entity.RegistrationNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entityVisit.ActualVisitDate, ctx);
                        entity.GCRegistrationStatus = registrationStatus;
                        entity.GCMedicalFileStatus = null;
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        if (chkIsHasMRN.Checked)
                        {
                            entityPatient = entityPatientDao.Get((int)entity.MRN);

                            entityPatient.LastVisitDate = Helper.GetDatePickerValue(txtRegistrationDate);
                            entityPatient.LastBusinessPartnerID = entity.BusinessPartnerID;
                            entityPatient.Notes = txtPatientNotes.Text;
                            entity.GCDependentType = entityPatient.GCDependentType;
                            entity.GCPatientCategory = entityPatient.GCPatientCategory;

                            //if (entity.GCCustomerType != Constant.CustomerType.PERSONAL)
                            //{
                            //    entityPatient.CorporateAccountNo = entity.CorporateAccountNo;
                            //}
                        }

                        if (cboRegistrationPayer.Value.ToString() == Constant.CustomerType.BPJS)
                        {
                            entity.IsBPJS = true;

                            if (entity.ReferrerID == null || entity.ReferrerID == 0)
                            {
                                if (txtReferralDescriptionCode.Text != "")
                                {
                                    string filterRefferer = string.Format("BusinessPartnerCode = '{0}'", txtReferralDescriptionCode.Text);
                                    BusinessPartners entityRefferer = BusinessLayer.GetBusinessPartnersList(filterRefferer, ctx).FirstOrDefault();
                                    if (entityRefferer != null)
                                    {
                                        entity.ReferrerID = entityRefferer.BusinessPartnerID;
                                    }
                                }
                            }
                        }

                        if (cboRegistrationPayer.Value.ToString() == Constant.CustomerType.BPJS)
                        {
                            entity.IsBPJS = true;
                        }

                        if (hdnDepartmentID.Value != Constant.Facility.MEDICAL_CHECKUP && hdnItemAIOID.Value != "" && hdnItemAIOID.Value != "0")
                        {
                            entity.IsHasAIOPackage = true;
                        }
                        else
                        {
                            entity.IsHasAIOPackage = false;
                        }

                        if (cboRegistrationPayer.Value.ToString() == Constant.CustomerType.PERSONAL)
                        {
                            if (hdnPromotionID.Value != "")
                            {
                                entity.PromotionSchemeID = Convert.ToInt32(hdnPromotionID.Value);
                            }
                            else
                            {
                                entity.PromotionSchemeID = null;
                            }
                        }

                        entity.RegistrationID = entityDao.InsertReturnPrimaryKeyID(entity);

                        if (hdnAppointmentID.Value != "")
                        {
                            AppointmentRequest entityApmReq = BusinessLayer.GetAppointmentRequestList(string.Format("AppointmentID = {0}", entityAppointment.AppointmentID), ctx).FirstOrDefault();

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            if (entityApmReq != null)
                            {
                                entityApmReq.RegistrationID = entity.RegistrationID;
                                entityApmReq.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityApmReq.LastUpdatedDate = DateTime.Now;
                                entityApmReqDao.Update(entityApmReq);

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                            }
                        }

                        #region General Consent Form
                        GeneralConsent objGeneralConsent = new GeneralConsent();
                        objGeneralConsent.RegistrationID = entity.RegistrationID;
                        objGeneralConsent.ConsentDate = Helper.GetDatePickerValue(txtRegistrationDate);
                        objGeneralConsent.ConsentTime = txtRegistrationHour.Text;
                        objGeneralConsent.Signature1Name = string.Empty;
                        objGeneralConsent.Signature2Name = string.Empty;
                        objGeneralConsent.LastUpdatedBy = 0;
                        generalConsentDao.Insert(objGeneralConsent);
                        #endregion

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        if (cboRegistrationPayer.Value.ToString() == Constant.CustomerType.BPJS)
                        {
                            entityRegistrationBPJS = new RegistrationBPJS();

                            string filterBPJSClass = string.Format("ClassID = {0}", entityVisit.ChargeClassID);
                            ClassCare bpjsClass = BusinessLayer.GetClassCareList(filterBPJSClass, ctx).FirstOrDefault();

                            ControlToEntity(entityRegistrationBPJS);

                            //int kelasSEP = Convert.ToInt16(entityRegistrationBPJS.KelasTanggungan);
                            if (bpjsClass.BPJSClassType != null && bpjsClass.BPJSClassType != "")
                            {
                                int kelasSEP = Convert.ToInt16(bpjsClass.BPJSClassType);
                                if (entityRegistrationBPJS.JenisPelayanan == "1")
                                {
                                    if (!string.IsNullOrEmpty(hdnChargeClassBPJSType.Value))
                                    {
                                        if (Convert.ToInt16(hdnChargeClassBPJSType.Value) >= kelasSEP)
                                            entityRegistrationBPJS.KelasSEP = Convert.ToInt16(hdnChargeClassBPJSType.Value);
                                        else
                                            entityRegistrationBPJS.KelasSEP = kelasSEP;
                                    }

                                }
                                else
                                {
                                    entityRegistrationBPJS.KelasSEP = kelasSEP;
                                }
                            }
                            else
                            {
                                entityRegistrationBPJS.KelasSEP = 1;
                            }

                            if (string.IsNullOrEmpty(entityRegistrationBPJS.NoSuratKontrol))
                            {
                                entityRegistrationBPJS.NoSuratKontrol = BusinessLayer.GenerateNoSuratKontrolBPJS(Helper.GetDatePickerValue(txtRegistrationDate), ctx);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                            }
                            entityRegistrationBPJS.RegistrationID = entity.RegistrationID;
                            entityRegistrationBPJS.NoPeserta = txtParticipantNo.Text;
                            entityRegistrationBPJS.CreatedBy = AppSession.UserLogin.UserID;
                            entityRegistrationBPJSDao.Insert(entityRegistrationBPJS);
                        }
                        if (hdnInhealthCustomerType.Value == cboRegistrationPayer.Value.ToString())
                        {
                            entityRegistrationInhealth = new RegistrationInhealth();

                            entityRegistrationInhealth.RegistrationID = entity.RegistrationID;
                            entityRegistrationInhealth.NoRM = txtMRN.Text;
                            entityRegistrationInhealth.NoKartuPeserta = txtInhealthParticipantNo.Text;
                            entityRegistrationInhealth.KodeDiagnosa = txtDiagnoseCode.Text;
                            entityRegistrationInhealth.NamaDiagnosa = txtDiagnoseText.Text;
                            entityRegistrationInhealth.TanggalSJP = DateTime.Now;
                            entityRegistrationInhealth.PraRegistrationNumber = !string.IsNullOrEmpty(hdnInhealthPraRegistrationNo.Value) ? hdnInhealthPraRegistrationNo.Value : string.Empty;

                            entityRegistrationInhealth.CreatedBy = AppSession.UserLogin.UserID;
                            entityRegistrationInhealth.CreatedDate = DateTime.Now;
                            entityRegistrationInhealthDao.Insert(entityRegistrationInhealth);
                        }
                        if (entityRegistrationPayer.GCCustomerType != Constant.CustomerType.PERSONAL)
                        {
                            entityRegistrationPayer.CreatedBy = AppSession.UserLogin.UserID;
                            entityRegistrationPayer.RegistrationID = entity.RegistrationID;
                            entityRegistrationPayerDao.Insert(entityRegistrationPayer);
                        }
                        if (entity.LinkedRegistrationID != null && entity.GCAdmissionSource == Constant.AdmissionSource.EMERGENCY)
                        {
                            EmergencyContact emergencyContact = emergencyContactDao.Get((int)entity.LinkedRegistrationID);
                            if (emergencyContact != null)
                            {
                                emergencyContact.RegistrationID = entity.RegistrationID;
                                emergencyContact.LastUpdatedBy = AppSession.UserLogin.UserID;
                                emergencyContactDao.Update(emergencyContact);
                            }
                        }

                        entityVisit.RegistrationID = entity.RegistrationID;
                        entityVisit.GCVisitStatus = registrationStatus;
                        entityVisit.CreatedBy = AppSession.UserLogin.UserID;
                        entityVisit.IsMainVisit = true;
                        bool flagQueueNoGenerated = false;
                        Appointment appointment = null;
                        int apmID = 0;

                        if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT && hdnIsQueueNoUsingAppointment.Value == "1")
                        {
                            if (entityAppointment != null)
                            {
                                entityVisit.Session = entityAppointment.Session;
                                entityVisit.QueueNo = entityAppointment.QueueNo;

                                if (hdnIsUsedReferenceQueueNo.Value == "1")
                                {
                                    entityVisit.ReferenceQueueNo = entityAppointment.ReferenceQueueNo;
                                }
                            }
                            else
                            {
                                if (entityVisit.ParamedicID != null)
                                {
                                    entityVisit.Session = BusinessLayer.GetRegistrationSession(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, entityVisit.VisitTime, ctx);
                                    if (newQueue == 0)
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entityVisit.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, Convert.ToInt32(entityVisit.Session), false, isBPJS, 0, ctx, 1));
                                        //entityVisit.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, Convert.ToInt32(entityVisit.Session), 0, ctx));
                                    }
                                    else
                                    {
                                        entityVisit.QueueNo = Convert.ToInt16(newQueue);
                                    }
                                    if (hdnIsUsedReferenceQueueNo.Value == "1")
                                    {
                                        entityVisit.ReferenceQueueNo = Convert.ToInt16(BusinessLayer.GenerateReferenceQueueNo(entityVisit.VisitDate, entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entity.GCCustomerType, ctx, 1));
                                    }
                                }

                                appointment = new Appointment();

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();


                                ValidateParamedicScSchedule(obj, objSchDate, appointment.StartDate);

                                int visitDuration = 0;
                                ParamedicVisitType entitySUVT = BusinessLayer.GetParamedicVisitTypeList(string.Format("VisitTypeID = {0} AND HealthcareServiceUnitID = {1} AND ParamedicID = {2}", hdnVisitTypeID.Value, hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value), ctx).FirstOrDefault();

                                if (entitySUVT != null)
                                {
                                    visitDuration = entitySUVT.VisitDuration;
                                }
                                else
                                {
                                    visitDuration = 10;
                                }

                                if (objSchDate != null)
                                {
                                    //OperationalTime entityOT = BusinessLayer.GetOperationalTimeList(string.Format("OperationalTimeID = {0}", objSchDate.OperationalTimeID), ctx).FirstOrDefault();
                                    Appointment entityApmCheck = BusinessLayer.GetAppointmentList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(VARCHAR, StartDate, 112) = '{2}' AND Session = {3} AND GCAppointmentStatus NOT IN ('{4}','{5}')", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), entityVisit.Session, Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED), ctx).LastOrDefault();

                                    if (entityApmCheck != null)
                                    {
                                        appointment.StartTime = entityApmCheck.EndTime;
                                    }
                                    else
                                    {
                                        if (entityVisit.Session == 5)
                                        {
                                            appointment.StartTime = objSchDate.StartTime5;
                                        }
                                        else if (entityVisit.Session == 4)
                                        {
                                            appointment.StartTime = objSchDate.StartTime4;
                                        }
                                        else if (entityVisit.Session == 3)
                                        {
                                            appointment.StartTime = objSchDate.StartTime3;
                                        }
                                        else if (entityVisit.Session == 2)
                                        {
                                            appointment.StartTime = objSchDate.StartTime2;
                                        }
                                        else
                                        {
                                            appointment.StartTime = objSchDate.StartTime1;
                                        }
                                    }

                                    string[] newTime = appointment.StartTime.Split(':');
                                    int hour = Convert.ToInt32(newTime[0]);
                                    int minute = Convert.ToInt32(newTime[1]);
                                    DateTime newDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0);
                                    newDate = newDate.AddMinutes(visitDuration);
                                    string hourInString = newDate.Hour.ToString();
                                    string minuteInString = newDate.Minute.ToString();
                                    if (newDate.Hour < 10)
                                    {
                                        hourInString = string.Format("0{0}", newDate.Hour);
                                    }

                                    if (newDate.Minute < 10)
                                    {
                                        minuteInString = string.Format("0{0}", newDate.Minute);
                                    }
                                    appointment.EndTime = string.Format("{0}:{1}", hourInString, minuteInString);

                                    appointment.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                                    appointment.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                                    appointment.ParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                                    appointment.StartDate = appointment.EndDate = entity.RegistrationDate;
                                    appointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(appointment.TransactionCode, appointment.StartDate);
                                    appointment.Name = entity.RegistrationNo;
                                    appointment.MRN = entity.MRN;
                                    appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                    appointment.IsAutoAppointment = true;
                                    appointment.Notes = string.Format("Registrasi Langsung Dengan No {0}", entity.RegistrationNo);
                                    appointment.Session = entityVisit.Session;
                                    appointment.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
                                    appointment.GCAppointmentMethod = Constant.AppointmentMethod.GOSHOW;
                                    appointment.GCAppointmentStatus = Constant.AppointmentStatus.COMPLETE;
                                    appointment.GCCustomerType = entity.GCCustomerType;
                                    appointment.BusinessPartnerID = entity.BusinessPartnerID;
                                    appointment.ContractID = entity.ContractID;
                                    appointment.ControlClassID = entity.ControlClassID;
                                    appointment.IsControlClassCare = entity.IsControlClassCare;
                                    appointment.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;
                                    appointment.CorporateAccountName = entity.CorporateAccountName;
                                    appointment.CorporateAccountNo = entity.CorporateAccountNo;
                                    appointment.CoverageLimitAmount = entity.CoverageLimitAmount;
                                    appointment.CoverageTypeID = entity.CoverageTypeID;
                                    appointment.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;

                                    if (cboResultDeliveryPlan.Value != null)
                                    {
                                        if (cboResultDeliveryPlan.Value.ToString() != "")
                                        {
                                            appointment.GCResultDeliveryPlan = cboResultDeliveryPlan.Value.ToString();
                                            if (cboResultDeliveryPlan.Value.ToString() == Constant.ResultDeliveryPlan.OTHERS)
                                            {
                                                appointment.ResultDeliveryPlanOthers = Request.Form[txtResultDeliveryPlanOthers.UniqueID];
                                            }
                                            else
                                            {
                                                appointment.ResultDeliveryPlanOthers = null;
                                            }
                                        }
                                        else
                                        {
                                            appointment.GCResultDeliveryPlan = null;
                                            appointment.ResultDeliveryPlanOthers = null;
                                        }
                                    }
                                    else
                                    {
                                        appointment.GCResultDeliveryPlan = null;
                                        appointment.ResultDeliveryPlanOthers = null;
                                    }

                                    appointment.QueueNo = entityVisit.QueueNo;
                                    if (hdnIsUsedReferenceQueueNo.Value == "1")
                                    {
                                        appointment.ReferenceQueueNo = entityVisit.ReferenceQueueNo;
                                    }
                                    appointment.CreatedBy = AppSession.UserLogin.UserID;
                                    appointment.CreatedDate = DateTime.Now;
                                    apmID = entityAppointmentDao.InsertReturnPrimaryKeyID(appointment);
                                }
                                else if (objSchDate == null && obj != null)
                                {
                                    Appointment entityApmCheck = BusinessLayer.GetAppointmentList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(VARCHAR, StartDate, 112) = '{2}' AND Session = {3}", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), entityVisit.Session), ctx).LastOrDefault();

                                    if (entityApmCheck != null)
                                    {
                                        appointment.StartTime = entityApmCheck.EndTime;
                                    }
                                    else
                                    {
                                        if (entityVisit.Session == 5)
                                        {
                                            appointment.StartTime = obj.StartTime5;
                                        }
                                        else if (entityVisit.Session == 4)
                                        {
                                            appointment.StartTime = obj.StartTime4;
                                        }
                                        else if (entityVisit.Session == 3)
                                        {
                                            appointment.StartTime = obj.StartTime3;
                                        }
                                        else if (entityVisit.Session == 2)
                                        {
                                            appointment.StartTime = obj.StartTime2;
                                        }
                                        else
                                        {
                                            appointment.StartTime = obj.StartTime1;
                                        }
                                    }

                                    string[] newTime = appointment.StartTime.Split(':');
                                    int hour = Convert.ToInt32(newTime[0]);
                                    int minute = Convert.ToInt32(newTime[1]);
                                    DateTime newDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0);
                                    newDate = newDate.AddMinutes(visitDuration);
                                    string hourInString = newDate.Hour.ToString();
                                    string minuteInString = newDate.Minute.ToString();
                                    if (newDate.Hour < 10)
                                    {
                                        hourInString = string.Format("0{0}", newDate.Hour);
                                    }

                                    if (newDate.Minute < 10)
                                    {
                                        minuteInString = string.Format("0{0}", newDate.Minute);
                                    }
                                    appointment.EndTime = string.Format("{0}:{1}", hourInString, minuteInString);

                                    appointment.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                                    appointment.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                                    appointment.ParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                                    appointment.StartDate = appointment.EndDate = entity.RegistrationDate;
                                    appointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(appointment.TransactionCode, appointment.StartDate);
                                    appointment.Name = entity.RegistrationNo;
                                    appointment.MRN = entity.MRN;
                                    appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                    appointment.IsAutoAppointment = true;
                                    appointment.Notes = string.Format("Registrasi Langsung Dengan No {0}", entity.RegistrationNo);
                                    appointment.Session = entityVisit.Session;
                                    appointment.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
                                    appointment.GCAppointmentMethod = Constant.AppointmentMethod.GOSHOW;
                                    appointment.GCAppointmentStatus = Constant.AppointmentStatus.COMPLETE;
                                    appointment.GCCustomerType = entity.GCCustomerType;
                                    appointment.BusinessPartnerID = entity.BusinessPartnerID;
                                    appointment.ContractID = entity.ContractID;
                                    appointment.ControlClassID = entity.ControlClassID;
                                    appointment.IsControlClassCare = entity.IsControlClassCare;
                                    appointment.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;
                                    appointment.CorporateAccountName = entity.CorporateAccountName;
                                    appointment.CorporateAccountNo = entity.CorporateAccountNo;
                                    appointment.CoverageLimitAmount = entity.CoverageLimitAmount;
                                    appointment.CoverageTypeID = entity.CoverageTypeID;
                                    appointment.IsCoverageLimitPerDay = entity.IsCoverageLimitPerDay;

                                    if (cboResultDeliveryPlan.Value != null)
                                    {
                                        if (cboResultDeliveryPlan.Value.ToString() != "")
                                        {
                                            appointment.GCResultDeliveryPlan = cboResultDeliveryPlan.Value.ToString();
                                            if (cboResultDeliveryPlan.Value.ToString() == Constant.ResultDeliveryPlan.OTHERS)
                                            {
                                                appointment.ResultDeliveryPlanOthers = Request.Form[txtResultDeliveryPlanOthers.UniqueID];
                                            }
                                            else
                                            {
                                                appointment.ResultDeliveryPlanOthers = null;
                                            }
                                        }
                                        else
                                        {
                                            appointment.GCResultDeliveryPlan = null;
                                            appointment.ResultDeliveryPlanOthers = null;
                                        }
                                    }
                                    else
                                    {
                                        appointment.GCResultDeliveryPlan = null;
                                        appointment.ResultDeliveryPlanOthers = null;
                                    }

                                    appointment.QueueNo = entityVisit.QueueNo;
                                    if (hdnIsUsedReferenceQueueNo.Value == "1")
                                    {
                                        appointment.ReferenceQueueNo = entityVisit.ReferenceQueueNo;
                                    }
                                    appointment.CreatedBy = AppSession.UserLogin.UserID;
                                    appointment.CreatedDate = DateTime.Now;
                                    apmID = entityAppointmentDao.InsertReturnPrimaryKeyID(appointment);
                                }
                                if (apmID != 0)
                                {
                                    entity.AppointmentID = apmID;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDao.Update(entity);
                                }
                            }
                            flagQueueNoGenerated = true;
                        }
                        else
                        {
                            if (entityAppointment == null)
                            {
                                bool isBPJS = false;
                                if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                {
                                    isBPJS = true;
                                }

                                appointment = new Appointment();

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();


                                ValidateParamedicScSchedule(obj, objSchDate, appointment.StartDate);

                                int visitDuration = 0;
                                ParamedicVisitType entitySUVT = BusinessLayer.GetParamedicVisitTypeList(string.Format("VisitTypeID = {0} AND HealthcareServiceUnitID = {1} AND ParamedicID = {2}", hdnVisitTypeID.Value, hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value), ctx).FirstOrDefault();

                                if (entitySUVT != null)
                                {
                                    visitDuration = entitySUVT.VisitDuration;
                                }
                                else
                                {
                                    visitDuration = 10;
                                }

                                if (objSchDate != null)
                                {
                                    Appointment entityApmCheck = BusinessLayer.GetAppointmentList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(VARCHAR, StartDate, 112) = '{2}'", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx).LastOrDefault();

                                    if (entityApmCheck != null)
                                    {
                                        appointment.StartTime = entityApmCheck.EndTime;
                                    }
                                    else
                                    {
                                        if (entityVisit.Session == 5)
                                        {
                                            appointment.StartTime = objSchDate.StartTime5;
                                        }
                                        else if (entityVisit.Session == 4)
                                        {
                                            appointment.StartTime = objSchDate.StartTime4;
                                        }
                                        else if (entityVisit.Session == 3)
                                        {
                                            appointment.StartTime = objSchDate.StartTime3;
                                        }
                                        else if (entityVisit.Session == 2)
                                        {
                                            appointment.StartTime = appointment.EndTime = objSchDate.StartTime2;
                                        }
                                        else
                                        {
                                            appointment.StartTime = appointment.EndTime = objSchDate.StartTime1;
                                        }
                                    }

                                    string[] newTime = appointment.StartTime.Split(':');
                                    int hour = Convert.ToInt32(newTime[0]);
                                    int minute = Convert.ToInt32(newTime[1]);
                                    DateTime newDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0);
                                    newDate = newDate.AddMinutes(visitDuration);
                                    string hourInString = newDate.Hour.ToString();
                                    string minuteInString = newDate.Minute.ToString();
                                    if (newDate.Hour < 10)
                                    {
                                        hourInString = string.Format("0{0}", newDate.Hour);
                                    }

                                    if (newDate.Minute < 10)
                                    {
                                        minuteInString = string.Format("0{0}", newDate.Minute);
                                    }
                                    appointment.EndTime = string.Format("{0}:{1}", hourInString, minuteInString);

                                    appointment.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                                    appointment.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                                    appointment.ParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                                    appointment.StartDate = appointment.EndDate = entity.RegistrationDate;
                                    appointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(appointment.TransactionCode, appointment.StartDate);
                                    appointment.Name = entity.RegistrationNo;
                                    appointment.MRN = entity.MRN;
                                    appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                    appointment.IsAutoAppointment = true;
                                    appointment.Notes = string.Format("Registrasi Langsung Dengan No {0}", entity.RegistrationNo);

                                    appointment.Session = BusinessLayer.GetRegistrationSession(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, entityVisit.VisitTime, ctx);

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, Convert.ToInt32(appointment.Session), false, isBPJS, 0, ctx, 1));

                                    if (hdnIsUsedReferenceQueueNo.Value == "1")
                                    {
                                        appointment.ReferenceQueueNo = entityVisit.ReferenceQueueNo;
                                    }

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    appointment.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
                                    appointment.GCAppointmentMethod = Constant.AppointmentMethod.GOSHOW;
                                    appointment.GCAppointmentStatus = Constant.AppointmentStatus.COMPLETE;

                                    if (cboResultDeliveryPlan.Value != null)
                                    {
                                        if (cboResultDeliveryPlan.Value.ToString() != "")
                                        {
                                            appointment.GCResultDeliveryPlan = cboResultDeliveryPlan.Value.ToString();
                                            if (cboResultDeliveryPlan.Value.ToString() == Constant.ResultDeliveryPlan.OTHERS)
                                            {
                                                appointment.ResultDeliveryPlanOthers = Request.Form[txtResultDeliveryPlanOthers.UniqueID];
                                            }
                                            else
                                            {
                                                appointment.ResultDeliveryPlanOthers = null;
                                            }
                                        }
                                        else
                                        {
                                            appointment.GCResultDeliveryPlan = null;
                                            appointment.ResultDeliveryPlanOthers = null;
                                        }
                                    }
                                    else
                                    {
                                        appointment.GCResultDeliveryPlan = null;
                                        appointment.ResultDeliveryPlanOthers = null;
                                    }

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    appointment.CreatedBy = AppSession.UserLogin.UserID;
                                    appointment.CreatedDate = DateTime.Now;
                                    apmID = entityAppointmentDao.InsertReturnPrimaryKeyID(appointment);
                                }
                                else if (objSchDate == null && obj != null)
                                {

                                    appointment.Session = BusinessLayer.GetRegistrationSession(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, entityVisit.VisitTime, ctx);

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    Appointment entityApmCheck = BusinessLayer.GetAppointmentList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(VARCHAR, StartDate, 112) = '{2}' AND Session = {3} AND GCAppointmentStatus NOT IN ('{4}','{5}')", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), appointment.Session, Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED), ctx).LastOrDefault();

                                    if (entityApmCheck != null)
                                    {
                                        appointment.StartTime = entityApmCheck.EndTime;
                                    }
                                    else
                                    {
                                        if (appointment.Session == 5)
                                        {
                                            appointment.StartTime = obj.StartTime5;
                                        }
                                        else if (appointment.Session == 4)
                                        {
                                            appointment.StartTime = obj.StartTime4;
                                        }
                                        else if (appointment.Session == 3)
                                        {
                                            appointment.StartTime = obj.StartTime3;
                                        }
                                        else if (appointment.Session == 2)
                                        {
                                            appointment.StartTime = obj.StartTime2;
                                        }
                                        else
                                        {
                                            appointment.StartTime = obj.StartTime1;
                                        }
                                    }

                                    string[] newTime = appointment.StartTime.Split(':');
                                    int hour = Convert.ToInt32(newTime[0]);
                                    int minute = Convert.ToInt32(newTime[1]);
                                    DateTime newDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0);
                                    newDate = newDate.AddMinutes(visitDuration);
                                    string hourInString = newDate.Hour.ToString();
                                    string minuteInString = newDate.Minute.ToString();
                                    if (newDate.Hour < 10)
                                    {
                                        hourInString = string.Format("0{0}", newDate.Hour);
                                    }

                                    if (newDate.Minute < 10)
                                    {
                                        minuteInString = string.Format("0{0}", newDate.Minute);
                                    }
                                    appointment.EndTime = string.Format("{0}:{1}", hourInString, minuteInString);

                                    appointment.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                                    appointment.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                                    appointment.ParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                                    appointment.StartDate = appointment.EndDate = entity.RegistrationDate;
                                    appointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(appointment.TransactionCode, appointment.StartDate);
                                    appointment.Name = entity.RegistrationNo;
                                    appointment.MRN = entity.MRN;
                                    appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                    appointment.IsAutoAppointment = true;
                                    appointment.Notes = string.Format("Registrasi Langsung Dengan No {0}", entity.RegistrationNo);

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, Convert.ToInt32(appointment.Session), false, isBPJS, 0, ctx, 1));

                                    if (hdnIsUsedReferenceQueueNo.Value == "1")
                                    {
                                        appointment.ReferenceQueueNo = entityVisit.ReferenceQueueNo;
                                    }

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    appointment.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
                                    appointment.GCAppointmentMethod = Constant.AppointmentMethod.GOSHOW;
                                    appointment.GCAppointmentStatus = Constant.AppointmentStatus.COMPLETE;

                                    if (cboResultDeliveryPlan.Value != null)
                                    {
                                        if (cboResultDeliveryPlan.Value.ToString() != "")
                                        {
                                            appointment.GCResultDeliveryPlan = cboResultDeliveryPlan.Value.ToString();
                                            if (cboResultDeliveryPlan.Value.ToString() == Constant.ResultDeliveryPlan.OTHERS)
                                            {
                                                appointment.ResultDeliveryPlanOthers = Request.Form[txtResultDeliveryPlanOthers.UniqueID];
                                            }
                                            else
                                            {
                                                appointment.ResultDeliveryPlanOthers = null;
                                            }
                                        }
                                        else
                                        {
                                            appointment.GCResultDeliveryPlan = null;
                                            appointment.ResultDeliveryPlanOthers = null;
                                        }
                                    }
                                    else
                                    {
                                        appointment.GCResultDeliveryPlan = null;
                                        appointment.ResultDeliveryPlanOthers = null;
                                    }

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    appointment.CreatedBy = AppSession.UserLogin.UserID;
                                    appointment.CreatedDate = DateTime.Now;
                                    apmID = entityAppointmentDao.InsertReturnPrimaryKeyID(appointment);
                                }
                                if (apmID != 0)
                                {
                                    entity.AppointmentID = apmID;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDao.Update(entity);
                                }
                            }
                        }

                        if (!flagQueueNoGenerated)
                        {
                            //entityVisit.QueueNo = BusinessLayer.GetConsultVisitMaxQueueNo(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND VisitDate = '{2}'", entityVisit.HealthcareServiceUnitID, entityVisit.ParamedicID, entityVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                            //entityVisit.QueueNo++;

                            if (entityAppointment == null)
                            {
                                if (apmID == 0)
                                {
                                    entityVisit.Session = BusinessLayer.GetRegistrationSession(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, entityVisit.VisitTime, ctx);
                                    if (newQueue == 0)
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entityVisit.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, Convert.ToInt32(entityVisit.Session), false, isBPJS, 0, ctx, 1));
                                    }
                                    else
                                    {
                                        entityVisit.QueueNo = Convert.ToInt16(newQueue);
                                    }
                                }
                                else
                                {
                                    entityVisit.Session = appointment.Session;
                                    if (newQueue == 0)
                                    {
                                        entityVisit.QueueNo = appointment.QueueNo;
                                    }
                                    else
                                    {
                                        entityVisit.QueueNo = Convert.ToInt16(newQueue);
                                    }
                                }
                            }
                            else
                            {
                                if (hdnIsQueueNoUsingAppointment.Value == "1")
                                {
                                    entityVisit.Session = entityAppointment.Session;
                                    entityVisit.QueueNo = entityAppointment.QueueNo;
                                }
                                else
                                {
                                    bool isAllowDifferentQueueNo = true;
                                    if (objSchDate != null)
                                    {
                                        if (!objSchDate.IsAllowDifferentQueueNo)
                                        {
                                            isAllowDifferentQueueNo = false;
                                            entityVisit.Session = entityAppointment.Session;
                                            entityVisit.QueueNo = entityAppointment.QueueNo;
                                        }
                                    }
                                    else if (obj != null)
                                    {
                                        if (!obj.IsAllowDifferentQueueNo)
                                        {
                                            if (entityAppointment != null)
                                            {
                                                isAllowDifferentQueueNo = false;
                                                entityVisit.Session = entityAppointment.Session;
                                                entityVisit.QueueNo = entityAppointment.QueueNo;
                                            }
                                        }
                                    }

                                    if (isAllowDifferentQueueNo)
                                    {
                                        entityVisit.Session = BusinessLayer.GetRegistrationSession(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, entityVisit.VisitTime, ctx);
                                        if (newQueue == 0)
                                        {
                                            bool isBPJS = false;
                                            if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                            {
                                                isBPJS = true;
                                            }
                                            entityVisit.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, Convert.ToInt32(entityVisit.Session), false, isBPJS, 0, ctx, 1));
                                        }
                                        else
                                        {
                                            entityVisit.QueueNo = Convert.ToInt16(newQueue);
                                        }
                                    }
                                }
                            }
                            if (hdnIsUsedReferenceQueueNo.Value == "1")
                            {
                                entityVisit.ReferenceQueueNo = Convert.ToInt16(BusinessLayer.GenerateReferenceQueueNo(entityVisit.VisitDate, entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entity.GCCustomerType, ctx, 1));
                            }
                        }

                        if (hdnIsUsedPatientOwnerStatus.Value == "1")
                        {
                            if (cboPatientOwnerStatus.Value != null)
                            {
                                entityVisit.GCPatientOwnerStatus = cboPatientOwnerStatus.Value.ToString();
                            }
                        }

                        if (hdnIsOutpatientUsingRoom.Value == "1")
                        {
                            if (hdnRoomID.Value != "0" && hdnRoomID.Value != "")
                            {
                                string registrationTicketNo = string.Empty;
                                registrationTicketNo = Helper.GenerateRegistrationTicketNo(Convert.ToInt32(entityVisit.QueueNo), Convert.ToInt32(hdnRoomID.Value));
                                entity.RegistrationTicketNo = registrationTicketNo;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(entity);
                            }
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityVisit.VisitID = entityVisitDao.InsertReturnPrimaryKeyID(entityVisit);

                        if (!string.IsNullOrEmpty(hdnAppointmentID.Value))
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            PatientReferral entityReferral = BusinessLayer.GetPatientReferralList(string.Format("ToAppointmentID = {0} AND IsDeleted = 0", hdnAppointmentID.Value), ctx).FirstOrDefault();
                            if (entityReferral != null)
                            {
                                entityReferral.ToVisitID = entityVisit.VisitID;
                                entityReferral.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityReferral.LastUpdatedDate = DateTime.Now;
                                entityPatientReferralDao.Update(entityReferral);
                            }
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                        }

                        if (entity.LinkedRegistrationID != null && entityVisit.VisitID != 0)
                        {
                            //Update Chief Complaint - Inpatient Initial Assessment into Current Registration
                            ConsultVisit oVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", entity.LinkedRegistrationID), ctx).FirstOrDefault();
                            if (oVisit != null)
                            {
                                string filterExp = string.Format("VisitID = {0} AND IsInpatientInitialAssessment = 1", oVisit.VisitID);
                                ChiefComplaint oChiefComplaint = BusinessLayer.GetChiefComplaintList(filterExp, ctx).FirstOrDefault();
                                if (oChiefComplaint != null)
                                {
                                    oChiefComplaint.VisitID = entityVisit.VisitID;
                                    oChiefComplaint.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityChiefComplaintDao.Update(oChiefComplaint);
                                }
                            }
                        }

                        if (entityPatientBirth != null)
                        {
                            Patient entityMother = entityPatientDao.Get(Convert.ToInt32(hdnMotherMRN.Value));
                            hdnVisitID.Value = Convert.ToString(entityVisit.VisitID);
                            if (hdnVisitID.Value != "")
                            {
                                entityPatientBirth.VisitID = Convert.ToInt32(hdnVisitID.Value);
                            }
                            if (entityPatientBirth.BirthRecordID != 0)
                            {
                                entityPatientBirth.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityPatientBirthRecordDao.Update(entityPatientBirth);
                            }
                            else
                            {
                                entityPatientBirth.CreatedBy = AppSession.UserLogin.UserID;
                                entityPatientBirthRecordDao.Insert(entityPatientBirth);
                            }

                            if (hdnIsRegistrationBabyLinkRegistrationMother.Value == "1")
                            {
                                String mothervisitID = hdnMotherVisitID.Value;
                                vConsultVisit9 entityRegistrationMother = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = '{0}'", mothervisitID), ctx).FirstOrDefault();

                                Registration entityregistration = entityDao.Get(Convert.ToInt32(entity.RegistrationID));
                                entityregistration.LinkedToRegistrationID = entityRegistrationMother.RegistrationID;
                                entityregistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDao.Update(entityregistration);
                            }

                            if (entityPatient != null)
                            {
                                entityPatient.MotherName = hdnMotherName.Value;
                            }

                            PatientFamily entityPatientFamily = BusinessLayer.GetPatientFamilyList(string.Format("MRN = '{0}' AND GCFamilyRelation = '{1}'", hdnMRN.Value, Constant.FamilyRelation.MOTHER), ctx).FirstOrDefault();
                            bool flagAddPatientFamily = false;
                            if (entityPatientFamily == null)
                            {
                                entityPatientFamily = new PatientFamily();
                                flagAddPatientFamily = true;
                            }
                            entityPatientFamily.MRN = Convert.ToInt32(hdnMRN.Value);
                            entityPatientFamily.GCFamilyRelation = Constant.FamilyRelation.MOTHER;
                            entityPatientFamily.FamilyMRN = Convert.ToInt32(hdnMotherMRN.Value);
                            entityPatientFamily.FirstName = entityMother.FirstName;
                            entityPatientFamily.LastName = entityMother.LastName;
                            entityPatientFamily.FullName = entityMother.FullName;
                            entityPatientFamily.AddressID = entityMother.HomeAddressID;
                            if (flagAddPatientFamily)
                            {
                                entityPatientFamily.CreatedBy = AppSession.UserLogin.UserID;
                                entityPatientFamilyDao.Insert(entityPatientFamily);
                            }
                            else
                            {
                                entityPatientFamily.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityPatientFamilyDao.Update(entityPatientFamily);
                            }
                        }

                        if (entityPatient != null)
                        {
                            if (entity.IsPregnant)
                            {
                                entityPatient.IsPregnant = true;
                            }

                            entityPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityPatientDao.Update(entityPatient);
                        }

                        PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
                        PatientDiagnosis diffDx = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}'", entityVisit.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS), ctx).FirstOrDefault();
                        if (diffDx == null)
                        {
                            diffDx = new PatientDiagnosis();
                            ControlToEntityPatientDiagnosis(diffDx);
                            if ((diffDx.DiagnoseID != "" && diffDx.DiagnoseID != null) || (diffDx.DiagnosisText.Trim() != "" && diffDx.DiagnosisText.Trim() != null))
                            {
                                diffDx.VisitID = entityVisit.VisitID;
                                diffDx.CreatedBy = AppSession.UserLogin.UserID;
                                patientDiagnosisDao.Insert(diffDx);
                            }
                        }
                        else
                        {
                            ControlToEntityPatientDiagnosis(diffDx);
                            diffDx.VisitID = entityVisit.VisitID;
                            diffDx.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientDiagnosisDao.Update(diffDx);
                        }

                        if (!chkIsHasMRN.Checked)
                        {
                            if (hdnGuestID.Value.ToString() == "")
                            {
                                hdnGuestID.Value = BusinessLayer.GetGuestMaxID(ctx).ToString();
                            }
                            entity.GuestID = Convert.ToInt32(hdnGuestID.Value);
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                        }

                        // Linked To RegistrationID
                        if (entity.LinkedRegistrationID != null && entity.LinkedRegistrationID != 0)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            Registration regLinked = entityDao.Get(Convert.ToInt32(entity.LinkedRegistrationID));
                            regLinked.LinkedToRegistrationID = entity.RegistrationID;
                            regLinked.LastUpdatedBy = AppSession.UserLogin.UserID;
                            regLinked.LastUpdatedDate = DateTime.Now;
                            entityDao.Update(regLinked);
                        }

                        if (registrationStatus == Constant.VisitStatus.CHECKED_IN)
                        {
                            String ChargeCodeAdministration = string.Empty;
                            bool flagSkipAdm = false;
                            if (hdnIsControlAdministrationCharges.Value == "1" && entityRegistrationPayer.GCCustomerType != Constant.CustomerType.PERSONAL) ChargeCodeAdministration = hdnChargeCodeAdministrationForInstansi.Value;
                            int cvItemID = 0;
                            if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
                            {
                                if (hdnItemID.Value != "" && hdnItemID.Value != "0")
                                {
                                    cvItemID = Convert.ToInt32(hdnItemID.Value);
                                }
                            }
                            else
                            {
                                if (hdnItemAIOID.Value != "" && hdnItemAIOID.Value != "0")
                                {
                                    cvItemID = Convert.ToInt32(hdnItemAIOID.Value);
                                }
                            }

                            if (hdnIsControlAdmCost.Value == "1")
                            {
                                if (entity.MRN != 0 && entity.MRN != null)
                                {
                                    int flagAdmCount = BusinessLayer.GetvPatientChargesDtList(string.Format("MRN = {0} AND ItemID = '{1}' AND datediff(day, RegistrationDate, '{2}') = 0  AND GCTransactionStatus = '{3}'", entity.MRN, hdnAdminID.Value, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.CLOSED), ctx).ToList().Count;
                                    if (flagAdmCount > 0) flagSkipAdm = true;
                                }
                            }
                            //Check : Kontrol Pembayaran Kartu
                            if (hdnIsControlPatientCardPayment.Value != "1")
                            {
                                Helper.InsertAutoBillItem(ctx, entityVisit, hdnDepartmentID.Value, (int)entityVisit.ChargeClassID, entity.GCCustomerType, entity.IsPrintingPatientCard, cvItemID, ChargeCodeAdministration, flagSkipAdm);
                            }
                            else
                            {
                                if (entity.IsPrintingPatientCard && hdnItemCardFee.Value != "")
                                {
                                    Helper.InsertPatientCardBillItem(ctx, entityVisit, hdnDepartmentID.Value, (int)entityVisit.ChargeClassID);
                                }
                                Helper.InsertAutoBillItem(ctx, entityVisit, hdnDepartmentID.Value, (int)entityVisit.ChargeClassID, entity.GCCustomerType, false, cvItemID, ChargeCodeAdministration, flagSkipAdm);
                            }
                        }
                        //untuk insert ConsultVisitItemPackage INPATIENT
                        else
                        {
                            int cvItemID = 0;
                            if (hdnItemAIOID.Value != "" && hdnItemAIOID.Value != "0")
                            {
                                cvItemID = Convert.ToInt32(hdnItemAIOID.Value);
                                Helper.InsertConsultVisitItemPackage(ctx, entityVisit, hdnDepartmentID.Value, cvItemID, 1);
                            }
                        }

                        if (entityVisit.BedID != null)
                        {
                            Bed entityBed = entityBedDao.Get((int)entityVisit.BedID);
                            if (entityVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
                                entityBed.GCBedStatus = Constant.BedStatus.OCCUPIED;
                            else
                                entityBed.GCBedStatus = Constant.BedStatus.BOOKED;

                            entityBed.RegistrationID = entity.RegistrationID;
                            entityBed.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityBedDao.Update(entityBed);
                        }

                        List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                                                                    AppSession.UserLogin.HealthcareID, //0
                                                                    Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP, //1
                                                                    Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_RMO, //2
                                                                    Constant.SettingParameter.IS_DIAGNOSTIC_ADD_RMO, //3
                                                                    Constant.SettingParameter.RM_CETAK_TRACER_OTOMATIS, //4
                                                                    Constant.SettingParameter.RM_CETAK_TRACER_PASIEN_BARU, //5
                                                                    Constant.SettingParameter.RM_JENIS_PRINTER_TRACER, //6
                                                                    Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER, //7
                                                                    Constant.SettingParameter.RM_ALAMAT_PRINTER_TRACER //8
                                                                ), ctx);

                        string dpjpRole = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP).FirstOrDefault().ParameterValue;
                        string rmoRole = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_RMO).FirstOrDefault().ParameterValue;

                        if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                        {
                            //Langsung simpan dokter yg tipenya RMO ke Tim Paramedis dgn tipe dokter 'RMO'
                            List<ParamedicMaster> lstParamedicMasterRMO = BusinessLayer.GetParamedicMasterList("IsRMO = 1 AND IsDeleted = 0", ctx);
                            foreach (ParamedicMaster entityParamedicMaster in lstParamedicMasterRMO)
                            {
                                if (entityParamedicMaster.ParamedicID == entityVisit.ParamedicID) continue;
                                ParamedicTeam entityParamedicTeam = new ParamedicTeam();
                                entityParamedicTeam.RegistrationID = entity.RegistrationID;
                                entityParamedicTeam.ParamedicID = entityParamedicMaster.ParamedicID;
                                entityParamedicTeam.GCParamedicRole = rmoRole;
                                entityParamedicTeam.CreatedBy = AppSession.UserLogin.UserID;
                                entityParamedicTeamDao.Insert(entityParamedicTeam);
                            }

                            ParamedicTeam entityParamedicTeamDPJP = new ParamedicTeam();
                            entityParamedicTeamDPJP.RegistrationID = entity.RegistrationID;
                            entityParamedicTeamDPJP.ParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                            entityParamedicTeamDPJP.GCParamedicRole = dpjpRole;
                            entityParamedicTeamDPJP.CreatedBy = AppSession.UserLogin.UserID;
                            entityParamedicTeamDao.Insert(entityParamedicTeamDPJP);

                            // Dokter yang dikonsulkan di UGD otomatis menjadi konsulen
                            if (hdnFromRegistrationID.Value != "" && hdnFromRegistrationID.Value != "0")
                            {
                                List<ParamedicTeam> lstParamedic = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND IsDeleted = 0 AND ParamedicID != {1} AND GCParamedicRole != '{2}' AND ParamedicID IN (SELECT ParamedicID FROM ParamedicMaster WHERE IsRMO = 0 AND IsDeleted = 0)", Convert.ToInt32(hdnFromRegistrationID.Value), entityVisit.ParamedicID, Constant.ParamedicRole.DPJP_UTAMA), ctx);
                                foreach (ParamedicTeam oParamedic in lstParamedic)
                                {
                                    ParamedicTeam oParamedicTeam = new ParamedicTeam();
                                    oParamedicTeam.RegistrationID = entity.RegistrationID;
                                    oParamedicTeam.ParamedicID = oParamedic.ParamedicID;
                                    oParamedicTeam.GCParamedicRole = oParamedic.GCParamedicRole;
                                    oParamedicTeam.CreatedBy = AppSession.UserLogin.UserID;
                                    entityParamedicTeamDao.Insert(oParamedicTeam);
                                }
                            }


                        }
                        else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                        {
                            string setvarRMO = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.IS_DIAGNOSTIC_ADD_RMO).FirstOrDefault().ParameterValue;

                            string healthcareServiceUnitIDVisit = Convert.ToString(entityVisit.HealthcareServiceUnitID);
                            if (healthcareServiceUnitIDVisit != hdnHealthcareServiceUnitIDRadiology.Value && healthcareServiceUnitIDVisit != hdnHealthcareServiceUnitIDLaboratory.Value)
                            {
                                if (setvarRMO == "1")
                                {
                                    //Langsung simpan dokter yg tipenya RMO ke Tim Paramedis dgn tipe dokter 'RMO'
                                    List<ParamedicMaster> lstParamedicMasterRMO = BusinessLayer.GetParamedicMasterList("IsRMO = 1 AND IsDeleted = 0", ctx);
                                    foreach (ParamedicMaster entityParamedicMaster in lstParamedicMasterRMO)
                                    {
                                        if (entityParamedicMaster.ParamedicID == entityVisit.ParamedicID) continue;
                                        ParamedicTeam entityParamedicTeam = new ParamedicTeam();
                                        entityParamedicTeam.RegistrationID = entity.RegistrationID;
                                        entityParamedicTeam.ParamedicID = entityParamedicMaster.ParamedicID;
                                        entityParamedicTeam.GCParamedicRole = rmoRole;
                                        entityParamedicTeam.CreatedBy = AppSession.UserLogin.UserID;
                                        entityParamedicTeamDao.Insert(entityParamedicTeam);
                                    }

                                    ParamedicTeam entityParamedicTeamDPJP = new ParamedicTeam();
                                    entityParamedicTeamDPJP.RegistrationID = entity.RegistrationID;
                                    entityParamedicTeamDPJP.ParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                                    entityParamedicTeamDPJP.GCParamedicRole = dpjpRole;
                                    entityParamedicTeamDPJP.CreatedBy = AppSession.UserLogin.UserID;
                                    entityParamedicTeamDao.Insert(entityParamedicTeamDPJP);
                                }
                            }
                        }

                        if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                        {
                            if (hdnAppointmentID.Value != "")
                            {
                                //Sync Medical File Status from Appointment to Registration
                                if (!string.IsNullOrEmpty(entityAppointment.GCMedicalFileStatus))
                                {
                                    if (entityAppointment.GCMedicalFileStatus == Constant.MedicalFileStatus.CHECK_OUT)
                                    {
                                        entity.GCMedicalFileStatus = entityAppointment.GCMedicalFileStatus;

                                        MRTrackingLogDao trackingDao = new MRTrackingLogDao(ctx);
                                        MRTrackingLog tracking = new MRTrackingLog();
                                        tracking.MRN = Convert.ToInt32(hdnMRN.Value);
                                        tracking.LogDate = DateTime.Now.Date;
                                        tracking.LogTime = DateTime.Now.Date.ToString(Constant.FormatString.TIME_FORMAT);
                                        tracking.VisitID = entityVisit.VisitID;
                                        tracking.GCMedicalFileStatus = Constant.MedicalFileStatus.CHECK_OUT;
                                        tracking.TransporterName = "SYSTEM";
                                        tracking.Remarks = "Berkas sudah dikeluarkan ketika appointment";
                                        tracking.CreatedBy = AppSession.UserLogin.UserID;

                                        trackingDao.Insert(tracking);
                                    }
                                }
                            }
                        }
                        ctx.CommitTransaction();
                        retval = entity.RegistrationNo;

                        string isRMCetakTracerOtomatis = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.RM_CETAK_TRACER_OTOMATIS).FirstOrDefault().ParameterValue;
                        if (isRMCetakTracerOtomatis == "1")
                        {
                            bool isValidAppointment;
                            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                            {
                                if (entity.AppointmentID != null)
                                {
                                    Appointment appointment1 = BusinessLayer.GetAppointment(Convert.ToInt32(entity.AppointmentID));
                                    string registrationDate = entity.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                                    string appointmentDate = appointment1.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                                    string appointmentCreatedDate = appointment1.CreatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                                    if (registrationDate != appointmentCreatedDate)
                                    {
                                        isValidAppointment = false;
                                    }
                                    else
                                    {
                                        isValidAppointment = true;
                                    }
                                }
                                else
                                {
                                    if (entity.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT))
                                    {
                                        isValidAppointment = true;
                                    }
                                    else
                                    {
                                        isValidAppointment = false;
                                    }
                                }
                            }
                            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY || hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                            {
                                isValidAppointment = true;
                            }
                            else
                            {
                                isValidAppointment = false;
                            }

                            if (lstSettingParameterDt != null)
                            {
                                string printerType = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_TRACER)).FirstOrDefault().ParameterValue;
                                string printFormat = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER)).FirstOrDefault().ParameterValue;
                                string printerUrl = lstSettingParameterDt.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_TRACER)).FirstOrDefault().ParameterValue;

                                vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", entity.RegistrationID)).FirstOrDefault();
                                if (oVisit != null)
                                {
                                    if (oVisit.IsAutomaticPrintTracer == true)
                                    {
                                        Healthcare healthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                                        if (healthcare.Initial == "RSMD")
                                        {
                                            if (isValidAppointment)
                                            {
                                                //Check Printer Type
                                                switch (printerType)
                                                {
                                                    case Constant.PrinterType.ZEBRA_PRINTER:
                                                        ZebraPrinting.PrintTracerRM(oVisit, printerUrl, printFormat);
                                                        ZebraPrinting.PrintTracerRM(oVisit, printerUrl, printFormat);
                                                        break;
                                                    case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                                        ZebraPrinting.PrintTracerRMDotMatrix(oVisit, printerUrl, printFormat);
                                                        break;
                                                    case Constant.PrinterType.EPSON_DOT_MATRIX_RSRA:
                                                        ZebraPrinting.PrintTracerRMRSRA(oVisit, printerUrl);
                                                        break;
                                                    default:
                                                        ZebraPrinting.PrintTracerRM(oVisit, printerUrl, printFormat);
                                                        break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Check Printer Type
                                            switch (printerType)
                                            {
                                                case Constant.PrinterType.ZEBRA_PRINTER:
                                                    ZebraPrinting.PrintTracerRM(oVisit, printerUrl, printFormat);
                                                    ZebraPrinting.PrintTracerRM(oVisit, printerUrl, printFormat);
                                                    break;
                                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                                    ZebraPrinting.PrintTracerRMDotMatrix(oVisit, printerUrl, printFormat);
                                                    break;
                                                case Constant.PrinterType.EPSON_DOT_MATRIX_RSRA:
                                                    ZebraPrinting.PrintTracerRMRSRA(oVisit, printerUrl);
                                                    break;
                                                default:
                                                    ZebraPrinting.PrintTracerRM(oVisit, printerUrl, printFormat);
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (hdnAppointmentID.Value != "")
                        {

                        }

                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            //hdnProviderGatewayService.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).ParameterValue;
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                            {
                                SendRegistrationNotification(entity.RegistrationID);
                            }
                        }

                        if (hdnIsBridgingToMobileJKN.Value == "1" && hdnIsSendNotifToMobileJKN.Value == "1")
                        {
                            BusinessLayer.OnInsertBPJSTaskLog(entity.RegistrationID, 3, AppSession.UserLogin.UserID, DateTime.Now);
                        }

                        if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                        {
                            if (entity.AppointmentID != null && entity.AppointmentID != 0)
                            {
                                AppointmentRequest entityApmReq = BusinessLayer.GetAppointmentRequestList(string.Format("AppointmentID = (SELECT AppointmentID FROM Appointment WHERE AppointmentID = {0})", entity.AppointmentID)).FirstOrDefault();
                                if (entityApmReq != null)
                                {
                                    if (entityApmReq.Remarks.Contains("^"))
                                    {
                                        BridgingToMedinfrasMobileApps(Convert.ToInt32(entityApmReq.AppointmentRequestID));
                                    }
                                }
                            }
                        }

                        if (AppSession.IsBridgingToQueue)
                        {
                            //If Bridging to Queue - Send Information
                            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT || hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC || hdnDepartmentID.Value == Constant.Facility.LABORATORY || hdnDepartmentID.Value == Constant.Facility.PHARMACY || hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                            {
                                try
                                {
                                    VisitInfo visitInfo = new VisitInfo();
                                    visitInfo = ConvertVisitToDTO(entityVisit);
                                    PatientData patientInfo = ConvertPatientToDTO(entityPatient);
                                    APIMessageLog entityAPILog = new APIMessageLog()
                                    {
                                        MessageDateTime = DateTime.Now,
                                        Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                                        Sender = Constant.BridgingVendor.HIS,
                                        IsSuccess = true
                                    };
                                    QueueService oService = new QueueService();
                                    string apiResult = oService.ADT_A01(AppSession.UserLogin.HealthcareID, entity, visitInfo, patientInfo);
                                    string[] apiResultInfo = apiResult.Split('|');
                                    if (apiResultInfo[0] == "0")
                                    {
                                        entityAPILog.IsSuccess = false;
                                        entityAPILog.MessageText = apiResultInfo[2];
                                        entityAPILog.Response = apiResult;
                                        entityAPILog.ErrorMessage = apiResultInfo[1];
                                        BusinessLayer.InsertAPIMessageLog(entityAPILog);

                                        Exception ex = new Exception(apiResultInfo[1]);
                                        Helper.InsertErrorLog(ex);
                                    }
                                    else
                                    {
                                        entityAPILog.MessageText = apiResultInfo[2];
                                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Helper.InsertErrorLog(ex);
                                }
                            }
                        }

                        if (hdnIsBridgingToQumatic.Value == "1")
                        {
                            if (hdnAppointmentID.Value != null)
                            {
                                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT || hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC || hdnDepartmentID.Value == Constant.Facility.LABORATORY || hdnDepartmentID.Value == Constant.Facility.PHARMACY || hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                                {
                                    Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
                                    QumaticBodyRequestApmConfirmation oBody = ConvertBodyRequestToDTO(entityReg);

                                    try
                                    {
                                        APIMessageLog entityAPILog = new APIMessageLog()
                                        {
                                            MessageDateTime = DateTime.Now,
                                            Recipient = Constant.BridgingVendor.QUEUE,
                                            Sender = Constant.BridgingVendor.HIS,
                                            IsSuccess = true
                                        };
                                        QueueService oService = new QueueService();
                                        string apiResult = oService.QumaticConfirmation(entity, oBody);
                                        string[] apiResultInfo = apiResult.Split('|');
                                        if (apiResultInfo[0] == "0")
                                        {
                                            entityAPILog.IsSuccess = false;
                                            entityAPILog.MessageText = apiResultInfo[1];
                                            entityAPILog.Response = apiResultInfo[1];
                                            Exception ex = new Exception(apiResultInfo[1]);
                                            Helper.InsertErrorLog(ex);
                                        }
                                        else
                                        {
                                            entityAPILog.MessageText = apiResultInfo[1];
                                        }

                                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                    }
                                    catch (Exception ex)
                                    {
                                        Helper.InsertErrorLog(ex);
                                    }
                                }
                            }
                        }


                        if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                        {

                            if (hdnIsBridgingToIPTV.Value == "1")
                            {
                                if (AppSession.Is_Bridging_To_IPTV)
                                {
                                    GatewayService oService = new GatewayService();
                                    APIMessageLog entityAPILog = new APIMessageLog()
                                    {
                                        MessageDateTime = DateTime.Now,
                                        Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                                        Sender = Constant.BridgingVendor.HIS,
                                        IsSuccess = true
                                    };

                                    List<CenterbackBedTransferDTO> lstCenterbackDTO = new List<CenterbackBedTransferDTO>();
                                    CenterbackBedTransferDTO cbObj = new CenterbackBedTransferDTO()
                                    {
                                        HealthcareID = AppSession.UserLogin.HealthcareID,
                                        ProcessType = "checkin",
                                        RegistrationID = entity.RegistrationID
                                    };
                                    lstCenterbackDTO.Add(cbObj);

                                    string apiResult = oService.IPTV_BedTransfer(lstCenterbackDTO);
                                    string[] apiResultInfo = apiResult.Split('|');
                                    if (apiResultInfo[0] == "0")
                                    {
                                        entityAPILog.IsSuccess = false;
                                        entityAPILog.Response = apiResultInfo[1];
                                        Exception ex = new Exception(apiResultInfo[1]);
                                        Helper.InsertErrorLog(ex);
                                    }
                                    else
                                        entityAPILog.MessageText = apiResultInfo[1];

                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                        }
                    }
                    else
                    {
                        result = false;
                        if (errMessage == "")
                        {
                            errMessage = "Pendaftaran Gagal. Tempat tidur sudah tidak dapat digunakan untuk pendaftaran ini.";
                        }
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

        private void ValidateParamedicScSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate, DateTime selectedDate)
        {
            Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            List<GetParamedicLeaveScheduleCompare> objLeave = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), ParamedicID);

            #region validate time slot
            #region if leave in period
            if (objLeave.FirstOrDefault().DayNumber != 0 && objLeave.Count() > 1)
            {
                #region set time slot Paramedic Schedule
                if (obj != null)
                {
                    if (obj.DayNumber == objLeave.FirstOrDefault().DayNumber && objLeave.FirstOrDefault().Date == selectedDate)
                    {
                        DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);

                        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                        if (obj.StartTime5 != "")
                        {

                            if (startTimeDefault.TimeOfDay >= objStart5.TimeOfDay)
                            {
                                obj.EndTime5 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                            {
                                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay <= objStart5.TimeOfDay)
                            {
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                        }
                        else if (obj.StartTime4 != "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                            {
                                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                        }
                        else if (obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                        }
                        else if (obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime2 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                            }
                        }
                    }
                    else if (obj.DayNumber == objLeave.LastOrDefault().DayNumber && objLeave.LastOrDefault().Date == selectedDate)
                    {
                        DateTime endTime = DateTime.Parse(String.Format("2012-01-28 {0}:15", objLeave.LastOrDefault().EndTime));
                        endTime = endTime.AddMinutes(15);

                        DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.LastOrDefault().EndTime);
                        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                        if (obj.StartTime5 != "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart5.TimeOfDay)
                            {
                                obj.StartTime5 = endTime.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = endTime.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime4 != "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = endTime.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                        }
                        else if (obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (objStart1.TimeOfDay <= endTimeDefault.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                        }
                    }
                    else
                    {
                        List<GetParamedicLeaveScheduleCompare> c = objLeave.Where(t => t.Date == selectedDate).ToList();
                        if (c.Count > 0)
                        {
                            obj.StartTime1 = "";
                            obj.StartTime2 = "";
                            obj.StartTime3 = "";
                            obj.StartTime4 = "";
                            obj.StartTime5 = "";

                            obj.EndTime1 = "";
                            obj.EndTime2 = "";
                            obj.EndTime3 = "";
                            obj.EndTime4 = "";
                            obj.EndTime5 = "";
                        }
                    }
                }
                #endregion

                #region set time slot Paramedic Schedule Date
                if (objSchDate != null)
                {
                    DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                    DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                    DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                    DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                    DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

                    DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
                    DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
                    DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
                    DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
                    DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                    if (objSchDate.ScheduleDate == objLeave.FirstOrDefault().Date)
                    {
                        DateTime startTimeDefault = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.FirstOrDefault().StartTime));
                        if (objSchDate.StartTime5 != "")
                        {

                            if (startTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay)
                            {
                                objSchDate.EndTime5 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                            {
                                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart5.TimeOfDay)
                            {
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                        }
                        else if (objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                            {
                                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                        }
                        else if (objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                        }
                        else if (objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                            }
                        }
                    }
                    else if (objSchDate.ScheduleDate == objLeave.LastOrDefault().Date)
                    {
                        DateTime endTime = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.LastOrDefault().EndTime));
                        endTime = endTime.AddMinutes(15);

                        DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.LastOrDefault().EndTime);

                        if (objSchDate.StartTime5 != "")
                        {

                            if (endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay)
                            {
                                objSchDate.StartTime5 = endTime.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = endTime.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = endTime.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (objSchStart1.TimeOfDay <= endTimeDefault.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                        }
                    }
                    else
                    {
                        List<GetParamedicLeaveScheduleCompare> c = objLeave.Where(t => t.Date == selectedDate).ToList();
                        if (c.Count > 0)
                        {
                            objSchDate.StartTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.StartTime5 = "";

                            objSchDate.EndTime1 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.EndTime5 = "";
                        }
                    }
                }
                #endregion
            }
            #endregion
            #region if leave only in one day
            else if (objLeave.FirstOrDefault().DayNumber != 0 && objLeave.Count() == 1)
            {
                #region set time slot Paramedic Schedule
                if (obj != null)
                {
                    DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);
                    DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().EndTime);

                    DateTime startTime = startTimeDefault.AddMinutes(15);
                    DateTime endTime = endTimeDefault.AddMinutes(15);

                    DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                    DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                    DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                    DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                    DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                    DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                    DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                    DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                    DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                    DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                    if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay) //1/2
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList2 = obj.IsAllowWaitingList1;
                            obj.MaximumWaitingList2 = obj.MaximumWaitingList1;

                            obj.IsAppointmentByTimeSlot2 = obj.IsAppointmentByTimeSlot1;
                            obj.MaximumAppointment2 = obj.MaximumAppointment1;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //2 modif
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //2 modif
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //2 modif
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList3 = obj.IsAllowWaitingList2;
                            obj.MaximumWaitingList3 = obj.MaximumWaitingList2;

                            obj.IsAppointmentByTimeSlot3 = obj.IsAppointmentByTimeSlot2;
                            obj.MaximumAppointment3 = obj.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //9
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList3 = obj.IsAllowWaitingList2;
                            obj.MaximumWaitingList3 = obj.MaximumWaitingList2;

                            obj.IsAppointmentByTimeSlot3 = obj.IsAppointmentByTimeSlot2;
                            obj.MaximumAppointment3 = obj.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //22
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd3.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //22
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.StartTime5 = obj.StartTime4;
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList5 = obj.IsAllowWaitingList4;
                            obj.MaximumWaitingList5 = obj.MaximumWaitingList4;

                            obj.IsAppointmentByTimeSlot5 = obj.IsAppointmentByTimeSlot4;
                            obj.MaximumAppointment5 = obj.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objStart4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList5 = obj.IsAllowWaitingList4;
                            obj.MaximumWaitingList5 = obj.MaximumWaitingList4;

                            obj.IsAppointmentByTimeSlot5 = obj.IsAppointmentByTimeSlot4;
                            obj.MaximumAppointment5 = obj.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime5;
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd3.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.StartTime5 = obj.StartTime4;
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objStart4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay == objStart5.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime5;
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime5;
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = endTime.ToString("HH:mm");
                            obj.EndTime5 = obj.EndTime5;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart5.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = startTime.ToString("HH:mm");
                            obj.EndTime5 = endTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                    }
                }
                #endregion

                #region set time slot Paramedic Schedule Date
                if (objSchDate != null)
                {
                    DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);
                    DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().EndTime);

                    DateTime startTime = startTimeDefault.AddMinutes(15);
                    DateTime endTime = endTimeDefault.AddMinutes(15);

                    DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                    DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                    DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                    DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                    DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

                    DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
                    DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
                    DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
                    DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
                    DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                    if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay) //1/2
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList2 = objSchDate.IsAllowWaitingList1;
                            objSchDate.MaximumWaitingList2 = objSchDate.MaximumWaitingList1;

                            objSchDate.IsAppointmentByTimeSlot2 = objSchDate.IsAppointmentByTimeSlot1;
                            objSchDate.MaximumAppointment2 = objSchDate.MaximumAppointment1;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //2 modif
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //2 modif
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //2 modif
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList3 = objSchDate.IsAllowWaitingList2;
                            objSchDate.MaximumWaitingList3 = objSchDate.MaximumWaitingList2;

                            objSchDate.IsAppointmentByTimeSlot3 = objSchDate.IsAppointmentByTimeSlot2;
                            objSchDate.MaximumAppointment3 = objSchDate.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //9
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList3 = objSchDate.IsAllowWaitingList2;
                            objSchDate.MaximumWaitingList3 = objSchDate.MaximumWaitingList2;

                            objSchDate.IsAppointmentByTimeSlot3 = objSchDate.IsAppointmentByTimeSlot2;
                            objSchDate.MaximumAppointment3 = objSchDate.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //22
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //22
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.StartTime5 = objSchDate.StartTime4;
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList5 = objSchDate.IsAllowWaitingList4;
                            objSchDate.MaximumWaitingList5 = objSchDate.MaximumWaitingList4;

                            objSchDate.IsAppointmentByTimeSlot5 = objSchDate.IsAppointmentByTimeSlot4;
                            objSchDate.MaximumAppointment5 = objSchDate.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList5 = objSchDate.IsAllowWaitingList4;
                            objSchDate.MaximumWaitingList5 = objSchDate.MaximumWaitingList4;

                            objSchDate.IsAppointmentByTimeSlot5 = objSchDate.IsAppointmentByTimeSlot4;
                            objSchDate.MaximumAppointment5 = objSchDate.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime5;
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.StartTime5 = objSchDate.StartTime4;
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime5;
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime5;
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                            objSchDate.EndTime5 = objSchDate.EndTime5;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = startTime.ToString("HH:mm");
                            objSchDate.EndTime5 = endTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                    }
                }
                #endregion
            }
            #endregion
            #endregion
        }

        #region Bridging to Queue - Methods
        private VisitInfo ConvertVisitToDTO(ConsultVisit entityVisit)
        {
            VisitInfo visitInfo = new VisitInfo();
            visitInfo.VisitID = entityVisit.VisitID;
            visitInfo.VisitDate = entityVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            visitInfo.VisitTime = entityVisit.VisitTime;
            visitInfo.DepartmentCode = hdnDepartmentID.Value;
            visitInfo.ServiceUnitCode = txtServiceUnitCode.Text;
            visitInfo.ServiceUnitName = Request.Form[txtServiceUnitName.UniqueID];
            if (hdnRoomID.Value != null && hdnRoomID.Value != "" && hdnRoomID.Value != "0")
            {
                visitInfo.RoomID = null;
            }
            visitInfo.RoomCode = Request.Form[txtRoomCode.UniqueID];
            visitInfo.RoomName = Request.Form[txtRoomName.UniqueID];
            visitInfo.PhysicianID = Convert.ToInt32(hdnParamedicID.Value);
            visitInfo.PhysicianCode = txtPhysicianCode.Text;
            visitInfo.PhysicianName = Request.Form[txtPhysicianName.Text];
            visitInfo.SpecialtyName = cboSpecialty.Text;
            visitInfo.BedCode = txtBedCode.Text;
            visitInfo.ExtensionNo = hdnExtensionNo.Value;
            return visitInfo;
        }
        private PatientData ConvertPatientToDTO(Patient oPatient)
        {
            PatientData oData = new PatientData();
            oData.PatientID = oPatient.MRN;
            oData.MedicalNo = oPatient.MedicalNo;
            oData.FirstName = oPatient.FirstName;
            oData.MiddleName = oPatient.MiddleName;
            oData.LastName = oPatient.LastName;
            oData.PrefferedName = oPatient.PreferredName;
            oData.Gender = string.Format("{0}^{1}", oPatient.GCGender.Substring(5), Request.Form[txtGender.UniqueID]);
            oData.Religion = string.Empty;
            oData.MaritalStatus = string.Empty;
            oData.Nationality = string.Empty;

            oData.DateOfBirth = oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112);
            oData.CityOfBirth = oPatient.CityOfBirth;
            oData.HomeAddress = Request.Form[txtAddress.UniqueID];
            oData.MobileNo1 = oPatient.MobilePhoneNo1;
            oData.MobileNo2 = oPatient.MobilePhoneNo2;
            oData.EmailAddress = oPatient.EmailAddress;
            return oData;
        }
        private QumaticBodyRequestApmConfirmation ConvertBodyRequestToDTO(Registration oRegistration)
        {
            QumaticBodyRequestApmConfirmation oData = new QumaticBodyRequestApmConfirmation();
            oData.AppointmentID = Convert.ToInt32(oRegistration.AppointmentID);
            oData.RegistrationID = oRegistration.RegistrationID;
            return oData;
        }
        #endregion

        #region Send Notification
        private void SendRegistrationNotification(int RegistrationID)
        {
            try
            {
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.QUEUE,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };
                GatewayService oService = new GatewayService();
                string apiResult = oService.SendNotificationRegistration(RegistrationID);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[1];
                    entityAPILog.Response = apiResultInfo[1];
                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    entityAPILog.MessageText = apiResultInfo[1];
                }

                BusinessLayer.InsertAPIMessageLog(entityAPILog);
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
            }
        }
        #endregion

        private void BridgingToMedinfrasMobileApps(int appointmentRequestID)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {
                MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                APIMessageLog entityAPILog = new APIMessageLog();
                entityAPILog.IsSuccess = true;
                entityAPILog.MessageDateTime = DateTime.Now;
                entityAPILog.Sender = "MEDINFRAS";
                entityAPILog.Recipient = "MOBILE APPS";

                string apiResult = oService.OnRegistrationFromAppointment(appointmentRequestID);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[1];
                    entityAPILog.ErrorMessage = apiResultInfo[2];
                    //entityAPILog.Response = apiResultInfo[1];
                    Exception ex = new Exception(apiResultInfo[2]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    entityAPILog.MessageText = apiResultInfo[1];
                    entityAPILog.Response = apiResultInfo[2];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
            }
        }

        private string BridgingToGatewayGetQueueNo(string medicalNo, string paramedicCode, string customerType, DateTime date, string hour, string via, string healthcareServiceUnitID, int session)
        {
            String queue = "";

            if (hdnIsBridgingToGateway.Value == "1")
            {
                GatewayService oService = new GatewayService();
                APIMessageLog entityAPILog = new APIMessageLog();
                entityAPILog.Sender = "MEDINFRAS";
                entityAPILog.Recipient = "QUEUE ENGINE";
                string apiResult = oService.GetQueueNo(medicalNo, paramedicCode, customerType, date, hour, via, healthcareServiceUnitID, session);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    queue = string.Format("{0}|{1}", apiResultInfo[0], apiResultInfo[1]);
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[2];
                    entityAPILog.Response = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    queue = apiResult;
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.MessageText = apiResultInfo[2];
                    entityAPILog.Response = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
            }

            return queue;
        }
        #endregion

        #region Getter Guest Data
        public String GuestGCSalutation { get { return Request.Form[hdnGuestGCSalutation.UniqueID]; } }
        public String GuestGCTitle { get { return Request.Form[hdnGuestGCTitle.UniqueID]; } }
        public String GuestFirstName { get { return Request.Form[hdnGuestFirstName.UniqueID]; } }
        public String GuestMiddleName { get { return Request.Form[hdnGuestMiddleName.UniqueID]; } }
        public String GuestLastName { get { return Request.Form[hdnGuestLastName.UniqueID]; } }
        public String GuestGCSuffix { get { return Request.Form[hdnGuestGCSuffix.UniqueID]; } }
        public String GuestGCGender { get { return Request.Form[hdnGuestGCGender.UniqueID]; } }
        public String GuestDateOfBirth { get { return Request.Form[hdnGuestDateOfBirth.UniqueID]; } }
        public String GuestStreetName { get { return Request.Form[hdnGuestStreetName.UniqueID]; } }
        public String GuestCounty { get { return Request.Form[hdnGuestCounty.UniqueID]; } }
        public String GuestCity { get { return Request.Form[hdnGuestCity.UniqueID]; } }
        public String GuestDistrict { get { return Request.Form[hdnGuestDistrictDomicile.UniqueID]; } }
        public String GuestStreetNameDomicile { get { return Request.Form[hdnGuestStreetNameDomicile.UniqueID]; } }
        public String GuestCountyDomicile { get { return Request.Form[hdnGuestCountyDomicile.UniqueID]; } }
        public String GuestCityDomicile { get { return Request.Form[hdnGuestCityDomicile.UniqueID]; } }
        public String GuestDistrictDomicile { get { return Request.Form[hdnGuestDistrict.UniqueID]; } }
        public String GuestPhoneNo { get { return Request.Form[hdnGuestPhoneNo.UniqueID]; } }
        public String GuestMobilePhoneNo { get { return Request.Form[hdnGuestMobilePhoneNo.UniqueID]; } }
        public String GuestEmailAddress { get { return Request.Form[hdnGuestEmailAddress.UniqueID]; } }
        public String GuestGCIdentityNoType { get { return Request.Form[hdnGuestGCIdentityNoType.UniqueID]; } }
        public String GuestSSN { get { return Request.Form[hdnGuestSSN.UniqueID]; } }
        #endregion

        #region BPJSSEP
        protected void cbpBPJSProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int RegistrationID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnRegistrationID.Value != "")
                {
                    //RegistrationID = BusinessLayer.GetRegistrationList(string.Format("RegistrationNo = '{0}'", txtRegistrationNo.Text)).FirstOrDefault().RegistrationID;
                    RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                    if (OnSaveAddRecord(ref errMessage, RegistrationID))
                    {
                        result += "success";
                        btnGenerateSEP.Attributes.Add("style", "display:block");
                        btnGenerateSEP.Attributes.Add("style", "display:none");
                    }
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "update")
            {
                if (OnUpdatePrintNumber(ref errMessage))
                {
                    result += "success";
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(RegistrationBPJS entity)
        {
            entity.NoPeserta = txtNHSRegistrationNo.Text;
            bool flagSEPNull = false;
            if (string.IsNullOrEmpty(entity.NoSEP))
            {
                flagSEPNull = true;
            }
            entity.NoSEP = Request.Form[txtNoSEP.UniqueID];
            if (!string.IsNullOrEmpty(entity.NoSEP))
            {
                entity.TanggalSEP = Helper.GetDatePickerValue(Request.Form[txtRegistrationDate.UniqueID]);
                entity.JamSEP = txtRegistrationHour.Text;
                if (flagSEPNull)
                {
                    if (entity.SequenceNumber == null)
                    {
                        int currentNo = BusinessLayer.GetRegistrationBPJSMaxQueueNo(string.Format("TanggalSEP = '{0}'", entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT_112)));
                        entity.SequenceNumber = ++currentNo;
                    }
                    else
                    {
                        if (entity.SequenceNumber == 0)
                        {
                            int currentNo = BusinessLayer.GetRegistrationBPJSMaxQueueNo(string.Format("TanggalSEP = '{0}'", entity.TanggalSEP.ToString(Constant.FormatString.DATE_FORMAT_112)));
                            entity.SequenceNumber = ++currentNo;
                        }
                    }
                }
            }
            entity.NamaPeserta = Request.Form[txtNamaPeserta.UniqueID];
            entity.NoRujukan = txtReferralNo.Text;
            if (!string.IsNullOrEmpty(txtSurkonBPJS.Text))
            {
                entity.NoSuratKontrolManual = txtSurkonBPJS.Text;
            }
            if (!string.IsNullOrEmpty(hdnTglRujukan.Value))
            {
                entity.TanggalRujukan = DateTime.ParseExact(hdnTglRujukan.Value, Constant.FormatString.DATE_PICKER_FORMAT2,
                              CultureInfo.InvariantCulture);
            }
            else
            {
                entity.TanggalRujukan = Helper.GetDatePickerValue(txtRegistrationDate);
            }

            string[] NamaFaskes = Request.Form[txtNamaFaskes.UniqueID].Split(new[] { " - " }, StringSplitOptions.None);
            entity.KodePPK = NamaFaskes[0];
            entity.NamaPPK = Request.Form[txtNamaFaskes.UniqueID];
            entity.KodeRujukan = txtReferralDescriptionCode.Text;
            entity.NamaRujukan = Request.Form[txtReferralDescriptionName.UniqueID];
            entity.JenisPeserta = Request.Form[txtJenisPeserta.UniqueID];
            entity.JenisPelayanan = hdnDepartmentID.Value == Constant.Facility.INPATIENT ? "1" : "2";
            string[] kelas = Request.Form[txtKelas.UniqueID].Split(new[] { " - " }, StringSplitOptions.None);
            entity.NamaKelasTanggungan = Request.Form[txtKelas.UniqueID];
            entity.KelasTanggungan = kelas[0];
            //int kelasSEP = Convert.ToInt16(entity.KelasTanggungan);
            //if (entity.JenisPelayanan == "1")
            //{
            //    if (!string.IsNullOrEmpty(hdnChargeClassBPJSType.Value))
            //    {
            //        if (Convert.ToInt16(hdnChargeClassBPJSType.Value) >= kelasSEP)
            //            entity.KelasSEP = Convert.ToInt16(hdnChargeClassBPJSType.Value);
            //        else
            //            entity.KelasSEP = kelasSEP;
            //    }

            //}
            //entity.KelasSEP = kelasSEP;
            string BPJSPoliName = hdnBPJSPoli.Value;
            entity.KodePoliklinik = BPJSPoliName.Split('|')[0];
            entity.NamaPoliklinik = BPJSPoliName;
            if (!string.IsNullOrEmpty(hdnPhysicianBPJSReferenceInfo.Value))
            {
                string[] bpjsInfo = hdnPhysicianBPJSReferenceInfo.Value.Split(';');
                string[] hfisInfo = bpjsInfo[1].Split('|');
                entity.KodeDPJP = hfisInfo[0];
            }
            entity.KodePoliRujukan = hdnPoliRujukan.Value.Contains('|') ? hdnPoliRujukan.Value.Split('|')[0] : hdnPoliRujukan.Value;
            entity.Dinsos = Request.Form[txtDinsos.UniqueID];
            entity.NoSKTM = Request.Form[txtNoSKTM.UniqueID];
            entity.ProlanisPRB = Request.Form[txtPRB.UniqueID];
            entity.KodeSubSpesialis = Request.Form[txtSubSpesialisCode.UniqueID];
            entity.NamaSubSpesialis = Request.Form[txtSubSpesialisName.UniqueID];

            if (cboVisitReason.Value != null)
            {
                if (cboVisitReason.Value.ToString() == Constant.VisitReason.ACCIDENT)
                {
                    entity.IsAccident = true;
                    entity.Suplesi = chkIsSuplesi.Checked;
                    entity.AccidentLocation = txtAccidentLocation.Text;
                    if (!string.IsNullOrEmpty(hdnGCState.Value) && hdnGCState.Value != "0")
                    {
                        entity.GCState = hdnGCState.Value;
                        entity.KodePropinsi = hdnKodePropinsiBPJS.Value;
                    }
                    if (!string.IsNullOrEmpty(hdnKabupatenID.Value) && hdnKabupatenID.Value != "0")
                    {
                        entity.KabupatenID = Convert.ToInt32(hdnKabupatenID.Value);
                        entity.KodeKabupaten = hdnKodePropinsiBPJS.Value;
                    }
                    if (!string.IsNullOrEmpty(hdnKecamatanID.Value) && hdnKecamatanID.Value != "0")
                    {
                        entity.KecamatanID = Convert.ToInt32(hdnKabupatenID.Value);
                        entity.KodeKecamatan = hdnKodeKecamatanBPJS.Value;
                    }

                    if (chkIsSuplesi.Checked)
                    {
                        entity.NoSEPSuplesi = txtNoSEPSuplesi.Text;
                    }

                    string accidentPayer = string.Empty;
                    if (chkBPJSAccidentPayer1.Checked)
                        accidentPayer = "1";
                    if (chkBPJSAccidentPayer2.Checked)
                    {
                        if (string.IsNullOrEmpty(accidentPayer))
                            accidentPayer = "2";
                        else
                            accidentPayer = string.Format("{0},{1}", accidentPayer, "2");
                    }
                    if (chkBPJSAccidentPayer3.Checked)
                    {
                        if (string.IsNullOrEmpty(accidentPayer))
                            accidentPayer = "3";
                        else
                            accidentPayer = string.Format("{0},{1}", accidentPayer, "3");
                    }
                    if (chkBPJSAccidentPayer4.Checked)
                    {
                        if (string.IsNullOrEmpty(accidentPayer))
                            accidentPayer = "4";
                        else
                            accidentPayer = string.Format("{0},{1}", accidentPayer, "4");
                    }
                    entity.AccidentPayer = accidentPayer;
                }
            }

            entity.KodeDiagnosa = hdnBPJSDiagnoseCode.Value;
            entity.NamaDiagnosa = Request.Form[txtDiagnoseName.UniqueID];
            entity.Keluhan = Request.Form[txtDiagnoseText.UniqueID];
            entity.Catatan = Request.Form[txtVisitNotes.UniqueID];
            entity.IsCataract = hdnIsCataract.Value == "1";
        }

        private void ControlToEntityPatientDiagnosis(PatientDiagnosis entity)
        {
            entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entity.DiagnoseID = txtDiagnoseCode.Text;
            entity.DiagnosisText = txtDiagnoseText.Text;
            entity.GCDiagnoseType = Constant.DiagnoseType.EARLY_DIAGNOSIS;
            entity.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
            entity.DifferentialDate = Helper.GetDatePickerValue(txtRegistrationDate);
            entity.DifferentialTime = txtRegistrationHour.Text;
        }


        private bool OnSaveAddRecord(ref string errMessage, int RegistrationID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao entityDtDao = new RegistrationBPJSDao(ctx);
            try
            {
                bool flagRegistrationBPJSExist = true;
                RegistrationBPJS entity = entityDtDao.Get(RegistrationID);
                if (entity == null)
                {
                    entity = new RegistrationBPJS();
                    entity.RegistrationID = RegistrationID;
                    flagRegistrationBPJSExist = false;
                }
                ControlToEntity(entity);

                if (!string.IsNullOrEmpty(txtAppointmentNo.Text))
                {
                    vRegistrationBPJSInfo obj = BusinessLayer.GetvRegistrationBPJSInfoList(string.Format("AppointmentNo = '{0}'", txtAppointmentNo.Text), ctx).FirstOrDefault();
                    if (obj != null)
                    {
                        entity.NoRujukanKe = obj.NoRujukanKe;
                        entity.KodeDPJPRujukan = obj.KodeDPJPRujukan;
                    }
                }

                int hakKelas = Convert.ToInt16(entity.KelasTanggungan);
                if (entity.JenisPelayanan == "1")
                {
                    if (!string.IsNullOrEmpty(hdnChargeClassBPJSType.Value))
                    {
                        if (Convert.ToInt16(hdnChargeClassBPJSType.Value) >= hakKelas)
                            entity.KelasSEP = Convert.ToInt16(hdnChargeClassBPJSType.Value);
                        else
                            entity.KelasSEP = hakKelas;
                    }

                }
                entity.KelasSEP = hakKelas;

                if (!flagRegistrationBPJSExist)
                {
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entity);
                }
                else
                {
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entity);
                }

                ConsultVisit visit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
                Registration regis = BusinessLayer.GetRegistration(RegistrationID);
                if (regis != null)
                {
                    RegistrationDao registrationDao = new RegistrationDao(ctx);
                    regis.ReferralNo = txtReferralNo.Text;
                    regis.LastUpdatedBy = AppSession.UserLogin.UserID;
                    registrationDao.Update(regis);
                }

                if (visit != null)
                {
                    PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
                    PatientDiagnosis diffDx = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}'", visit.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS)).FirstOrDefault();
                    if (diffDx == null)
                    {
                        diffDx = new PatientDiagnosis();
                        ControlToEntityPatientDiagnosis(diffDx);
                        if ((diffDx.DiagnoseID != "" && diffDx.DiagnoseID != null) || (diffDx.DiagnosisText.Trim() != "" && diffDx.DiagnosisText.Trim() != null))
                        {
                            diffDx.VisitID = visit.VisitID;
                            diffDx.CreatedBy = AppSession.UserLogin.UserID;
                            patientDiagnosisDao.Insert(diffDx);
                        }
                    }
                    else
                    {
                        ControlToEntityPatientDiagnosis(diffDx);
                        diffDx.VisitID = visit.VisitID;
                        diffDx.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientDiagnosisDao.Update(diffDx);
                    }
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            string url = "";
            if (type == "transaction")
            {
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0} AND IsMainVisit = 1", hdnVisitID.Value)).FirstOrDefault();
                SetSessionRegisteredPatient(entity);

                if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                {
                    url = string.Format("~/Program/Prescription/PrescriptionEntry/PrescriptionEntryDetail.aspx?id=ptp|{0}|{1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
                }
                else
                {
                    hdnRegistrationButtonTransactionDirectMenu.Value = AppSession.RegistrationButtonTransactionDirectMenu;

                    if (hdnRegistrationButtonTransactionDirectMenu.Value == "1")
                    {
                        if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                        {

                            SettingParameterDt setvarRealisasi = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MENU_REALISASI_TAMPIL_HARGA_TIDAK);
                            if (setvarRealisasi != null)
                            {
                                if (setvarRealisasi.ParameterValue == "0")
                                {
                                    url = "~/Libs/Program/Module/PatientManagement/TransactionPage/PatientUse/PatientUse.aspx";
                                }
                                else
                                {
                                    url = "~/Libs/Program/Module/PatientManagement/TransactionPage/Charges/TransactionPageCharges.aspx";
                                }
                            }
                            else
                            {
                                url = "~/Libs/Program/Module/PatientManagement/TransactionPage/Charges/TransactionPageCharges.aspx";
                            }
                        }
                    }
                    else
                    {
                        if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                        {
                            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                            {
                                String temp = String.Empty;
                                url = string.Format("~/Program/Worklist/LaboratoryOrder/LaboratoryOrderDetail.aspx?id={0}|{1}|{2}", hdnVisitID.Value, temp, hdnHealthcareServiceUnitID.Value);
                            }
                            else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                            {
                                url = string.Format("~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetail.aspx?id={0}|{1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
                            }
                            else
                            {
                                url = string.Format("~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetail.aspx?id={0}|{1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
                            }
                        }
                        else
                        {
                            url = string.Format("~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetail.aspx?id={0}|{1}", hdnVisitID.Value, hdnHealthcareServiceUnitID.Value);
                        }
                    }
                }
                retval = url;
            }
            return true;
        }

        private void SetSessionRegisteredPatient(vConsultVisit entity)
        {
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.DateOfBirth = entity.DateOfBirth;
            AppSession.RegisteredPatient = pt;
        }

        private bool OnUpdatePrintNumber(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao entityDao = new RegistrationBPJSDao(ctx);
            try
            {
                RegistrationBPJS entity = entityDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                entity.PrintNumber += 1;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
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
    }
}