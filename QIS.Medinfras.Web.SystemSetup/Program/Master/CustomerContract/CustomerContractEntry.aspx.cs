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
    public partial class CustomerContractEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.CUSTOMER_CONTRACT;
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            hdnCustomerID.Value = param[0];

            if (param.Length > 1)
            {
                IsAdd = false;
                hdnID.Value = param[1];
                CustomerContract entity = BusinessLayer.GetCustomerContract(Convert.ToInt32(param[1]));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
            }
            txtContractNo.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtContractNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEndDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsControlCoverageLimit, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(CustomerContract entity)
        {
            txtContractNo.Text = entity.ContractNo;
            if (entity.StartDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (entity.EndDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                txtEndDate.Text = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            chkIsControlCoverageLimit.Checked = entity.IsControlCoverageLimit;
            txtContractSummary.Text = entity.ContractSummary;
        }

        private void ControlToEntity(CustomerContract entity)
        {
            entity.ContractNo = txtContractNo.Text;
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.EndDate = Helper.GetDatePickerValue(txtEndDate);
            entity.IsControlCoverageLimit = chkIsControlCoverageLimit.Checked;
            entity.ContractSummary = Helper.GetHTMLEditorText(txtContractSummary);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ContractNo = '{0}'", txtContractNo.Text);
            List<CustomerContract> lst = BusinessLayer.GetCustomerContractList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Contract with No " + txtContractNo.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ContractNo = '{0}' AND ContractID != {1}", txtContractNo.Text, hdnID.Value);
            List<CustomerContract> lst = BusinessLayer.GetCustomerContractList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Contract with No " + txtContractNo.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            CustomerContractDao entityDao = new CustomerContractDao(ctx);
            bool result = false;
            try
            {
                CustomerContract entity = new CustomerContract();
                ControlToEntity(entity);
                entity.BusinessPartnerID = Convert.ToInt32(hdnCustomerID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetCustomerContractMaxID(ctx).ToString();
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
                CustomerContract entity = BusinessLayer.GetCustomerContract(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCustomerContract(entity);
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