using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class GuestEntryCtl : BaseEntryPopupCtl
    {
        protected string GetTodayDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
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
        protected string OnGetSCEthnicFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ETHNIC);
        }
        protected string OnGetSCPatientJobFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.OCCUPATION);
        }
        protected string OnGetSCLanguageFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.LANGUAGE);
        }
        protected string OnGetSCEducationFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.EDUCATION);
        }
        public override void InitializeDataControl(string param)
        {
            SetControlProperties();
            string guestID = param;
            if (param.Contains('|'))
            {
                string[] parameter = param.Split('|');
                guestID = parameter[0];
                hdnRegistrationNo.Value = parameter[1];
            }
            if (guestID != "" && guestID != "0")
            {
                IsAdd = false;

                hdnGuestID.Value = guestID;
                Guest entity = BusinessLayer.GetGuest(Convert.ToInt32(guestID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;

                RegistrationEntry page = Page as RegistrationEntry;
                #region Patient Data
                cboSalutation.Value = page.GuestGCSalutation;
                cboTitle.Value = page.GuestGCTitle;
                txtFirstName.Text = page.GuestFirstName;
                txtMiddleName.Text = page.GuestMiddleName;
                txtFamilyName.Text = page.GuestLastName;
                cboSuffix.Value = page.GuestGCSuffix;
                cboGender.Value = page.GuestGCGender;
                string DOB = page.GuestDateOfBirth;
                if (DOB != "")
                    txtDOB.Text = DOB;
                #endregion

                #region Patient Address
                txtAddress.Text = page.GuestStreetName;
                txtCounty.Text = page.GuestCounty; // Desa
                txtDistrict.Text = page.GuestDistrict; //Kabupaten
                txtCity.Text = page.GuestCity;
                #endregion

                #region Patient Address Domicile
                txtAddressDomicile.Text = page.GuestStreetName;
                txtCountyDomicile.Text = page.GuestCounty; // Desa
                txtDistrictDomicile.Text = page.GuestDistrict; //Kabupaten
                txtCityDomicile.Text = page.GuestCity;
                #endregion

                #region Patient Contact
                string phoneNo = page.GuestPhoneNo;
                if (phoneNo != "")
                    txtTelephoneNo.Text = phoneNo;
                txtMobilePhone.Text = page.GuestMobilePhoneNo;
                txtEmail.Text = page.GuestEmailAddress;
                cboIdentityCardType.Value = page.GuestGCIdentityNoType;
                txtIdentityCardNo.Text = page.GuestSSN;
                #endregion
            }
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.TITLE, Constant.StandardCode.SALUTATION, Constant.StandardCode.SUFFIX, Constant.StandardCode.IDENTITY_NUMBERY_TYPE, Constant.StandardCode.GENDER, Constant.StandardCode.BLOOD_TYPE));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSalutation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTitle, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffix, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboIdentityCardType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.IDENTITY_NUMBERY_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGender, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.GENDER).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBloodType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.BLOOD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            List<Variable> lstBloodRhesus = new List<Variable>();
            lstBloodRhesus.Add(new Variable { Code = "+", Value = "+" });
            lstBloodRhesus.Add(new Variable { Code = "-", Value = "-" });
            Methods.SetComboBoxField<Variable>(cboBloodRhesus, lstBloodRhesus, "Value", "Code");

            cboSalutation.SelectedIndex = 0;
            cboTitle.SelectedIndex = 0;
            cboSuffix.SelectedIndex = 0;
            cboIdentityCardType.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string defaultGCState = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];
            string defaultPhoneArea = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PHONE_AREA).ParameterValue;
            StandardCode scNationality = BusinessLayer.GetStandardCode(Constant.Nationality.INDONESIA);

            #region Patient Data
            SetControlEntrySetting(cboSalutation, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTitle, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFamilyName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboSuffix, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGender, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDOB, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtAgeInDay, new ControlEntrySetting(true, false, true, 0));
            SetControlEntrySetting(txtAgeInMonth, new ControlEntrySetting(true, false, true, 0));
            SetControlEntrySetting(txtAgeInYear, new ControlEntrySetting(true, false, true, 0));
            SetControlEntrySetting(txtReligionCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReligionName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNationalityCode, new ControlEntrySetting(true, true, false, scNationality.StandardCodeID.Split('^')[1]));
            SetControlEntrySetting(txtNationalityName, new ControlEntrySetting(true, true, false, scNationality.StandardCodeName));
            SetControlEntrySetting(cboBloodType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboBloodRhesus, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMaritalStatusCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMaritalStatusName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEthnicCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEthnicName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEducationCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPatientJobCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCorporateAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCorporateAccountName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCorporateAccountDepartment, new ControlEntrySetting(true, true, false));
            #endregion

            #region Patient Address
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, false));
            #endregion

            #region Patient Address Domicile
            SetControlEntrySetting(txtAddressDomicile, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCountyDomicile, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrictDomicile, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCityDomicile, new ControlEntrySetting(true, true, false));
            #endregion

            #region Patient Contact
            SetControlEntrySetting(txtTelephoneNo, new ControlEntrySetting(true, true, true, defaultPhoneArea));
            SetControlEntrySetting(txtMobilePhone, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmail, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtIdentityCardNo, new ControlEntrySetting(true, true, false));
            #endregion
        }

        private void EntityToControl(Guest entity)
        {
            #region Patient Data
            hdnGuestID.Value = entity.GuestID.ToString();
            txtGuestNo.Text = entity.GuestNo;
            cboSalutation.Value = entity.GCSalutation;
            cboTitle.Value = entity.GCTitle;
            txtFirstName.Text = entity.FirstName;
            txtMiddleName.Text = entity.MiddleName;
            txtFamilyName.Text = entity.LastName;
            cboSuffix.Value = entity.GCSuffix;
            cboGender.Value = entity.GCGender;
            if (entity.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
            {
                txtDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtAgeInYear.Text = entity.AgeInYear.ToString();
                txtAgeInMonth.Text = entity.AgeInMonth.ToString();
                txtAgeInDay.Text = entity.AgeInDay.ToString();
            }
            else
            {
                txtDOB.Text = "";
                txtAgeInYear.Text = "";
                txtAgeInMonth.Text = "";
                txtAgeInDay.Text = "";
            }
            txtBirthPlace.Text = entity.CityOfBirth;
            #endregion

            #region Patient Address
            txtAddress.Text = entity.StreetName;
            txtCounty.Text = entity.County; // Desa
            txtDistrict.Text = entity.District; //Kabupaten
            txtCity.Text = entity.City;
            #endregion

            #region Patient Address Domicile
            txtAddressDomicile.Text = entity.StreetName;
            txtCountyDomicile.Text = entity.County; // Desa
            txtDistrictDomicile.Text = entity.District; //Kabupaten
            txtCityDomicile.Text = entity.City;
            #endregion

            #region Patient Contact
            txtTelephoneNo.Text = entity.PhoneNo;
            txtMobilePhone.Text = entity.MobilePhoneNo;
            txtEmail.Text = entity.EmailAddress;
            cboIdentityCardType.Value = entity.GCIdentityNoType;
            txtIdentityCardNo.Text = entity.SSN;
            #endregion

            #region Additional Information
            vGuest entityGuest = BusinessLayer.GetvGuestList(string.Format("GuestID = {0}", entity.GuestID)).FirstOrDefault();
            if (entity.GCReligion != "")
                txtReligionCode.Text = entity.GCReligion.Split('^')[1];
            else
                txtReligionCode.Text = "";
            txtReligionName.Text = entityGuest.Religion;
            if (entity.GCNationality != "")
                txtNationalityCode.Text = entity.GCNationality.Split('^')[1];
            else
                txtNationalityCode.Text = "";
            txtNationalityName.Text = entityGuest.Nationality;
            cboBloodType.Value = entity.GCBloodType;
            cboBloodRhesus.Value = entity.BloodRhesus;
            if (entity.GCEthnic != "")
                txtEthnicCode.Text = entity.GCEthnic.Split('^')[1];
            else
                txtEthnicCode.Text = "";
            txtEthnicName.Text = entityGuest.Ethnic;
            if (entity.GCMaritalStatus != "")
                txtMaritalStatusCode.Text = entity.GCMaritalStatus.Split('^')[1];
            else
                txtMaritalStatusCode.Text = "";
            txtMaritalStatusName.Text = entityGuest.MaritalStatus;
            if (entity.GCLanguage != "")
                txtlanguageCode.Text = entity.GCLanguage.Split('^')[1];
            else
                txtlanguageCode.Text = "";
            txtlanguageName.Text = entityGuest.Language;
            if (entity.GCEducation != "")
                txtEducationCode.Text = entity.GCEducation.Split('^')[1];
            else
                txtEducationCode.Text = "";
            txtEducationName.Text = entityGuest.Education;
            if (entity.GCOccupation != "")
                txtPatientJobCode.Text = entity.GCOccupation.Split('^')[1];
            else
                txtPatientJobCode.Text = "";
            txtPatientJobName.Text = entityGuest.Occupation;
            txtCorporateAccountNo.Text = entityGuest.CorporateAccountNo;
            txtCorporateAccountName.Text = entityGuest.CorporateAccountName;
            txtCorporateAccountDepartment.Text = entityGuest.CorporateAccountDepartment;
            #endregion
        }

        private void ControlToEntity(Guest entity)
        {
            #region Patient Data
            entity.GCSalutation = Helper.GetComboBoxValue(cboSalutation, true);
            entity.GCTitle = Helper.GetComboBoxValue(cboTitle, true);

            entity.FirstName = txtFirstName.Text;
            entity.MiddleName = txtMiddleName.Text;
            entity.LastName = txtFamilyName.Text;
            entity.GCSuffix = Helper.GetComboBoxValue(cboSuffix, true);
            entity.GCGender = cboGender.Value.ToString();
            if (txtDOB.Text == "")
                entity.DateOfBirth = Helper.InitializeDateTimeNull();
            else
                entity.DateOfBirth = Helper.GetDatePickerValue(txtDOB);

            string suffix = cboSuffix.Value == null ? "" : cboSuffix.Text;
            string title = cboTitle.Value == null ? "" : cboTitle.Text;
            entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
            entity.FullName = Helper.GenerateFullName(entity.Name, title, suffix);
            entity.CityOfBirth = txtBirthPlace.Text;
            #endregion

            #region Patient Address
            entity.StreetName = txtAddress.Text;
            entity.County = txtCounty.Text; // Desa
            entity.District = txtDistrict.Text; //Kabupaten
            entity.City = txtCity.Text;
            #endregion

            #region Patient Address Domicile
            entity.StreetNameDomicile = txtAddressDomicile.Text;
            entity.CountyDomicile = txtCountyDomicile.Text; // Desa
            entity.DistrictDomicile = txtDistrictDomicile.Text; //Kabupaten
            entity.CityDomicile = txtCityDomicile.Text;
            #endregion

            #region Patient Contact
            entity.PhoneNo = txtTelephoneNo.Text;
            entity.MobilePhoneNo = txtMobilePhone.Text;
            entity.EmailAddress = txtEmail.Text;
            entity.GCIdentityNoType = Helper.GetComboBoxValue(cboIdentityCardType, true);
            entity.SSN = txtIdentityCardNo.Text;
            #endregion

            #region Additional Information
            entity.GCReligion = txtReligionCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.RELIGION, txtReligionCode.Text);
            entity.GCNationality = txtNationalityCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.NATIONALITY, txtNationalityCode.Text);
            entity.GCBloodType = Helper.GetComboBoxValue(cboBloodType, true);
            entity.BloodRhesus = Helper.GetComboBoxValue(cboBloodRhesus, true);
            entity.GCLanguage = txtlanguageCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.LANGUAGE, txtlanguageCode.Text);
            entity.GCMaritalStatus = txtMaritalStatusCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.MARITAL_STATUS, txtMaritalStatusCode.Text);
            entity.GCEducation = txtEducationCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.EDUCATION, txtEducationCode.Text);
            entity.GCOccupation = txtPatientJobCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.OCCUPATION, txtPatientJobCode.Text);
            entity.GCEthnic = txtEthnicCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.ETHNIC, txtEthnicCode.Text);
            entity.CorporateAccountNo = !string.IsNullOrEmpty(txtCorporateAccountNo.Text) ? txtCorporateAccountNo.Text : string.Empty;
            entity.CorporateAccountName = !string.IsNullOrEmpty(txtCorporateAccountName.Text) ? txtCorporateAccountName.Text : string.Empty;
            entity.CorporateAccountDepartment = !string.IsNullOrEmpty(txtCorporateAccountDepartment.Text) ? txtCorporateAccountDepartment.Text : string.Empty;
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GuestDao entityDao = new GuestDao(ctx);
            try
            {
                Guest entity = new Guest();
                entity.GuestNo = BusinessLayer.GenerateGuestNo(DateTime.Now);
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

                if (!string.IsNullOrEmpty(hdnRegistrationNo.Value))
                {
                    retval = hdnRegistrationNo.Value;
                }
                else
                {
                    retval = entity.GuestNo;
                }

                result = true;

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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GuestDao entityDao = new GuestDao(ctx);
            try
            {
                Guest entity = entityDao.Get(Convert.ToInt32(hdnGuestID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                if (!string.IsNullOrEmpty(hdnRegistrationNo.Value))
                {
                    retval = hdnRegistrationNo.Value;
                }
                else
                {
                    retval = entity.GuestNo;
                }

                result = true;

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