using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxEditors;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PatientDataEntry : BasePageEntry
    {
        protected string dateNow = "";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_DATA;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetTodayDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                SetControlProperties();
                String ID = Request.QueryString["id"];
                hdnMRN.Value = ID;
                vPatient entity = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", ID))[0];
                PatientTagField entityTagField = BusinessLayer.GetPatientTagField(entity.MRN);
                EntityToControl(entity, entityTagField);
                vMrnmergehistory entityMerge = BusinessLayer.GetvMrnmergehistoryList(String.Format("ToMRN = {0}", hdnMRN.Value)).FirstOrDefault();
                trPayerCompany.Style.Remove("display");

                if (entityMerge != null)
                {
                    txtMRNMerge.Text = entityMerge.FromMedicalNo;
                    trInformationMergeMRN.Style.Remove("display");
                }

                if (cboPayer.Value != null)
                {
                    if (cboPayer.Value.ToString() == Constant.CustomerType.PERSONAL)
                    {
                        trPayerCompany.Style.Add("display", "none");
                    }
                }
                else
                {
                    trPayerCompany.Style.Add("display", "none");
                }

                if (entity.IsVIP)
                {
                    tdVIPGroup.Style.Remove("display");

                    if (cboVIPGroup.Value.ToString() == Constant.VIPPatientGroup.VIP_OTHER)
                    {
                        trVIPGroupOther.Style.Remove("display");
                    }
                }
                if (entity.IsHasCommunicationRestriction)
                {
                    tdCommunication.Style.Remove("display");
                }

                if (entity.IsHasPhysicalLimitation)
                {
                    tdPhysicalLimitation.Style.Remove("display");
                }

                if (entity.IsBlacklist)
                {
                    tdGCBlacklistReason.Style.Remove("display");

                    if (cboGCBlacklistReason.Value.ToString() == Constant.Patient_BlackList_Reason.OTHER)
                    {
                        trOtherBlackListReason.Style.Remove("display");
                    }
                }
            }
            else
            {
                hdnMRN.Value = "";
                IsAdd = true;
                SetControlProperties();
            }
            txtMRN.Focus();
            dateNow = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            GetSettingParameter();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}', '{2}')",
                Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS,
                Constant.SettingParameter.RM_BLOK_DOUBLE_PATIENT_DATA,
                Constant.SettingParameter.RM_IS_MOBILE_PHONE_NUMERIC
                ));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
            hdnIsBlockDoublePatientData.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM_BLOK_DOUBLE_PATIENT_DATA).FirstOrDefault().ParameterValue;
            hdnIsMobilePhoneNumeric.Value = lstSetParDt.Where(p => p.ParameterCode == Constant.SettingParameter.RM_IS_MOBILE_PHONE_NUMERIC).FirstOrDefault().ParameterValue;

        }

        #region Popup Filter Expression
        protected string OnGetSCEthnicFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ETHNIC);
        }
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }
        protected string OnGetSCEducationFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.EDUCATION);
        }
        protected string OnGetSCReligionFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.RELIGION);
        }
        protected string OnGetSCNationalityFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.NATIONALITY);
        }
        protected string OnGetSCMaritalStatusFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.MARITAL_STATUS);
        }
        protected string OnGetSCPatientCategoryFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_CATEGORY);
        }
        protected string OnGetSCPatientJobFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.OCCUPATION);
        }
        protected string OnGetSCLanguageFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.LANGUAGE);
        }
        #endregion

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format(
                "ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.TITLE, Constant.StandardCode.SALUTATION, Constant.StandardCode.SUFFIX, Constant.StandardCode.BLOOD_TYPE,
                Constant.StandardCode.IDENTITY_NUMBERY_TYPE, Constant.StandardCode.GENDER, Constant.StandardCode.CUSTOMER_TYPE, Constant.StandardCode.PATIENT_STATUS, Constant.StandardCode.VIP_PATIENT_GROUP, Constant.StandardCode.PATIENT_BLACKLIST_REASON, Constant.StandardCode.COMINICATION, Constant.StandardCode.PHYSICAL_LIMITATION_TYPE));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSalutation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTitle, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffix, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBloodType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.BLOOD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboIdentityCardType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.IDENTITY_NUMBERY_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGender, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.GENDER).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboVIPGroup, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.VIP_PATIENT_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCBlacklistReason, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_BLACKLIST_REASON).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCommunication, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.COMINICATION).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPhysicalLimitation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.PHYSICAL_LIMITATION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            List<Variable> lstBloodRhesus = new List<Variable>();
            lstBloodRhesus.Add(new Variable { Code = "+", Value = "+" });
            lstBloodRhesus.Add(new Variable { Code = "-", Value = "-" });
            Methods.SetComboBoxField<Variable>(cboBloodRhesus, lstBloodRhesus, "Value", "Code");

            cboSalutation.SelectedIndex = 0;
            cboTitle.SelectedIndex = 0;
            cboSuffix.SelectedIndex = 0;
            cboBloodType.SelectedIndex = 0;
            cboIdentityCardType.SelectedIndex = 0;
            cboGender.SelectedIndex = 0;
            cboBloodRhesus.SelectedIndex = 0;
            cboVIPGroup.SelectedIndex = 0;
            cboGCBlacklistReason.SelectedIndex = 0;
            cboCommunication.SelectedIndex = 0;
            cboPhysicalLimitation.SelectedIndex = 0;

            List<StandardCode> lstPatientStatus = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.PATIENT_STATUS));
            Methods.SetComboBoxField<StandardCode>(cboPatientStatus, lstPatientStatus, "StandardCodeName", "StandardCodeID");
            cboPatientStatus.SelectedIndex = 0;

            List<StandardCode> lstVIPGroup = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.VIP_PATIENT_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboVIPGroup, lstVIPGroup, "StandardCodeName", "StandardCodeID");
            cboVIPGroup.SelectedIndex = 0;

            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_BLACKLIST_REASON));
            Methods.SetComboBoxField<StandardCode>(cboGCBlacklistReason, lstSC, "StandardCodeName", "StandardCodeID");
            cboGCBlacklistReason.SelectedIndex = 0;

            List<StandardCode> lstComunication = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.COMINICATION));
            Methods.SetComboBoxField<StandardCode>(cboCommunication, lstComunication, "StandardCodeName", "StandardCodeID");
            cboCommunication.SelectedIndex = 0;

            List<StandardCode> lstPhysicalLimitation = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PHYSICAL_LIMITATION_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboPhysicalLimitation, lstPhysicalLimitation, "StandardCodeName", "StandardCodeID");
            cboPhysicalLimitation.SelectedIndex = 0;

            List<StandardCode> lstCustomerType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CUSTOMER_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboPayer, lstCustomerType, "StandardCodeName", "StandardCodeID");
            cboPayer.SelectedIndex = 0;

            cboPayer.Value = Constant.CustomerType.PERSONAL;

            trPayerCompany.Style.Remove("display");
            if (cboPayer.Value.ToString() == Constant.CustomerType.PERSONAL)
                trPayerCompany.Style.Add("display", "none");
        }

        protected override void OnControlEntrySetting()
        {
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string defaultGCState = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];
            string defaultPhoneArea = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PHONE_AREA).ParameterValue;

            SetControlEntrySetting(txtMRN, new ControlEntrySetting(false, false, false));

            #region Patient Data

            SetControlEntrySetting(txtEKlaimMedicalNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSITBRegisterNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFamilyCardNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTitle, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFamilyName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPreferredName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboSuffix, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEthnicCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEthnicName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBirthPlace, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGender, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBloodType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboBloodRhesus, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDOB, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtAgeInDay, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtAgeInMonth, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtAgeInYear, new ControlEntrySetting(true, true, true, 0));
            #endregion

            #region Patient Address
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRTData, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRWData, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProvinceCode, new ControlEntrySetting(true, true, false, defaultGCState));
            SetControlEntrySetting(txtProvinceName, new ControlEntrySetting(false, false, false, healthcare.State));
            SetControlEntrySetting(hdnZipCode, new ControlEntrySetting(true, true));
            //            SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, true));
            #endregion

            #region Patient Contact
            SetControlEntrySetting(txtTelephoneNo1, new ControlEntrySetting(true, true, true, defaultPhoneArea));
            SetControlEntrySetting(txtTelephoneNo2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMobilePhone1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMobilePhone2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmail, new ControlEntrySetting(true, true, false));
            #endregion

            #region Additonal Information
            //SetControlEntrySetting(txtEducationCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEducationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtReligionCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReligionName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMaritalStatusCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMaritalStatusName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNationalityCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNationalityName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPatientCategoryCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPatientCategoryName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnEmployeeID, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmployeeCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmployeeName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtlanguageCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtlanguageName, new ControlEntrySetting(false, false, false));
            #endregion

            #region Patient Payer
            SetControlEntrySetting(cboPayer, new ControlEntrySetting(true, true, true, Constant.CustomerType.PERSONAL));
            SetControlEntrySetting(hdnPayerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPayerCompanyCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPayerCompanyName, new ControlEntrySetting(false, false, false));
            #endregion

            #region Patient Job
            //SetControlEntrySetting(txtPatientJobCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPatientJobName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtPatientJobOffice, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPatientJobOfficeAddress, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRTOfficeData, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRWOfficeData, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPatientJobOfficeCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPatientJobOfficeDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPatientJobOfficeCity, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPatientJobOfficeProvinceCode, new ControlEntrySetting(true, true, false, defaultGCState));
            SetControlEntrySetting(txtPatientJobOfficeProvinceName, new ControlEntrySetting(false, false, false, healthcare.State));
            SetControlEntrySetting(hdnOfficeZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPatientJobOfficeZipCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPatientJobOfficeTelephone, new ControlEntrySetting(true, true, false, defaultPhoneArea));
            SetControlEntrySetting(txtPatientJobOfficeEmail, new ControlEntrySetting(true, true, false));
            #endregion

            #region Patient Status
            SetControlEntrySetting(cboPatientStatus, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAlive, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBlackList, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCBlacklistReason, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtOtherBlackListReason, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsVIP, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboVIPGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtOtherVIPGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsDonor, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsG6PD, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasInfectious, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasAllergy, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsIlliteracy, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsSmoking, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasCommunicationRestriction, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboCommunication, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsHasPhysicalLimitation, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboPhysicalLimitation, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsPregnant, new ControlEntrySetting(true, true, false));
            #endregion

            #region Other Information
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCorporateAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCorporateAccountName, new ControlEntrySetting(true, true, false));
            #endregion

            #region setvar Mandatory

            string filterSetvar = string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                                                    Constant.SettingParameter.IS_FIRST_NAME_MANDATORY,
                                                    Constant.SettingParameter.IS_MIDDLE_NAME_ALLOW_NULL,
                                                    Constant.SettingParameter.IS_ZIP_CODE_MANDATORY,
                                                    Constant.SettingParameter.IS_OCCUPATION_PATIENT_MANDATORY,
                                                    Constant.SettingParameter.IS_EDUCATION_PATIENT_MANDATORY,
                                                    Constant.SettingParameter.RM_IS_SSN_MANDATORY,
                                                    Constant.SettingParameter.RM_IS_SALUTATION_MANDATORY
                                                    );
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            string isFirstNameMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_FIRST_NAME_MANDATORY).FirstOrDefault().ParameterValue;
            string isMiddleNameAllowNull = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_MIDDLE_NAME_ALLOW_NULL).FirstOrDefault().ParameterValue;
            string isZipCodeMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_ZIP_CODE_MANDATORY).FirstOrDefault().ParameterValue;
            string isOccupationMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_OCCUPATION_PATIENT_MANDATORY).FirstOrDefault().ParameterValue;
            string isEducationMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_EDUCATION_PATIENT_MANDATORY).FirstOrDefault().ParameterValue;
            string isSSNMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.RM_IS_SSN_MANDATORY).FirstOrDefault().ParameterValue;
            string isSalutationMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.RM_IS_SALUTATION_MANDATORY).FirstOrDefault().ParameterValue;

            if (isFirstNameMandatory == "1")
            {
                SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, true, true));
            }
            else
            {
                SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, true, false));
                lblPatientName.Attributes.Remove("class");
                lblPatientName.Attributes.Add("class", "lblNormal");
            }

            if (isMiddleNameAllowNull == "1")
            {
                SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(false, false, false));
            }
            else
            {
                SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(true, true, false));
            }

            if (isZipCodeMandatory == "1")
            {
                SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, true));
            }
            else
            {
                SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, false));

                lblZipCode.Attributes.Remove("class");
                lblZipCode.Attributes.Add("class", "lblLink");
            }

            if (isOccupationMandatory == "1")
            {
                SetControlEntrySetting(txtPatientJobCode, new ControlEntrySetting(true, true, true));
            }
            else
            {
                SetControlEntrySetting(txtPatientJobCode, new ControlEntrySetting(true, true, false));

                lblPatientJob.Attributes.Remove("class");
                lblPatientJob.Attributes.Add("class", "lblLink");
            }

            if (isEducationMandatory == "1")
            {
                SetControlEntrySetting(txtEducationCode, new ControlEntrySetting(true, true, true));
            }
            else
            {
                SetControlEntrySetting(txtEducationCode, new ControlEntrySetting(true, true, false));

                lblEducation.Attributes.Remove("class");
                lblEducation.Attributes.Add("class", "lblLink");
            }

            if (isSSNMandatory == "1")
            {
                SetControlEntrySetting(txtIdentityCardNo, new ControlEntrySetting(true, true, true));
            }
            else
            {
                SetControlEntrySetting(txtIdentityCardNo, new ControlEntrySetting(true, true, false));
                lblSSN.Attributes.Remove("class");
                lblSSN.Attributes.Add("class", "lblNormal");
            }

            if (isSalutationMandatory == "1")
            {
                SetControlEntrySetting(cboSalutation, new ControlEntrySetting(true, true, true));
            }
            else
            {
                SetControlEntrySetting(cboSalutation, new ControlEntrySetting(true, true, false));
                lblSalutation.Attributes.Remove("class");
                lblSalutation.Attributes.Add("class", "lblNormal");
            }

            #endregion
        }

        protected override void OnReInitControl()
        {
            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                txt.Text = "";
            }
            #endregion
        }

        private void EntityToControl(vPatient entity, PatientTagField entityTagField)
        {
            txtMRN.Text = entity.MedicalNo;

            #region Patient Data

            cboIdentityCardType.Value = entity.GCIdentityNoType;
            txtIdentityCardNo.Text = entity.SSN;
            txtFamilyCardNo.Text = entity.FamilyCardNo;
            txtNHSRegistrationNo.Text = entity.NHSRegistrationNo;
            txtInhealthParticipantNo.Text = entity.InhealthParticipantNo;
            txtEKlaimMedicalNo.Text = entity.EKlaimMedicalNo;
            txtSITBRegisterNo.Text = entity.SITBRegisterNo;

            cboSalutation.Value = entity.GCSalutation;
            cboTitle.Value = entity.GCTitle;
            txtFirstName.Text = entity.FirstName;
            txtMiddleName.Text = entity.MiddleName;
            txtFamilyName.Text = entity.LastName;
            txtName2.Text = entity.name2;
            txtPreferredName.Text = entity.PreferredName;
            cboSuffix.Value = entity.GCSuffix;
            cboGender.Value = entity.GCGender;
            if (entity.GCEthnic != "")
                txtEthnicCode.Text = entity.GCEthnic.Split('^')[1];
            else
                txtEthnicCode.Text = "";
            txtEthnicName.Text = entity.Ethnic;
            txtBirthPlace.Text = entity.CityOfBirth;
            cboBloodType.Value = entity.GCBloodType;
            cboBloodRhesus.Value = entity.BloodRhesus;
            txtDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAgeInYear.Text = entity.AgeInYear.ToString();
            txtAgeInMonth.Text = entity.AgeInMonth.ToString();
            txtAgeInDay.Text = entity.AgeInDay.ToString();
            txtMotherName.Text = entity.MotherName;
            txtFatherName.Text = entity.FatherName;
            txtSpouseName.Text = entity.SpouseName;
            PatientFamily entityChild = BusinessLayer.GetPatientFamilyList(string.Format("MRN = {0} AND GCFamilyRelation = '{1}' AND IsDeleted = 0", entity.MRN, Constant.FamilyRelation.CHILD)).FirstOrDefault();
            if (entityChild != null)
            {
                txtChildName.Text = entityChild.Name;
            }
            else
            {
                txtChildName.Text = string.Empty;
            }
            chkIsGeriatricPatient.Checked = entity.IsGeriatricPatient;
            #endregion

            #region Patient Address
            txtAddress.Text = entity.StreetName;
            txtRTData.Text = entity.RT;
            txtRWData.Text = entity.RW;
            txtCounty.Text = entity.County; // Desa
            txtDistrict.Text = entity.District; //Kabupaten
            txtCity.Text = entity.City;
            if (entity.GCState != "")
                txtProvinceCode.Text = entity.GCState.Split('^')[1];
            else
                txtProvinceCode.Text = "";
            txtProvinceName.Text = entity.State;
            hdnZipCode.Value = entity.ZipCodeID.ToString();
            txtZipCode.Text = entity.ZipCode;
            #endregion

            #region Patient Contact
            txtTelephoneNo1.Text = entity.PhoneNo1;
            txtTelephoneNo2.Text = entity.PhoneNo2;
            txtMobilePhone1.Text = entity.MobilePhoneNo1;
            txtMobilePhone2.Text = entity.MobilePhoneNo2;
            txtEmail.Text = entity.EmailAddress;
            #endregion

            #region Additonal Information
            if (entity.GCEducation != "")
                txtEducationCode.Text = entity.GCEducation.Split('^')[1];
            else
                txtEducationCode.Text = "";
            txtEducationName.Text = entity.Education;
            if (entity.GCReligion != "")
                txtReligionCode.Text = entity.GCReligion.Split('^')[1];
            else
                txtReligionCode.Text = "";
            txtReligionName.Text = entity.Religion;
            if (entity.GCMaritalStatus != "")
                txtMaritalStatusCode.Text = entity.GCMaritalStatus.Split('^')[1];
            else
                txtMaritalStatusCode.Text = "";
            txtMaritalStatusName.Text = entity.MaritalStatus;
            if (entity.GCNationality != "")
                txtNationalityCode.Text = entity.GCNationality.Split('^')[1];
            else
                txtNationalityCode.Text = "";
            txtNationalityName.Text = entity.Nationality;

            if (entity.GCPatientCategory != "")
                txtPatientCategoryCode.Text = entity.GCPatientCategory.Split('^')[1];
            else
                txtPatientCategoryCode.Text = "";
            txtPatientCategoryName.Text = entity.PatientCategory;

            if (entity.GCLanguage != "")
                txtlanguageCode.Text = entity.GCLanguage.Split('^')[1];
            else
                txtlanguageCode.Text = "";
            txtlanguageName.Text = entity.Bahasa;

            hdnEmployeeID.Value = entity.EmployeeID.ToString();
            txtEmployeeCode.Text = entity.EmployeeCode;
            txtEmployeeName.Text = entity.EmployeeName;
            #endregion

            #region Patient Payer
            cboPayer.Value = entity.GCCustomerType;
            hdnPayerID.Value = entity.BusinessPartnerID.ToString();
            txtPayerCompanyCode.Text = entity.BusinessPartnerCode;
            txtPayerCompanyName.Text = entity.BusinessPartnerName;
            #endregion

            #region Patient Job
            if (entity.GCOccupation != "")
                txtPatientJobCode.Text = entity.GCOccupation.Split('^')[1];
            else
                txtPatientJobCode.Text = "";
            txtPatientJobName.Text = entity.Occupation;
            txtPatientJobOffice.Text = entity.Company;
            txtPatientJobOfficeAddress.Text = entity.OfficeStreetName;
            txtRTOfficeData.Text = entity.OfficeRT;
            txtRWOfficeData.Text = entity.OfficeRW;
            txtPatientJobOfficeCounty.Text = entity.OfficeCounty; // Desa
            txtPatientJobOfficeDistrict.Text = entity.OfficeDistrict; //Kabupaten
            txtPatientJobOfficeCity.Text = entity.OfficeCity;
            if (entity.OfficeGCState != "")
                txtPatientJobOfficeProvinceCode.Text = entity.OfficeGCState.Split('^')[1];
            else
                txtPatientJobOfficeProvinceCode.Text = "";
            txtPatientJobOfficeProvinceName.Text = entity.OfficeState;
            hdnOfficeZipCode.Value = entity.OfficeZipCodeID.ToString();
            txtPatientJobOfficeZipCode.Text = entity.OfficeZipCode;
            txtPatientJobOfficeTelephone.Text = entity.OfficePhoneNo1;
            txtPatientJobOfficeEmail.Text = entity.OfficeEmail;
            #endregion

            #region Patient Status
            cboPatientStatus.Value = entity.GCPatientStatus;
            chkIsAlive.Checked = entity.IsAlive;
            chkIsBlackList.Checked = entity.IsBlacklist;
            cboGCBlacklistReason.Value = entity.GCBlacklistReason;
            txtOtherBlackListReason.Text = entity.OtherBlacklistReason;
            chkIsVIP.Checked = entity.IsVIP;
            cboVIPGroup.Value = entity.GCVIPGroup;
            chkIsHasCommunicationRestriction.Checked = entity.IsHasCommunicationRestriction;
            cboCommunication.Value = entity.GCCommunicationRestriction;
            chkIsHasPhysicalLimitation.Checked = entity.IsHasPhysicalLimitation;
            cboPhysicalLimitation.Value = entity.GCPhysicalLimitationType;
            txtOtherVIPGroup.Text = entity.OtherVIPGroup;
            chkIsDonor.Checked = entity.IsDonor;
            chkIsG6PD.Checked = entity.IsG6PD;
            chkIsHasInfectious.Checked = entity.IsHasInfectious;
            chkIsHasAllergy.Checked = entity.IsHasAllergy;
            chkIsIlliteracy.Checked = entity.IsIlliteracy;
            chkIsSmoking.Checked = entity.IsSmoking;
            chkIsPregnant.Checked = entity.IsPregnant;
            //chkIsVIP.Checked = entity.IsVIP;
            //cboVIPGroup.Value = entity.GCVIPGroup;
            #endregion

            #region Other Information
            txtOldMedicalNo.Text = entity.OldMedicalNo;
            txtNotes.Text = entity.Notes;
            txtCorporateAccountNo.Text = entity.CorporateAccountNo;
            txtCorporateAccountName.Text = entity.CorporateAccountName;
            #endregion

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                txt.Text = entityTagField.GetType().GetProperty("TagField" + hdn.Value).GetValue(entityTagField, null).ToString();
            }
            #endregion

            #region SatuSEHAT
            txtIHSNumber.Text = entity.IHSNumber;
            rblSATUSEHAT.SelectedValue = entity.PelepasanInformasiSatuSEHAT;
            txtSatuSEHATConsentDate.Text = entity.cfLastSatuSEHATConsentDate;
            #endregion

            divCreatedBy.InnerHtml = entity.CreatedByName;
            if (entity.CreatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divCreatedDate.InnerHtml = "";
            }
            else
            {
                divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            #region GuestData
            if (entity != null)
            {

                Guest oGuest = BusinessLayer.GetGuestList(string.Format("MRN='{0}'", entity.MRN)).FirstOrDefault();
                if (oGuest != null)
                {
                    txtGuestNo.Text = oGuest.GuestNo;
                    hdnGuestID.Value = oGuest.GuestID.ToString();
                    btnGuest.Disabled = true;
                    btnGuest.Style.Add("display", "none");
                    txtGuestNo.Enabled = false;

                }

            }
            #endregion
        }

        private void ControlToEntity(Patient entity, Address entityAddress, Address entityOfficeAddress, PatientTagField entityTagField, ref PatientFamily entityPatientMother, ref PatientFamily entityPatientFather, ref PatientFamily entityPatientSpouse, ref PatientFamily entityPatientChild)
        {
            #region Patient Data

            entity.GCIdentityNoType = Helper.GetComboBoxValue(cboIdentityCardType, true);
            entity.SSN = txtIdentityCardNo.Text;
            entity.NHSRegistrationNo = txtNHSRegistrationNo.Text;
            entity.InhealthParticipantNo = txtInhealthParticipantNo.Text;
            entity.FamilyCardNo = txtFamilyCardNo.Text;
            entity.IHSNumber = txtIHSNumber.Text;
            entity.EKlaimMedicalNo = txtEKlaimMedicalNo.Text;
            entity.SITBRegisterNo = txtSITBRegisterNo.Text;

            entity.GCSalutation = Helper.GetComboBoxValue(cboSalutation, true);
            entity.GCTitle = Helper.GetComboBoxValue(cboTitle, true);

            entity.FirstName = txtFirstName.Text;
            entity.MiddleName = txtMiddleName.Text;
            entity.LastName = txtFamilyName.Text;
            entity.PreferredName = txtPreferredName.Text;
            entity.GCSuffix = Helper.GetComboBoxValue(cboSuffix, true);
            entity.GCSex = entity.GCGender = cboGender.Value.ToString();
            entity.GCEthnic = txtEthnicCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.ETHNIC, txtEthnicCode.Text);
            entity.CityOfBirth = txtBirthPlace.Text;
            entity.GCBloodType = Helper.GetComboBoxValue(cboBloodType, true);
            entity.BloodRhesus = Helper.GetComboBoxValue(cboBloodRhesus, true);
            entity.DateOfBirth = Helper.GetDatePickerValue(txtDOB);

            string suffix = cboSuffix.Value == null ? "" : cboSuffix.Text;
            string title = cboTitle.Value == null ? "" : cboTitle.Text;
            entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
            entity.FullName = Helper.GenerateFullName(entity.Name, title, suffix);

            if (!string.IsNullOrEmpty(txtName2.Text))
            {
                entity.Name2 = txtName2.Text;
            }
            else
            {
                entity.Name2 = entity.FullName;
            }

            if (!string.IsNullOrEmpty(txtMotherName.Text))
            {
                if (entityPatientMother == null)
                {
                    entityPatientMother = new PatientFamily();
                }
                if (!String.IsNullOrEmpty(hdnMRN.Value))
                {
                    entityPatientMother.MRN = Convert.ToInt32(hdnMRN.Value);
                }
                entityPatientMother.LastName = txtMotherName.Text;
                entityPatientMother.FullName = txtMotherName.Text;
                entityPatientMother.Name = txtMotherName.Text;
                entityPatientMother.GCFamilyRelation = Constant.FamilyRelation.MOTHER;
            }
            entity.MotherName = txtMotherName.Text;
            if (!string.IsNullOrEmpty(txtFatherName.Text))
            {
                if (entityPatientFather == null)
                {
                    entityPatientFather = new PatientFamily();
                }
                if (!String.IsNullOrEmpty(hdnMRN.Value))
                {
                    entityPatientFather.MRN = Convert.ToInt32(hdnMRN.Value);
                }
                entityPatientFather.LastName = txtFatherName.Text;
                entityPatientFather.FullName = txtFatherName.Text;
                entityPatientFather.Name = txtFatherName.Text;
                entityPatientFather.GCFamilyRelation = Constant.FamilyRelation.FATHER;
            }
            entity.FatherName = txtFatherName.Text;
            if (!string.IsNullOrEmpty(txtSpouseName.Text))
            {
                if (entityPatientSpouse == null)
                {
                    entityPatientSpouse = new PatientFamily();
                }

                if (!String.IsNullOrEmpty(hdnMRN.Value))
                {
                    entityPatientSpouse.MRN = Convert.ToInt32(hdnMRN.Value);
                }
                entityPatientSpouse.LastName = txtSpouseName.Text;
                entityPatientSpouse.FullName = txtSpouseName.Text;
                entityPatientSpouse.Name = txtSpouseName.Text;
                entityPatientSpouse.GCFamilyRelation = Constant.FamilyRelation.SPOUSE;
            }
            entity.SpouseName = txtSpouseName.Text;
            if (!string.IsNullOrEmpty(txtChildName.Text))
            {
                if (entityPatientChild == null)
                {
                    entityPatientChild = new PatientFamily();
                }

                if (!string.IsNullOrEmpty(hdnMRN.Value))
                {
                    entityPatientChild.MRN = Convert.ToInt32(hdnMRN.Value);
                }
                entityPatientChild.LastName = txtChildName.Text;
                entityPatientChild.FullName = txtChildName.Text;
                entityPatientChild.Name = txtChildName.Text;
                entityPatientChild.GCFamilyRelation = Constant.FamilyRelation.CHILD;
            }
            entity.IsGeriatricPatient = chkIsGeriatricPatient.Checked;
            #endregion

            #region Patient Address
            entityAddress.StreetName = txtAddress.Text;
            entityAddress.RT = txtRTData.Text;
            entityAddress.RW = txtRWData.Text;
            entityAddress.County = txtCounty.Text; // Desa
            entityAddress.District = txtDistrict.Text; //Kabupaten
            entityAddress.City = txtCity.Text;
            entityAddress.GCState = txtProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCode.Text);
            if (hdnZipCode.Value == "" || hdnZipCode.Value == "0")
                entityAddress.ZipCode = null;
            else
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCode.Value);
            #endregion

            #region Patient Contact
            entityAddress.PhoneNo1 = txtTelephoneNo1.Text;
            entityAddress.PhoneNo2 = txtTelephoneNo2.Text;
            entity.MobilePhoneNo1 = txtMobilePhone1.Text;
            entity.MobilePhoneNo2 = txtMobilePhone2.Text;
            entity.EmailAddress = txtEmail.Text;
            entityAddress.Email = txtEmail.Text;
            #endregion

            #region Additonal Information
            entity.GCEducation = txtEducationCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.EDUCATION, txtEducationCode.Text);
            entity.GCReligion = txtReligionCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.RELIGION, txtReligionCode.Text);
            entity.GCMaritalStatus = txtMaritalStatusCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.MARITAL_STATUS, txtMaritalStatusCode.Text);
            entity.GCNationality = txtNationalityCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.NATIONALITY, txtNationalityCode.Text);
            entity.GCPatientCategory = txtPatientCategoryCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PATIENT_CATEGORY, txtPatientCategoryCode.Text);
            entity.GCLanguage = txtlanguageCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.LANGUAGE, txtlanguageCode.Text);

            if (hdnEmployeeID.Value != "" && hdnEmployeeID.Value != "0")
                entity.EmployeeID = Convert.ToInt32(hdnEmployeeID.Value);
            else
                entity.EmployeeID = null;

            #endregion

            #region Patient Payer
            string GCCustomerType = cboPayer.Value.ToString();
            if (GCCustomerType == Constant.CustomerType.PERSONAL)
            {
                entity.BusinessPartnerID = 1;
            }
            else
            {
                if (!string.IsNullOrEmpty(hdnPayerID.Value))
                {
                    entity.BusinessPartnerID = Convert.ToInt32(hdnPayerID.Value);
                }
                else
                {
                    entity.BusinessPartnerID = null;
                }
            }
            #endregion

            #region Patient Job
            entity.GCOccupation = txtPatientJobCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.OCCUPATION, txtPatientJobCode.Text);
            entity.Company = txtPatientJobOffice.Text;
            entityOfficeAddress.StreetName = txtPatientJobOfficeAddress.Text;
            entityOfficeAddress.RT = txtRTOfficeData.Text;
            entityOfficeAddress.RW = txtRWOfficeData.Text;
            entityOfficeAddress.County = txtPatientJobOfficeCounty.Text; // Desa
            entityOfficeAddress.District = txtPatientJobOfficeDistrict.Text; //Kabupaten
            entityOfficeAddress.City = txtPatientJobOfficeCity.Text;
            entityOfficeAddress.GCState = txtPatientJobOfficeProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtPatientJobOfficeProvinceCode.Text);
            if (hdnOfficeZipCode.Value == "")
                entityOfficeAddress.ZipCode = null;
            else
                entityOfficeAddress.ZipCode = Convert.ToInt32(hdnOfficeZipCode.Value);
            entityOfficeAddress.PhoneNo1 = txtPatientJobOfficeTelephone.Text;
            entityOfficeAddress.Email = txtPatientJobOfficeEmail.Text;
            #endregion

            #region Patient Status
            entity.GCPatientStatus = cboPatientStatus.Value.ToString();
            entity.IsAlive = chkIsAlive.Checked;
            entity.IsBlackList = chkIsBlackList.Checked;
            if (entity.IsBlackList)
            {
                entity.GCBlacklistReason = cboGCBlacklistReason.Value.ToString();

                if (entity.GCBlacklistReason == Constant.Patient_BlackList_Reason.OTHER)
                {
                    entity.OtherBlacklistReason = txtOtherBlackListReason.Text;
                }
                else
                {
                    entity.OtherBlacklistReason = "";
                }
            }
            else
            {
                entity.IsBlackList = false;
                entity.GCBlacklistReason = null;
                entity.OtherBlacklistReason = null;
            }
            entity.IsVIP = chkIsVIP.Checked;
            if (entity.IsVIP)
            {
                entity.GCVIPGroup = cboVIPGroup.Value.ToString();

                if (entity.GCVIPGroup == Constant.VIPPatientGroup.VIP_OTHER)
                {
                    entity.OtherVIPGroup = txtOtherVIPGroup.Text;
                }
                else
                {
                    entity.OtherVIPGroup = "";
                }
            }
            else
            {
                entity.IsVIP = false;
                entity.GCVIPGroup = null;
                entity.OtherVIPGroup = null;
            }

            if (chkIsHasCommunicationRestriction.Checked == true)
            {
                entity.IsHasCommunicationRestriction = chkIsHasCommunicationRestriction.Checked;
                entity.GCCommunicationRestriction = cboCommunication.Value.ToString();

            }
            else
            {
                if (chkIsHasCommunicationRestriction.Checked == false)
                {

                    entity.IsHasCommunicationRestriction = false;
                    entity.GCCommunicationRestriction = " ";
                }
            }

            if (chkIsHasPhysicalLimitation.Checked == true)
            {
                entity.IsHasPhysicalLimitation = chkIsHasPhysicalLimitation.Checked;
                entity.GCPhysicalLimitationType = cboPhysicalLimitation.Value.ToString();

            }
            else
            {
                if (chkIsHasPhysicalLimitation.Checked == false)
                {

                    entity.IsHasPhysicalLimitation = false;
                    entity.GCPhysicalLimitationType = " ";
                }
            }

            entity.IsDonor = chkIsDonor.Checked;
            entity.IsG6PD = chkIsG6PD.Checked;
            entity.IsHasInfectious = chkIsHasInfectious.Checked;
            entity.IsHasAllergy = chkIsHasAllergy.Checked;
            entity.IsIlliteracy = chkIsIlliteracy.Checked;
            entity.IsSmoking = chkIsSmoking.Checked;
            entity.IsPregnant = chkIsPregnant.Checked;

            if (entity.GCGender == Constant.Gender.FEMALE)
            {
                chkIsPregnant.Enabled = true;
            }
            else
            {
                chkIsPregnant.Enabled = false;
            }

            //            entity.GCPatientStatus = Constant.PatientStatus.ACTIVE;
            #endregion

            #region Other Information
            entity.OldMedicalNo = txtOldMedicalNo.Text;
            entity.Notes = txtNotes.Text;
            entity.CorporateAccountNo = txtCorporateAccountNo.Text;
            entity.CorporateAccountName = txtCorporateAccountName.Text;
            #endregion

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                entityTagField.GetType().GetProperty("TagField" + hdn.Value).SetValue(entityTagField, txt.Text, null);
            }
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDao entityDao = new PatientDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            PatientTagFieldDao entityTagFieldDao = new PatientTagFieldDao(ctx);
            PatientFamilyDao entityPatientFamilyDao = new PatientFamilyDao(ctx);
            GuestDao guestDao = new GuestDao(ctx);
            try
            {
                Patient entity = new Patient();
                Address entityAddress = new Address();
                Address entityOfficeAddress = new Address();
                PatientTagField entityTagField = new PatientTagField();

                PatientFamily entityPatientMother = null;
                Address entityPatientMotherAddress = new Address();
                PatientFamily entityPatientFather = null;
                Address entityPatientFatherAddress = new Address();
                PatientFamily entityPatientSpouse = null;
                Address entityPatientSpouseAddress = new Address();
                PatientFamily entityPatientChild = null;
                Address entityPatientChildAddress = new Address();

                ControlToEntity(entity, entityAddress, entityOfficeAddress, entityTagField, ref entityPatientMother, ref entityPatientFather, ref entityPatientSpouse, ref entityPatientChild);

                entity.IsAlive = true;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.CreatedBy = AppSession.UserLogin.UserID;

                entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                entityAddressDao.Insert(entityAddress);
                entity.HomeAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);

                entityOfficeAddress.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entity.OfficeAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityOfficeAddress);

                entity.MedicalNo = Helper.GenerateMRN(entity.MRN, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entity.MRN = entityDao.InsertReturnPrimaryKeyID(entity);
                entity.PictureFileName = string.Format("{0}.jpg", entity.MedicalNo);
                entityDao.Update(entity);

                entityTagField.MRN = entity.MRN;
                entityTagFieldDao.Insert(entityTagField);

                if (entityPatientMother != null)
                {
                    entityPatientMother.MRN = entity.MRN;

                    entityPatientMotherAddress.StreetName = entityAddress.StreetName;
                    entityPatientMotherAddress.RT = entityAddress.RT;
                    entityPatientMotherAddress.RW = entityAddress.RW;
                    entityPatientMotherAddress.County = entityAddress.County;
                    entityPatientMotherAddress.District = entityAddress.District;
                    entityPatientMotherAddress.City = entityAddress.City;
                    entityPatientMotherAddress.GCState = entityAddress.GCState;
                    entityPatientMotherAddress.ZipCode = entityAddress.ZipCode;
                    entityPatientMotherAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientMother.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientMotherAddress);

                    entityPatientMother.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityPatientFamilyDao.Insert(entityPatientMother);
                }
                if (entityPatientFather != null)
                {
                    entityPatientFather.MRN = entity.MRN;

                    entityPatientFatherAddress.StreetName = entityAddress.StreetName;
                    entityPatientFatherAddress.RT = entityAddress.RT;
                    entityPatientFatherAddress.RW = entityAddress.RW;
                    entityPatientFatherAddress.County = entityAddress.County;
                    entityPatientFatherAddress.District = entityAddress.District;
                    entityPatientFatherAddress.City = entityAddress.City;
                    entityPatientFatherAddress.GCState = entityAddress.GCState;
                    entityPatientFatherAddress.ZipCode = entityAddress.ZipCode;
                    entityPatientFatherAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientFather.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientFatherAddress);

                    entityPatientFather.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityPatientFamilyDao.Insert(entityPatientFather);
                }
                if (entityPatientSpouse != null)
                {
                    entityPatientSpouse.MRN = entity.MRN;

                    entityPatientSpouseAddress.StreetName = entityAddress.StreetName;
                    entityPatientSpouseAddress.RT = entityAddress.RT;
                    entityPatientSpouseAddress.RW = entityAddress.RW;
                    entityPatientSpouseAddress.County = entityAddress.County;
                    entityPatientSpouseAddress.District = entityAddress.District;
                    entityPatientSpouseAddress.City = entityAddress.City;
                    entityPatientSpouseAddress.GCState = entityAddress.GCState;
                    entityPatientSpouseAddress.ZipCode = entityAddress.ZipCode;
                    entityPatientSpouseAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientSpouse.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientSpouseAddress);

                    entityPatientSpouse.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityPatientFamilyDao.Insert(entityPatientSpouse);
                }
                if (entityPatientChild != null)
                {
                    entityPatientChild.MRN = entity.MRN;

                    entityPatientChildAddress.StreetName = entityAddress.StreetName;
                    entityPatientChildAddress.RT = entityAddress.RT;
                    entityPatientChildAddress.RW = entityAddress.RW;
                    entityPatientChildAddress.County = entityAddress.County;
                    entityPatientChildAddress.District = entityAddress.District;
                    entityPatientChildAddress.City = entityAddress.City;
                    entityPatientChildAddress.GCState = entityAddress.GCState;
                    entityPatientChildAddress.ZipCode = entityAddress.ZipCode;
                    entityPatientChildAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entityPatientChild.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientChildAddress);

                    entityPatientChild.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityPatientFamilyDao.Insert(entityPatientChild);
                }

                if (!string.IsNullOrEmpty(hdnGuestID.Value))
                {
                    if (Convert.ToInt32(hdnGuestID.Value) > 0)
                    {
                        Guest oGuest = BusinessLayer.GetGuestList(string.Format("GuestID={0}", hdnGuestID.Value), ctx).FirstOrDefault();
                        oGuest.MRN = entity.MRN;
                        guestDao.Update(oGuest);
                    }
                }


                retval = entity.MRN.ToString();


                ctx.CommitTransaction();

                BridgingToMedinfrasMobileApps(entity, Convert.ToInt32(retval), "001");
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDao entityDao = new PatientDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            PatientTagFieldDao entityTagFieldDao = new PatientTagFieldDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            PatientFamilyDao entityPatientFamilyDao = new PatientFamilyDao(ctx);
            try
            {
                AuditLog entityAuditLog = new AuditLog();
                Patient entity = entityDao.Get(Convert.ToInt32(hdnMRN.Value));
                entityAuditLog.OldValues = JsonConvert.SerializeObject(entity);

                Address entityAddress = entityAddressDao.Get((int)entity.HomeAddressID);
                Address entityOfficeAddress = null;

                bool flagAddOfficeAddress = false;
                if (entity.OfficeAddressID == null)
                {
                    flagAddOfficeAddress = true;
                    entityOfficeAddress = new Address();
                }
                else entityOfficeAddress = entityAddressDao.Get((int)entity.OfficeAddressID);

                PatientTagField entityTagField = entityTagFieldDao.Get(entity.MRN);

                bool flagInsertMother = true, flagInsertFather = true, flagInsertSpouse = true, flagInsertChild = true;
                List<PatientFamily> lstEntityPatientFamily = BusinessLayer.GetPatientFamilyList(string.Format("MRN = {0} AND GCFamilyRelation IN ('{1}','{2}','{3}','{4}') AND IsDeleted = 0", hdnMRN.Value, Constant.FamilyRelation.MOTHER, Constant.FamilyRelation.FATHER, Constant.FamilyRelation.SPOUSE, Constant.FamilyRelation.CHILD));

                PatientFamily entityPatientMother = lstEntityPatientFamily.Where(t => t.GCFamilyRelation == Constant.FamilyRelation.MOTHER).FirstOrDefault();
                PatientFamily entityPatientFather = lstEntityPatientFamily.Where(t => t.GCFamilyRelation == Constant.FamilyRelation.FATHER).FirstOrDefault();
                PatientFamily entityPatientSpouse = lstEntityPatientFamily.Where(t => t.GCFamilyRelation == Constant.FamilyRelation.SPOUSE).FirstOrDefault();
                PatientFamily entityPatientChild = lstEntityPatientFamily.Where(t => t.GCFamilyRelation == Constant.FamilyRelation.CHILD).FirstOrDefault();

                if (entityPatientMother != null) flagInsertMother = false;
                if (entityPatientFather != null) flagInsertFather = false;
                if (entityPatientSpouse != null) flagInsertSpouse = false;
                if (entityPatientChild != null) flagInsertChild = false;

                ControlToEntity(entity, entityAddress, entityOfficeAddress, entityTagField, ref entityPatientMother, ref entityPatientFather, ref entityPatientSpouse, ref entityPatientChild);

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityAddressDao.Update(entityAddress);

                if (flagAddOfficeAddress)
                {
                    entityOfficeAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Insert(entityOfficeAddress);
                    entity.OfficeAddressID = BusinessLayer.GetAddressMaxID(ctx);
                }
                else
                {
                    entityOfficeAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Update(entityOfficeAddress);
                }

                entityOfficeAddress.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityAddressDao.Update(entityOfficeAddress);
                entityTagFieldDao.Update(entityTagField);

                entityDao.Update(entity);
                entityAuditLog.ObjectType = Constant.BusinessObjectType.PATIENT;
                entityAuditLog.NewValues = JsonConvert.SerializeObject(entity);
                entityAuditLog.UserID = AppSession.UserLogin.UserID;
                entityAuditLog.LogDate = DateTime.Now;
                entityAuditLog.TransactionID = entity.MRN;
                entityAuditLogDao.Insert(entityAuditLog);

                Address entityPatientMotherAddress = null;
                Address entityPatientFatherAddress = null;
                Address entityPatientSpouseAddress = null;
                Address entityPatientChildAddress = null;

                if (entityPatientMother != null)
                {
                    if (flagInsertMother)
                    {
                        entityPatientMotherAddress = new Address();
                        entityPatientMotherAddress.StreetName = entityAddress.StreetName;
                        entityPatientMotherAddress.RT = entityAddress.RT;
                        entityPatientMotherAddress.RW = entityAddress.RW;
                        entityPatientMotherAddress.County = entityAddress.County;
                        entityPatientMotherAddress.District = entityAddress.District;
                        entityPatientMotherAddress.City = entityAddress.City;
                        entityPatientMotherAddress.GCState = entityAddress.GCState;
                        entityPatientMotherAddress.ZipCode = entityAddress.ZipCode;
                        entityPatientMotherAddress.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientMother.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientMotherAddress);

                        entityPatientMother.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientFamilyDao.Insert(entityPatientMother);
                    }
                    else
                    {
                        if (entityPatientMother.AddressID != null)
                        {
                            entityPatientMotherAddress = entityAddressDao.Get(Convert.ToInt32(entityPatientMother.AddressID));
                            entityPatientMotherAddress.StreetName = entityAddress.StreetName;
                            entityPatientMotherAddress.RT = entityAddress.RT;
                            entityPatientMotherAddress.RW = entityAddress.RW;
                            entityPatientMotherAddress.County = entityAddress.County;
                            entityPatientMotherAddress.District = entityAddress.District;
                            entityPatientMotherAddress.City = entityAddress.City;
                            entityPatientMotherAddress.GCState = entityAddress.GCState;
                            entityPatientMotherAddress.ZipCode = entityAddress.ZipCode;
                            entityPatientMotherAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityAddressDao.Update(entityPatientMotherAddress);
                        }
                        else
                        {
                            entityPatientMotherAddress = new Address();
                            entityPatientMotherAddress.StreetName = entityAddress.StreetName;
                            entityPatientMotherAddress.RT = entityAddress.RT;
                            entityPatientMotherAddress.RW = entityAddress.RW;
                            entityPatientMotherAddress.County = entityAddress.County;
                            entityPatientMotherAddress.District = entityAddress.District;
                            entityPatientMotherAddress.City = entityAddress.City;
                            entityPatientMotherAddress.GCState = entityAddress.GCState;
                            entityPatientMotherAddress.ZipCode = entityAddress.ZipCode;
                            entityPatientMotherAddress.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityPatientMother.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientMotherAddress);
                        }

                        entityPatientMother.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientFamilyDao.Update(entityPatientMother);
                    }
                }

                if (entityPatientFather != null)
                {
                    if (flagInsertFather)
                    {
                        entityPatientFatherAddress = new Address();
                        entityPatientFatherAddress.StreetName = entityAddress.StreetName;
                        entityPatientFatherAddress.RT = entityAddress.RT;
                        entityPatientFatherAddress.RW = entityAddress.RW;
                        entityPatientFatherAddress.County = entityAddress.County;
                        entityPatientFatherAddress.District = entityAddress.District;
                        entityPatientFatherAddress.City = entityAddress.City;
                        entityPatientFatherAddress.GCState = entityAddress.GCState;
                        entityPatientFatherAddress.ZipCode = entityAddress.ZipCode;
                        entityPatientFatherAddress.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientFather.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientFatherAddress);

                        entityPatientFather.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientFamilyDao.Insert(entityPatientFather);
                    }
                    else
                    {
                        if (entityPatientFather.AddressID != null)
                        {
                            entityPatientFatherAddress = entityAddressDao.Get(Convert.ToInt32(entityPatientFather.AddressID));
                            entityPatientFatherAddress.StreetName = entityAddress.StreetName;
                            entityPatientFatherAddress.RT = entityAddress.RT;
                            entityPatientFatherAddress.RW = entityAddress.RW;
                            entityPatientFatherAddress.County = entityAddress.County;
                            entityPatientFatherAddress.District = entityAddress.District;
                            entityPatientFatherAddress.City = entityAddress.City;
                            entityPatientFatherAddress.GCState = entityAddress.GCState;
                            entityPatientFatherAddress.ZipCode = entityAddress.ZipCode;
                            entityPatientFatherAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityAddressDao.Update(entityPatientFatherAddress);
                        }
                        else
                        {
                            entityPatientFatherAddress = new Address();
                            entityPatientFatherAddress.StreetName = entityAddress.StreetName;
                            entityPatientFatherAddress.RT = entityAddress.RT;
                            entityPatientFatherAddress.RW = entityAddress.RW;
                            entityPatientFatherAddress.County = entityAddress.County;
                            entityPatientFatherAddress.District = entityAddress.District;
                            entityPatientFatherAddress.City = entityAddress.City;
                            entityPatientFatherAddress.GCState = entityAddress.GCState;
                            entityPatientFatherAddress.ZipCode = entityAddress.ZipCode;
                            entityPatientFatherAddress.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityPatientFather.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientFatherAddress);
                        }

                        entityPatientFather.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientFamilyDao.Update(entityPatientFather);
                    }
                }

                if (entityPatientSpouse != null)
                {
                    if (flagInsertSpouse)
                    {
                        entityPatientSpouseAddress = new Address();
                        entityPatientSpouseAddress.StreetName = entityAddress.StreetName;
                        entityPatientSpouseAddress.RT = entityAddress.RT;
                        entityPatientSpouseAddress.RW = entityAddress.RW;
                        entityPatientSpouseAddress.County = entityAddress.County;
                        entityPatientSpouseAddress.District = entityAddress.District;
                        entityPatientSpouseAddress.City = entityAddress.City;
                        entityPatientSpouseAddress.GCState = entityAddress.GCState;
                        entityPatientSpouseAddress.ZipCode = entityAddress.ZipCode;
                        entityPatientSpouseAddress.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientSpouse.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientSpouseAddress);

                        entityPatientSpouse.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientFamilyDao.Insert(entityPatientSpouse);
                    }
                    else
                    {
                        if (entityPatientSpouse.AddressID != null)
                        {
                            entityPatientSpouseAddress = entityAddressDao.Get(Convert.ToInt32(entityPatientSpouse.AddressID));
                            entityPatientSpouseAddress.StreetName = entityAddress.StreetName;
                            entityPatientSpouseAddress.RT = entityAddress.RT;
                            entityPatientSpouseAddress.RW = entityAddress.RW;
                            entityPatientSpouseAddress.County = entityAddress.County;
                            entityPatientSpouseAddress.District = entityAddress.District;
                            entityPatientSpouseAddress.City = entityAddress.City;
                            entityPatientSpouseAddress.GCState = entityAddress.GCState;
                            entityPatientSpouseAddress.ZipCode = entityAddress.ZipCode;
                            entityPatientSpouseAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityAddressDao.Update(entityPatientSpouseAddress);
                        }
                        else
                        {
                            entityPatientSpouseAddress = new Address();
                            entityPatientSpouseAddress.StreetName = entityAddress.StreetName;
                            entityPatientSpouseAddress.RT = entityAddress.RT;
                            entityPatientSpouseAddress.RW = entityAddress.RW;
                            entityPatientSpouseAddress.County = entityAddress.County;
                            entityPatientSpouseAddress.District = entityAddress.District;
                            entityPatientSpouseAddress.City = entityAddress.City;
                            entityPatientSpouseAddress.GCState = entityAddress.GCState;
                            entityPatientSpouseAddress.ZipCode = entityAddress.ZipCode;
                            entityPatientSpouseAddress.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityPatientSpouse.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientSpouseAddress);
                        }

                        entityPatientSpouse.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientFamilyDao.Update(entityPatientSpouse);
                    }
                }

                if (entityPatientChild != null)
                {
                    if (flagInsertChild)
                    {
                        entityPatientChildAddress = new Address();
                        entityPatientChildAddress.StreetName = entityAddress.StreetName;
                        entityPatientChildAddress.RT = entityAddress.RT;
                        entityPatientChildAddress.RW = entityAddress.RW;
                        entityPatientChildAddress.County = entityAddress.County;
                        entityPatientChildAddress.District = entityAddress.District;
                        entityPatientChildAddress.City = entityAddress.City;
                        entityPatientChildAddress.GCState = entityAddress.GCState;
                        entityPatientChildAddress.ZipCode = entityAddress.ZipCode;
                        entityPatientChildAddress.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientChild.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientChildAddress);

                        entityPatientChild.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientFamilyDao.Insert(entityPatientChild);
                    }
                    else
                    {
                        if (entityPatientChild.AddressID != null)
                        {
                            entityPatientChildAddress = entityAddressDao.Get(Convert.ToInt32(entityPatientChild.AddressID));
                            entityPatientChildAddress.StreetName = entityAddress.StreetName;
                            entityPatientChildAddress.RT = entityAddress.RT;
                            entityPatientChildAddress.RW = entityAddress.RW;
                            entityPatientChildAddress.County = entityAddress.County;
                            entityPatientChildAddress.District = entityAddress.District;
                            entityPatientChildAddress.City = entityAddress.City;
                            entityPatientChildAddress.GCState = entityAddress.GCState;
                            entityPatientChildAddress.ZipCode = entityAddress.ZipCode;
                            entityPatientChildAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityAddressDao.Update(entityPatientChildAddress);
                        }
                        else
                        {
                            entityPatientChildAddress = new Address();
                            entityPatientChildAddress.StreetName = entityAddress.StreetName;
                            entityPatientChildAddress.RT = entityAddress.RT;
                            entityPatientChildAddress.RW = entityAddress.RW;
                            entityPatientChildAddress.County = entityAddress.County;
                            entityPatientChildAddress.District = entityAddress.District;
                            entityPatientChildAddress.City = entityAddress.City;
                            entityPatientChildAddress.GCState = entityAddress.GCState;
                            entityPatientChildAddress.ZipCode = entityAddress.ZipCode;
                            entityPatientChildAddress.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityPatientChild.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityPatientChildAddress);
                        }

                        entityPatientChild.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientFamilyDao.Update(entityPatientChild);
                    }
                }

                ctx.CommitTransaction();
                BridgingToMedinfrasMobileApps(entity, entity.MRN, "002");
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (pnlCustomAttribute.Visible)
            {
                List<Variable> ListCustomAttribute = initListCustomAttribute();
                if (ListCustomAttribute.Count == 0)
                    pnlCustomAttribute.Visible = false;
                else
                {
                    rptCustomAttribute.DataSource = ListCustomAttribute;
                    rptCustomAttribute.DataBind();
                }
            }
        }

        private List<Variable> initListCustomAttribute()
        {
            List<Variable> ListCustomAttribute = new List<Variable>();
            TagField tagField = BusinessLayer.GetTagField(Constant.BusinessObjectType.PATIENT);
            if (tagField != null)
            {
                if (tagField.TagField1 != "") { ListCustomAttribute.Add(new Variable { Code = "1", Value = tagField.TagField1 }); }
                if (tagField.TagField2 != "") { ListCustomAttribute.Add(new Variable { Code = "2", Value = tagField.TagField2 }); }
                if (tagField.TagField3 != "") { ListCustomAttribute.Add(new Variable { Code = "3", Value = tagField.TagField3 }); }
                if (tagField.TagField4 != "") { ListCustomAttribute.Add(new Variable { Code = "4", Value = tagField.TagField4 }); }
                if (tagField.TagField5 != "") { ListCustomAttribute.Add(new Variable { Code = "5", Value = tagField.TagField5 }); }
                if (tagField.TagField6 != "") { ListCustomAttribute.Add(new Variable { Code = "6", Value = tagField.TagField6 }); }
                if (tagField.TagField7 != "") { ListCustomAttribute.Add(new Variable { Code = "7", Value = tagField.TagField7 }); }
                if (tagField.TagField8 != "") { ListCustomAttribute.Add(new Variable { Code = "8", Value = tagField.TagField8 }); }
                if (tagField.TagField9 != "") { ListCustomAttribute.Add(new Variable { Code = "9", Value = tagField.TagField9 }); }
                if (tagField.TagField10 != "") { ListCustomAttribute.Add(new Variable { Code = "10", Value = tagField.TagField10 }); }
                if (tagField.TagField11 != "") { ListCustomAttribute.Add(new Variable { Code = "11", Value = tagField.TagField11 }); }
                if (tagField.TagField12 != "") { ListCustomAttribute.Add(new Variable { Code = "12", Value = tagField.TagField12 }); }
                if (tagField.TagField13 != "") { ListCustomAttribute.Add(new Variable { Code = "13", Value = tagField.TagField13 }); }
                if (tagField.TagField14 != "") { ListCustomAttribute.Add(new Variable { Code = "14", Value = tagField.TagField14 }); }
                if (tagField.TagField15 != "") { ListCustomAttribute.Add(new Variable { Code = "15", Value = tagField.TagField15 }); }
                if (tagField.TagField16 != "") { ListCustomAttribute.Add(new Variable { Code = "16", Value = tagField.TagField16 }); }
                if (tagField.TagField17 != "") { ListCustomAttribute.Add(new Variable { Code = "17", Value = tagField.TagField17 }); }
                if (tagField.TagField18 != "") { ListCustomAttribute.Add(new Variable { Code = "18", Value = tagField.TagField18 }); }
                if (tagField.TagField19 != "") { ListCustomAttribute.Add(new Variable { Code = "19", Value = tagField.TagField19 }); }
                if (tagField.TagField20 != "") { ListCustomAttribute.Add(new Variable { Code = "20", Value = tagField.TagField20 }); }
            }
            return ListCustomAttribute;
        }

        private void BridgingToMedinfrasMobileApps(Patient oPatient, int mrn, string eventType)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {
                if (oPatient != null)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnPatientMasterChanged(oPatient, mrn, eventType);
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
                        entityAPILog.MessageText = apiResultInfo[0];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
            }
        }
    }
}