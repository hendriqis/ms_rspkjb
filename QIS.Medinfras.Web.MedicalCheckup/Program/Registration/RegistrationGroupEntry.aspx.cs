using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.IO;
using System.Data.OleDb;
using DevExpress.Web.ASPxUploadControl;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class RegistrationGroupEntry : BasePageTrx
    {
        protected string maxBackDate = "";
        protected int serviceUnitUserCount = 0;
        protected string filterExpressionOtherMedicalDiagnostic = "";
        private const string LABORATORY = "LABORATORY";
        private const string IMAGING = "IMAGING";
        private const string PHARMACY = "PHARMACY";
        private const string REGISTRATION_ORDER_CLAUSE = "RegistrationID DESC";
        private bool isAdd = false;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.REGISTRATION_GROUP;
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
            return string.Format("ItemID IN (SELECT ItemID FROM vItemService WHERE GCItemType = '{0}' AND IsPackageItem = 1) AND IsDeleted = 0", Constant.ItemGroupMaster.MEDICAL_CHECKUP);
        }

        protected string GetCustomerContractFilterExpression()
        {
            return string.Format(" AND EndDate >= CONVERT(DATE,GetDate()) AND IsDeleted = 0", hdnPayerID.Value, DateTime.Now.ToString("yyyyMMdd"));
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
            hdnDepartmentID.Value = Constant.Facility.MEDICAL_CHECKUP;
            vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();
            hdnIsServiceUnitHasParamedic.Value = hsu.IsHasParamedic ? "1" : "0";
            hdnIsServiceUnitHasVisitType.Value = hsu.IsHasVisitType ? "1" : "0";
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";
            GetSettingParameter();
            InitializeControlProperties();
        }

        private void GetSettingParameter()
        {
            //Global Settting Parameter
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')",
                Constant.SettingParameter.MAX_BACK_DATE, Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU));

            maxBackDate = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MAX_BACK_DATE).ParameterValue;

            //Healthcare Setting Parameter
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.FN_KONTROL_BIAYA_KARTU,
                Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU,
                Constant.SettingParameter.FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ,
                Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI,
                Constant.SettingParameter.OP_PENDAFTARAN_DENGAN_RUANG,
                Constant.SettingParameter.FN_KONTROL_BIAYA_ADM_PASIEN_RJ,
                Constant.SettingParameter.SA_CHECK_DATA_PASIEN,
                Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI,
                Constant.SettingParameter.SA_TIPE_SEARCH_DIALOG_PASIEN,
                Constant.SettingParameter.FN_WARNING_PASIEN_ADA_PIUTANG_PRIBADI));

            hdnItemCardFee.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU).ParameterValue;
            hdnIsControlPatientCardPayment.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_BIAYA_KARTU).ParameterValue;
            hdnIsControlAdministrationCharges.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ).ParameterValue;
            hdnChargeCodeAdministrationForInstansi.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI).ParameterValue;
            hdnIsOutpatientUsingRoom.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP_PENDAFTARAN_DENGAN_RUANG).ParameterValue;
            hdnIsControlAdmCost.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_BIAYA_ADM_PASIEN_RJ).ParameterValue;
            hdnIsCheckNewPatient.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_CHECK_DATA_PASIEN).ParameterValue;
            hdnAdminID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI).ParameterValue;
            hdnIsWarningPatientHaveAR.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_WARNING_PASIEN_ADA_PIUTANG_PRIBADI).ParameterValue;
            hdnPatientSearchDialogType.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_TIPE_SEARCH_DIALOG_PASIEN).ParameterValue;
        }

        protected string GetPageTitle()
        {
            return BusinessLayer.GetMenuMasterList(string.Format("MenuCode='{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void InitializeControlProperties()
        {
            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                string departmentID = Page.Request.QueryString["id"];
                if (departmentID != LABORATORY && departmentID != IMAGING)
                    filterExpressionOtherMedicalDiagnostic = string.Format("HealthcareServiceUnitID NOT IN ({0},{1})", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
            }
            serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", hdnDepartmentID.Value, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REFERRAL, Constant.StandardCode.CUSTOMER_TYPE, Constant.StandardCode.ADMISSION_SOURCE, Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            List<StandardCode> lstAdmissionSource = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_SOURCE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboVisitReason, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.VISIT_REASON || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.REFERRAL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRegistrationPayer, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAdmissionCondition, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_CONDITION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            cboRegistrationPayer.SelectedIndex = 0;

            cboVisitReason.SelectedIndex = 0;
            cboAdmissionCondition.SelectedIndex = 0;

            List<Specialty> lstSpecialty = BusinessLayer.GetSpecialtyList("IsDeleted = 0");
            Methods.SetComboBoxField<Specialty>(cboSpecialty, lstSpecialty, "SpecialtyName", "SpecialtyID");
            cboSpecialty.SelectedIndex = 0;

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            Methods.SetComboBoxField<ClassCare>(cboControlClassCare, lstClassCare, "ClassName", "ClassID");
            cboControlClassCare.SelectedIndex = 0;
            cboRegistrationPayer.Value = Constant.CustomerType.PERSONAL;
            hdnGCTariffSchemePersonal.Value = BusinessLayer.GetCustomer(1).GCTariffScheme;
        }

        protected override void SetControlProperties()
        {
            string filterExpression = OnGetRegistrationNoFilterExpression();
            filterExpression += string.Format(" AND RegistrationDate = '{0}' ORDER BY CreatedDate DESC", DateTime.Now.ToString("yyyy-MM-dd"));
            vRegistration entity = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();
            if (entity != null && hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                txtPhysicianCode.Text = entity.ParamedicCode;
                txtPhysicianName.Text = entity.ParamedicName;
                cboSpecialty.Value = entity.SpecialtyID;
            }

            if (isAdd)
            {
                btnReferral.Attributes.Add("style", "display:block");
            }
            else
            {
                btnReferral.Attributes.Add("style", "display:none");
            }

            tblPayerCompany.Style.Remove("display");
            chkUsingCOB.Style.Remove("display");

            if (cboRegistrationPayer.Value.ToString() == Constant.CustomerType.PERSONAL)
            {
                tblPayerCompany.Style.Add("display", "none");
                chkUsingCOB.Style.Add("display", "none");
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnIsNewPatient, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnIsAdd, new ControlEntrySetting(false, false, false, "0"));
            hdnIsAllowBackDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IS_MEDICAL_CHECKUP_ALLOW_BACK_DATE).ParameterValue;
            SetControlEntrySetting(hdnRegistrationStatus, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting((hdnIsAllowBackDate.Value == "1"), false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtRegistrationHour, new ControlEntrySetting((hdnIsAllowBackDate.Value == "1"), false, true, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(txtRegistrar, new ControlEntrySetting(false, false, false, AppSession.UserLogin.UserName));
            SetControlEntrySetting(chkCardFee, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(chkIsUsingCOB, new ControlEntrySetting(true, false, false, false));
            SetControlEntrySetting(hdnMRN, new ControlEntrySetting(true, true));
            if (hdnDepartmentID.Value == Constant.Facility.MEDICAL_CHECKUP)
            {
                SetControlEntrySetting(hdnClassID, new ControlEntrySetting(true, true, false, BusinessLayer.GetSettingParameter(Constant.SettingParameter.MEDICAL_CHECKUP_CLASS).ParameterValue));
                vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true, false, hsu.HealthcareServiceUnitID.ToString()));
                SetControlEntrySetting(hdnIsServiceUnitHasParamedic, new ControlEntrySetting(true, true, false, hsu.IsHasParamedic ? "1" : "0"));
                SetControlEntrySetting(hdnIsServiceUnitHasVisitType, new ControlEntrySetting(true, true, false, hsu.IsHasVisitType ? "1" : "0"));
            }
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
            SetControlEntrySetting(txtDiagnoseCode, new ControlEntrySetting(true, false, false, string.Empty));
            SetControlEntrySetting(hdnBPJSDiagnoseCode, new ControlEntrySetting(true, false, false, string.Empty));
            SetControlEntrySetting(txtDiagnoseText, new ControlEntrySetting(true, false, false, string.Empty));
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
            SetControlEntrySetting(hdnCoverageTypeID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCoverageTypeCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtCoverageTypeName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnEmployeeID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtEmployeeCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtEmployeeName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtParticipantNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtCoverageLimit, new ControlEntrySetting(true, false, true, "0"));
            SetControlEntrySetting(hdnIsControlClassCare, new ControlEntrySetting(true, false, true, "0"));
            SetControlEntrySetting(cboControlClassCare, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(lblItemMCU, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblImportFile, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblPhysician, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblVisitType, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblReferralDescription, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblPayerCompany, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblCoverageType, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblContract, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblEmployee, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnVisitID, new ControlEntrySetting(true, true));
        }

        public override void OnAddRecord()
        {
            isAdd = true;
        }

        #region Load Entity
        protected string OnGetRegistrationNoFilterExpression()
        {
            string filterExpression = "";
            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
                    filterExpression = string.Format("HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID);
                else
                    filterExpression = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1},{2})", hdnDepartmentID.Value, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
            }
            else
                filterExpression = string.Format("DepartmentID = '{0}'", hdnDepartmentID.Value);
            return filterExpression;
        }

        protected string OnGetMotherRegistrationNoFilterExpression()
        {
            return string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND DepartmentID = '{3}' AND IsParturition = 1", Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, hdnDepartmentID.Value);
        }
        #endregion

        #region Save Entity
        private void ControlToEntity(Registration entity, RegistrationPayer entityRegistrationPayer, ConsultVisit entityVisit, PatientDiagnosis entityPatientDiagnosis, Guest entityGuest)
        {
            ControlToEntity(entity, entityRegistrationPayer, entityVisit, entityPatientDiagnosis);

            #region Patient Data
            entityGuest.GCSalutation = hdnGuestGCSalutation.Value;
            entityGuest.GCTitle = hdnGuestGCTitle.Value;
            entityGuest.FirstName = hdnGuestFirstName.Value;
            entityGuest.MiddleName = hdnGuestMiddleName.Value;
            entityGuest.LastName = hdnGuestLastName.Value;
            entityGuest.GCSuffix = hdnGuestGCSuffix.Value;
            entityGuest.GCGender = hdnGuestGCGender.Value;
            entityGuest.DateOfBirth = Helper.GetDatePickerValue(hdnGuestDateOfBirth.Value);
            #endregion

            #region Patient Address
            entityGuest.StreetName = hdnGuestStreetName.Value;
            entityGuest.County = hdnGuestCounty.Value; // Desa
            entityGuest.District = hdnGuestDistrict.Value; //Kabupaten
            entityGuest.City = hdnGuestCity.Value;
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
                registrationDate = Helper.GetDatePickerValue(txtRegistrationDate);
                registrationTime = txtRegistrationHour.Text;
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
            //entity.IsPregnant = chkIsPregnant.Checked;

            //if (chkIsHasMRN.Checked)
            //{
                entity.MRN = Convert.ToInt32(hdnMRN.Value);
                Registration reg = BusinessLayer.GetRegistrationList(String.Format("MRN = {0} AND GCRegistrationStatus != '{1}'" ,entity.MRN ,Constant.VisitStatus.CANCELLED)).FirstOrDefault();
                if (reg != null)
                {
                    entity.IsNewPatient = false;
                }
                else
                {
                    Patient pat = BusinessLayer.GetPatient(Convert.ToInt32(hdnMRN.Value));
                    if (pat.RegisteredDate == entity.RegistrationDate)
                    {
                        entity.IsNewPatient = true;
                    }
                    else
                    {
                        entity.IsNewPatient = false;
                    }
                }
            //}
            //else
            //{
            //    entity.MRN = null;
            //    entity.IsNewPatient = true;
            //}

            //entity.AgeInYear = Convert.ToInt16(Request.Form[txtAgeInYear.UniqueID]);
            //entity.AgeInMonth = Convert.ToInt16(Request.Form[txtAgeInMonth.UniqueID]);
            //entity.AgeInDay = Convert.ToInt16(Request.Form[txtAgeInDay.UniqueID]);

            entityVisit.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entityVisit.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);

            if (cboSpecialty.Value != null && cboSpecialty.Value.ToString() != "")
                entityVisit.SpecialtyID = cboSpecialty.Value.ToString();
            else
                entityVisit.SpecialtyID = null;
            entityVisit.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);

            if (hdnReferrerID.Value == "" || hdnReferrerID.Value == "0")
                entity.ReferrerID = null;
            else
                entity.ReferrerID = Convert.ToInt32(hdnReferrerID.Value);

            if (hdnReferrerParamedicID.Value == "" || hdnReferrerParamedicID.Value == "0")
                entity.ReferrerParamedicID = null;
            else
                entity.ReferrerParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);


            entityVisit.ChargeClassID = entityVisit.ClassID = Convert.ToInt32(hdnClassID.Value);
            //if ((hdnIsControlClassCare.Value == "1")) entityVisit.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);
            entityVisit.RoomID = null;
            entityVisit.BedID = null;
            entity.LinkedRegistrationID = null;

            entity.IsNewBorn = false;
            entity.IsParturition = false;

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
            }
            else
            {
                entity.BusinessPartnerID = Convert.ToInt32(hdnPayerID.Value);
                entity.ContractID = Convert.ToInt32(hdnContractID.Value);
                entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                entity.CorporateAccountNo = txtParticipantNo.Text;
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
                entityRegistrationPayer.CorporateAccountNo = txtParticipantNo.Text;
                //entityRegistrationPayer.CorporateAccountName = txtPatientName.Text;
                entityRegistrationPayer.CoverageLimitAmount = Convert.ToDecimal(txtCoverageLimit.Text);
                entityRegistrationPayer.IsCoverageLimitPerDay = chkIsCoverageLimitPerDay.Checked;
                if (entity.IsControlClassCare)
                    entityRegistrationPayer.ControlClassID = Convert.ToInt32(cboControlClassCare.Value);
                else
                    entityRegistrationPayer.ControlClassID = null;
                entityRegistrationPayer.IsPrimaryPayer = true;

            }
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

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            return true;
        }

        protected void upcFileUploadExcel_FileUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs e)
        {
            UploadedFile file = e.UploadedFile;
            string FileName = Path.GetFileName(file.FileName);
            string Extension = Path.GetExtension(file.FileName);
            string FilePath = AppConfigManager.QISPhysicalDirectory + FileName;
            file.SaveAs(FilePath, true);
            ImportToList(FilePath, Extension);
        }

        private void ImportToList(string FilePath, string Extension)
        {
            string conStr = "";

            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                    break;
                case ".xlsx": //Excel 07
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
                    break;
            }

            conStr = String.Format(conStr, FilePath, "Yes");
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();

            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;
            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            SheetName = "Test$";
            connExcel.Close();
            //Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            foreach (DataRow row in dt.Rows) 
            {
                foreach(DataColumn col in dt.Columns)
                {
                    string temp = row[col.ColumnName].ToString();
                }
            }
            connExcel.Close();

            //Bind Data to GridView
            //GridView1.Caption = Path.GetFileName(FilePath);
            //GridView1.DataSource = dt;
            //GridView1.DataBind();
        }


        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityDao = new RegistrationDao(ctx);
            PatientDao entityPatientDao = new PatientDao(ctx);
            ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);
            SettingParameterDao entitySettingParameterDao = new SettingParameterDao(ctx);
            PatientDiagnosisDao entityPatientDiagnosisDao = new PatientDiagnosisDao(ctx);
            RegistrationPayerDao entityRegistrationPayerDao = new RegistrationPayerDao(ctx);
            try
            {
                Registration entity = new Registration();
                ConsultVisit entityVisit = new ConsultVisit();
                RegistrationPayer entityRegistrationPayer = new RegistrationPayer();
                PatientDiagnosis entityPatientDiagnosis = new PatientDiagnosis();
                Patient entityPatient = null;
                ControlToEntity(entity, entityRegistrationPayer, entityVisit, entityPatientDiagnosis);
                entity.TransactionCode = Constant.TransactionCode.MCU_REGISTRATION;
                string registrationStatus = Constant.VisitStatus.CHECKED_IN;
                entity.RegistrationNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entityVisit.ActualVisitDate, ctx);
                entity.GCRegistrationStatus = registrationStatus;
                entity.GCMedicalFileStatus = null;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                //if (chkIsHasMRN.Checked)
                //{
                    entityPatient = entityPatientDao.Get((int)entity.MRN);
                    entity.GCDependentType = entityPatient.GCDependentType;
                    entity.GCPatientCategory = entityPatient.GCPatientCategory;
                //}

                if (cboRegistrationPayer.Value.ToString() == Constant.CustomerType.BPJS)
                    entity.IsBPJS = true;

                entityDao.Insert(entity);
                entity.RegistrationID = BusinessLayer.GetRegistrationMaxID(ctx);

                if (entityRegistrationPayer.GCCustomerType != Constant.CustomerType.PERSONAL)
                {
                    entityRegistrationPayer.CreatedBy = AppSession.UserLogin.UserID;
                    entityRegistrationPayer.RegistrationID = entity.RegistrationID;
                    entityRegistrationPayerDao.Insert(entityRegistrationPayer);
                }
                entityVisit.RegistrationID = entity.RegistrationID;
                entityVisit.GCVisitStatus = registrationStatus;
                entityVisit.CreatedBy = AppSession.UserLogin.UserID;
                entityVisit.IsMainVisit = true;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityVisitDao.Insert(entityVisit);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityVisit.VisitID = BusinessLayer.GetConsultVisitMaxID(ctx);

                if (txtDiagnoseCode.Text != string.Empty)
                {
                    entityPatientDiagnosis.VisitID = entityVisit.VisitID;
                    entityPatientDiagnosis.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientDiagnosisDao.Insert(entityPatientDiagnosis);
                }

                if (registrationStatus == Constant.VisitStatus.CHECKED_IN)
                {
                    String ChargeCodeAdministration = string.Empty;
                    bool flagSkipAdm = false;
                    if (hdnIsControlAdministrationCharges.Value == "1" && entityRegistrationPayer.GCCustomerType != Constant.CustomerType.PERSONAL) ChargeCodeAdministration = hdnChargeCodeAdministrationForInstansi.Value;
                    int itemMCUID = 0;
                    if (hdnItemID.Value != "" && hdnItemID.Value != "0")
                        itemMCUID = Convert.ToInt32(hdnItemID.Value);
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
                        Helper.InsertAutoBillItem(ctx, entityVisit, hdnDepartmentID.Value, (int)entityVisit.ChargeClassID, entity.GCCustomerType, entity.IsPrintingPatientCard, itemMCUID, ChargeCodeAdministration, flagSkipAdm);
                    }
                    else
                    {
                        if (entity.IsPrintingPatientCard && hdnItemCardFee.Value != "")
                        {
                            Helper.InsertPatientCardBillItem(ctx, entityVisit, hdnDepartmentID.Value, (int)entityVisit.ChargeClassID);
                        }
                        Helper.InsertAutoBillItem(ctx, entityVisit, hdnDepartmentID.Value, (int)entityVisit.ChargeClassID, entity.GCCustomerType, false, itemMCUID, ChargeCodeAdministration, flagSkipAdm);
                    }
                }

                ctx.CommitTransaction();
                retval = entity.RegistrationNo;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        
    }
}