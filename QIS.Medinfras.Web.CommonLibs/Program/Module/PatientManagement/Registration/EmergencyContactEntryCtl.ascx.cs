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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EmergencyContactEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
                txtRegistrationNoCtlPF.Text = entity.RegistrationNo;
                txtPatientCtlPF.Text = string.Format("{0} - {1}", entity.MedicalNo, entity.PatientName);
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnMRN.Value = entity.MRN.ToString();
                
                SetControlProperties();

                List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.RM0078));
                hdnIsAddressUseZipCode.Value = lstSetParDt.Where(p => p.ParameterCode == Constant.SettingParameter.RM0078).FirstOrDefault().ParameterValue;

                if (hdnIsAddressUseZipCode.Value == "1")
                {
                    txtCounty.Attributes.Add("readonly", "readonly");
                    txtDistrict.Attributes.Add("readonly", "readonly");
                    txtCity.Attributes.Add("readonly", "readonly");
                    txtProvinceCode.Attributes.Add("readonly", "readonly");
                    lblProvince.Attributes.Remove("class");
                    lblProvince.Attributes.Add("class", "lblNormal");
                }

                EmergencyContact entityEmergency = BusinessLayer.GetEmergencyContact(entity.RegistrationID);
                if (entityEmergency != null)
                {
                    IsAdd = false;
                    vAddress entityAddress;
                    if (entityEmergency.FamilyID != null)
                    {
                        PatientFamily enFam = BusinessLayer.GetPatientFamily(Convert.ToInt32(entityEmergency.FamilyID));
                        entityAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", enFam.AddressID)).FirstOrDefault();
                        hdnFamilyID.Value = entityEmergency.FamilyID.ToString();
                        chkIsFamily.Checked = true;
                        txtContactName.ReadOnly = true;
                    }
                    else
                    {
                        entityAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entityEmergency.AddressID)).FirstOrDefault();
                        chkIsFamily.Checked = false;
                        txtContactName.ReadOnly = false;
                    }
                    EntityToControl(entityEmergency, entityAddress);
                }
                else
                {
                    IsAdd = true;
                }
            }
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }

        protected string OnGetSCPatientJobFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.OCCUPATION);
        }
        #endregion

        private void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID LIKE '%{0}%' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.FAMILY_RELATION));
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboRelationship, lstSc, "StandardCodeName", "StandardCodeID");
            cboRelationship.SelectedIndex = 0;
            
        }
        protected override void OnControlEntrySetting()
        {
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string defaultGCState = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];
            string defaultPhoneArea = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PHONE_AREA).ParameterValue;

            SetControlEntrySetting(txtTelephoneNo, new ControlEntrySetting(true, true, true, defaultPhoneArea));
            SetControlEntrySetting(txtMobilePhone, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDOB, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtBirthPlace, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtIdentityCardNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPatientJobCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPatientJobName, new ControlEntrySetting(false, false, false));


            #region Patient Address
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRTData, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRWData, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceCode, new ControlEntrySetting(true, true, false, defaultGCState));
            SetControlEntrySetting(txtProvinceName, new ControlEntrySetting(false, false, false, healthcare.State));
            SetControlEntrySetting(hdnZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmail, new ControlEntrySetting(true, true, false));
            #endregion

        }
        private void EntityToControl(EmergencyContact entity, vAddress entityAddress)
        {
            if (entity.FamilyID != null)
            {
                hdnFamilyID.Value = entity.FamilyID.ToString();
                vPatientFamily entityFamily = BusinessLayer.GetvPatientFamilyList(string.Format("FamilyID = {0}", entity.FamilyID)).FirstOrDefault();
                txtFamily.Text = entityFamily.FamilyMedicalNo;
                txtContactName.Text = entityFamily.FullName;

                lblFamily.Attributes.Add("class", "lblLink");
                SetControlEntrySetting(txtFamily, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(cboRelationship, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtContactName, new ControlEntrySetting(false, true, false));
            }
            else
            {
                txtContactName.Text = entity.ContactName;
                txtBirthPlace.Text = entity.CityOfBirth;
                txtDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtIdentityCardNo.Text = entity.SSN;

                lblFamily.Attributes.Remove("class");
                SetControlEntrySetting(txtFamily, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(cboRelationship, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtContactName, new ControlEntrySetting(true, true, false));
            }

            if (entity.GCOccupation != "")
            {
                txtPatientJobCode.Text = entity.GCOccupation.Split('^')[1];
                StandardCode std = BusinessLayer.GetStandardCode(entity.GCOccupation);
                txtPatientJobName.Text = std.StandardCodeName;
            }
            else
            {
                txtPatientJobCode.Text = "";
                txtPatientJobName.Text = "";
            }

            if (entity.GCRelationship != "")
                cboRelationship.Value = entity.GCRelationship;
            else
                cboRelationship.SelectedIndex = 0;

            txtTelephoneNo.Text = entity.PhoneNo;
            txtMobilePhone.Text = entity.MobilePhoneNo;
            txtRemarks.Text = entity.Remarks;

            #region Address
            hdnAddressID.Value = entity.AddressID.ToString();
            txtAddress.Text = entityAddress.StreetName;
            txtRTData.Text = entityAddress.RT;
            txtRWData.Text = entityAddress.RW;
            txtCounty.Text = entityAddress.County;
            txtDistrict.Text = entityAddress.District;
            txtCity.Text = entityAddress.City;
            if (entityAddress.GCState != "")
            {
                txtProvinceCode.Text = entityAddress.GCState.Split('^')[1];
                StandardCode std = BusinessLayer.GetStandardCode(entityAddress.GCState);
                txtProvinceName.Text = std.StandardCodeName;
            }
            else
            {
                txtProvinceCode.Text = "";
                txtProvinceName.Text = "";
            }
            hdnZipCode.Value = entityAddress.ZipCodeID.ToString();
            txtZipCode.Text = entityAddress.ZipCode.ToString();
            txtEmail.Text = entityAddress.Email;
            #endregion
        }

        private void ControlToEntity(EmergencyContact entity, Address entityAddress)
        {
            #region Address
            entityAddress.StreetName = txtAddress.Text;
            entityAddress.RT = txtRTData.Text;
            entityAddress.RW = txtRWData.Text;
            entityAddress.County = txtCounty.Text;
            entityAddress.District = txtDistrict.Text;
            entityAddress.City = txtCity.Text;
            entityAddress.GCState = txtProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCode.Text);
            if (hdnZipCode.Value == "")
                entityAddress.ZipCode = null;
            else
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCode.Value);
            entityAddress.PhoneNo1 = txtTelephoneNo.Text;
            entityAddress.Email = txtEmail.Text;
            #endregion

            entity.ContactName = txtContactName.Text;
            entity.GCRelationship = Helper.GetComboBoxValue(cboRelationship, true);
            entity.PhoneNo = txtTelephoneNo.Text;
            entity.MobilePhoneNo = txtMobilePhone.Text;
            entity.SSN = txtIdentityCardNo.Text;
            entity.CityOfBirth = txtBirthPlace.Text;
            entity.DateOfBirth = Helper.GetDatePickerValue(txtDOB);
            entity.Remarks = txtRemarks.Text;
            entity.GCOccupation = txtPatientJobCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.OCCUPATION, txtPatientJobCode.Text);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);

            EmergencyContactDao entityDao = new EmergencyContactDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            try
            {
                EmergencyContact entity = new EmergencyContact();
                Address entityAddress = new Address();

                ControlToEntity(entity, entityAddress);

                entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                entityAddressDao.Insert(entityAddress);
                entity.AddressID = BusinessLayer.GetAddressMaxID(ctx);

                if (chkIsFamily.Checked)
                {
                    entityAddress = entityAddressDao.Get(Convert.ToInt32(hdnAddressID.Value));
                    ControlToEntity(entity, entityAddress);
                    entity.FamilyID = Convert.ToInt32(hdnFamilyID.Value);
                    entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Update(entityAddress);
                }
                else
                {
                    entity.FamilyID = null;
                }
                entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = txtRegistrationNoCtlPF.Text;
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
            EmergencyContactDao entityDao = new EmergencyContactDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            try
            {
                EmergencyContact entity = entityDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                Address entityAddress = entityAddressDao.Get(entity.AddressID);
                
                ControlToEntity(entity, entityAddress);
                entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityAddressDao.Update(entityAddress);
                
                if (chkIsFamily.Checked)
                {
                    entity.FamilyID = Convert.ToInt32(hdnFamilyID.Value);
                    PatientFamily enFam = BusinessLayer.GetPatientFamily(Convert.ToInt32(hdnFamilyID.Value));
                    entityAddress = entityAddressDao.Get(Convert.ToInt32(enFam.AddressID));
                    ControlToEntity(entity, entityAddress);
                    entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Update(entityAddress);
                }
                else
                {
                    entity.FamilyID = null;
                }
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