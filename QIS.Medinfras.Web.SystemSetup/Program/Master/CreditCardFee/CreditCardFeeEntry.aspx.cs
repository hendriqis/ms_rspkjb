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
    public partial class CreditCardFeeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.CREDIT_CARD_FEE;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                CreditCard entity = BusinessLayer.GetCreditCard(Convert.ToInt32(hdnID.Value));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            cboHealthcare.Focus();
        }

        protected override void SetControlProperties()
        {
            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(cboHealthcare, lstHealthcare, "HealthcareName", "HealthcareID");

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.CARD_TYPE, Constant.StandardCode.CARD_PROVIDER));
            Methods.SetComboBoxField<StandardCode>(cboCardType, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCardProvider, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_PROVIDER).ToList(), "StandardCodeName", "StandardCodeID");

            List<EDCMachine> lstEDCMachine = BusinessLayer.GetEDCMachineList("IsDeleted = 0");
            Methods.SetComboBoxField<EDCMachine>(cboEDCMachine, lstEDCMachine, "EDCMachineName", "EDCMachineID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboHealthcare, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCardType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCardProvider, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboEDCMachine, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCreditCardFee, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(CreditCard entity)
        {
            cboHealthcare.Value = entity.HealthcareID;
            cboCardType.Value = entity.GCCardType;
            cboCardProvider.Value = entity.GCCardProvider;
            cboEDCMachine.Value = entity.EDCMachineID.ToString();
            txtCreditCardFee.Text = entity.CreditCardFee.ToString();
        }

        private void ControlToEntity(CreditCard entity)
        {
            entity.HealthcareID = cboHealthcare.Value.ToString();
            entity.GCCardType = cboCardType.Value.ToString();
            entity.GCCardProvider = cboCardProvider.Value.ToString();
            entity.EDCMachineID = Convert.ToInt32(cboEDCMachine.Value);
            entity.CreditCardFee = Convert.ToDecimal(txtCreditCardFee.Text);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("EDCMachineID = {0} AND HealthcareID = '{1}' AND GCCardType = '{2}'", cboEDCMachine.Value, cboHealthcare.Value, cboCardType.Value);
            List<CreditCard> lst = BusinessLayer.GetCreditCardList(FilterExpression);

            if (lst.Count > 0)
                errMessage = string.Format("Credit Card Fee With Healthcare {0}, EDC Machine {1}, AND Card Type {2} is already exist!", cboHealthcare.Text, cboEDCMachine.Text, cboCardType.Text);

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            CreditCardDao entityDao = new CreditCardDao(ctx);
            bool result = false;
            try
            {
                CreditCard entity = new CreditCard();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetCreditCardMaxID(ctx).ToString();
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
                CreditCard entity = BusinessLayer.GetCreditCard(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCreditCard(entity);
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