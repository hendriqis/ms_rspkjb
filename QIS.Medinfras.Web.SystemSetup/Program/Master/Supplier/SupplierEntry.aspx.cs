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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class SupplierEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.SUPPLIER;
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
                Supplier entitySup = BusinessLayer.GetSupplier(BusinessPartnerID);
                BusinessPartnerTagField entityTagField = BusinessLayer.GetBusinessPartnerTagField(BusinessPartnerID);
                vAddress entityAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entity.AddressID))[0];

                SetControlProperties();
                EntityToControl(entity, entitySup, entityAddress, entityTagField);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtSupplierCode.Focus();
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }
        #endregion

        protected override void SetControlProperties()
        {
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
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtContactPerson, new ControlEntrySetting(true, true, false));
            #endregion

            #region Customer Information
            SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtVATRegistrationNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTerm, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMaxPOAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLeadTime, new ControlEntrySetting(true, true, true));
            #endregion

            #region Customer Status
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
            SetControlEntrySetting(txtProvinceName, new ControlEntrySetting(false, false, false,healthcare.State));
            SetControlEntrySetting(hdnZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTelephoneNo, new ControlEntrySetting(true, true, false, defaultPhoneArea));
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

        private void EntityToControl(BusinessPartners entity, Supplier entitySup, vAddress entityAddress, BusinessPartnerTagField entityTagField)
        {
            #region General Information
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.BusinessPartnerName;
            txtShortName.Text = entity.ShortName;
            txtContactPerson.Text = entity.ContactPerson;
            #endregion

            #region Customer Information
            cboHealthcare.Value = entity.HealthcareID;
            txtVATRegistrationNo.Text = entity.VATRegistrationNo;
            cboTerm.Value = entity.TermID.ToString();
            txtMaxPOAmount.Text = entitySup.MaxPOAmount.ToString();
            txtLeadTime.Text = entitySup.LeadTime.ToString();
            #endregion

            #region Customer Status
            chkIsBlacklist.Checked = entity.IsBlackList;
            chkIsPaymentHold.Checked = entitySup.IsPaymentHold;
            chkIsTaxable.Checked = entity.IsTaxable;
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

        private void ControlToEntity(BusinessPartners entity, Supplier entitySup, Address entityAddress, BusinessPartnerTagField entityTagField)
        {
            #region General Information
            entity.BusinessPartnerCode = txtSupplierCode.Text;
            entity.BusinessPartnerName = txtSupplierName.Text;
            entity.ShortName = txtShortName.Text;
            entity.ContactPerson = txtContactPerson.Text;
            #endregion

            #region Customer Information
            if (cboHealthcare.Value != null && cboHealthcare.Value.ToString() != "")
                entity.HealthcareID = cboHealthcare.Value.ToString();
            else
                entity.HealthcareID = null;
            entity.VATRegistrationNo = txtVATRegistrationNo.Text;

            if (cboTerm.Value != null && cboTerm.Value.ToString() != "0")
                entity.TermID = Convert.ToInt32(cboTerm.Value);
            else
                entity.TermID = null;
            entitySup.MaxPOAmount = Convert.ToDecimal(txtMaxPOAmount.Text);
            entitySup.LeadTime = Convert.ToInt16(txtLeadTime.Text);
            #endregion

            #region Customer Status
            entity.IsBlackList = chkIsBlacklist.Checked;
            entitySup.IsPaymentHold = chkIsPaymentHold.Checked;
            entity.IsTaxable = chkIsTaxable.Checked;
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

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("BusinessPartnerCode = '{0}'", txtSupplierCode.Text);
            List<BusinessPartners> lst = BusinessLayer.GetBusinessPartnersList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Supplier with Code " + txtSupplierCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

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
                BusinessPartnerTagField entityTagField = new BusinessPartnerTagField();
                ControlToEntity(entity, entitySup, entityAddress, entityTagField);

                entity.GCBusinessPartnerType = Constant.BusinessObjectType.SUPPLIER;

                entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                entityAddressDao.Insert(entityAddress);
                entity.AddressID = BusinessLayer.GetAddressMaxID(ctx);

                entity.CreatedBy = AppSession.UserLogin.UserID;
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
                Address entityAddress = entityAddressDao.Get((int)entity.AddressID);

                ControlToEntity(entity, entitySup, entityAddress, entityTagField);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entitySup.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityTagField.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityAddressDao.Update(entityAddress);
                entitySupDao.Update(entitySup);
                entityTagFieldDao.Update(entityTagField);
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