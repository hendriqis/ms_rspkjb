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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class BillingGroupEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BILL_GROUP;
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
                BillingGroup entity = BusinessLayer.GetBillingGroup(Convert.ToInt32(ID));
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtBillingGroupCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'",Constant.StandardCode.BILLING_GROUP_DISPLAY_BY));
            lst.Insert(0,new StandardCode(){StandardCodeName = "", StandardCodeID = ""});
            Methods.SetComboBoxField<StandardCode>(cboPrintOption, lst, "StandardCodeName", "StandardCodeID");
            cboPrintOption.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtBillingGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBillingGroupName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBillingGroupName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrintOrder, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPrintOption, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(BillingGroup entity)
        {
            txtBillingGroupCode.Text = entity.BillingGroupCode;
            txtBillingGroupName1.Text = entity.BillingGroupName1;
            txtBillingGroupName2.Text = entity.BillingGroupName2;
            txtPrintOrder.Text = entity.PrintOrder.ToString();
            cboPrintOption.Value = entity.GCPrintOption;
        }

        private void ControlToEntity(BillingGroup entity)
        {
            entity.BillingGroupCode = txtBillingGroupCode.Text;
            entity.BillingGroupName1 = txtBillingGroupName1.Text;
            entity.BillingGroupName2 = txtBillingGroupName2.Text;
            entity.PrintOrder = string.IsNullOrEmpty(txtPrintOrder.Text) ? Convert.ToInt16(0) : Convert.ToInt16(txtPrintOrder.Text);
            if (cboPrintOption.Value != null && cboPrintOption.Value.ToString() != "")
                entity.GCPrintOption = cboPrintOption.Value.ToString();
            else
                entity.GCPrintOption = null;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("BillingGroupCode = '{0}'", txtBillingGroupCode.Text);
            List<BillingGroup> lst = BusinessLayer.GetBillingGroupList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Billing Group Code With Code " + txtBillingGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("BillingGroupCode = '{0}' AND BillingGroupID != {1}", txtBillingGroupCode.Text, hdnID.Value);
            List<BillingGroup> lst = BusinessLayer.GetBillingGroupList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " BillingGroup With Code " + txtBillingGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            BillingGroupDao entityDao = new BillingGroupDao(ctx);
            bool result = false;
            try
            {
                BillingGroup entity = new BillingGroup();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetBillingGroupMaxID(ctx).ToString();
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
                BillingGroup entity = BusinessLayer.GetBillingGroup(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateBillingGroup(entity);
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