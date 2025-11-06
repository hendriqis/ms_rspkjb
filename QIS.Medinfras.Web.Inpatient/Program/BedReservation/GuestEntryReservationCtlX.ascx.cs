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

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class GuestEntryReservationCtlX : BaseEntryPopupCtl
    {
        protected string GetTodayDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            SetControlProperties();
            if (param != "" && param != "0")
            {
                IsAdd = false;

                hdnGuestID.Value = param;
                Guest entity = BusinessLayer.GetGuest(Convert.ToInt32(param));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;

                //BedReservationEntry page = Page as BedReservationEntry;
                #region Patient Data
                //cboSalutation.Value = page.GuestGCSalutation;
                //cboTitle.Value = page.GuestGCTitle;
                //txtFirstName.Text = page.GuestFirstName;
                //txtMiddleName.Text = page.GuestMiddleName;
                //txtFamilyName.Text = page.GuestLastName;
                //cboSuffix.Value = page.GuestGCSuffix;
                //cboGender.Value = page.GuestGCGender;
                //string DOB = page.GuestDateOfBirth;
                //if (DOB != "")
                //    txtDOB.Text = DOB;
                #endregion

                #region Patient Address
                //txtAddress.Text = page.GuestStreetName;
                //txtCounty.Text = page.GuestCounty; // Desa
                //txtDistrict.Text = page.GuestDistrict; //Kabupaten
                //txtCity.Text = page.GuestCity;
                #endregion

                #region Patient Contact
                //string phoneNo = page.GuestPhoneNo;
                //if (phoneNo != "")
                //    txtTelephoneNo.Text = phoneNo;
                //txtMobilePhone.Text = page.GuestMobilePhoneNo;
                //txtEmail.Text = page.GuestEmailAddress;
                //cboIdentityCardType.Value = page.GuestGCIdentityNoType;
                //txtIdentityCardNo.Text = page.GuestSSN;
                #endregion
            }
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.TITLE, Constant.StandardCode.SALUTATION, Constant.StandardCode.SUFFIX,Constant.StandardCode.IDENTITY_NUMBERY_TYPE, Constant.StandardCode.GENDER));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSalutation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTitle, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffix, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboIdentityCardType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.IDENTITY_NUMBERY_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGender, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.GENDER).ToList(), "StandardCodeName", "StandardCodeID");
            
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

            #region Patient Data
            SetControlEntrySetting(cboSalutation, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTitle, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFamilyName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboSuffix, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGender, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDOB, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtAgeInDay, new ControlEntrySetting(true, false, true, 0));
            SetControlEntrySetting(txtAgeInMonth, new ControlEntrySetting(true, false, true, 0));
            SetControlEntrySetting(txtAgeInYear, new ControlEntrySetting(true, false, true, 0));
            #endregion

            #region Patient Address
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, false));
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
            #endregion

            #region Patient Address
            txtAddress.Text = entity.StreetName;
            txtCounty.Text = entity.County; // Desa
            txtDistrict.Text = entity.District; //Kabupaten
            txtCity.Text = entity.City;
            #endregion

            #region Patient Contact
            txtTelephoneNo.Text = entity.PhoneNo;
            txtMobilePhone.Text = entity.MobilePhoneNo;
            txtEmail.Text = entity.EmailAddress;
            cboIdentityCardType.Value = entity.GCIdentityNoType;
            txtIdentityCardNo.Text = entity.SSN;
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
            #endregion

            #region Patient Address
            entity.StreetName = txtAddress.Text;
            entity.County = txtCounty.Text; // Desa
            entity.District = txtDistrict.Text; //Kabupaten
            entity.City = txtCity.Text;
            #endregion

            #region Patient Contact
            entity.PhoneNo = txtTelephoneNo.Text;
            entity.MobilePhoneNo = txtMobilePhone.Text;
            entity.EmailAddress = txtEmail.Text;
            entity.GCIdentityNoType = Helper.GetComboBoxValue(cboIdentityCardType, true);
            entity.SSN = txtIdentityCardNo.Text;
            #endregion
        }


        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                Guest entity = BusinessLayer.GetGuest(Convert.ToInt32(hdnGuestID.Value));
                ControlToEntity(entity);
                BusinessLayer.InsertGuest(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                Guest entity = BusinessLayer.GetGuest(Convert.ToInt32(hdnGuestID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGuest(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}