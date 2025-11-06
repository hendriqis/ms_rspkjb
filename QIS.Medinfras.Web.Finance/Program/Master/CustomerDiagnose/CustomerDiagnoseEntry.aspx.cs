using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CustomerDiagnoseEntry : BasePageEntry
    {
        String filterExpressionBusinessPartner = "";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.CUSTOMERS_DIAGNOSE;
        }

        protected string OnGetCustomerFilterExpression()
        {
            return string.Format("GCBusinessPartnerType = '{0}' AND IsDeleted = 0 ", Constant.BusinessObjectType.CUSTOMER);
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
                CustomerDiagnoseHd entity = BusinessLayer.GetCustomerDiagnoseHd(Convert.ToInt32(ID));
                BusinessPartners entityCus = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = {0}", entity.BusinessPartnerID))[0];
                EntityToControl(entity, entityCus);
            }
            else
            {
                IsAdd = true;
            }
            txtCustomerDiagnoseNo.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCustomerDiagnoseNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCustomerDiagnoseName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(CustomerDiagnoseHd entity, BusinessPartners entityCus)
        {
            txtCustomerDiagnoseNo.Text = entity.CustomerDiagnoseNo;
            txtCustomerDiagnoseName.Text = entity.CustomerDiagnoseName;
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            txtBusinessPartnerCode.Text = entityCus.BusinessPartnerCode;
            txtBusinesPartnerName.Text = entityCus.BusinessPartnerName;
           
        }

        private void ControlToEntity(CustomerDiagnoseHd entity)
        {
            entity.CustomerDiagnoseNo = txtCustomerDiagnoseNo.Text;
            entity.CustomerDiagnoseName = txtCustomerDiagnoseName.Text;
            entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);


        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("CustomerDiagnoseNo = '{0}'", txtCustomerDiagnoseNo.Text);
            List<CustomerDiagnoseHd> lst = BusinessLayer.GetCustomerDiagnoseHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Customer Diagnose No " + String.Format("{0}", txtCustomerDiagnoseNo.Text) + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CustomerDiagnoseHdDao customerDiagnoseHdDao = new CustomerDiagnoseHdDao(ctx);
            try
            {
                CustomerDiagnoseHd entity = new CustomerDiagnoseHd();
                ControlToEntity(entity);
                //entity.CustomerDiagnoseNo = Helper.GenerateCustomerGroupCode(ctx, entity.CustomerDiagnoseName);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                customerDiagnoseHdDao.Insert(entity);
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
            CustomerDiagnoseHdDao customerDiagnoseHdDao = new CustomerDiagnoseHdDao(ctx);
            try
            {
                CustomerDiagnoseHd entity = BusinessLayer.GetCustomerDiagnoseHd(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                customerDiagnoseHdDao.Update(entity);
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