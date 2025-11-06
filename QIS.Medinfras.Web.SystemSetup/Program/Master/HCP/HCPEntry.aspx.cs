using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class HCPEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.HEALTHCARE_PROFESSIONAL;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                SetControlProperties();
                vParamedicMaster entity = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", ID))[0];
                List<vAddress> lstAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entity.HomeAddressID));
                vAddress entityAddress = new vAddress();
                if (lstAddress.Count > 0)
                {
                    entityAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entity.HomeAddressID))[0];
                }
                EntityToControl(entity, entityAddress);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtParamedicCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            GetSettingParameter();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }
        protected string OnGetSCReligionFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.RELIGION);
        }
        protected string OnGetSCNationalityFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.NATIONALITY);
        }
        #endregion

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format(
                                    "ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0",
                                    Constant.StandardCode.TITLE,
                                    Constant.StandardCode.SUFFIX,
                                    Constant.StandardCode.EMPLOYMENT_STATUS,
                                    Constant.StandardCode.HEALTHCARE_PROFESSIONAL_TYPE,
                                    Constant.StandardCode.GENDER,
                                    Constant.StandardCode.REVENUE_PAYMENT_METHOD)
                                );
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboTitle, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffix, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboParamedicMasterType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.HEALTHCARE_PROFESSIONAL_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboEmploymentStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.EMPLOYMENT_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGender, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.GENDER || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.REVENUE_PAYMENT_METHOD || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<Specialty> lstSpecialty = BusinessLayer.GetSpecialtyList("IsDeleted = 0 ORDER BY SpecialtyName");
            lstSpecialty.Insert(0, new Specialty { SpecialtyID = "", SpecialtyName = "" });
            Methods.SetComboBoxField<Specialty>(cboSpecialty, lstSpecialty, "SpecialtyName", "SpecialtyID");

            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            lstHealthcare.Insert(0, new Healthcare { HealthcareID = "", HealthcareName = "" });
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstHealthcare, "HealthcareName", "HealthcareID");

            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1"));
            lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
            Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
        }

        protected override void OnControlEntrySetting()
        {
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string defaultGCState = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];
            string defaultPhoneArea = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PHONE_AREA).ParameterValue;

            SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true));

            #region Personal Data
            SetControlEntrySetting(cboTitle, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFamilyName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboSuffix, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtInitial, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGender, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDOB, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCityOfBirth, new ControlEntrySetting(true, true, false));
            #endregion

            #region Professional Data
            SetControlEntrySetting(cboParamedicMasterType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboSpecialty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboEmploymentStatus, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtHiredDate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTerminatedDate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLicenseNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLicenseExpiredDate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTaxRegistrationNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsAvailable, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtNotAvailableUntil, new ControlEntrySetting(true, true, false));
            #endregion

            #region Professional Address
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceCode, new ControlEntrySetting(true, true, false, defaultGCState));
            SetControlEntrySetting(txtProvinceName, new ControlEntrySetting(false, false, false, healthcare.State));
            SetControlEntrySetting(hdnZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, false));
            #endregion

            #region Professional Contact
            SetControlEntrySetting(txtTelephoneNo, new ControlEntrySetting(true, true, false, defaultPhoneArea));
            SetControlEntrySetting(txtMobilePhone, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMobilePhone2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmail, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmail2, new ControlEntrySetting(true, true, false));
            #endregion

            #region Additonal Information
            SetControlEntrySetting(txtVKlaimDokterDPJPCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtVKlaimDokterDPJPName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBPJSReferenceInfo2Code, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBPJSReferenceInfo2Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtKodeSpesialisasi, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNamaSpesialisasi, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtKodeSubSpesialisasi, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNamaSubSpesialisasi, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReligionCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReligionName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNationalityCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNationalityName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtEKlaimParamedicName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEKlaimParamedicSIP, new ControlEntrySetting(true, true, false));
            #endregion

            #region Professional Status
            SetControlEntrySetting(chkIsAnesthesist, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAppointmentByTimeSlot, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowPrescribeNarcotics, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasRevenueSharing, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasPhysicianRole, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRevenueSharingName, new ControlEntrySetting(false, false, false));
            #endregion

            #region Bank Information
            SetControlEntrySetting(txtBankName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBankBranch, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBankAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtVirtualAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBankAccountName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBankClearingPassword, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboPaymentMethod, new ControlEntrySetting(true, true, false));
            #endregion

            #region Other Information
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMaximumWaitingList, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtMaxAppointment, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(chkIsAllowWaitingList, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(txtVisitDuration, new ControlEntrySetting(true, true, false));
            #endregion
        }

        private void EntityToControl(vParamedicMaster entity, vAddress entityAddress)
        {
            txtParamedicCode.Text = entity.ParamedicCode;

            #region Personal Data
            cboTitle.Value = entity.GCTitle;
            txtFirstName.Text = entity.FirstName;
            txtMiddleName.Text = entity.MiddleName;
            txtFamilyName.Text = entity.LastName;
            cboSuffix.Value = entity.GCSuffix;
            txtInitial.Text = entity.Initial;
            cboGender.Value = entity.GCGender;
            if (entity.DateOfBirth.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                txtDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtCityOfBirth.Text = entity.CityOfBirth;
            txtNIK.Text = entity.NIK;
            txtParamedicNameNIK.Text = entity.NamaNIK;
            txtIHSNumber.Text = entity.IHSNumber;
            #endregion

            #region Professional Data
            cboParamedicMasterType.Value = entity.GCParamedicMasterType;
            cboSpecialty.Value = entity.SpecialtyID;
            cboHealthcare.Value = entity.HealthcareID;
            cboDepartment.Value = entity.DepartmentID;
            cboEmploymentStatus.Value = entity.GCEmploymentStatus;
            if (entity.HiredDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                txtHiredDate.Text = entity.HiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (entity.TerminatedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                txtTerminatedDate.Text = entity.TerminatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLicenseNo.Text = entity.LicenseNo;
            if (entity.LicenseExpiredDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                txtLicenseExpiredDate.Text = entity.LicenseExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTaxRegistrationNo.Text = entity.VATRegistrationNo;
            chkIsAvailable.Checked = entity.IsAvailable;
            if (entity.NotAvailableUntil.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                txtNotAvailableUntil.Text = entity.NotAvailableUntil.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (entity.RevenueSharingID != null)
            {
                hdnRevenueSharingID.Value = entity.RevenueSharingID.ToString();
                txtRevenueSharingCode.Text = entity.RevenueSharingCode;
                txtRevenueSharingName.Text = entity.RevenueSharingName;
            }
            else
            {
                hdnRevenueSharingID.Value = string.Empty;
                txtRevenueSharingCode.Text = string.Empty;
                txtRevenueSharingName.Text = string.Empty;
            }
            #endregion

            #region Professional Address
            txtAddress.Text = entityAddress.StreetName;
            txtCounty.Text = entityAddress.County; // Desa
            txtDistrict.Text = entityAddress.District; //Kabupaten
            txtCity.Text = entityAddress.City;
            if (!string.IsNullOrEmpty(entityAddress.GCState))
                txtProvinceCode.Text = entityAddress.GCState.Split('^')[1];
            else
                txtProvinceCode.Text = "";
            txtProvinceName.Text = entityAddress.State;
            hdnZipCode.Value = entityAddress.ZipCodeID.ToString();
            txtZipCode.Text = entityAddress.ZipCode;
            #endregion

            #region Professional Contact
            txtTelephoneNo.Text = entityAddress.PhoneNo1;
            txtMobilePhone.Text = entity.MobileNo1;
            txtMobilePhone2.Text = entity.MobileNo2;
            txtEmail.Text = entity.EmailAddress1;
            txtEmail2.Text = entity.EmailAddress2;
            #endregion

            #region Additional Information
            if (!string.IsNullOrEmpty(entity.BPJSReferenceInfo) && entity.BPJSReferenceInfo.Contains(';'))
            {
                string[] bpjsInfo = entity.BPJSReferenceInfo.Split(';');
                if (!string.IsNullOrEmpty(bpjsInfo[0]))
                {
                    string[] vclaimInfo = bpjsInfo[0].Split('|');
                    txtVKlaimDokterDPJPCode.Text = vclaimInfo[0];
                    txtVKlaimDokterDPJPName.Text = vclaimInfo[1];
                    hdnVKlaimDokterDPJPName.Value = vclaimInfo[1];
                }
                if (!string.IsNullOrEmpty(bpjsInfo[1]))
                {
                    string[] hfisInfo = bpjsInfo[1].Split('|');
                    txtBPJSReferenceInfo2Code.Text = hfisInfo[0];
                    txtBPJSReferenceInfo2Name.Text = hfisInfo[1];
                    hdnBPJSReferenceInfo2Name.Value = hfisInfo[1];
                }
                if (bpjsInfo.Length >= 3)
                {
                    if (!string.IsNullOrEmpty(bpjsInfo[2]))
                    {
                        string[] spesialisasiInfo = bpjsInfo[2].Split('|');
                        txtKodeSpesialisasi.Text = spesialisasiInfo[0];
                        txtNamaSpesialisasi.Text = spesialisasiInfo[1];
                        hdnBPJSSpesialisasi.Value = spesialisasiInfo[1];
                    }
                }
                if (bpjsInfo.Length > 3)
                {
                    if (!string.IsNullOrEmpty(bpjsInfo[3]))
                    {
                        string[] subSpesialisasiInfo = bpjsInfo[3].Split('|');
                        txtKodeSubSpesialisasi.Text = subSpesialisasiInfo[0];
                        txtNamaSubSpesialisasi.Text = subSpesialisasiInfo[1];
                        hdnBPJSSubSpesialisasi.Value = subSpesialisasiInfo[1];
                    }
                }

            }
            else
            {
                txtVKlaimDokterDPJPCode.Text = string.Empty;
                txtVKlaimDokterDPJPName.Text = string.Empty;
                hdnVKlaimDokterDPJPName.Value = string.Empty;
                txtBPJSReferenceInfo2Code.Text = string.Empty;
                txtBPJSReferenceInfo2Name.Text = string.Empty;
                hdnBPJSReferenceInfo2Name.Value = string.Empty;
                txtKodeSpesialisasi.Text = string.Empty;
                txtNamaSpesialisasi.Text = string.Empty;
                hdnBPJSSpesialisasi.Value = string.Empty;
                txtKodeSubSpesialisasi.Text = string.Empty;
                txtNamaSubSpesialisasi.Text = string.Empty;
                hdnBPJSSubSpesialisasi.Value = string.Empty;
            }
            if (entity.GCReligion != "")
                txtReligionCode.Text = entity.GCReligion.Split('^')[1];
            else
                txtReligionCode.Text = "";
            txtReligionName.Text = entity.Religion;
            if (entity.GCNationality != "")
                txtNationalityCode.Text = entity.GCNationality.Split('^')[1];
            else
                txtNationalityCode.Text = "";
            txtNationalityName.Text = entity.Nationality;
            txtEKlaimParamedicName.Text = entity.EKlaimParamedicName;
            txtEKlaimParamedicSIP.Text = entity.EKlaimParamedicSIP;
            txtInhealthReferenceInfo.Text = entity.InhealthReferenceInfo;
            #endregion

            #region Professional Status
            chkIsSpecialist.Checked = entity.IsSpecialist;
            chkIsSubSpecialist.Checked = entity.IsSubSpecialist;
            chkIsAnesthesist.Checked = entity.IsAnesthetist;
            chkIsAppointmentByTimeSlot.Checked = entity.IsAppointmentByTimeSlot;
            chkIsAllowPrescribeNarcotics.Checked = entity.IsAllowPrescribeNarcotics;
            chkIsHasPhysicianRole.Checked = entity.IsHasPhysicianRole;
            chkIsHasRevenueSharing.Checked = entity.IsHasRevenueSharing;
            chkIsRMO.Checked = entity.IsRMO;
            chkIsRequestMRFile.Checked = entity.IsRequestMRFile;
            chkIsPrimaryNurse.Checked = entity.IsPrimaryNurse;
            #endregion

            #region Bank Information
            txtBankName.Text = entity.BankName;
            txtBankBranch.Text = entity.BankBranch;
            txtBankAccountNo.Text = entity.BankAccountNo;
            txtVirtualAccountNo.Text = entity.VirtualAccountNo;
            txtBankAccountName.Text = entity.BankAccountName;
            txtBankClearingPassword.Text = entity.BankClearingPassword;
            cboPaymentMethod.Value = entity.GCRevenueSharingPayment;
            #endregion

            #region Other Information
            txtNotes.Text = entity.Notes;
            txtMaxAppointment.Text = entity.MaximumAppointment.ToString();
            txtMaximumWaitingList.Text = entity.MaximumWaitingList.ToString();
            chkIsAllowWaitingList.Checked = entity.IsAllowWaitingList;
            chkIsDummy.Checked = entity.IsDummy;
            txtVisitDuration.Text = entity.VisitDurationDefault.ToString();

            txtPicName.Text = entity.PictureFileName; 
            #endregion
        }

        private void ControlToEntity(ParamedicMaster entity, Address entityAddress)
        {
            entity.ParamedicCode = txtParamedicCode.Text;

            #region Personal Data
            if (cboTitle.Value != null && cboTitle.Value.ToString() != "")
                entity.GCTitle = cboTitle.Value.ToString();
            else
                entity.GCTitle = null;

            entity.FirstName = txtFirstName.Text;
            entity.MiddleName = txtMiddleName.Text;
            entity.LastName = txtFamilyName.Text;
            if (cboSuffix.Value != null && cboSuffix.Value.ToString() != "")
                entity.GCSuffix = cboSuffix.Value.ToString();
            else
                entity.GCSuffix = null;

            entity.Initial = txtInitial.Text;
            if (cboGender.Value != null && cboGender.Value.ToString() != "")
                entity.GCGender = cboGender.Value.ToString();
            else
                entity.GCGender = null;
            if (txtDOB.Text != "")
                entity.DateOfBirth = Helper.GetDatePickerValue(txtDOB);
            else
                entity.DateOfBirth = new DateTime(1900, 1, 1);
            entity.CityOfBirth = txtCityOfBirth.Text;
            string suffix = cboSuffix.Value == null ? "" : cboSuffix.Text;
            string title = cboTitle.Value == null ? "" : cboTitle.Text;
            entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
            entity.FullName = Helper.GenerateFullName(entity.Name, title, suffix);

            entity.NIK = txtNIK.Text;
            entity.NamaNIK = txtParamedicNameNIK.Text;
            #endregion

            #region Professional Data
            entity.GCParamedicMasterType = cboParamedicMasterType.Value.ToString();

            if (cboSpecialty.Value != null && cboSpecialty.Value.ToString() != "")
            {
                entity.SpecialtyID = cboSpecialty.Value.ToString();
            }
            else
            {
                entity.SpecialtyID = null;
            }

            if (cboHealthcare.Value != null && cboHealthcare.Value.ToString() != "")
            {
                entity.HealthcareID = cboHealthcare.Value.ToString();
            }
            else
            {
                entity.HealthcareID = null;
            }

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
            {
                entity.DepartmentID = cboDepartment.Value.ToString();
            }
            else
            {
                entity.DepartmentID = null;
            }

            entity.GCEmploymentStatus = cboEmploymentStatus.Value.ToString();

            if (entity.GCEmploymentStatus == "")
            {
                entity.GCEmploymentStatus = null;
            }

            if (txtHiredDate.Text != "")
            {
                entity.HiredDate = Helper.GetDatePickerValue(txtHiredDate);
            }

            if (txtLicenseExpiredDate.Text != "")
            {
                entity.LicenseExpiredDate = Helper.GetDatePickerValue(txtLicenseExpiredDate);
            }

            if (txtTerminatedDate.Text != "")
            {
                entity.TerminatedDate = Helper.GetDatePickerValue(txtTerminatedDate);
            }
            entity.LicenseNo = txtLicenseNo.Text.Trim();
            entity.VATRegistrationNo = txtTaxRegistrationNo.Text.Trim();
            entity.IsAvailable = chkIsAvailable.Checked;

            if (txtNotAvailableUntil.Text != "")
            {
                entity.NotAvailableUntil = Helper.GetDatePickerValue(txtNotAvailableUntil);
            }

            if (hdnRevenueSharingID.Value != "" && hdnRevenueSharingID.Value != "0")
            {
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            }
            else
            {
                entity.RevenueSharingID = null;
            }
            #endregion

            #region Patient Address
            entityAddress.StreetName = txtAddress.Text;
            entityAddress.County = txtCounty.Text; // Desa
            entityAddress.District = txtDistrict.Text; //Kabupaten
            entityAddress.City = txtCity.Text;
            entityAddress.GCState = txtProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCode.Text);
            if (hdnZipCode.Value == "")
                entityAddress.ZipCode = null;
            else
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCode.Value);
            #endregion

            #region Patient Contact
            entityAddress.PhoneNo1 = txtTelephoneNo.Text;
            entity.MobileNo1 = txtMobilePhone.Text;
            entity.MobileNo2 = txtMobilePhone2.Text;
            entity.EmailAddress1 = txtEmail.Text;
            entity.EmailAddress2 = txtEmail2.Text;
            #endregion

            #region Additional Information
            entity.GCReligion = txtReligionCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.RELIGION, txtReligionCode.Text);
            entity.GCNationality = txtNationalityCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.NATIONALITY, txtNationalityCode.Text);
            entity.EKlaimParamedicName = txtEKlaimParamedicName.Text;
            entity.EKlaimParamedicSIP = txtEKlaimParamedicSIP.Text;
            entity.InhealthReferenceInfo = txtInhealthReferenceInfo.Text;
            entity.IHSNumber = txtIHSNumber.Text;
            #endregion

            #region Professional Status
            entity.IsSpecialist = chkIsSpecialist.Checked;
            entity.IsSubSpecialist = chkIsSubSpecialist.Checked;
            entity.IsAnesthetist = chkIsAnesthesist.Checked;
            entity.IsAppointmentByTimeSlot = chkIsAppointmentByTimeSlot.Checked;
            entity.IsAllowPrescribeNarcotics = chkIsAllowPrescribeNarcotics.Checked;
            entity.IsHasRevenueSharing = chkIsHasRevenueSharing.Checked;
            entity.IsRMO = chkIsRMO.Checked;
            entity.IsRequestMRFile = chkIsRequestMRFile.Checked;
            entity.IsHasPhysicianRole = chkIsHasPhysicianRole.Checked;
            if (entity.GCParamedicMasterType == Constant.ParamedicType.Nurse || entity.GCParamedicMasterType == Constant.ParamedicType.Bidan || entity.GCParamedicMasterType == Constant.ParamedicType.Nutritionist
                    || entity.GCParamedicMasterType == Constant.ParamedicType.Physiotherapist)
            {
                entity.IsPrimaryNurse = chkIsPrimaryNurse.Checked;
            }
            #endregion

            #region Bank Information
            entity.BankName = txtBankName.Text;
            entity.BankBranch = txtBankBranch.Text;
            entity.BankAccountNo = txtBankAccountNo.Text;
            entity.VirtualAccountNo = txtVirtualAccountNo.Text;
            entity.BankAccountName = txtBankAccountName.Text;
            entity.BankClearingPassword = txtBankClearingPassword.Text;
            if (cboPaymentMethod.Value != null && cboPaymentMethod.Value.ToString() != "")
                entity.GCRevenueSharingPayment = cboPaymentMethod.Value.ToString();
            else
                entity.GCRevenueSharingPayment = null;
            #endregion

            #region Other Information
            entity.Notes = txtNotes.Text;
            entity.IsAllowWaitingList = chkIsAllowWaitingList.Checked;
            entity.IsDummy = chkIsDummy.Checked;
            entity.MaximumAppointment = Convert.ToInt16(txtMaxAppointment.Text);
            if (entity.IsAllowWaitingList) entity.MaximumWaitingList = Convert.ToInt16(txtMaximumWaitingList.Text);
            else entity.MaximumWaitingList = 0;
            if (txtVisitDuration.Text == "")
                entity.VisitDurationDefault = 0;
            else
                entity.VisitDurationDefault = Convert.ToInt32(txtVisitDuration.Text);

            #endregion

            entity.PictureFileName = txtPicName.Text;

            string vClaimInfo = "";
            if (txtVKlaimDokterDPJPCode.Text != "" && hdnVKlaimDokterDPJPName.Value != "")
            {
                vClaimInfo = string.Format("{0}|{1}", txtVKlaimDokterDPJPCode.Text, hdnVKlaimDokterDPJPName.Value);
            }

            string hfisInfo = "";
            if (txtBPJSReferenceInfo2Code.Text != "" && hdnBPJSReferenceInfo2Name.Value != "")
            {
                hfisInfo = string.Format("{0}|{1}", txtBPJSReferenceInfo2Code.Text, hdnBPJSReferenceInfo2Name.Value);
            }

            string spesialisasiInfo = "";
            if (txtKodeSpesialisasi.Text != "" && hdnBPJSSpesialisasi.Value != "")
            {
                spesialisasiInfo = string.Format("{0}|{1}", txtKodeSpesialisasi.Text, hdnBPJSSpesialisasi.Value);
            }

            string subSpesialisasiInfo = "";
            if (txtKodeSubSpesialisasi.Text != "" && hdnBPJSSubSpesialisasi.Value != "")
            {
                subSpesialisasiInfo = string.Format("{0}|{1}", txtKodeSubSpesialisasi.Text, hdnBPJSSubSpesialisasi.Value);
            }

            if (vClaimInfo != "" || hfisInfo != "" || spesialisasiInfo != "" || subSpesialisasiInfo != "")
            {
                //if (String.IsNullOrEmpty(vClaimInfo) && !String.IsNullOrEmpty(hfisInfo))
                //{
                //    entity.BPJSReferenceInfo = String.Format(";{0}", hfisInfo);
                //}
                //else
                //{
                //    entity.BPJSReferenceInfo = String.Format("{0};{1}", vClaimInfo, hfisInfo);
                //}
                entity.BPJSReferenceInfo = string.Format("{0};{1};{2};{3}", vClaimInfo, hfisInfo, spesialisasiInfo, subSpesialisasiInfo);
            }
            else
            {
                entity.BPJSReferenceInfo = string.Empty;
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ParamedicCode = '{0}'", txtParamedicCode.Text);
            List<ParamedicMaster> lst = BusinessLayer.GetParamedicMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Healthcare Professional with Code " + txtParamedicCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("ParamedicCode = '{0}' AND ParamedicID != {1}", txtParamedicCode.Text, ID);
            List<ParamedicMaster> lst = BusinessLayer.GetParamedicMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Healthcare Professional with Code " + txtParamedicCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicMasterDao entityDao = new ParamedicMasterDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            try
            {
                ParamedicMaster entity = new ParamedicMaster();
                Address entityAddress = new Address();
                ControlToEntity(entity, entityAddress);

                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityAddress.CreatedBy = AppSession.UserLogin.UserID;

                entityAddressDao.Insert(entityAddress);
                entity.HomeAddressID = BusinessLayer.GetAddressMaxID(ctx);

                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

                ctx.CommitTransaction();

                BridgingToMedinfrasMobileApps(entity, Convert.ToInt32(retval), "001");
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicMasterDao entityDao = new ParamedicMasterDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            try
            {
                ParamedicMaster entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                Address entityAddress = new Address();

                if (entity.HomeAddressID != 0 && entity.HomeAddressID != null)
                    entityAddress = entityAddressDao.Get((int)entity.HomeAddressID);

                ControlToEntity(entity, entityAddress);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                if (entity.HomeAddressID != 0 && entity.HomeAddressID != null)
                {
                    entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Update(entityAddress);
                }
                else
                {
                    entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Insert(entityAddress);
                }

                entityDao.Update(entity);

                ctx.CommitTransaction();

                BridgingToMedinfrasMobileApps(entity, entity.ParamedicID, "002");
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

        private void BridgingToMedinfrasMobileApps(ParamedicMaster oParamedicMaster, int paramedicID, string eventType)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {

                if (oParamedicMaster != null)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnParamedicMasterChanged(oParamedicMaster, paramedicID, eventType);
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