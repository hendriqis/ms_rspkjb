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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CustomerEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.CUSTOMER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID > 1 AND GCBusinessPartnerType = '{0}' AND IsDeleted = 0", Constant.BusinessObjectType.CUSTOMER);
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                int BusinessPartnerID = Convert.ToInt32(ID);
                BusinessPartners entity = BusinessLayer.GetBusinessPartners(BusinessPartnerID);
                vCustomer entityCus = BusinessLayer.GetvCustomerList(string.Format("BusinessPartnerID = {0}", BusinessPartnerID))[0];
                BusinessPartnerTagField entityTagField = BusinessLayer.GetBusinessPartnerTagField(BusinessPartnerID);
                vAddress entityAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entity.AddressID))[0];

                if (entityCus.CustomerLineID != 0 || entityCus.CustomerLineID != null)
                {
                    hdnOldCustomerLineID.Value = entityCus.CustomerLineID.ToString();
                }

                SetControlProperties();

                if (entity.BillingAddressID != null)
                {
                    vAddress entityBillingAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entity.BillingAddressID))[0];
                    EntityToControl(entity, entityCus, entityAddress, entityBillingAddress, entityTagField);
                }
                else
                {
                    vAddress entityBillingAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entity.AddressID))[0];
                    EntityToControl(entity, entityCus, entityAddress, entityBillingAddress, entityTagField);
                }
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
                cboHealthcare.Value = AppSession.UserLogin.HealthcareID;
            }

            txtCustomerName.Focus();

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
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND StandardCodeID != '{3}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.CUSTOMER_TYPE, Constant.StandardCode.TARIFF_SCHEME, Constant.StandardCode.RL_REPORT_GROUP_3_19, Constant.CustomerType.PERSONAL));
            Methods.SetComboBoxField<StandardCode>(cboCustomerType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTariffScheme, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.TARIFF_SCHEME).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            
            List<StandardCode> lstGroupRL = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.RL_REPORT_GROUP_3_19));
            lstGroupRL.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboRLReportGroup, lstGroupRL, "StandardCodeName", "StandardCodeID");
            
            List<Term> listTerm = BusinessLayer.GetTermList("IsDeleted = 0");
            listTerm.Insert(0, new Term { TermID = 0, TermName = "" });
            Methods.SetComboBoxField<Term>(cboTerm, listTerm, "TermName", "TermID");

            List<Healthcare> listHealthcare = BusinessLayer.GetHealthcareList("");
            listHealthcare.Insert(0, new Healthcare { HealthcareID = "", HealthcareName = "" });
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, listHealthcare, "HealthcareName", "HealthcareID");
        }

        protected override void OnControlEntrySetting()
        {
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string defaultGCState = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];
            string defaultPhoneArea = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PHONE_AREA).ParameterValue;

            #region General Information
            SetControlEntrySetting(txtCustomerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtExternalCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCustomerName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, false));
            #endregion

            #region Contact Person
            SetControlEntrySetting(txtContactPersonName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtContactPersonPhone, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtContactPersonEmail, new ControlEntrySetting(true, true, false));
            #endregion

            #region Customer Information
            SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtVATRegistrationNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTerm, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboCustomerType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboTariffScheme, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboRLReportGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnParamedicID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParamedicName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnCustomerBillToID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCustomerBillToCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCustomerBillToName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnCustomerGroupID,new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCustomerGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCustomerGroupName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnCustomerLineID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCustomerLineCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCustomerLineName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtGLAccountSegmentNo, new ControlEntrySetting(true, false, false));
            #endregion

            #region Customer Status
            SetControlEntrySetting(chkIsBlacklist, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsActive, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotActiveReason, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsCreditHold, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsDummy, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasContract, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsTaxable, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUsingDunningLetter, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsControlMember, new ControlEntrySetting(true, true, false));
            #endregion

            #region Address
            SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProvinceCode, new ControlEntrySetting(true, true, false,defaultGCState));
            SetControlEntrySetting(txtProvinceName, new ControlEntrySetting(false, false, false,healthcare.State));
            SetControlEntrySetting(hdnZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTelephoneNo, new ControlEntrySetting(true, true, false,defaultPhoneArea));
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
            #endregion

            #region Bank Information
            SetControlEntrySetting(txtNamaBank, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNoAccountBank, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNoAccountVirtualBank, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNamaAccountBank, new ControlEntrySetting(true, true, false));
            #endregion

            #region Other Information
            SetControlEntrySetting(txtReceiptName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEKlaimPayorID, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEKlaimPayorCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEKlaimCOBCode, new ControlEntrySetting(true, true, false));
            #endregion

            #region Late Charge
            SetControlEntrySetting(chkIsLateChargeInPercentage, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtLateChargeInPercentage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLateChargeInAmount, new ControlEntrySetting(true, true, false, "0.00"));
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

        private void EntityToControl(BusinessPartners entity, vCustomer entityCus, vAddress entityAddress, vAddress entityBillingAddress, BusinessPartnerTagField entityTagField)
        {
            #region General Information
            txtCustomerCode.Text = entity.BusinessPartnerCode;
            txtExternalCode.Text = entity.CommCode;
            txtCustomerName.Text = entity.BusinessPartnerName;
            txtShortName.Text = entity.ShortName;
            #endregion

            #region Contact Person
            txtContactPersonName.Text = entity.ContactPerson;
            txtContactPersonPhone.Text = entity.ContactPersonMobileNo;
            txtContactPersonEmail.Text = entity.ContactPersonEmail;
            #endregion

            #region Customer Information
            cboHealthcare.Value = entity.HealthcareID;
            txtVATRegistrationNo.Text = entity.VATRegistrationNo;
            cboTerm.Value = entity.TermID.ToString();
            cboCustomerType.Value = entityCus.GCCustomerType;
            cboTariffScheme.Value = entityCus.GCTariffScheme;
            cboRLReportGroup.Value = entityCus.GCRLReportGroup;
            hdnParamedicID.Value = entityCus.ParamedicID.ToString();
            txtParamedicCode.Text = entityCus.ParamedicCode;
            txtParamedicName.Text = entityCus.ParamedicName;
            hdnCustomerBillToID.Value = entityCus.CustomerBillTo.ToString();
            txtCustomerBillToCode.Text = entityCus.CustomerBillToCode;
            txtCustomerBillToName.Text = entityCus.CustomerBillToName;
            if (entityCus.CustomerGroupID != null)
            {
                hdnCustomerGroupID.Value = entityCus.CustomerGroupID.ToString();
                txtCustomerGroupCode.Text = entityCus.CustomerGroupCode;
                txtCustomerGroupName.Text = entityCus.CustomerGroupName;
            }
            if (entityCus.CustomerLineID != null)
            {
                hdnCustomerLineID.Value = entityCus.CustomerLineID.ToString();
                txtCustomerLineCode.Text = entityCus.CustomerLineCode;
                txtCustomerLineName.Text = entityCus.CustomerLineName;
            }
            txtGLAccountSegmentNo.Text = entity.GLAccountNoSegment;
            
            #endregion

            #region Customer Status
            chkIsBlacklist.Checked = entity.IsBlackList;
            chkIsActive.Checked = entity.IsActive;
            txtNotActiveReason.Text = entity.NotActiveReason;
            chkIsCreditHold.Checked = entityCus.IsCreditHold;
            chkIsDummy.Checked = entityCus.IsDummy;
            chkIsHasContract.Checked = entityCus.IsHasContract;
            chkIsTaxable.Checked = entity.IsTaxable;
            chkIsUsingDunningLetter.Checked = entityCus.IsUsingDunningLetter;
            chkIsControlMember.Checked = entityCus.IsControlMember;
            #endregion

            #region Logo Perusahaan
            txtFileName.Text = entity.LogoFileName;
            //            string logoPath = string.Format("BusinessPartner/{0}_{1}/", entity.BusinessPartnerCode, entity.BusinessPartnerName);
            string logoPath = string.Format("BusinessPartner/{0}/", entity.BusinessPartnerCode);
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
            #endregion

            #region Bank Information
            txtNamaBank.Text = entity.BankName;
            txtNoAccountBank.Text = entity.BankAccountNo;
            txtNoAccountVirtualBank.Text = entity.VirtualAccountNo;
            txtNamaAccountBank.Text = entity.BankAccountName;
            #endregion

            #region Other Information
            txtReceiptName.Text = entity.ReceiptName;
            if (entityCus.EKlaimPayorID != 0)
            {
                txtEKlaimPayorID.Text = entityCus.EKlaimPayorID.ToString();
            }
            txtEKlaimPayorCode.Text = entityCus.EKlaimPayorCode;
            txtEKlaimCOBCode.Text = entityCus.EKlaimCOBCode;
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

            #region Late Charge
            chkIsLateChargeInPercentage.Checked = entity.IsLateChargeInPercentage;
            txtLateChargeInPercentage.Text = entity.LateChargePercentage.ToString();
            txtLateChargeInAmount.Text = entity.LateChargeAmount.ToString();
            #endregion
        }
        
        private void ControlToEntity(BusinessPartners entity, Customer entityCus, Address entityAddress, Address entityBillingAddress, BusinessPartnerTagField entityTagField)
        {
            #region General Information
            entity.BusinessPartnerCode = txtCustomerCode.Text;
            entity.CommCode = txtExternalCode.Text;
            entity.BusinessPartnerName = txtCustomerName.Text;
            entity.ShortName = txtShortName.Text;
            #endregion

            #region Contact Person
            entity.ContactPerson = txtContactPersonName.Text;
            entity.ContactPersonMobileNo = txtContactPersonPhone.Text;
            entity.ContactPersonEmail = txtContactPersonEmail.Text;
            #endregion

            #region Customer Information
            if (cboHealthcare.Value != null)
            {
                entity.HealthcareID = cboHealthcare.Value.ToString();
            }
            else
            {
                entity.HealthcareID = "";
            }
            entity.VATRegistrationNo = txtVATRegistrationNo.Text;
            if (cboTerm.Value != null && cboTerm.Value != "")
            {
                entity.TermID = Convert.ToInt32(cboTerm.Value);
            }
            else
            {
                entity.TermID = null;
            }
            if (cboRLReportGroup.Value != null && cboRLReportGroup.Value != "")
            {
                entityCus.GCRLReportGroup = cboRLReportGroup.Value.ToString();
            }
            else
            {
                entityCus.GCRLReportGroup = null;
            }
            entityCus.GCCustomerType = cboCustomerType.Value.ToString();
            entityCus.GCTariffScheme = cboTariffScheme.Value.ToString();
            if (hdnParamedicID.Value == "" || hdnParamedicID.Value == "0")
            {
                entity.ParamedicID = null;
            }
            else
            {
                entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            }
            if (hdnCustomerBillToID.Value == "" || hdnCustomerBillToID.Value == "0")
            {
                entityCus.CustomerBillTo = null;
            }
            else
            {
                entityCus.CustomerBillTo = Convert.ToInt32(hdnCustomerBillToID.Value);
            }
            if (hdnCustomerLineID.Value == "" || hdnCustomerLineID.Value == "0")
            {
                entityCus.CustomerLineID = null;
            }
            else
            {
                entityCus.CustomerLineID = Convert.ToInt32(hdnCustomerLineID.Value);
            }
            entity.GLAccountNoSegment = txtGLAccountSegmentNo.Text;
            if (hdnCustomerGroupID.Value == "" || hdnCustomerGroupID.Value == "0")
            {
                entityCus.CustomerGroupID = null;
            }
            else
            {
                entityCus.CustomerGroupID = Convert.ToInt32(hdnCustomerGroupID.Value);
            }
            #endregion

            #region Customer Status
            entity.IsBlackList = chkIsBlacklist.Checked;
            entity.IsActive = chkIsActive.Checked;
            if (entity.IsActive == false)
            {
                entity.NotActiveReason = txtNotActiveReason.Text;
                entity.NotActiveBy = AppSession.UserLogin.UserID;
                entity.NotActiveDate = DateTime.Now;
            }
            
            entityCus.IsCreditHold = chkIsCreditHold.Checked;
            entityCus.IsDummy = chkIsDummy.Checked;
            entityCus.IsHasContract = chkIsHasContract.Checked;
            entity.IsTaxable = chkIsTaxable.Checked;
            entityCus.IsUsingDunningLetter = chkIsUsingDunningLetter.Checked;
            entityCus.IsControlMember = chkIsControlMember.Checked;
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
            #endregion

            #region Bank Information
            entity.BankName = txtNamaBank.Text;
            entity.BankAccountNo = txtNoAccountBank.Text;
            entity.VirtualAccountNo = txtNoAccountVirtualBank.Text;
            entity.BankAccountName = txtNamaAccountBank.Text;
            #endregion

            #region Other Information
            entity.ReceiptName = txtReceiptName.Text;
            if (!string.IsNullOrEmpty(txtEKlaimPayorID.Text))
            {
                entityCus.EKlaimPayorID = Convert.ToInt32(txtEKlaimPayorID.Text);
            }
            entityCus.EKlaimPayorCode = txtEKlaimPayorCode.Text;
            entityCus.EKlaimCOBCode = txtEKlaimCOBCode.Text;
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

            #region Late Charge
            entity.IsLateChargeInPercentage = chkIsLateChargeInPercentage.Checked;
            entity.LateChargePercentage = Convert.ToDecimal(txtLateChargeInPercentage.Text);
            entity.LateChargeAmount = Convert.ToDecimal(txtLateChargeInAmount.Text);
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

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("BusinessPartnerCode = '{0}' AND BusinessPartnerID != {1}", txtCustomerCode.Text, hdnID.Value);
            List<BusinessPartners> lst = BusinessLayer.GetBusinessPartnersList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Customer with Code " + txtCustomerCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BusinessPartnersDao entityDao = new BusinessPartnersDao(ctx);
            CustomerDao entityCusDao = new CustomerDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            BusinessPartnerTagFieldDao entityTagFieldDao = new BusinessPartnerTagFieldDao(ctx);
            try
            {
                BusinessPartners entity = new BusinessPartners();
                Customer entityCus = new Customer();
                Address entityAddress = new Address();
                Address entityBillingAddress = new Address();
                BusinessPartnerTagField entityTagField = new BusinessPartnerTagField();
                ControlToEntity(entity, entityCus, entityAddress, entityBillingAddress, entityTagField);
                entity.BusinessPartnerCode = Helper.GeneratePartnerCode(ctx, entity.BusinessPartnerName);

                entity.GCBusinessPartnerType = Constant.BusinessObjectType.CUSTOMER;

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
                if (entity.IsActive == false)
                {
                    entity.NotActiveReason = txtNotActiveReason.Text;
                    entity.NotActiveBy = AppSession.UserLogin.UserID;
                    entity.NotActiveDate = DateTime.Now;
                }
            

                string fileName = "";
                UploadPhoto(entity.BusinessPartnerID, entity.BusinessPartnerCode, entity.BusinessPartnerName, ref fileName);

                entity.LogoFileName = fileName;

                entity.BusinessPartnerID = entityDao.InsertReturnPrimaryKeyID(entity);
                ;
                entityCus.BusinessPartnerID = entity.BusinessPartnerID;
                entityCusDao.Insert(entityCus);

                entityTagField.BusinessPartnerID = entity.BusinessPartnerID;
                entityTagFieldDao.Insert(entityTagField);

                retval = entity.BusinessPartnerID.ToString();

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
            BusinessPartnersDao entityDao = new BusinessPartnersDao(ctx);
            CustomerDao entityCusDao = new CustomerDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            BusinessPartnerTagFieldDao entityTagFieldDao = new BusinessPartnerTagFieldDao(ctx);
            try
            {
                int BusinessPartnerID = Convert.ToInt32(hdnID.Value);
                BusinessPartners entity = entityDao.Get(BusinessPartnerID);
                Customer entityCus = entityCusDao.Get(BusinessPartnerID);
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

                ControlToEntity(entity, entityCus, entityAddress, entityBillingAddress, entityTagField);

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

                string FilterExpressionReg = string.Format("(BusinessPartnerID IN (SELECT BusinessPartnerID FROM Customer WHERE CustomerLineID = {0}) AND BusinessPartnerID = {1}) AND GCRegistrationStatus != '{2}'", hdnOldCustomerLineID.Value, hdnID.Value, Constant.VisitStatus.CANCELLED);
                List<Registration> lstReg = BusinessLayer.GetRegistrationList(FilterExpressionReg);

                if (lstReg.Count() > 0)
                {
                    if (hdnCustomerLineID.Value != hdnOldCustomerLineID.Value)
                    {
                        result = false;
                    }
                }

                if (result)
                {
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityCus.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityTagField.LastUpdatedBy = AppSession.UserLogin.UserID;

                    if (String.IsNullOrEmpty(entity.CommCode))
                    {
                        entity.CommCode = entity.BusinessPartnerCode;
                    }
                    if (entity.IsActive == false)
                    {
                        entity.NotActiveReason = txtNotActiveReason.Text;
                        entity.NotActiveBy = AppSession.UserLogin.UserID;
                        entity.NotActiveDate = DateTime.Now;
                    }

                    string fileName = "";
                    UploadPhoto(entity.BusinessPartnerID, entity.BusinessPartnerCode, entity.BusinessPartnerName, ref fileName);

                    entity.LogoFileName = fileName;

                    entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityBillingAddress.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityAddressDao.Update(entityAddress);
                    entityAddressDao.Update(entityBillingAddress);
                    entityCusDao.Update(entityCus);
                    entityTagFieldDao.Update(entityTagField);
                    entityDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Tidak bisa diubah karena masih ada Registrasi di Penjamin berdasarkan CustomerLine tersebut.";
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
            TagField tagField = BusinessLayer.GetTagField(Constant.BusinessObjectType.CUSTOMER);
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