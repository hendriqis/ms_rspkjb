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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class BankEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BANK;
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

                vBank entity = BusinessLayer.GetvBankList(string.Format("BankID = {0}", ID))[0];

                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtBankCode.Focus();

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
            List<Healthcare> lst = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lst, "HealthcareName", "HealthcareID");

            String filterExpression = string.Format("ParentID IN ('{0}', '{1}')", Constant.StandardCode.BANK_TYPE, Constant.StandardCode.VIRTUAL_PAYMENT_CHANNEL);
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(filterExpression);
            List<StandardCode> lstScItemUnit = lstSC.Where(w => w.ParentID == Constant.StandardCode.BANK_TYPE).ToList();
            lstScItemUnit.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCBankType, lstScItemUnit, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstSCPaymentChannel = lstSC.Where(w => w.ParentID == Constant.StandardCode.VIRTUAL_PAYMENT_CHANNEL).ToList();
            lstSCPaymentChannel.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboVirtualPaymentChannel, lstSCPaymentChannel, "StandardCodeName", "StandardCodeID");

        }

        protected override void OnControlEntrySetting()
        {
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string defaultGCState = healthcare.GCState == "" ? "" : healthcare.GCState.Split('^')[1];

            SetControlEntrySetting(txtBankCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBankName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBankNameDisplay, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBankAccountNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBankAccountName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCBankType, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRTData, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRWData, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCounty, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDistrict, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCity, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProvinceCode, new ControlEntrySetting(true, true, false, defaultGCState));
            SetControlEntrySetting(txtProvinceName, new ControlEntrySetting(false, false, false, healthcare.State));
            SetControlEntrySetting(hdnZipCode, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtZipCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsVirtualPayment, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vBank entity)
        {
            #region Bank
            txtBankCode.Text = entity.BankCode;
            txtBankName.Text = entity.BankName;
            txtBankAccountNo.Text = entity.BankAccountNo;
            txtBankAccountName.Text = entity.BankAccountName;
            txtBankNameDisplay.Text = entity.BankNameDisplay;
            cboHealthcare.Value = entity.HealthcareID;
            if (entity.GCBankType != null)
            {
                cboGCBankType.Value = entity.GCBankType;
            }
            chkIsVirtualPayment.Checked = entity.IsVirtualPayment;
            if (entity.IsVirtualPayment)
            {
                trVirtualPayment.Attributes.Remove("style");
            }
            if (!string.IsNullOrEmpty(entity.GCVirtualPaymentChannel))
            {
                cboVirtualPaymentChannel.Value = entity.GCVirtualPaymentChannel;
            }
            #endregion

            #region Bank Address
            txtAddress.Text = entity.StreetName;
            txtRTData.Text = entity.RT;
            txtRWData.Text = entity.RW;
            txtCounty.Text = entity.County; // Desa
            txtDistrict.Text = entity.District; //Kabupaten
            txtCity.Text = entity.City;
            if (entity.GCState != "")
                txtProvinceCode.Text = entity.GCState.Split('^')[1];
            else
                txtProvinceCode.Text = "";
            txtProvinceName.Text = entity.State;
            hdnZipCode.Value = entity.ZipCodeID.ToString();
            txtZipCode.Text = entity.ZipCode.ToString();
            #endregion
        }

        private void ControlToEntity(Bank entity, Address entityAddress)
        {
            #region Bank
            entity.BankCode = txtBankCode.Text;
            entity.BankName = txtBankName.Text;
            entity.BankNameDisplay = txtBankNameDisplay.Text;
            entity.BankAccountNo = txtBankAccountNo.Text;
            entity.BankAccountName = txtBankAccountName.Text;
            entity.HealthcareID = cboHealthcare.Value.ToString();
            if (cboGCBankType.Value == null)
            {
                entity.GCBankType = "";
            }
            else
            {
                entity.GCBankType = cboGCBankType.Value.ToString();
            }
            entity.IsVirtualPayment = chkIsVirtualPayment.Checked;
            if (cboVirtualPaymentChannel.Value != null)
            {
                entity.GCVirtualPaymentChannel = cboVirtualPaymentChannel.Value.ToString();
            }
            if (!chkIsVirtualPayment.Checked)
            {
                entity.GCVirtualPaymentChannel = null;
            }
            #endregion

            #region Bank Address
            entityAddress.StreetName = txtAddress.Text;
            entityAddress.RT = txtRTData.Text;
            entityAddress.RW = txtRWData.Text;
            entityAddress.County = txtCounty.Text; // Desa
            entityAddress.District = txtDistrict.Text; //Kabupaten
            entityAddress.City = txtCity.Text;
            entityAddress.GCState = txtProvinceCode.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCode.Text);
            if (hdnZipCode.Value == "" || hdnZipCode.Value == "0")
                entityAddress.ZipCode = null;
            else
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCode.Value);
            #endregion
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("BankCode = '{0}' AND IsDeleted = 0", txtBankCode.Text);
            List<Bank> lst = BusinessLayer.GetBankList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Bank With Code " + txtBankCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("BankCode = '{0}' AND BankID != {1} AND IsDeleted = 0", txtBankCode.Text, hdnID.Value);
            List<Bank> lst = BusinessLayer.GetBankList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Bank With Code " + txtBankCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BankDao entityDao = new BankDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            try
            {
                Bank entity = new Bank();
                Address entityAddress = new Address();
                ControlToEntity(entity, entityAddress);

                entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                entity.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);

                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

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
            BankDao entityDao = new BankDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            try
            {
                int BankID = Convert.ToInt32(hdnID.Value);
                Bank entity = entityDao.Get(BankID);
                if (entity.AddressID != null)
                {
                    Address entityAddress = entityAddressDao.Get((int)entity.AddressID);

                    ControlToEntity(entity, entityAddress);

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityAddressDao.Update(entityAddress);
                }
                else
                {
                    Address entityAddress = new Address();

                    ControlToEntity(entity, entityAddress);

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddress.CreatedBy = AppSession.UserLogin.UserID;

                    entity.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);
                }
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