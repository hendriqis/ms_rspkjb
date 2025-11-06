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
    public partial class CustomerContractEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.CUSTOMER_CONTRACT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            hdnCustomerID.Value = param[0];

            if (param.Length > 1)
            {
                IsAdd = false;
                hdnID.Value = param[1];
                SetControlProperties();
                CustomerContract entity = BusinessLayer.GetCustomerContract(Convert.ToInt32(param[1]));
                EntityToControl(entity);
            }
            else
            {
                hdnID.Value = "";
                SetControlProperties();
                IsAdd = true;
            }
            txtContractNo.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0 AND IsUsedInChargeClass = 1");
            lstClassCare.Insert(0,new ClassCare() { ClassID = 0, ClassCode = "0", ClassName = string.Empty });
            Methods.SetComboBoxField<ClassCare>(cboClassCare, lstClassCare, "ClassName", "ClassID");
            cboClassCare.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtContractNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEndDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboClassCare, new ControlEntrySetting(true, true, true));
            //SetControlEntrySetting(chkIsControlCoverageLimit, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(chkIsControlUnitPrice, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtServiceUnitPrice, new ControlEntrySetting(true, true, true, "0.00"));
            SetControlEntrySetting(txtDrugSuppliesUnitPrice, new ControlEntrySetting(true, true, true, "0.00"));
            SetControlEntrySetting(txtLogisticUnitPrice, new ControlEntrySetting(true, true, true, "0.00"));

            #region Administration & Service
            //SetControlEntrySetting(chkAdministrationPercentage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAdministrationAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMinAdministrationAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMaxAdministrationAmount, new ControlEntrySetting(true, true, true, 0));

            SetControlEntrySetting(txtPatientAdmAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMinPatientAdmAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMaxPatientAdmAmount, new ControlEntrySetting(true, true, true, 0));

            //SetControlEntrySetting(chkServiceChargePercentage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtServiceChargeAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMinServiceChargeAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMaxServiceChargeAmount, new ControlEntrySetting(true, true, true, 0));

            SetControlEntrySetting(txtPatientServiceAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMinPatientServiceAmount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtMaxPatientServiceAmount, new ControlEntrySetting(true, true, true, 0));

            #endregion
        
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
            chkIsGenericDrugOnly.Checked = entity.IsGenericDrugOnly;
            chkIsControlClassCare.Checked = entity.IsControlClassCare;
            if (entity.IsControlClassCare)
                if (entity.ControlClassID != null)
                    cboClassCare.Value = entity.ControlClassID.ToString();

            chkIsControlUnitPrice.Checked = entity.IsControlUnitPrice;
            txtServiceUnitPrice.Text = entity.ServiceUnitPrice.ToString();
            txtDrugSuppliesUnitPrice.Text = entity.DrugSuppliesUnitPrice.ToString();
            txtLogisticUnitPrice.Text = entity.LogisticUnitPrice.ToString();

            #region Administration & Service
            chkAdministrationPercentage.Checked = entity.IsAdministrationFeeInPct;
            txtAdministrationAmount.Text = entity.AdministrationFeeAmount.ToString();
            txtMinAdministrationAmount.Text = entity.MinAdministrationFeeAmount.ToString();
            txtMaxAdministrationAmount.Text = entity.MaxAdministrationFeeAmount.ToString();

            chkPatientAdminPercentage.Checked = entity.IsPatientAdmFeeInPct;
            txtPatientAdmAmount.Text = entity.PatientAdmFeeAmount.ToString();
            txtMinPatientAdmAmount.Text = entity.MinPatientAdmFeeAmount.ToString();
            txtMaxPatientAdmAmount.Text = entity.MaxPatientAdmFeeAmount.ToString();

            chkServiceChargePercentage.Checked = entity.IsServiceFeeInPct;
            txtServiceChargeAmount.Text = entity.ServiceFeeAmount.ToString();
            txtMinServiceChargeAmount.Text = entity.MinServiceFeeAmount.ToString();
            txtMaxServiceChargeAmount.Text = entity.MaxServiceFeeAmount.ToString();

            chkPatientServicePercentage.Checked = entity.IsPatientServiceFeeInPct;
            txtPatientServiceAmount.Text = entity.PatientServiceFeeAmount.ToString();
            txtMinPatientServiceAmount.Text = entity.MinPatientServiceFeeAmount.ToString();
            txtMaxPatientServiceAmount.Text = entity.MaxPatientServiceFeeAmount.ToString();

            #endregion
        }

        private void ControlToEntity(CustomerContract entity)
        {
            entity.ContractNo = txtContractNo.Text;
            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.EndDate = Helper.GetDatePickerValue(txtEndDate);
            entity.IsControlCoverageLimit = chkIsControlCoverageLimit.Checked;
            entity.IsControlClassCare = chkIsControlClassCare.Checked;
            entity.IsGenericDrugOnly = chkIsGenericDrugOnly.Checked;
            if (entity.IsControlClassCare)
                if (!string.IsNullOrEmpty(cboClassCare.Text))
                    entity.ControlClassID = Convert.ToInt32(cboClassCare.Value);
            else
                entity.ControlClassID = null;
            entity.ContractSummary = Helper.GetHTMLEditorText(txtContractSummary);

            entity.IsControlUnitPrice = chkIsControlUnitPrice.Checked;
            entity.ServiceUnitPrice = Convert.ToDecimal(txtServiceUnitPrice.Text);
            entity.DrugSuppliesUnitPrice = Convert.ToDecimal(txtDrugSuppliesUnitPrice.Text);
            entity.LogisticUnitPrice = Convert.ToDecimal(txtLogisticUnitPrice.Text);

            #region Administration & Service
            entity.IsAdministrationFeeInPct = chkAdministrationPercentage.Checked;
            entity.AdministrationFeeAmount = Convert.ToDecimal(txtAdministrationAmount.Text);
            entity.MinAdministrationFeeAmount = Convert.ToDecimal(txtMinAdministrationAmount.Text);
            entity.MaxAdministrationFeeAmount = Convert.ToDecimal(txtMaxAdministrationAmount.Text);

            entity.IsPatientAdmFeeInPct = chkPatientAdminPercentage.Checked;
            entity.PatientAdmFeeAmount = Convert.ToDecimal(txtPatientAdmAmount.Text);
            entity.MinPatientAdmFeeAmount = Convert.ToDecimal(txtMinPatientAdmAmount.Text);
            entity.MaxPatientAdmFeeAmount = Convert.ToDecimal(txtMaxPatientAdmAmount.Text);

            entity.IsServiceFeeInPct = chkServiceChargePercentage.Checked;
            entity.ServiceFeeAmount = Convert.ToDecimal(txtServiceChargeAmount.Text);
            entity.MinServiceFeeAmount = Convert.ToDecimal(txtMinServiceChargeAmount.Text);
            entity.MaxServiceFeeAmount = Convert.ToDecimal(txtMaxServiceChargeAmount.Text);

            entity.IsPatientServiceFeeInPct = chkPatientServicePercentage.Checked;
            entity.PatientServiceFeeAmount = Convert.ToDecimal(txtPatientServiceAmount.Text);
            entity.MinPatientServiceFeeAmount = Convert.ToDecimal(txtMinPatientServiceAmount.Text);
            entity.MaxPatientServiceFeeAmount = Convert.ToDecimal(txtMaxPatientServiceAmount.Text);
            #endregion
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ContractNo = '{0}' AND IsDeleted = 0", txtContractNo.Text);
            List<CustomerContract> lst = BusinessLayer.GetCustomerContractList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Contract with No " + txtContractNo.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ContractNo = '{0}' AND IsDeleted = 0 AND ContractID != {1}", txtContractNo.Text, hdnID.Value);
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
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}