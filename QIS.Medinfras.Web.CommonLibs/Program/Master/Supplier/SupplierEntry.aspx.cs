using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SupplierEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            String menu = Request.QueryString["menu"];

            if (menu == "FN")
            {
                return Constant.MenuCode.Finance.SUPPLIER;
            }
            else
            {
                return Constant.MenuCode.Inventory.SUPPLIER;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 1)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                int BusinessPartnerID = Convert.ToInt32(ID);
                BusinessPartners entity = BusinessLayer.GetBusinessPartners(BusinessPartnerID);
                vSupplier entitySup = BusinessLayer.GetvSupplierList(string.Format("BusinessPartnerID = {0}", BusinessPartnerID)).FirstOrDefault();
                BusinessPartnerTagField entityTagField = BusinessLayer.GetBusinessPartnerTagField(BusinessPartnerID);
                vAddress entityAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entity.AddressID))[0];

                SetControlProperties();

                if (entity.BillingAddressID != null)
                {
                    vAddress entityBillingAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entity.BillingAddressID))[0];
                    EntityToControl(entity, entitySup, entityAddress, entityBillingAddress, entityTagField);
                }
                else
                {
                    vAddress entityBillingAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entity.AddressID))[0];
                    EntityToControl(entity, entitySup, entityAddress, entityBillingAddress, entityTagField);
                }
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
                cboHealthcare.Value = AppSession.UserLogin.HealthcareID;
            }

            txtSupplierName.Focus();

            String menu = Request.QueryString["menu"];
            if (menu == "FN")
            {
                lblInfoTransactionOpen.Style.Add("display", "none");
            }

            if (hdnIM0133.Value == "1")
            {
                lblInfoTransactionOpen.Style.Add("display", "none");
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }

        protected string onGetSCBillingProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }
        #endregion

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.SUPPLIER_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboSupplierType, lstSc, "StandardCodeName", "StandardCodeID");

            List<Term> listTerm = BusinessLayer.GetTermList("IsDeleted = 0");
            listTerm.Insert(0, new Term { TermID = 0, TermName = "" });
            Methods.SetComboBoxField<Term>(cboTerm, listTerm, "TermName", "TermID");

            List<Healthcare> listHealthcare = BusinessLayer.GetHealthcareList("");
            listHealthcare.Insert(0, new Healthcare { HealthcareID = "", HealthcareName = "" });
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, listHealthcare, "HealthcareName", "HealthcareID");
        }

        protected override void OnControlEntrySetting()
        {
            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM0133);
            hdnIM0133.Value = setvarDt.ParameterValue;

            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string defaultGCState = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];
            string defaultPhoneArea = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PHONE_AREA).ParameterValue;

            SetControlEntrySetting(txtFileName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnFileName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnUploadedFile1, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(imgPreview, new ControlEntrySetting(false, false, false));

            #region General Information
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtExternalCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, false));
            #endregion

            #region Contact Person
            SetControlEntrySetting(txtContactPersonName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtContactPersonPhoneNumber, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtContactPersonEmail, new ControlEntrySetting(true, true, false));
            #endregion

            #region Supplier Information
            SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtVATRegistrationNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTerm, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMaxPOAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLeadTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnSupplierLineID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSupplierLineCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSupplierLineName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboSupplierType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsPharmacySupplier, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsLogisticSupplier, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountSegmentNo, new ControlEntrySetting(true, false, false));
            #endregion

            #region Supplier Status
            SetControlEntrySetting(chkIsActive, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtNotActiveReason, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBlacklist, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsPaymentHold, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsTaxable, new ControlEntrySetting(true, true, false));
            #endregion

            #region Address
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceCode, new ControlEntrySetting(true, true, false, defaultGCState));
            SetControlEntrySetting(txtProvinceName, new ControlEntrySetting(false, false, false, healthcare.State));
            SetControlEntrySetting(hdnZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTelephoneNo, new ControlEntrySetting(true, true, false, defaultPhoneArea));
            SetControlEntrySetting(txtFaxNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmail, new ControlEntrySetting(true, true, false));
            #endregion

            #region BillingAddress
            SetControlEntrySetting(txtBillingAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBillingCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBillingDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBillingCity, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBillingProvinceCode, new ControlEntrySetting(true, true, false, defaultGCState));
            SetControlEntrySetting(txtBillingProvinceName, new ControlEntrySetting(false, false, false, healthcare.State));
            SetControlEntrySetting(hdnBillingZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtBillingZipCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBillingTelephoneNo, new ControlEntrySetting(true, true, false, defaultPhoneArea));
            SetControlEntrySetting(txtBillingFaxNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBillingEmail, new ControlEntrySetting(true, true, false));
            #endregion

            #region Bank Information
            String menu = Request.QueryString["menu"];
            if (menu == "FN")
            {
                SetControlEntrySetting(txtBankName, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtBankBranch, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtBankAccountNo, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtBankVirtualAccountNo, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtBankAccountName, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtBankClearingPassword, new ControlEntrySetting(true, true, false));
            }
            else
            {
                if (hdnIM0133.Value == "1")
                {
                    SetControlEntrySetting(txtBankName, new ControlEntrySetting(true, true, false));
                    SetControlEntrySetting(txtBankBranch, new ControlEntrySetting(true, true, false));
                    SetControlEntrySetting(txtBankAccountNo, new ControlEntrySetting(true, true, false));
                    SetControlEntrySetting(txtBankVirtualAccountNo, new ControlEntrySetting(true, true, false));
                    SetControlEntrySetting(txtBankAccountName, new ControlEntrySetting(true, true, false));
                    SetControlEntrySetting(txtBankClearingPassword, new ControlEntrySetting(true, true, false));
                }
                else
                {
                    SetControlEntrySetting(txtBankName, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtBankBranch, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtBankAccountNo, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtBankVirtualAccountNo, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtBankAccountName, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtBankClearingPassword, new ControlEntrySetting(false, false, false));
                }
            }
            #endregion

            #region Other Information
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
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

        private void EntityToControl(BusinessPartners entity, vSupplier entitySup, vAddress entityAddress, vAddress entityBillingAddress, BusinessPartnerTagField entityTagField)
        {
            #region General Information
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtExternalCode.Text = entity.CommCode;
            txtSupplierName.Text = entity.BusinessPartnerName;
            txtShortName.Text = entity.ShortName;
            #endregion

            #region Contact Person
            txtContactPersonName.Text = entity.ContactPerson;
            txtContactPersonPhoneNumber.Text = entity.ContactPersonMobileNo;
            txtContactPersonEmail.Text = entity.ContactPersonEmail;
            #endregion

            #region Supplier Information
            cboHealthcare.Value = entity.HealthcareID;
            txtVATRegistrationNo.Text = entity.VATRegistrationNo;
            cboTerm.Value = entity.TermID.ToString();
            txtMaxPOAmount.Text = entitySup.MaxPOAmount.ToString();
            txtLeadTime.Text = entitySup.LeadTime.ToString();
            hdnSupplierLineID.Value = entitySup.SupplierLineID.ToString();
            txtSupplierLineCode.Text = entitySup.SupplierLineCode;
            txtSupplierLineName.Text = entitySup.SupplierLineName;
            cboSupplierType.Value = entitySup.GCSupplierType;

            chkIsPharmacySupplier.Checked = entitySup.IsPharmacySupplier;
            chkIsLogisticSupplier.Checked = entitySup.IsLogisticSupplier;

            //if (entitySup.IsLogisticSupplier)
            //{
            //    rblItemType.SelectedValue = "0";
            //}
            //else
            //{
            //    rblItemType.SelectedValue = "1";
            //}
            txtGLAccountSegmentNo.Text = entity.GLAccountNoSegment;
            #endregion

            #region Supplier Status
            chkIsPaymentHold.Checked = entitySup.IsPaymentHold;
            chkIsTaxable.Checked = entity.IsTaxable;
            chkIsActive.Checked = entity.IsActive;
            txtNotActiveReason.Text = entity.NotActiveReason;
            chkIsBlacklist.Checked = entity.IsBlackList;
            #endregion

            #region Logo Perusahaan
            txtFileName.Text = entity.LogoFileName;
            string logoPath = string.Format("BusinessPartner/{0}_{1}/", entity.BusinessPartnerCode, entity.BusinessPartnerName);
            imgPreview.Src = string.Format("{0}{1}logo{2}.png", AppConfigManager.QISVirtualDirectory, logoPath, entity.BusinessPartnerCode);
            #endregion

            #region Address
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
            #endregion

            #region Billing Address
            txtBillingAddress.Text = entityBillingAddress.StreetName;
            txtBillingCounty.Text = entityBillingAddress.County; // Desa
            txtBillingDistrict.Text = entityBillingAddress.District; //Kabupaten
            txtBillingCity.Text = entityBillingAddress.City;
            if (entityBillingAddress.GCState != "")
                txtBillingProvinceCode.Text = entityBillingAddress.GCState.Split('^')[1];
            else
                txtBillingProvinceCode.Text = "";
            txtBillingProvinceName.Text = entityBillingAddress.State;
            hdnBillingZipCode.Value = entityBillingAddress.ZipCodeID.ToString();
            txtBillingZipCode.Text = entityBillingAddress.ZipCode;
            txtBillingTelephoneNo.Text = entityBillingAddress.PhoneNo1;
            txtBillingFaxNo.Text = entityBillingAddress.FaxNo1;
            txtBillingEmail.Text = entityBillingAddress.Email;
            #endregion

            #region Bank Information
            txtBankName.Text = entity.BankName;
            txtBankBranch.Text = entity.BankBranch;
            txtBankAccountNo.Text = entity.BankAccountNo;
            txtBankVirtualAccountNo.Text = entity.VirtualAccountNo;
            txtBankAccountName.Text = entity.BankAccountName;
            txtBankClearingPassword.Text = entity.BankClearingPassword;
            #endregion

            #region Other Information
            txtNotes.Text = entity.Remarks;
            #endregion

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                txt.Text = entityTagField.GetType().GetProperty("TagField" + hdn.Value).GetValue(entityTagField, null).ToString();
            }
            #endregion
        }

        private void ControlToEntity(BusinessPartners entity, Supplier entitySup, Address entityAddress, Address entityBillingAddress, BusinessPartnerTagField entityTagField)
        {
            #region General Information
            entity.BusinessPartnerCode = txtSupplierCode.Text;
            entity.CommCode = txtExternalCode.Text;
            entity.BusinessPartnerName = txtSupplierName.Text;
            entity.ShortName = txtShortName.Text;
            #endregion

            #region Contact Person
            entity.ContactPerson = txtContactPersonName.Text;
            entity.ContactPersonMobileNo = txtContactPersonPhoneNumber.Text;
            entity.ContactPersonEmail = txtContactPersonEmail.Text;
            #endregion

            #region Supplier Information
            if (cboHealthcare.Value != null && cboHealthcare.Value.ToString() != "")
                entity.HealthcareID = cboHealthcare.Value.ToString();
            else
                entity.HealthcareID = null;
            entitySup.GCSupplierType = cboSupplierType.Value.ToString();
            entity.VATRegistrationNo = txtVATRegistrationNo.Text;
            if (cboTerm.Value != null && cboTerm.Value.ToString() != "0")
                entity.TermID = Convert.ToInt32(cboTerm.Value);
            else
                entity.TermID = null;
            entitySup.MaxPOAmount = Convert.ToDecimal(txtMaxPOAmount.Text);
            entitySup.LeadTime = Convert.ToInt16(txtLeadTime.Text);

            if (hdnSupplierLineID.Value == "" || hdnSupplierLineID.Value == "0")
                entitySup.SupplierLineID = null;
            else
                entitySup.SupplierLineID = Convert.ToInt32(hdnSupplierLineID.Value);

            entitySup.IsPharmacySupplier = chkIsPharmacySupplier.Checked;
            entitySup.IsLogisticSupplier = chkIsLogisticSupplier.Checked;

            //if (rblItemType.SelectedValue == "0")
            //{
            //    entitySup.IsPharmacySupplier = false;
            //    entitySup.IsLogisticSupplier = true;
            //}
            //else
            //{
            //    entitySup.IsPharmacySupplier = true;
            //    entitySup.IsLogisticSupplier = false;
            //}
            entity.GLAccountNoSegment = txtGLAccountSegmentNo.Text;
            #endregion

            #region Supplier Status
            entity.IsBlackList = chkIsBlacklist.Checked;
            entitySup.IsPaymentHold = chkIsPaymentHold.Checked;
            entity.IsTaxable = chkIsTaxable.Checked;
            entity.IsActive = chkIsActive.Checked;
            if (entity.IsActive == false)
            {
                entity.NotActiveReason = txtNotActiveReason.Text;
                entity.NotActiveBy = AppSession.UserLogin.UserID;
                entity.NotActiveDate = DateTime.Now;
            }
            else
            {
                entity.NotActiveReason = null;
                entity.NotActiveBy = null;
                entity.NotActiveDate = null;
            }
            #endregion

            #region Address
            entityAddress.StreetName = txtAddress.Text;
            entityAddress.County = txtCounty.Text; // Desa
            entityAddress.District = txtDistrict.Text; //Kabupaten
            entityAddress.City = txtCity.Text;
            entityAddress.GCState = txtProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCode.Text);
            if (hdnZipCode.Value == "")
                entityAddress.ZipCode = null;
            else
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCode.Value);
            entityAddress.PhoneNo1 = txtTelephoneNo.Text;
            entityAddress.FaxNo1 = txtFaxNo.Text;
            entityAddress.Email = txtEmail.Text;
            #endregion

            #region Billing Address
            entityBillingAddress.StreetName = txtBillingAddress.Text;
            entityBillingAddress.County = txtBillingCounty.Text; // Desa
            entityBillingAddress.District = txtBillingDistrict.Text; //Kabupaten
            entityBillingAddress.City = txtBillingCity.Text;
            entityBillingAddress.GCState = txtBillingProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtBillingProvinceCode.Text);
            if (hdnBillingZipCode.Value == "")
                entityBillingAddress.ZipCode = null;
            else
                entityBillingAddress.ZipCode = Convert.ToInt32(hdnBillingZipCode.Value);
            entityBillingAddress.PhoneNo1 = txtBillingTelephoneNo.Text;
            entityBillingAddress.FaxNo1 = txtBillingFaxNo.Text;
            entityBillingAddress.Email = txtBillingEmail.Text;
            #endregion

            #region Bank Information
            entity.BankName = txtBankName.Text;
            entity.BankBranch = txtBankBranch.Text;
            entity.BankAccountNo = txtBankAccountNo.Text;
            entity.VirtualAccountNo = txtBankVirtualAccountNo.Text;
            entity.BankAccountName = txtBankAccountName.Text;
            entity.BankClearingPassword = txtBankClearingPassword.Text;
            #endregion

            #region Other Information
            entity.Remarks = txtNotes.Text;
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

        private void UploadPhoto(int businessPartnerID, string businessPartnerCode, string businessPartnerName, ref string fileName)
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
                path += string.Format("{0}\\", AppConfigManager.QISBusinessPartnerLogoPath.Replace('/', '\\'));

                //path = path.Replace("#BusinessPartnerCode_BusinessPartnerName", string.Format("{0}_{1}", businessPartnerCode, businessPartnerName));
                path = path.Replace("#BusinessPartnerCode", string.Format("{0}", businessPartnerCode));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                fileName = String.Format("logo{0}.png", businessPartnerCode);
                FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                byte[] data = Convert.FromBase64String(imageData);
                bw.Write(data);
                bw.Close();
            }
        }

        //protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        //{
        //    errMessage = string.Empty;

        //    string FilterExpression = string.Format("BusinessPartnerCode = '{0}'", txtSupplierCode.Text);
        //    List<BusinessPartners> lst = BusinessLayer.GetBusinessPartnersList(FilterExpression);

        //    if (lst.Count > 0)
        //        errMessage = " Supplier with Code " + txtSupplierCode.Text + " is already exist!";

        //    return (errMessage == string.Empty);
        //}

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("BusinessPartnerCode = '{0}' AND BusinessPartnerID != {1}", txtSupplierCode.Text, hdnID.Value);
            List<BusinessPartners> lst = BusinessLayer.GetBusinessPartnersList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Supplier with Code " + txtSupplierCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BusinessPartnersDao entityDao = new BusinessPartnersDao(ctx);
            SupplierDao entitySupDao = new SupplierDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            BusinessPartnerTagFieldDao entityTagFieldDao = new BusinessPartnerTagFieldDao(ctx);
            try
            {
                BusinessPartners entity = new BusinessPartners();
                Supplier entitySup = new Supplier();
                Address entityAddress = new Address();
                Address entityBillingAddress = new Address();
                BusinessPartnerTagField entityTagField = new BusinessPartnerTagField();
                ControlToEntity(entity, entitySup, entityAddress, entityBillingAddress, entityTagField);
                entity.BusinessPartnerCode = Helper.GeneratePartnerCode(ctx, entity.BusinessPartnerName);

                entity.GCBusinessPartnerType = Constant.BusinessObjectType.SUPPLIER;

                entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                entityAddressDao.Insert(entityAddress);
                entity.AddressID = BusinessLayer.GetAddressMaxID(ctx);

                entityBillingAddress.CreatedBy = AppSession.UserLogin.UserID;
                entityAddressDao.Insert(entityBillingAddress);
                entity.BillingAddressID = BusinessLayer.GetAddressMaxID(ctx);

                entity.CreatedBy = AppSession.UserLogin.UserID;

                if (String.IsNullOrEmpty(entity.CommCode))
                {
                    entity.CommCode = entity.BusinessPartnerCode;
                }

                string fileName = "";
                UploadPhoto(entity.BusinessPartnerID, entity.BusinessPartnerCode, entity.BusinessPartnerName, ref fileName);

                entity.LogoFileName = fileName;

                entityDao.Insert(entity);

                entity.BusinessPartnerID = BusinessLayer.GetBusinessPartnersMaxID(ctx);
                entitySup.BusinessPartnerID = entity.BusinessPartnerID;
                entitySupDao.Insert(entitySup);

                entityTagField.BusinessPartnerID = entity.BusinessPartnerID;
                entityTagFieldDao.Insert(entityTagField);

                retval = entity.BusinessPartnerID.ToString();

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
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
            BusinessPartnersDao entityDao = new BusinessPartnersDao(ctx);
            SupplierDao entitySupDao = new SupplierDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            BusinessPartnerTagFieldDao entityTagFieldDao = new BusinessPartnerTagFieldDao(ctx);
            try
            {
                int BusinessPartnerID = Convert.ToInt32(hdnID.Value);
                BusinessPartners entity = entityDao.Get(BusinessPartnerID);
                Supplier entitySup = entitySupDao.Get(BusinessPartnerID);
                BusinessPartnerTagField entityTagField = entityTagFieldDao.Get(BusinessPartnerID);

                Address entityAddress = null;
                Address entityBillingAddress = null;
                bool flagAddress = true;
                bool flagBillingAddress = true;

                if (entity.AddressID != null)
                {
                    entityAddress = entityAddressDao.Get((int)entity.AddressID);
                }
                else
                {
                    entityAddress = new Address();
                    flagAddress = false;
                }

                if (entity.BillingAddressID != null)
                {
                    entityBillingAddress = entityAddressDao.Get((int)entity.BillingAddressID);
                }
                else
                {
                    entityBillingAddress = new Address();
                    flagBillingAddress = false;
                }

                ControlToEntity(entity, entitySup, entityAddress, entityBillingAddress, entityTagField);

                if (flagAddress)
                {
                    entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Update(entityAddress);
                }
                else
                {
                    entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Insert(entityAddress);
                    entity.AddressID = BusinessLayer.GetAddressMaxID(ctx);
                }

                if (flagBillingAddress)
                {
                    entityBillingAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Update(entityBillingAddress);
                }
                else
                {
                    entityBillingAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Insert(entityBillingAddress);
                    entity.BillingAddressID = BusinessLayer.GetAddressMaxID(ctx);
                }

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entitySup.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityTagField.LastUpdatedBy = AppSession.UserLogin.UserID;

                if (String.IsNullOrEmpty(entity.CommCode))
                {
                    entity.CommCode = entity.BusinessPartnerCode;
                }

                entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityBillingAddress.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityAddressDao.Update(entityAddress);
                entityAddressDao.Update(entityBillingAddress);
                entitySupDao.Update(entitySup);
                entityTagFieldDao.Update(entityTagField);

                string fileName = "";
                UploadPhoto(entity.BusinessPartnerID, entity.BusinessPartnerCode, entity.BusinessPartnerName, ref fileName);

                entity.LogoFileName = fileName;

                entityDao.Update(entity);

                ctx.CommitTransaction();
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
            TagField tagField = BusinessLayer.GetTagField(Constant.BusinessObjectType.SUPPLIER);
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
    }
}