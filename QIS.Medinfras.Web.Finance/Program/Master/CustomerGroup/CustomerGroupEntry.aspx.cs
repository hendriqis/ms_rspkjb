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
    public partial class CustomerGroupEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.CUSTOMER_GROUP;
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
                CustomerGroup entity = BusinessLayer.GetCustomerGroup(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtCustomerGroupCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCustomerGroupCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtCustomerGroupName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(CustomerGroup entity)
        {
            txtCustomerGroupCode.Text = entity.CustomerGroupCode;
            txtCustomerGroupName.Text = entity.CustomerGroupName;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(CustomerGroup entity)
        {
            entity.CustomerGroupName = txtCustomerGroupName.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("CustomerGroupCode = '{0}'", txtCustomerGroupCode.Text);
            List<CustomerGroup> lst = BusinessLayer.GetCustomerGroupList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Customer Group Code " + String.Format("{0}", txtCustomerGroupCode.Text) + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CustomerGroupDao customerGroupDao = new CustomerGroupDao(ctx);
            try
            {
                CustomerGroup entity = new CustomerGroup();
                ControlToEntity(entity);
                entity.CustomerGroupCode = Helper.GenerateCustomerGroupCode(ctx, entity.CustomerGroupName);
                entity.IsDeleted = false;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                customerGroupDao.Insert(entity);
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
            CustomerGroupDao customerGroupDao = new CustomerGroupDao(ctx);
            try
            {
                CustomerGroup entity = BusinessLayer.GetCustomerGroup(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                customerGroupDao.Update(entity);
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