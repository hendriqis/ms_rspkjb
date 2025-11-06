using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class EmployeeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.EMPLOYEE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                vEmployee entity = BusinessLayer.GetvEmployeeList(String.Format("EmployeeID = {0}",Convert.ToInt32(ID)))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
            }
            txtEmployeeCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.TITLE, Constant.StandardCode.SUFFIX, Constant.StandardCode.DEPARTMENT, Constant.StandardCode.EMPLOYEE_OCCUPATION, Constant.StandardCode.EMPLOYMENT_STATUS, Constant.StandardCode.EMPLOYEE_OCCUPATION_LEVEL));
            
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboTitle, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.TITLE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSuffix, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.SUFFIX || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            
            Methods.SetComboBoxField<StandardCode>(cboGCDepartment, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DEPARTMENT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCOccupation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.EMPLOYEE_OCCUPATION).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCOccupationLevel, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.EMPLOYEE_OCCUPATION_LEVEL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCEmployeeStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.EMPLOYMENT_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string defaultGCState = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];

            #region Patient Data
            SetControlEntrySetting(txtEmployeeCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFamilyName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInitial, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTitle, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboSuffix, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBirthPlace, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDOB, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            #endregion

            #region Data Karyawan
            SetControlEntrySetting(cboGCDepartment, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCOccupation, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCOccupationLevel, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtVATRegistrationNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCEmployeeStatus, new ControlEntrySetting(true, true, true));
            #endregion

            #region Alamat Karyawan
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceCode, new ControlEntrySetting(true, true, false, defaultGCState));
            SetControlEntrySetting(txtProvinceName, new ControlEntrySetting(false, false, false, healthcare.State));
            SetControlEntrySetting(hdnZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, false));
            #endregion

            #region Data Kontak Karywan
            SetControlEntrySetting(txtMobilePhone1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMobilePhone2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmail1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmail2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtOfficeExtension, new ControlEntrySetting(true, true, false));
            #endregion

            #region Inforamsi Lain
            SetControlEntrySetting(txtPictureFileName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            #endregion
        }

        private void EntityToControl(vEmployee entity)
        {
            #region Patient Data
            txtEmployeeCode.Text = entity.EmployeeCode;
            txtFirstName.Text = entity.FirstName;
            txtMiddleName.Text = entity.MiddleName;
            txtFamilyName.Text = entity.LastName;
            txtInitial.Text = entity.Initial;
            cboTitle.Value = entity.GCTitle;
            cboSuffix.Value = entity.GCSuffix;
            txtBirthPlace.Text = entity.CityOfBirth;
            txtDOB.Text = entity.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            #endregion

            #region Data Karyawan
            cboGCDepartment.Value = entity.GCDepartment;
            cboGCOccupation.Value = entity.GCOccupation;
            cboGCOccupationLevel.Value = entity.GCOccupationLevel;
            txtVATRegistrationNo.Text = entity.VATRegistrationNo;
            cboGCEmployeeStatus.Value = entity.GCEmployeeStatus;
            #endregion
            
            #region Alamat Karyawan
            txtAddress.Text = entity.StreetName;
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

            #region Data Kontak Karyawan
            txtMobilePhone1.Text = entity.MobilePhoneNo1;
            txtMobilePhone2.Text = entity.MobilePhoneNo2;
            txtEmail1.Text = entity.EmailAddress1;
            txtEmail2.Text = entity.EmailAddress2;
            txtOfficeExtension.Text = entity.OfficeExtensionNo;
            #endregion

            #region Informasi Lain
            txtPictureFileName.Text = entity.PictureFileName;
            txtNotes.Text = entity.Remarks;
            #endregion
        }

        private void ControlToEntity(Employee entity,Address entityAddress)
        {
            #region Patient Data
            entity.EmployeeCode = txtEmployeeCode.Text;
            entity.FirstName = txtFirstName.Text;
            entity.MiddleName = txtMiddleName.Text;
            entity.LastName = txtFamilyName.Text;
            entity.Initial = txtInitial.Text;
            entity.GCTitle = Helper.GetComboBoxValue(cboTitle, true);
            entity.GCSuffix = Helper.GetComboBoxValue(cboSuffix, true);
            entity.CityOfBirth = txtBirthPlace.Text;
            entity.DateOfBirth = Helper.GetDatePickerValue(txtDOB);
            string suffix = cboSuffix.Value == null ? "" : cboSuffix.Text;
            string title = cboTitle.Value == null ? "" : cboTitle.Text;
            string Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
            entity.FullName = Helper.GenerateFullName(Name, title, suffix);
            #endregion
            
            #region Data Karyawan
            entity.GCDepartment = Helper.GetComboBoxValue(cboGCDepartment, true);
            entity.GCOccupation = Helper.GetComboBoxValue(cboGCOccupation, true);
            entity.GCOccupationLevel = Helper.GetComboBoxValue(cboGCOccupationLevel, true);
            entity.VATRegistrationNo = txtVATRegistrationNo.Text;
            entity.GCEmployeeStatus = Helper.GetComboBoxValue(cboGCEmployeeStatus, true);
            #endregion

            #region Alamat Karyawan
            entityAddress.StreetName = txtAddress.Text;
            entityAddress.County = txtCounty.Text; // Desa
            entityAddress.District = txtDistrict.Text; //Kabupaten
            entityAddress.City = txtCity.Text;
            entityAddress.GCState = txtProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCode.Text);
            if (hdnZipCode.Value == "" || hdnZipCode.Value == "0")
                entityAddress.ZipCode = null;
            else
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCode.Value);
            #endregion

            #region Data Kontak Karyawan
            entity.MobilePhoneNo1 = txtMobilePhone1.Text;
            entity.MobilePhoneNo2 = txtMobilePhone2.Text;
            entity.EmailAddress1 = txtEmail1.Text;
            entity.EmailAddress2 = txtEmail2.Text;
            entity.OfficeExtensionNo = txtOfficeExtension.Text;
            #endregion

            #region Informasi Lain
            entity.PictureFileName = txtPictureFileName.Text;
            entity.Remarks = txtNotes.Text;
            #endregion
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("EmployeeCode = '{0}'", txtEmployeeCode.Text);
            List<Employee> lst = BusinessLayer.GetEmployeeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Employee with Code " + txtEmployeeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            
            IDbContext ctx = DbFactory.Configure(true);
            EmployeeDao employeeDao = new EmployeeDao(ctx);
            AddressDao addressDao = new AddressDao(ctx);
            bool result = false;
            try
            {
                Address entityAddress = new Address();
                Employee entity = new Employee();
                ControlToEntity(entity, entityAddress);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                addressDao.Insert(entityAddress);
                entity.AddressID = BusinessLayer.GetAddressMaxID(ctx);
                employeeDao.Insert(entity);
                
                retval = Convert.ToString(entity.EmployeeID);
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                
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
            EmployeeDao employeeDao = new EmployeeDao(ctx);
            AddressDao addressDao = new AddressDao(ctx);

            try
            {
                Employee entity = employeeDao.Get(Convert.ToInt32(hdnID.Value));
                Address entityAddress = addressDao.Get(entity.AddressID);

                ControlToEntity(entity, entityAddress);

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;

                addressDao.Update(entityAddress);
                employeeDao.Update(entity);
                ctx.CommitTransaction();
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
    }
}