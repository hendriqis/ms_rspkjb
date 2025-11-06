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
    public partial class CashFlowTypeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.CASH_FLOW_TYPE;
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
                vCashFlowType entity = BusinessLayer.GetvCashFlowTypeList(string.Format("CashFlowTypeID = {0}", hdnID.Value))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtCashFlowTypeCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCashFlowTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCashFlowTypeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCashFlowTypeParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCashFlowTypeParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTypeLevel, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(hdnCashFlowTypeParentID, new ControlEntrySetting(true, true));
        }

        private void EntityToControl(vCashFlowType entity)
        {
            txtCashFlowTypeCode.Text = entity.CashFlowTypeCode;
            txtCashFlowTypeName.Text = entity.CashFlowTypeName;
            hdnCashFlowTypeParentID.Value = entity.ParentTypeID.ToString();
            txtCashFlowTypeParentCode.Text = entity.CashFlowTypeParentCode;
            txtCashFlowTypeParentName.Text = entity.CashFlowTypeParentName;
            txtTypeLevel.Text = entity.TypeLevel.ToString();
            chkIsHeader.Checked = entity.IsHeader;
            hdnCashFlowTypeParentID.Value = entity.ParentTypeID.ToString();

        }
        private void ControlToEntity(CashFlowType entity)
        {
            entity.CashFlowTypeCode = txtCashFlowTypeCode.Text;
            entity.CashFlowTypeName = txtCashFlowTypeName.Text;
            if (hdnCashFlowTypeParentID.Value != "" && hdnCashFlowTypeParentID.Value != "0")
                entity.ParentTypeID = Convert.ToInt32(hdnCashFlowTypeParentID.Value);
            else
                entity.ParentTypeID = null;
            entity.IsHeader = chkIsHeader.Checked;
            entity.TypeLevel = Convert.ToInt16(txtTypeLevel.Text);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("CashFlowTypeCode = '{0}'", txtCashFlowTypeCode.Text);
            List<vCashFlowType> lst = BusinessLayer.GetvCashFlowTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Cash Flow Type With Code " + txtCashFlowTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("CashFlowTypeCode = '{0}' AND CashFlowTypeID != {1}", txtCashFlowTypeCode.Text, hdnID.Value);
            List<vCashFlowType> lst = BusinessLayer.GetvCashFlowTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Cash Flow Type With Code " + txtCashFlowTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            CashFlowTypeDao entityDao = new CashFlowTypeDao(ctx);
            bool result = false;
            try
            {
                CashFlowType entity = new CashFlowType();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetCashFlowTypeMaxID(ctx).ToString();
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
                CashFlowType entity = BusinessLayer.GetCashFlowType(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCashFlowType(entity);
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