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
    public partial class BankEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.BANK;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                Bank entity = BusinessLayer.GetBank(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtBankCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<Healthcare> lst = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lst, "HealthcareName", "HealthcareID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtBankCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBankName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBankAccountNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBankAccountName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Bank entity)
        {
            txtBankCode.Text = entity.BankCode;
            txtBankName.Text = entity.BankName;
            txtBankAccountNo.Text = entity.BankAccountNo;
            txtBankAccountName.Text = entity.BankAccountName;
            cboHealthcare.Value = entity.HealthcareID;
        }

        private void ControlToEntity(Bank entity)
        {
            entity.BankCode = txtBankCode.Text;
            entity.BankName = txtBankName.Text;
            entity.BankAccountNo = txtBankAccountNo.Text;
            entity.BankAccountName = txtBankAccountName.Text;
            entity.HealthcareID = cboHealthcare.Value.ToString();
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("BankCode = '{0}'", txtBankCode.Text);
            List<Bank> lst = BusinessLayer.GetBankList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Bank With Code " + txtBankCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("BankCode = '{0}' AND BankID != {1}", txtBankCode.Text, hdnID.Value);
            List<Bank> lst = BusinessLayer.GetBankList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Bank With Code " + txtBankCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            BankDao entityDao = new BankDao(ctx);
            bool result = false;
            try
            {
                Bank entity = new Bank();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetBankMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
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
            try
            {
                Bank entity = BusinessLayer.GetBank(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateBank(entity);
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