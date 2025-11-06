using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientEntryCtl : BaseEntryPopupCtl
    {
        protected int gridPatientNotesPageCount = 1;
        protected string dateNow = "";
        protected string GetTodayDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        public override void InitializeDataControl(string param)
        {
            hdnDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            dateNow = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
            txtPENamaPeserta.Attributes.Add("readonly", "readonly");
            txtPEJenisPeserta.Attributes.Add("readonly", "readonly");
            txtPEKelas.Attributes.Add("readonly", "readonly");
            txtPEPpkRujukan.Attributes.Add("readonly", "readonly");
            hdnIsBridgingBPJSVClaimVersion.Value = AppSession.SA0167;
            hdnDepartmentID.Value = AppSession.UserLogin.DepartmentID;

            if (AppSession.IsBridgingToBPJS == false)
            {
                tdPEValidateBPJS.Style.Add("display", "none");
                hdPEBPJSInformation.Attributes.Add("style", "display:none");
                tblPEBPJSInformation.Attributes.Add("style", "display:none");
            }
            else
            {
                tdPEValidateBPJS.Style.Add("display", "block");
                hdPEBPJSInformation.Attributes.Add("style", "display:block");
                tblPEBPJSInformation.Attributes.Add("style", "display:block");
            }

            if (AppSession.IsBridgingToInhealth == false)
            {
                tdPEValidateInhealth.Style.Add("display", "none");
            }
            else
            {
                tdPEValidateInhealth.Style.Add("display", "block");
                hdnTokenInhealth.Value = AppSession.Inheatlh_Access_Token;
                hdnKodeProviderInhealth.Value = AppSession.Inhealth_Provider_Code;
                hdnUsername.Value = AppSession.UserLogin.UserName;
                hdnTodayDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
            }

            if (param != "|")
            {
                string[] temp = param.Split('|');
                if (temp[0] == "app")
                {
                    IsAdd = true;
                    hdnAppointmentID.Value = temp[1];
                    Appointment entity = BusinessLayer.GetAppointment(Convert.ToInt32(hdnAppointmentID.Value));

                    DateTime tgllahir;
                    DateTime tglskrng = DateTime.Now;

                    SetControlProperties();
                    hdnMRN.Value = "";
                    txtFirstName.Text = entity.FirstName;
                    txtMiddleName.Text = entity.MiddleName;
                    txtFamilyName.Text = entity.LastName;
                    txtAddress.Text = entity.StreetName;
                    txtMobilePhone1.Text = entity.MobilePhoneNo;
                    txtTelephoneNo1.Text = entity.PhoneNo;
                    cboSalutation.Value = entity.GCSalutation;
                    txtCorporateAccountNoCtl.Text = entity.CorporateAccountNo;
                    txtCorporateAccountNameCtl.Text = entity.CorporateAccountName;

                    if (entity.GuestID != null && entity.GuestID != 0)
                    {
                        hdnIsGenerateRMFromGuest.Value = "1";
                        vGuest entityGuest = BusinessLayer.GetvGuestList(string.Format("GuestID = {0}", entity.GuestID)).FirstOrDefault();
                        hdnGuestID.Value = entityGuest.GuestID.ToString();
                        txtDOB.Text = entityGuest.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        if (string.IsNullOrEmpty(txtFirstName.Text))
                        {
                            txtFirstName.Text = entityGuest.FirstName;
                        }
                        if (string.IsNullOrEmpty(txtMiddleName.Text))
                        {
                            txtMiddleName.Text = entityGuest.MiddleName;
                        }
                        if (string.IsNullOrEmpty(txtFamilyName.Text))
                        {
                            txtFamilyName.Text = entityGuest.LastName;
                        }
                        cboGender.Value = entityGuest.GCGender;
                        txtIdentityCardNo.Text = entityGuest.SSN;
                        txtCity.Text = entityGuest.City;
                        txtCardName.Text = entityGuest.FullName;
                        txtCityDomicile.Text = entityGuest.CityDomicile;
                        txtAddressDomicile.Text = entityGuest.StreetNameDomicile;
                        txtBirthPlace.Text = entityGuest.CityOfBirth;
                        txtEmail.Text = !string.IsNullOrEmpty(entityGuest.EmailAddress) ? entityGuest.EmailAddress : string.Empty;
                        cboIdentityCardType.Value = entityGuest.GCIdentityNoType;
                        if (string.IsNullOrEmpty(txtMobilePhone1.Text))
                        {
                            txtMobilePhone1.Text = entityGuest.MobilePhoneNo;
                        }
                        if (string.IsNullOrEmpty(txtTelephoneNo1.Text))
                        {
                            txtTelephoneNo1.Text = entityGuest.PhoneNo;
                        }
                        if (!string.IsNullOrEmpty(entityGuest.GCReligion))
                        {
                            string[] religion = entityGuest.GCReligion.Split('^');
                            txtReligionCode.Text = religion[1];
                            txtReligionName.Text = entityGuest.Religion;
                        }
                        if (!string.IsNullOrEmpty(entityGuest.GCNationality))
                        {
                            string[] nationality = entityGuest.GCNationality.Split('^');
                            txtNationalityCode.Text = nationality[1];
                            txtNationalityName.Text = entityGuest.Nationality;
                        }
                        tgllahir = entityGuest.DateOfBirth;
                        cboBloodRhesus.Value = entityGuest.BloodRhesus;
                        cboBloodType.Value = entityGuest.GCBloodType;
                        if (!string.IsNullOrEmpty(entityGuest.GCEthnic))
                        {
                            string[] ethnic = entityGuest.GCEthnic.Split('^');
                            txtEthnicCode.Text = ethnic[1];
                            txtEthnicName.Text = entityGuest.Ethnic;
                        }
                        if (!string.IsNullOrEmpty(entityGuest.GCMaritalStatus))
                        {
                            string[] maritalStatus = entityGuest.GCMaritalStatus.Split('^');
                            txtMaritalStatusCode.Text = maritalStatus[1];
                            txtMaritalStatusName.Text = entityGuest.MaritalStatus;
                        }
                        if (!string.IsNullOrEmpty(entityGuest.GCLanguage))
                        {
                            string[] language = entityGuest.GCLanguage.Split('^');
                            txtlanguageCode.Text = language[1];
                            txtlanguageName.Text = entityGuest.Language;
                        }
                        if (!string.IsNullOrEmpty(entityGuest.GCEducation))
                        {
                            string[] education = entityGuest.GCEducation.Split('^');
                            txtEducationCode.Text = education[1];
                            txtEducationName.Text = entityGuest.Education;
                        }
                        if (!string.IsNullOrEmpty(entityGuest.GCOccupation))
                        {
                            string[] occupation = entityGuest.GCOccupation.Split('^');
                            txtPatientJobCode.Text = occupation[1];
                            txtPatientJobName.Text = entityGuest.Occupation;
                        }

                    }
                    else
                    {
                        Patient entityPatient = BusinessLayer.GetPatient(Convert.ToInt32(entity.MRN));
                        txtDOB.Text = entityPatient.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtEmail.Text = !string.IsNullOrEmpty(entityPatient.EmailAddress) ? entityPatient.EmailAddress : string.Empty;
                        tgllahir = entityPatient.DateOfBirth;
                    }

                    string[] patientAge = Helper.CalculateAge(tgllahir).Split('|');
                    txtAgeInYear.Text = patientAge[0];
                    txtAgeInMonth.Text = patientAge[1];
                    txtAgeInDay.Text = patientAge[2];

                    lblMRN.Attributes.Remove("class");
                    lblMRN.Attributes.Add("class", "lblDisabled");

                    txtMRNPatientEntryCtl.ReadOnly = true;
                }
                else if (temp[0] == "rev")
                {
                    IsAdd = true;
                    hdnReservationID.Value = temp[1];
                    vBedReservation entity = BusinessLayer.GetvBedReservationList(string.Format("ReservationID = {0}", hdnReservationID.Value)).FirstOrDefault();
                    SetControlProperties();
                    hdnMRN.Value = "";
                    txtFirstName.Text = entity.PatientFirstName;
                    txtMiddleName.Text = entity.PatientMiddleName;
                    txtFamilyName.Text = entity.PatientName;
                    txtAddress.Text = entity.PatientAddress;
                    txtMobilePhone1.Text = entity.MobilePhoneNo1;
                    txtTelephoneNo1.Text = entity.PhoneNo1;
                    lblMRN.Attributes.Remove("class");
                    lblMRN.Attributes.Add("class", "lblDisabled");

                    txtMRNPatientEntryCtl.ReadOnly = true;
                }
                else if (temp[0] == "mother")
                {
                    IsAdd = true;
                    vPatient entityPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", temp[1])).FirstOrDefault();
                    vPatientFamily entityFamily = BusinessLayer.GetvPatientFamilyList(string.Format("MRN = {0} AND GCFamilyRelation = '{1}' AND IsDeleted = 0", temp[1], Constant.FamilyRelation.SPOUSE)).FirstOrDefault();

                    SetControlProperties();
                    if (entityPatient != null)
                    {
                        if (hdnIsPatientNewBornNotTakeFromMotherName.Value == "0")
                        {
                            txtFirstName.Text = "Bayi";
                            txtMiddleName.Text = "Ny.";
                        }
                        txtFamilyName.Text = entityPatient.PatientName;
                        txtAddress.Text = entityPatient.StreetName;
                        txtCounty.Text = entityPatient.County;
                        txtDistrict.Text = entityPatient.District;
                        txtCity.Text = entityPatient.City;
                        txtMotherName.Text = entityPatient.PatientName;
                        if (entityFamily != null)
                        {
                            txtFatherName.Text = entityFamily.FullName;
                        }

                        string[] religion = entityPatient.GCReligion.Split('^');
                        if (!string.IsNullOrEmpty(entityPatient.GCReligion))
                        {
                            txtReligionCode.Text = religion[1];
                        }
                        txtReligionName.Text = entityPatient.Religion;

                        if (!string.IsNullOrEmpty(entityPatient.GCNationality))
                        {
                            string[] nationality = entityPatient.GCNationality.Split('^');
                            txtNationalityCode.Text = nationality[1];
                            txtNationalityName.Text = entityPatient.Nationality;
                        }

                        txtMobilePhone1.Text = entityPatient.MobilePhoneNo1;
                        txtCorporateAccountNoCtl.Text = entityPatient.CorporateAccountNo;
                        txtCorporateAccountNameCtl.Text = entityPatient.CorporateAccountName;
                        txtCorporateAccountDepartmentCtl.Text = entityPatient.CorporateAccountDepartment;

                    }
                }
                else if (temp[0] == "cekDuplikat")
                {
                    SetControlProperties();
                    string[] nama = temp[1].Split(' ');
                    if (nama.Length == 1)
                    {
                        txtFamilyName.Text = nama[0];
                    }
                    else if (nama.Length == 2)
                    {
                        txtFirstName.Text = nama[0];
                        txtFamilyName.Text = nama[1];
                    }
                    else if (nama.Length > 2)
                    {
                        txtFirstName.Text = nama[0];
                        txtMiddleName.Text = nama[1];
                        string[] lastName = nama.Where((val, idx) => idx > 1).ToArray();
                        txtFamilyName.Text = string.Join(" ", lastName);
                    }
                    txtAddress.Text = temp[2];
                    txtMobilePhone1.Text = temp[3];
                    txtIdentityCardNo.Text = temp[4];
                    txtMotherName.Text = temp[5];
                }
                else if (temp[0] == "inhealth")
                {
                    IsAdd = true;
                    txtInhealthParticipantNo.Text = temp[1];
                    txtMobilePhone1.Text = temp[2];
                    txtEmail.Text = temp[3];
                    SetControlProperties();
                }
                else if (temp[0] == "generate")
                {
                    hdnIsGenerateRMFromGuest.Value = "1";
                    vGuest entityGuest = BusinessLayer.GetvGuestList(string.Format("GuestID = {0}", temp[1])).FirstOrDefault();
                    hdnGuestID.Value = entityGuest.GuestID.ToString();
                    IsAdd = true;
                    if (!string.IsNullOrEmpty(entityGuest.GCIdentityNoType))
                    {
                        cboIdentityCardType.Value = entityGuest.GCIdentityNoType;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.SSN))
                    {
                        txtIdentityCardNo.Text = entityGuest.SSN;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.FirstName))
                    {
                        txtFirstName.Text = entityGuest.FirstName;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.MiddleName))
                    {
                        txtMiddleName.Text = entityGuest.MiddleName;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.LastName))
                    {
                        txtFamilyName.Text = entityGuest.LastName;
                    }
                    txtCardName.Text = entityGuest.FullName;
                    txtDOB.Text = entityGuest.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    cboGender.Value = entityGuest.GCGender;
                    txtAddress.Text = entityGuest.StreetName;
                    txtDistrict.Text = entityGuest.District;
                    txtBirthPlace.Text = entityGuest.CityOfBirth;
                    txtCity.Text = entityGuest.City;
                    txtCounty.Text = entityGuest.County;
                    txtAddressDomicile.Text = entityGuest.StreetNameDomicile;
                    txtDistrictDomicile.Text = entityGuest.DistrictDomicile;
                    txtCityDomicile.Text = entityGuest.CityDomicile;
                    txtCountyDomicile.Text = entityGuest.CountyDomicile;
                    txtEmail.Text = entityGuest.EmailAddress;
                    txtMobilePhone1.Text = entityGuest.MobilePhoneNo;
                    txtTelephoneNo1.Text = entityGuest.PhoneNo;
                    txtCorporateAccountNoCtl.Text = entityGuest.CorporateAccountNo;
                    txtCorporateAccountNameCtl.Text = entityGuest.CorporateAccountName;
                    txtCorporateAccountDepartmentCtl.Text = entityGuest.CorporateAccountDepartment;

                    string[] patientAge = Helper.CalculateAge(entityGuest.DateOfBirth).Split('|');
                    //txtAgeInYear.Text = (tglskrng.Year - tgllahir.Year).ToString();
                    //txtAgeInMonth.Text = (tglskrng.Month - tgllahir.Month).ToString();
                    //txtAgeInDay.Text = (tglskrng.Day - tgllahir.Day).ToString();
                    txtAgeInYear.Text = patientAge[0];
                    txtAgeInMonth.Text = patientAge[1];
                    txtAgeInDay.Text = patientAge[2];

                    if (!string.IsNullOrEmpty(entityGuest.GCBloodType))
                    {
                        cboBloodType.Value = entityGuest.GCBloodType;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCTitle))
                    {
                        cboTitle.Value = entityGuest.GCTitle;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCReligion))
                    {
                        txtReligionCode.Text = entityGuest.GCReligion.Split('^')[1];
                        txtReligionName.Text = entityGuest.Religion;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCIdentityNoType))
                    {
                        cboIdentityCardType.Value = entityGuest.GCIdentityNoType;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCTitle))
                    {
                        cboTitle.Value = entityGuest.GCTitle;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCSalutation))
                    {
                        cboSalutation.Value = entityGuest.GCSalutation;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCEthnic))
                    {
                        txtEthnicCode.Text = entityGuest.GCEthnic.Split('^')[1];
                        txtEthnicName.Text = entityGuest.Ethnic;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCMaritalStatus))
                    {
                        txtMaritalStatusCode.Text = entityGuest.GCMaritalStatus.Split('^')[1];
                        txtMaritalStatusName.Text = entityGuest.MaritalStatus;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCOccupation))
                    {
                        txtPatientJobCode.Text = entityGuest.GCOccupation.Split('^')[1];
                        txtPatientJobName.Text = entityGuest.Occupation;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCEducation))
                    {
                        txtEducationCode.Text = entityGuest.GCEducation.Split('^')[1];
                        txtEducationName.Text = entityGuest.Education;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCLanguage))
                    {
                        txtlanguageCode.Text = entityGuest.GCLanguage.Split('^')[1];
                        txtlanguageName.Text = entityGuest.Language;
                    }
                    if (!string.IsNullOrEmpty(entityGuest.GCSuffix))
                    {
                        cboSuffix.Value = entityGuest.Suffix;
                    }

                    SetControlProperties();

                }
                else
                {
                    IsAdd = false;
                    SetControlProperties();
                    hdnReservationID.Value = "";
                    hdnAppointmentID.Value = "";
                    hdnMRN.Value = temp[0];
                    hdnRegistrationID.Value = temp[1];
                    vPatient entity = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", temp[0]))[0];
                    PatientTagField entityTagField = BusinessLayer.GetPatientTagField(entity.MRN);
                    EntityToControl(entity, entityTagField);

                    trPayerCompany.Style.Remove("display");
                    if (cboPayer.Value != null)
                    {
                        if (cboPayer.Value.ToString() == Constant.CustomerType.PERSONAL)
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
            }
            else
            {
                hdnReservationID.Value = "";
                hdnAppointmentID.Value = "";
                hdnMRN.Value = "";
                IsAdd = true;
                SetControlProperties();
                BindGridViewPatientNotes(1, true, ref gridPatientNotesPageCount);
            }

            if (IsAdd)
                tdReusedRM.Style.Add("display", "block");
            else
                tdReusedRM.Style.Add("display", "none");


            if (!String.IsNullOrEmpty(hdnMRN.Value))
            {
                btnSavePatientNotes.Style.Remove("display");
                classSavePatientNotes.Style.Remove("display");
            }
            else
            {
                btnSavePatientNotes.Style.Add("display", "none");
                classSavePatientNotes.Style.Add("display", "none");
            }

            GetSettingParameter();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}')",
                        Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS,
                        Constant.SettingParameter.RM_BLOK_DOUBLE_PATIENT_DATA,
                        Constant.SettingParameter.RM_IS_MOBILE_PHONE_NUMERIC,
                        Constant.SettingParameter.RM_PATIENT_NAME_NEW_BORN_NOT_TAKE_FROM_MOTHER_NAME
                        ));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
            hdnIsBlockDoublePatientData.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM_BLOK_DOUBLE_PATIENT_DATA).FirstOrDefault().ParameterValue;
            hdnIsMobilePhoneNumeric.Value = lstSetParDt.Where(p => p.ParameterCode == Constant.SettingParameter.RM_IS_MOBILE_PHONE_NUMERIC).FirstOrDefault().ParameterValue;
            hdnIsPatientNewBornNotTakeFromMotherName.Value = lstSetParDt.Where(p => p.ParameterCode == Constant.SettingParameter.RM_PATIENT_NAME_NEW_BORN_NOT_TAKE_FROM_MOTHER_NAME).FirstOrDefault().ParameterValue;
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

        private void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}') AND IsActive = 1 AND IsDeleted = 0",
                                                                            Constant.StandardCode.TITLE, //0
                                                                            Constant.StandardCode.SALUTATION, //1
                                                                            Constant.StandardCode.SUFFIX, //2
                                                                            Constant.StandardCode.BLOOD_TYPE, //3
                                                                            Constant.StandardCode.IDENTITY_NUMBERY_TYPE, //4
                                                                            Constant.StandardCode.GENDER, //5
                                                                            Constant.StandardCode.CUSTOMER_TYPE, //6
                                                                            Constant.StandardCode.VIP_PATIENT_GROUP, //7
                                                                            Constant.StandardCode.PATIENT_BLACKLIST_REASON, //8
                                                                            Constant.StandardCode.COMINICATION, //9
                                                                            Constant.StandardCode.PHYSICAL_LIMITATION_TYPE //10
                                                                        ));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSalutation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTitle, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffix, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBloodType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.BLOOD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboIdentityCardType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.IDENTITY_NUMBERY_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGender, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.GENDER).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPayer, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
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
            cboIdentityCardType.SelectedIndex = 0;
            cboPayer.Value = Constant.CustomerType.PERSONAL;
            cboVIPGroup.SelectedIndex = 0;
            cboGCBlacklistReason.SelectedIndex = 0;
            cboCommunication.SelectedIndex = 0;
            cboPhysicalLimitation.SelectedIndex = 0;

            trPayerCompany.Style.Remove("display");
            if (cboPayer.Value.ToString() == Constant.CustomerType.PERSONAL)
                trPayerCompany.Style.Add("display", "none");

        }

        protected override void OnControlEntrySetting()
        {
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string defaultGCState = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];
            string defaultPhoneArea = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PHONE_AREA).ParameterValue;
            hdnPatientLastName.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_LASTNAME_FROM_BPJS).ParameterValue;
            StandardCode scNationality = BusinessLayer.GetStandardCode(Constant.Nationality.INDONESIA);
            SetControlEntrySetting(hdnMRN, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMRNPatientEntryCtl, new ControlEntrySetting(true, false, false));

            #region Patient Data

            SetControlEntrySetting(lblMRN, new ControlEntrySetting(true, false));

            SetControlEntrySetting(txtFamilyCardNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsSSNTemporary, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(txtEKlaimMedicalNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSITBRegisterNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGuestNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboTitle, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFatherName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMotherName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSpouseName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtChildName, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFamilyName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCardName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPreferredName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboSuffix, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEthnicCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEthnicName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBirthPlace, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGender, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBloodType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboBloodRhesus, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDOB, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtAgeInDay, new ControlEntrySetting(true, true, true, 1));
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
            SetControlEntrySetting(txtNationalityCode, new ControlEntrySetting(true, true, true, scNationality.StandardCodeID.Split('^')[1]));
            SetControlEntrySetting(txtNationalityName, new ControlEntrySetting(true, false, true, scNationality.StandardCodeName));
            SetControlEntrySetting(txtAddressDomicile, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRTDomicileData, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRWDomicileData, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCountyDomicile, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrictDomicile, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCityDomicile, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceDomicileCode, new ControlEntrySetting(true, true, false, defaultGCState));
            SetControlEntrySetting(txtProvinceDomicileName, new ControlEntrySetting(false, false, false, healthcare.State));
            SetControlEntrySetting(hdnZipCodeDomicile, new ControlEntrySetting(true, true));
            //            SetControlEntrySetting(txtZipCodeDomicile, new ControlEntrySetting(true, true, false));
            #endregion

            #region Patient Contact
            SetControlEntrySetting(txtTelephoneNo1, new ControlEntrySetting(true, true, true, defaultPhoneArea));
            SetControlEntrySetting(txtTelephoneNo2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMobilePhone1, new ControlEntrySetting(true, true, true));
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
            SetControlEntrySetting(txtPatientJobOfficeZipCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnOfficeZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPatientJobOfficeTelephone, new ControlEntrySetting(true, true, false, defaultPhoneArea));
            SetControlEntrySetting(txtPatientJobOfficeEmail, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCorporateAccountNoCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCorporateAccountNameCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCorporateAccountDepartmentCtl, new ControlEntrySetting(true, true, false));
            #endregion

            #region Patient Status
            SetControlEntrySetting(chkIsAlive, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(chkIsBlackList, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCBlacklistReason, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtOtherBlackListReason, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsVIP, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboVIPGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtOtherVIPGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsDonor, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsG6PD, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasAllergy, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsIlliteracy, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsSmoking, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasCommunicationRestriction, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboCommunication, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsHasPhysicalLimitation, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboPhysicalLimitation, new ControlEntrySetting(true, true, true));
            #endregion

            #region Other Information
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            #endregion

            #region setvar Mandatory
            string filterSetvar = string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                    Constant.SettingParameter.IS_FIRST_NAME_MANDATORY,
                                                    Constant.SettingParameter.IS_MIDDLE_NAME_ALLOW_NULL,
                                                    Constant.SettingParameter.IS_ZIP_CODE_MANDATORY,
                                                    Constant.SettingParameter.IS_OCCUPATION_PATIENT_MANDATORY,
                                                    Constant.SettingParameter.IS_EDUCATION_PATIENT_MANDATORY,
                                                    Constant.SettingParameter.RM_IS_SSN_MANDATORY,
                                                    Constant.SettingParameter.RM_IS_SALUTATION_MANDATORY,
                                                    Constant.SettingParameter.RM0078 
                                                    );
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            string isFirstNameMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_FIRST_NAME_MANDATORY).FirstOrDefault().ParameterValue;
            string isMiddleNameAllowNull = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_MIDDLE_NAME_ALLOW_NULL).FirstOrDefault().ParameterValue;
            string isZipCodeMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_ZIP_CODE_MANDATORY).FirstOrDefault().ParameterValue;
            string isOccupationMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_OCCUPATION_PATIENT_MANDATORY).FirstOrDefault().ParameterValue;
            string isEducationMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_EDUCATION_PATIENT_MANDATORY).FirstOrDefault().ParameterValue;
            string isSSNMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.RM_IS_SSN_MANDATORY).FirstOrDefault().ParameterValue;
            string isSalutationMandatory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.RM_IS_SALUTATION_MANDATORY).FirstOrDefault().ParameterValue;
            string isAddressUseZipCode = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.RM0078).FirstOrDefault().ParameterValue;

            if (isAddressUseZipCode == "1")
            {
                txtCounty.Attributes.Add("readonly", "readonly");
                txtDistrict.Attributes.Add("readonly", "readonly");
                txtCity.Attributes.Add("readonly", "readonly");
                txtProvinceCode.Attributes.Add("readonly", "readonly");
                lblProvince.Attributes.Remove("class");
                lblProvince.Attributes.Add("class", "lblNormal");
                lblCity.Attributes.Remove("class");
                lblCity.Attributes.Add("class", "lblNormal");

                txtCountyDomicile.Attributes.Add("readonly", "readonly");
                txtDistrictDomicile.Attributes.Add("readonly", "readonly");
                txtCityDomicile.Attributes.Add("readonly", "readonly");
                txtProvinceDomicileCode.Attributes.Add("readonly", "readonly");
                lblDomicileProvince.Attributes.Remove("class");
                lblDomicileProvince.Attributes.Add("class", "lblNormal");
                lblCityDomicile.Attributes.Remove("class");
                lblCityDomicile.Attributes.Add("class", "lblNormal");

                txtPatientJobOfficeCounty.Attributes.Add("readonly", "readonly");
                txtPatientJobOfficeDistrict.Attributes.Add("readonly", "readonly");
                txtPatientJobOfficeCity.Attributes.Add("readonly", "readonly");
                txtPatientJobOfficeProvinceCode.Attributes.Add("readonly", "readonly");
                lblPatientJobOfficeCity.Attributes.Remove("class");
                lblPatientJobOfficeCity.Attributes.Add("class", "lblNormal");
                lblOfficeProvince.Attributes.Remove("class");
                lblOfficeProvince.Attributes.Add("class", "lblNormal");
            }

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
                SetControlEntrySetting(txtZipCodeDomicile, new ControlEntrySetting(true, true, true));
            }
            else
            {
                SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtZipCodeDomicile, new ControlEntrySetting(true, true, false));

                lblZipCodeDomicile.Attributes.Remove("class");
                lblZipCodeDomicile.Attributes.Add("class", "lblLink");

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

            GetSettingParameter();
        }

        private void EntityToControl(vPatient entity, PatientTagField entityTagField)
        {
            txtMRNPatientEntryCtl.Text = entity.MedicalNo;

            #region Patient Data

            chkIsSSNTemporary.Checked = entity.IsSSNTemporary;
            cboIdentityCardType.Value = entity.GCIdentityNoType;
            txtIdentityCardNo.Text = entity.SSN;
            txtFamilyCardNo.Text = entity.FamilyCardNo;
            txtNHSRegistrationNo.Text = entity.NHSRegistrationNo;
            txtInhealthParticipantNo.Text = entity.InhealthParticipantNo;
            txtIHSNumber.Text = entity.IHSNumber;
            txtEKlaimMedicalNo.Text = entity.EKlaimMedicalNo;
            txtSITBRegisterNo.Text = entity.SITBRegisterNo;

            cboSalutation.Value = entity.GCSalutation;
            cboTitle.Value = entity.GCTitle;
            txtFirstName.Text = entity.FirstName;
            txtMiddleName.Text = entity.MiddleName;
            txtFamilyName.Text = entity.LastName;
            txtName2.Text = entity.name2;
            txtCardName.Text = entity.CardName;
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
            txtPENamaPeserta.Text = entity.NamaPesertaBPJS;
            txtPEJenisPeserta.Text = entity.JenisPesertaBPJS;
            txtPEKelas.Text = entity.KodeKelasBPJS;
            txtPEPpkRujukan.Text = entity.KodePPK1BPJS + " - " + entity.NamaPPK1BPJS;
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

            #region Patient Image
            string logoPath = string.Format("Patient/{0}/", entity.MedicalNo);
            imgPreview.Src = string.Format("{0}{1}{2}", AppConfigManager.QISVirtualDirectory, logoPath, entity.PictureFileName);

            //if (File.Exists(imgPreview.Src) == false)
            //if (!File.Exists(Server.MapPath(imgPreview.Src)))
            if (!File.Exists(string.Format("{0}{1}{2}", AppConfigManager.QISPhysicalDirectory, logoPath, entity.PictureFileName)))
            {
                if (entity.GCGender == Constant.Gender.MALE)
                {
                    imgPreview.Src = string.Format("{0}Patient/patient_male.png", AppConfigManager.QISVirtualDirectory);
                }
                else if (entity.GCGender == Constant.Gender.FEMALE)
                {
                    imgPreview.Src = string.Format("{0}Patient/patient_female.png", AppConfigManager.QISVirtualDirectory);
                }
            }
            #endregion

            #region Patient Address
            txtAddress.Text = entity.StreetName;
            txtRTData.Text = entity.RT;
            txtRWData.Text = entity.RW;
            txtCounty.Text = entity.County; // Desa Kelurahan
            txtDistrict.Text = entity.District; // Kecamatan
            txtCity.Text = entity.City; // Kota Kabupaten
            if (entity.GCState != "")
                txtProvinceCode.Text = entity.GCState.Split('^')[1];
            else
                txtProvinceCode.Text = "";
            txtProvinceName.Text = entity.State;
            hdnZipCode.Value = entity.ZipCodeID.ToString();
            txtZipCode.Text = entity.ZipCode;
            #endregion

            #region Patient Other Address
            txtAddressDomicile.Text = entity.OtherStreetName;
            txtRTDomicileData.Text = entity.OtherRT;
            txtRWDomicileData.Text = entity.OtherRW;
            txtCountyDomicile.Text = entity.OtherCounty; // Desa Kelurahan
            txtDistrictDomicile.Text = entity.OtherDistrict; // Kecamatan
            txtCityDomicile.Text = entity.OtherCity; // Kota Kabupaten
            if (entity.OtherGCState != "")
                txtProvinceDomicileCode.Text = entity.OtherGCState.Split('^')[1];
            else
                txtProvinceDomicileCode.Text = "";
            txtProvinceDomicileName.Text = entity.OtherState;
            hdnZipCodeDomicile.Value = entity.OtherZipCodeID.ToString();
            txtZipCodeDomicile.Text = entity.OtherZipCode;
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

            if (entity.GCLanguage != "")
                txtlanguageCode.Text = entity.GCLanguage.Split('^')[1];
            else
                txtlanguageCode.Text = "";
            txtlanguageName.Text = entity.Bahasa;

            txtPatientCategoryName.Text = entity.PatientCategory;
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
            txtCorporateAccountNoCtl.Text = entity.CorporateAccountNo;
            txtCorporateAccountNameCtl.Text = entity.CorporateAccountName;
            txtCorporateAccountDepartmentCtl.Text = entity.CorporateAccountDepartment;
            #endregion

            #region Patient Status
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
            chkIsHasAllergy.Checked = entity.IsHasAllergy;
            chkIsIlliteracy.Checked = entity.IsIlliteracy;
            chkIsCataract.Checked = entity.IsCataract;
            chkIsSmoking.Checked = entity.IsSmoking;
            chkIsPregnant.Checked = entity.IsPregnant;

            if (entity.GCGender == Constant.Gender.FEMALE)
            {
                chkIsPregnant.Enabled = true;
            }
            else
            {
                chkIsPregnant.Enabled = false;
            }
            #endregion

            #region Other Information
            txtOldMedicalNo.Text = entity.OldMedicalNo;
            txtNotes.Text = entity.Notes;
            #endregion

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                txt.Text = entityTagField.GetType().GetProperty("TagField" + hdn.Value).GetValue(entityTagField, null).ToString();
            }
            #endregion

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
                }

            }
            #endregion

            BindGridViewPatientNotes(1, true, ref gridPatientNotesPageCount);
        }

        private void ControlToEntity(Patient entity, Address entityAddress, Address entityOfficeAddress, Address entityDomicileAddress, PatientTagField entityTagField,
            ref PatientFamily entityPatientMother, ref PatientFamily entityPatientFather, ref PatientFamily entityPatientSpouse, ref PatientFamily entityPatientChild)
        {
            #region Patient Data

            entity.IsSSNTemporary = chkIsSSNTemporary.Checked;
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

            bool flagToUpperPatientName = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CAPITALIZE_PATIENT_NAME).ParameterValue == "1";
            if (flagToUpperPatientName)
            {
                entity.FirstName = entity.FirstName.ToUpper();
                entity.MiddleName = entity.MiddleName.ToUpper();
                entity.LastName = entity.LastName.ToUpper();
                entity.PreferredName = entity.PreferredName.ToUpper();
            }
            entity.GCSuffix = Helper.GetComboBoxValue(cboSuffix, true);
            entity.GCSex = entity.GCGender = cboGender.Value.ToString();
            entity.GCEthnic = txtEthnicCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.ETHNIC, txtEthnicCode.Text);
            entity.CityOfBirth = txtBirthPlace.Text;
            entity.GCBloodType = Helper.GetComboBoxValue(cboBloodType, true);
            entity.BloodRhesus = Helper.GetComboBoxValue(cboBloodRhesus, true);
            entity.DateOfBirth = Helper.GetDatePickerValue(txtDOB);
            if (!txtPENamaPeserta.Text.Equals(string.Empty))
            {
                entity.NamaPesertaBPJS = txtPENamaPeserta.Text;
                entity.JenisPesertaBPJS = txtPEJenisPeserta.Text;
                entity.KodeKelasBPJS = txtPEKelas.Text;
                String[] PPK = txtPEPpkRujukan.Text.Split(new string[] { " - " }, StringSplitOptions.None);
                entity.KodePPK1BPJS = PPK[0];
                entity.NamaPPK1BPJS = PPK[1];
            }
            string suffix = cboSuffix.Value == null ? "" : cboSuffix.Text;
            string title = cboTitle.Value == null ? "" : cboTitle.Text;
            entity.Name = txtCardName.Text;
            entity.FullName = Helper.GenerateFullName(Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName), title, suffix);

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
            if (IsAdd)
            {
                entity.RegisteredDate = DateTime.Now.Date;
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
            {
                entityAddress.ZipCode = null;
            }
            else
            {
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCode.Value);
            }

            entityDomicileAddress.StreetName = txtAddressDomicile.Text;
            entityDomicileAddress.RT = txtRTDomicileData.Text;
            entityDomicileAddress.RW = txtRWDomicileData.Text;
            entityDomicileAddress.County = txtCountyDomicile.Text; // Desa
            entityDomicileAddress.District = txtDistrictDomicile.Text; //Kabupaten
            entityDomicileAddress.City = txtCityDomicile.Text;
            entityDomicileAddress.GCState = txtProvinceDomicileCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceDomicileCode.Text);
            if (hdnZipCodeDomicile.Value == "" || hdnZipCodeDomicile.Value == "0")
            {
                entityDomicileAddress.ZipCode = null;
            }
            else
            {
                entityDomicileAddress.ZipCode = Convert.ToInt32(hdnZipCodeDomicile.Value);
            }

            bool flagToUpperPatientAddress = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CAPITALIZE_PATIENT_ADDRESS).ParameterValue == "1";
            if (flagToUpperPatientAddress)
            {
                entityAddress.StreetName = txtAddress.Text.ToUpper();
                entityDomicileAddress.StreetName = txtAddressDomicile.Text.ToUpper();
            }
            else
            {
                entityAddress.StreetName = txtAddress.Text;
                entityDomicileAddress.StreetName = txtAddressDomicile.Text;
            }
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
            if (GCCustomerType != Constant.CustomerType.PERSONAL)
            {
                if (string.IsNullOrEmpty(GCCustomerType))
                {
                    entity.BusinessPartnerID = null;
                }
                else
                {
                    if (!String.IsNullOrEmpty(hdnPayerID.Value)) entity.BusinessPartnerID = Convert.ToInt32(hdnPayerID.Value);
                }
            }
            else
            {
                entity.BusinessPartnerID = 1;
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
            if (entityOfficeAddress.GCState != "" || entityOfficeAddress.GCState != null)
            {
                entityOfficeAddress.GCState = txtPatientJobOfficeProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtPatientJobOfficeProvinceCode.Text);
            }
            else
            {
                entityOfficeAddress.GCState = txtPatientJobOfficeProvinceCode.Text;
            }
            if (hdnOfficeZipCode.Value == "")
                entityOfficeAddress.ZipCode = null;
            else
                entityOfficeAddress.ZipCode = Convert.ToInt32(hdnOfficeZipCode.Value);
            entityOfficeAddress.PhoneNo1 = txtPatientJobOfficeTelephone.Text;
            entityOfficeAddress.Email = txtPatientJobOfficeEmail.Text;
            entity.CorporateAccountNo = txtCorporateAccountNoCtl.Text;
            entity.CorporateAccountName = txtCorporateAccountNameCtl.Text;
            entity.CorporateAccountDepartment = txtCorporateAccountDepartmentCtl.Text;
            #endregion

            #region Patient Status
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
                    entity.OtherBlacklistReason = txtOtherBlackListReason.Text;
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
            entity.IsCataract = chkIsCataract.Checked;
            entity.IsHasAllergy = chkIsHasAllergy.Checked;
            entity.IsIlliteracy = chkIsIlliteracy.Checked;
            entity.IsSmoking = chkIsSmoking.Checked;
            entity.GCPatientStatus = Constant.PatientStatus.ACTIVE;
            entity.IsPregnant = chkIsPregnant.Checked;
            #endregion

            #region Other Information
            if (entity.OldMedicalNo == null || entity.OldMedicalNo == "")
            {
                entity.OldMedicalNo = txtOldMedicalNo.Text;
            }
            entity.Notes = txtNotes.Text;
            #endregion

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                entityTagField.GetType().GetProperty("TagField" + hdn.Value).SetValue(entityTagField, txt.Text, null);
            }
            #endregion

            //StandardCode scSalutation = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND '{3:000}-{4:00}-{5:00}' BETWEEN SUBSTRING(TagProperty,3,9) AND SUBSTRING(TagProperty,13,9) AND LEFT(TagProperty,1) IN ('{1}',' ') AND SUBSTRING(TagProperty,23,1) IN ('{2}',' ')", Constant.StandardCode.SALUTATION, entity.GCSex.Split('^')[1], txtMaritalStatusCode.Text, Request.Form[txtAgeInYear.UniqueID], Request.Form[txtAgeInMonth.UniqueID], Request.Form[txtAgeInDay.UniqueID]), ctx).FirstOrDefault();
            //string suffix = "";
            //if (scSalutation != null)
            //{
            //    entity.GCSalutation = scSalutation.StandardCodeID;
            //    suffix = scSalutation.StandardCodeName;
            //}
            //else
            //    entity.GCSalutation = null;

            //string title = cboTitle.Value == null ? "" : cboTitle.Text;
            //entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
            //entity.FullName = Helper.GenerateFullName(entity.Name, title, suffix);
        }

        private void ControlToEntity(PatientNotes entityPatientNotes)
        {
            entityPatientNotes.MRN = Convert.ToInt32(hdnMRN.Value);
            entityPatientNotes.Notes = txtPatientNotes.Text;
        }

        private void UploadPhoto(string Medical_No)
        {
            if (hdnUploadedFile1.Value != "")
            {
                string imageData = hdnUploadedFile1.Value;
                if (imageData != "")
                {
                    string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
                    imageData = String.Join(",", parts);
                }

                string path = AppConfigManager.QISPhysicalDirectory;
                path += string.Format("{0}\\Identity\\", AppConfigManager.QISPatientDocumentsPath.Replace('/', '\\'));

                path = path.Replace("#MRN", Medical_No);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileName = String.Format("{0}_Identitas.png", Medical_No);
                FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                byte[] data = Convert.FromBase64String(imageData);
                bw.Write(data);
                bw.Close();
            }
        }

        protected string IdentityPatientExis()
        {
            string Fullname = string.Format("{0}", txtFamilyName.Text.ToLower());

            string firstName = txtFirstName.Text.ToLower();
            if (firstName != "")
            {
                Fullname = string.Format("{0} {1}", txtFirstName.Text.ToLower(), txtFamilyName.Text.ToLower());
            }

            string MobilePhone1 = txtMobilePhone1.Text.Trim();
            string StreetName = txtAddress.Text;
            if (StreetName == "")
            {
                StreetName = "StreetName IS NULL";
            }
            else
            {
                StreetName = string.Format("StreetName LIKE '%{0}%'", txtAddress.Text);
            }

            string County = txtCounty.Text;
            if (County == "")
            {
                County = "County IS NULL";
            }
            else
            {
                County = string.Format("County LIKE '%{0}%'", txtCounty.Text);
            }

            string District = txtDistrict.Text;
            if (District == "")
            {
                District = "District IS NULL";
            }
            else
            {
                District = string.Format("District LIKE '%{0}%'", txtDistrict.Text);
            }

            string filterExpression = string.Format("PatientName = '{0}' AND DateOfBirth='{1}' AND GCGender = '{2}' AND {3} AND {4} AND {5} AND IsDeleted = 0", Fullname, Helper.GetDatePickerValue(txtDOB).ToString(Constant.FormatString.DATE_PICKER_FORMAT2), cboGender.Value, StreetName, County, District);
            vPatient oPatient = BusinessLayer.GetvPatientList(filterExpression).FirstOrDefault();
            if (oPatient != null)
            {
                return string.Format("1|{0}", oPatient.MedicalNo);
            }
            return string.Format("0|"); ;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDao entityDao = new PatientDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            PatientTagFieldDao entityTagFieldDao = new PatientTagFieldDao(ctx);
            AppointmentDao entityAppointmentDao = new AppointmentDao(ctx);
            RegistrationDao entityRegDao = new RegistrationDao(ctx);
            BedReservationDao entityReservationDao = new BedReservationDao(ctx);
            PatientFamilyDao entityPatientFamilyDao = new PatientFamilyDao(ctx);
            GuestDao guestDao = new GuestDao(ctx);
            try
            {
                //string[] PatientExis = IdentityPatientExis().Split('|');
                //if (PatientExis[0] == "1")
                //{
                //    errMessage = string.Format("sudah terdaftar menjadi pasien dengan medical No. {0}", PatientExis[1]);
                //    result = false;
                //}

                Patient entity = new Patient();

                Address entityAddress = new Address();
                Address entityOfficeAddress = new Address();
                Address entityDomicileAddress = new Address();

                PatientTagField entityTagField = new PatientTagField();

                PatientFamily entityPatientMother = null;
                Address entityPatientMotherAddress = new Address();
                PatientFamily entityPatientFather = null;
                Address entityPatientFatherAddress = new Address();
                PatientFamily entityPatientSpouse = null;
                Address entityPatientSpouseAddress = new Address();
                PatientFamily entityPatientChild = null;
                Address entityPatientChildAddress = new Address();

                ControlToEntity(entity, entityAddress, entityOfficeAddress, entityDomicileAddress, entityTagField, ref entityPatientMother, ref entityPatientFather, ref entityPatientSpouse, ref entityPatientChild);

                entity.IsAlive = true;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.CreatedBy = AppSession.UserLogin.UserID;

                entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                entityOfficeAddress.CreatedBy = AppSession.UserLogin.UserID;
                entityDomicileAddress.CreatedBy = AppSession.UserLogin.UserID;

                entity.HomeAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);
                entity.OfficeAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityOfficeAddress);
                entity.OtherAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityDomicileAddress);

                string medicalNo = Helper.GenerateMRN(entity.MRN, ctx);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();


                if (medicalNo != string.Empty)
                {
                    if (IsMedicalNoValid(medicalNo, ctx))
                    {
                        entity.MedicalNo = medicalNo;
                        entity.PictureFileName = string.Format("{0}.jpg", entity.MedicalNo);
                        //entityDao.Update(entity);
                    }
                    else
                    {
                        errMessage = "Terjadi kesalahan ketika proses pembuatan Nomor Rekam Medis (101)";
                        ctx.RollBackTransaction();
                        ctx.Close();
                        return false;
                    }
                }
                else
                {
                    errMessage = "Terjadi kesalahan ketika proses pembuatan Nomor Rekam Medis";
                    ctx.RollBackTransaction();
                    ctx.Close();
                    return false;
                }

                if (hdnIsGenerateRMFromGuest.Value == "1")
                {
                    entity.GuestID = Convert.ToInt32(hdnGuestID.Value);
                }

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entity.MRN = entityDao.InsertReturnPrimaryKeyID(entity);

                //guest
                if (!string.IsNullOrEmpty(hdnGuestID.Value))
                {
                    if (Convert.ToInt32(hdnGuestID.Value) > 0)
                    {
                        Guest oGuest = BusinessLayer.GetGuestList(string.Format("GuestID={0}", hdnGuestID.Value), ctx).FirstOrDefault();
                        oGuest.MRN = entity.MRN;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        guestDao.Update(oGuest);

                        if (!string.IsNullOrEmpty(oGuest.PictureFileName))
                        {
                            string fromPath = AppConfigManager.QISPhysicalDirectory + AppConfigManager.QISGuestImagePath.Replace("#GUESTNO", oGuest.GuestNo).Replace("/", "\\") + "Document\\";// +oGuest.PictureFileName;
                            if (Directory.Exists(fromPath))
                            {
                                string toPath = AppConfigManager.QISPhysicalDirectory + AppConfigManager.QISPatientImagePath.Replace("#MRN", entity.MedicalNo).Replace("/", "\\") + "Document\\Identity\\";
                                if (!Directory.Exists(toPath))
                                    Directory.CreateDirectory(toPath);
                                string[] fileList = System.IO.Directory.GetFiles(fromPath);
                                foreach (string file in fileList)
                                {
                                    string moveTo = toPath + entity.PictureFileName;
                                    //moving file
                                    File.Move(file, moveTo);
                                }
                            }
                        }
                    }
                }

                entityTagField.MRN = entity.MRN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityTagFieldDao.Insert(entityTagField);

                if (hdnIsGenerateRMFromGuest.Value == "1")
                {
                    BusinessLayer.MoveGuestToMedicalNumber(Convert.ToInt32(hdnGuestID.Value), entity.MRN, ctx);
                }

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                if (hdnReservationID.Value != "")
                {
                    BedReservation entityBedReservation = entityReservationDao.Get(Convert.ToInt32(hdnReservationID.Value));
                    entityBedReservation.MRN = entity.MRN;
                    entityBedReservation.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityReservationDao.Update(entityBedReservation);
                }

                if (hdnAppointmentID.Value != "")
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    Appointment entityAppointment = entityAppointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                    entityAppointment.MRN = entity.MRN;
                    entityAppointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityAppointmentDao.Update(entityAppointment);
                }

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

                if (result)
                {
                    UploadPhoto(entity.MedicalNo);
                    retval = entity.MedicalNo;
                    ctx.CommitTransaction();

                    BridgingToMedinfrasMobileApps(entity, entity.MRN, "001");
                }
                else
                {
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
            return result;
        }

        private bool IsMedicalNoValid(string medicalNo, IDbContext ctx)
        {
            string filterExpression = string.Format("MedicalNo = '{0}'", medicalNo);
            Patient oPatient = BusinessLayer.GetPatientList(filterExpression, ctx).FirstOrDefault();
            return oPatient == null;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientDao entityDao = new PatientDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            PatientTagFieldDao entityTagFieldDao = new PatientTagFieldDao(ctx);
            PatientFamilyDao entityPatientFamilyDao = new PatientFamilyDao(ctx);
            RegistrationDao entityRegDao = new RegistrationDao(ctx);
            RegistrationBPJSDao entityRegBPJSDao = new RegistrationBPJSDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            try
            {
                Patient entity = entityDao.Get(Convert.ToInt32(hdnMRN.Value));

                AuditLog entityAuditLog = new AuditLog();
                entityAuditLog.OldValues = JsonConvert.SerializeObject(entity);

                Address entityAddress = null;

                PatientTagField entityTagField = entityTagFieldDao.Get(entity.MRN);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                Address entityOfficeAddress = null;
                Address entityDomicileAddress = null;

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

                bool flagHomeAddress = true;
                bool flagOfficeAddress = true;
                bool flagOtherAddress = true;

                if (entity.HomeAddressID != null)
                {
                    entityAddress = entityAddressDao.Get((int)entity.HomeAddressID);
                }
                else
                {
                    entityAddress = new Address();
                    flagHomeAddress = false;
                }

                if (entity.OfficeAddressID != null)
                {
                    entityOfficeAddress = entityAddressDao.Get((int)entity.OfficeAddressID);
                }
                else
                {
                    entityOfficeAddress = new Address();
                    flagOfficeAddress = false;
                }

                if (entity.OtherAddressID != null)
                {
                    entityDomicileAddress = entityAddressDao.Get((int)entity.OtherAddressID);
                }
                else
                {
                    entityDomicileAddress = new Address();
                    flagOtherAddress = false;
                }

                ControlToEntity(entity, entityAddress, entityOfficeAddress, entityDomicileAddress, entityTagField, ref entityPatientMother, ref entityPatientFather, ref entityPatientSpouse, ref entityPatientChild);

                if (flagHomeAddress)
                {
                    entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityAddressDao.Update(entityAddress);
                }
                else
                {
                    entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entity.HomeAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);
                }

                if (flagOfficeAddress)
                {
                    entityOfficeAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityAddressDao.Update(entityOfficeAddress);
                }
                else
                {
                    entityOfficeAddress.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entity.OfficeAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityOfficeAddress);
                }

                if (flagOtherAddress)
                {
                    entityDomicileAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityAddressDao.Update(entityDomicileAddress);
                }
                else
                {
                    entityDomicileAddress.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entity.OtherAddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityDomicileAddress);
                }

                if (entity.IsLinked)
                {
                    entity.IsLinked = false;
                }
                entityTagFieldDao.Update(entityTagField);
                entityAuditLog.ObjectType = Constant.BusinessObjectType.PATIENT;
                entityAuditLog.NewValues = JsonConvert.SerializeObject(entity);
                entityAuditLog.UserID = AppSession.UserLogin.UserID;
                entityAuditLog.LogDate = DateTime.Now;
                entityAuditLog.TransactionID = entity.MRN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityAuditLogDao.Insert(entityAuditLog);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityDao.Update(entity);

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


                if (!String.IsNullOrEmpty(hdnRegistrationID.Value))
                {
                    Registration reg = entityRegDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                    reg.AgeInYear = Convert.ToInt16(Function.GetPatientAgeInYear(entity.DateOfBirth, DateTime.Now));
                    reg.AgeInMonth = Convert.ToInt16(Function.GetPatientAgeInMonth(entity.DateOfBirth, DateTime.Now));
                    reg.AgeInDay = Convert.ToInt16(Function.GetPatientAgeInDay(entity.DateOfBirth, DateTime.Now));
                    reg.CorporateAccountNo = entity.CorporateAccountNo;
                    reg.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityRegDao.Update(reg);

                    RegistrationBPJS entityRegBPJS = BusinessLayer.GetRegistrationBPJSList(string.Format("RegistrationID = {0}", reg.RegistrationID), ctx).FirstOrDefault();

                    if (entityRegBPJS != null)
                    {
                        entityRegBPJS.NoPeserta = txtNHSRegistrationNo.Text;
                        entityRegBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityRegBPJSDao.Update(entityRegBPJS);
                    }

                    if (chkIsHasPhysicalLimitation.Checked)
                    {
                        reg.IsFastTrack = true;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityRegDao.Update(reg);
                    }
                    else
                    {
                        reg.IsFastTrack = false;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityRegDao.Update(reg);
                    }
                }
                else
                {
                    Registration reg = entityRegDao.Get(Convert.ToInt32(entity.MRN));
                    if (reg != null)
                    {
                        if (chkIsHasPhysicalLimitation.Checked)
                        {
                            reg.IsFastTrack = true;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityRegDao.Update(reg);
                        }
                        else
                        {
                            reg.IsFastTrack = false;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityRegDao.Update(reg);
                        }
                    }

                }

                UploadPhoto(entity.MedicalNo);
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
                    entityAPILog.Sender = "MEDINFRAS";
                    entityAPILog.Recipient = "MOBILE APPS";
                    entityAPILog.IsSuccess = true;
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

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0].Contains("PatientExis"))
            {
                string isResult = "0";
                CheckPatientExis(ref isResult);
                if (isResult == "1")
                {
                    result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    result += string.Format("success|{0}", errMessage);
                }

            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewPatientNotes(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<vPatientNotes> lstEntity = new List<vPatientNotes>();
            if (hdnMRN.Value != "0" && !string.IsNullOrEmpty(hdnMRN.Value))
            {
                string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", hdnMRN.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientNotesRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                lstEntity = BusinessLayer.GetvPatientNotesList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID");
            }

            grdPatientNotesView.DataSource = lstEntity;
            grdPatientNotesView.DataBind();
        }

        protected void cbpPatientNotesView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewPatientNotes(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewPatientNotes(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        }

        protected void cbpPatientNotes_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        PatientNotes entity = new PatientNotes();

                        entity.MRN = Convert.ToInt32(hdnMRN.Value);
                        entity.Notes = txtPatientNotes.Text;
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        BusinessLayer.InsertPatientNotes(entity);

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnPatientNotesID.Value);
                        PatientNotes entity = BusinessLayer.GetPatientNotes(recordID);

                        if (entity != null)
                        {
                            entity.Notes = txtPatientNotes.Text;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientNotes(entity);

                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Instruction Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnPatientNotesID.Value);
                        PatientNotes entity = BusinessLayer.GetPatientNotes(recordID);

                        if (entity != null)
                        {
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientNotes(entity);

                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Instruction Record Information");
                        }
                        result = "1|delete|";
                    }

                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void CheckPatientExis(ref string Result)
        {
            string Fullname = string.Format("{0}", txtFamilyName.Text.ToLower());

            string firstName = txtFirstName.Text.ToLower();
            if (firstName != "")
            {
                Fullname = string.Format("{0} {1}", txtFirstName.Text.ToLower(), txtFamilyName.Text.ToLower());
            }

            string MobilePhone1 = txtMobilePhone1.Text.Trim();
            string StreetName = txtAddress.Text;
            if (StreetName == "")
            {
                StreetName = "StreetName IS NULL";
            }
            else
            {
                StreetName = string.Format("StreetName LIKE '%{0}%'", txtAddress.Text);
            }

            string County = txtCounty.Text;
            if (County == "")
            {
                County = "County IS NULL";
            }
            else
            {
                County = string.Format("County LIKE '%{0}%'", txtCounty.Text);
            }

            string District = txtDistrict.Text;
            if (District == "")
            {
                District = "District IS NULL";
            }
            else
            {
                District = string.Format("District LIKE '%{0}%'", txtDistrict.Text);
            }

            string filterExpression = string.Format("PatientName = '{0}' AND DateOfBirth='{1}' AND GCGender = '{2}' AND {3} AND {4} AND {5} AND IsDeleted = 0", Fullname, Helper.GetDatePickerValue(txtDOB).ToString(Constant.FormatString.DATE_PICKER_FORMAT2), cboGender.Value, StreetName, County, District);
            vPatient oPatient = BusinessLayer.GetvPatientList(filterExpression).FirstOrDefault();
            if (oPatient != null)
            {
                Result = "1";
            }
            else
            {
                Result = "0";
            }

        }

    }
}