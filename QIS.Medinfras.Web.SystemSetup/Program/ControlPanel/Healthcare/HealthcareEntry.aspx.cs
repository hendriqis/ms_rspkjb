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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class HealthcareEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.HEALTHCARE;
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
                String healthcareID = Request.QueryString["id"];
                hdnID.Value = healthcareID;
                SetControlProperties();
                Healthcare entity = BusinessLayer.GetHealthcare(healthcareID);
                vAddress entityAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entity.AddressID))[0];
                EntityToControl(entity, entityAddress);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtHealthcareID.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }
        #endregion

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}', '{2}') AND IsDeleted = 0", 
                Constant.StandardCode.HEALTHCARE_OPERATING_GROUP, 
                Constant.StandardCode.HEALTHCARE_CLASS, 
                Constant.StandardCode.KDTARIF_INACBG
                ));
            List<StandardCode> lst1 = lstStandardCode.Where(lst => lst.StandardCodeID.StartsWith(Constant.StandardCode.HEALTHCARE_OPERATING_GROUP)).ToList();
            List<StandardCode> lst2 = lstStandardCode.Where(lst => lst.StandardCodeID.StartsWith(Constant.StandardCode.HEALTHCARE_CLASS)).ToList();
            List<StandardCode> lst3 = lstStandardCode.Where(lst => lst.StandardCodeID.StartsWith(Constant.StandardCode.KDTARIF_INACBG)).ToList();
            Methods.SetComboBoxField<StandardCode>(cboOperatingGroup, lst1, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboHealthcareClass, lst2, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboKdTarifINACBG1, lst3, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboKdTarifINACBG2, lst3, "StandardCodeName", "StandardCodeID"); 
        }

        protected override void OnControlEntrySetting()
        {
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string defaultGCState = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];
            string defaultPhoneArea = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PHONE_AREA).ParameterValue;

            SetControlEntrySetting(txtHealthcareID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtHealthcareName, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtInitial, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboOperatingGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboHealthcareClass, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboKdTarifINACBG1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboKdTarifINACBG2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNpwpNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLicenseNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBPJSCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCompanyName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDirectorName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccountSegmentNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtLogoFileName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLoginPageImageFileName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingCustomRightPanel, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceCode, new ControlEntrySetting(true, true, false, defaultGCState));
            SetControlEntrySetting(txtProvinceName, new ControlEntrySetting(false, false, false, healthcare.State));
            SetControlEntrySetting(hdnZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTelephoneNo, new ControlEntrySetting(true, true, false, defaultPhoneArea));
            SetControlEntrySetting(txtEmail, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLicenseDate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLastLicenseDateExpired, new ControlEntrySetting(false, false, false));
        }

        private void EntityToControl(Healthcare entity, vAddress entityAddress)
        {
            txtHealthcareID.Text = entity.HealthcareID;
            txtHealthcareName.Text = entity.HealthcareName;
            txtShortName.Text = entity.ShortName;
            txtInitial.Text = entity.Initial;
            cboOperatingGroup.Value = entity.GCOperatingGroup;
            cboHealthcareClass.Value = entity.GCHealthcareClass;
            txtNpwpNo.Text = entity.TaxRegistrantNo;
            txtLicenseNo.Text = entity.LicenseNo;
            txtBPJSCode.Text = entity.BPJSCode;
            txtCompanyName.Text = entity.CompanyName;
            txtDirectorName.Text = entity.DirectorName;
            txtGLAccountSegmentNo.Text = entity.GLAccountNoSegment;
            txtLogoFileName.Text = entity.LogoFileName;
            txtLoginPageImageFileName.Text = entity.LoginPageImageFileName;
            chkIsUsingCustomRightPanel.Checked = entity.IsUsingCustomRightPanel;

            txtAddress.Text = entityAddress.StreetName;
            txtCounty.Text = entityAddress.County; // Desa
            txtDistrict.Text = entityAddress.District; //Kabupaten
            txtCity.Text = entityAddress.City;
            if (entityAddress.GCState != "")
                txtProvinceCode.Text = entityAddress.GCState.Split('^')[1];
            else
                txtProvinceCode.Text = "";
            txtProvinceName.Text = entityAddress.State;
            hdnZipCode.Value = entityAddress.ZipCodeID.ToString();
            txtZipCode.Text = entityAddress.ZipCode;
            txtTelephoneNo.Text = entityAddress.PhoneNo1;
            txtFaxNo.Text = entityAddress.FaxNo1;
            txtEmail.Text = entityAddress.Email;
            txtLicenseDate.Text = entity.ExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLastLicenseDateExpired.Text = entity.ExpiredUpdatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT_2);

            if (entity.GCEKlaimTariffCategory != null && entity.GCEKlaimTariffCategory != "")
            {
                string[] tariffCatList = entity.GCEKlaimTariffCategory.Split('|');
                cboKdTarifINACBG1.Value = tariffCatList[0];
                cboKdTarifINACBG2.Value = tariffCatList[1];
            }
        }

        private void ControlToEntity(Healthcare entity, Address entityAddress)
        {
            entity.HealthcareName = txtHealthcareName.Text;
            entity.ShortName = txtShortName.Text;
            entity.Initial = txtInitial.Text;
            entity.GCOperatingGroup = cboOperatingGroup.Value.ToString();
            entity.GCHealthcareClass = cboHealthcareClass.Value.ToString();
            entity.TaxRegistrantNo = txtNpwpNo.Text;
            entity.LicenseNo = txtLicenseNo.Text;
            entity.BPJSCode = txtBPJSCode.Text;
            entity.CompanyName = txtCompanyName.Text;
            entity.DirectorName = txtDirectorName.Text;
            entity.GLAccountNoSegment = txtGLAccountSegmentNo.Text;
            entity.LogoFileName = txtLogoFileName.Text;
            entity.LoginPageImageFileName = txtLoginPageImageFileName.Text;
            entity.IsUsingCustomRightPanel = chkIsUsingCustomRightPanel.Checked;

            if (!string.IsNullOrEmpty(txtLicenseDate.Text))
            {
                entity.ExpiredDate = Helper.GetDatePickerValue(txtLicenseDate.Text);
                entity.ExpiredUpdatedDate = DateTime.Now; 
            }

            entityAddress.StreetName = txtAddress.Text;
            entityAddress.County = txtCounty.Text; // Desa
            entityAddress.District = txtDistrict.Text; //Kabupaten
            entityAddress.City = txtCity.Text;
            entityAddress.GCState = txtProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCode.Text);
            if (hdnZipCode.Value == "" || hdnZipCode.Value == "0")
                entityAddress.ZipCode = null;
            else
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCode.Value);
            entityAddress.PhoneNo1 = txtTelephoneNo.Text;
            entityAddress.FaxNo1 = txtFaxNo.Text;
            entityAddress.Email = txtEmail.Text;

            if (cboKdTarifINACBG1.Value != null && cboKdTarifINACBG2.Value != null)
            {
                entity.GCEKlaimTariffCategory = cboKdTarifINACBG1.Value.ToString() + "|" + cboKdTarifINACBG2.Value.ToString();
            }
            else if (cboKdTarifINACBG1.Value != null && cboKdTarifINACBG2.Value == null)
            {
                entity.GCEKlaimTariffCategory = cboKdTarifINACBG1.Value.ToString() + "|" + cboKdTarifINACBG1.Value.ToString();
            }            
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("HealthcareID = '{0}'", Request.Form[txtHealthcareID.UniqueID]);
            List<Healthcare> lst = BusinessLayer.GetHealthcareList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Healthcare Code is already exist";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            HealthcareDao entityDao = new HealthcareDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            try
            {
                Healthcare entity = new Healthcare();
                Address entityAddress = new Address();
                ControlToEntity(entity, entityAddress);

                entity.HealthcareID = Request.Form[txtHealthcareID.UniqueID];
                entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                entityAddressDao.Insert(entityAddress);
                entity.AddressID = BusinessLayer.GetAddressMaxID(ctx);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Insert(entity);

                retval = entity.HealthcareID;
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            HealthcareDao entityDao = new HealthcareDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            try
            {
                Healthcare entity = entityDao.Get(hdnID.Value);
                Address entityAddress = entityAddressDao.Get((int)entity.AddressID);
                ControlToEntity(entity, entityAddress);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityAddressDao.Update(entityAddress);
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