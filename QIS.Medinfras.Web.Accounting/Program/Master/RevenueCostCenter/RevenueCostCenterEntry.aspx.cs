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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class RevenueCostCenterEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.REVENUE_COST_CENTER;
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
                vRevenueCostCenter entity = BusinessLayer.GetvRevenueCostCenterList(string.Format("RevenueCostCenterID = {0}", hdnID.Value))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtRevenueCostCenterCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtRevenueCostCenterCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRevenueCostCenterName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRevenueCostCenterParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRevenueCostCenterParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnRevenueCostCenterParentID, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vRevenueCostCenter entity)
        {
            txtRevenueCostCenterCode.Text = entity.RevenueCostCenterCode;
            txtRevenueCostCenterName.Text = entity.RevenueCostCenterName;
            hdnRevenueCostCenterParentID.Value = entity.RevenueCostCenterParentID.ToString();
            txtRevenueCostCenterParentCode.Text = entity.RevenueCostCenterParentCode;
            txtRevenueCostCenterParentName.Text = entity.RevenueCostCenterParentName;
            hdnRevenueCostCenterParentID.Value = entity.RevenueCostCenterParentID.ToString();
        }

        private void ControlToEntity(RevenueCostCenter entity)
        {
            entity.RevenueCostCenterCode = txtRevenueCostCenterCode.Text;
            entity.RevenueCostCenterName = txtRevenueCostCenterName.Text;
               //  entity.RevenueCostCenterParentID = Convert.ToInt32(hdnRevenueCostCenterParentID.Value);
            if (hdnRevenueCostCenterParentID.Value == "0" || hdnRevenueCostCenterParentID.Value == "")
                entity.RevenueCostCenterParentID = null;
            else
                entity.RevenueCostCenterParentID = Convert.ToInt32(hdnRevenueCostCenterParentID.Value);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("RevenueCostCenterCode = '{0}' AND IsDeleted = 0", txtRevenueCostCenterCode.Text);
            List<vRevenueCostCenter> lst = BusinessLayer.GetvRevenueCostCenterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Revenue Cost Center With Code " + txtRevenueCostCenterCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("RevenueCostCenterCode = '{0}' AND RevenueCostCenterID != {1} AND IsDeleted = 0", txtRevenueCostCenterCode.Text, hdnID.Value);
            List<vRevenueCostCenter> lst = BusinessLayer.GetvRevenueCostCenterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Revenue Cost Center With Code " + txtRevenueCostCenterCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RevenueCostCenterDao entityDao = new RevenueCostCenterDao(ctx);
            bool result = false;
            try
            {
                RevenueCostCenter entity = new RevenueCostCenter();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetRevenueCostCenterMaxID(ctx).ToString();
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
                RevenueCostCenter entity = BusinessLayer.GetRevenueCostCenter(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateRevenueCostCenter(entity);
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