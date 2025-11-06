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
    public partial class CoverageTypeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.COVERAGE_TYPE;
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
                CoverageType entity = BusinessLayer.GetCoverageType(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtCoverageTypeCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCoverageTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCoverageTypeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsMarkupMarginReplaceDefault, new ControlEntrySetting(true, true, false,false));
        }

        private void EntityToControl(CoverageType entity)
        {
            txtCoverageTypeCode.Text = entity.CoverageTypeCode;
            txtCoverageTypeName.Text = entity.CoverageTypeName;
            chkIsMarkupMarginReplaceDefault.Checked = entity.IsMarkupMarginReplaceDefault;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(CoverageType entity)
        {
            entity.CoverageTypeCode = txtCoverageTypeCode.Text;
            entity.CoverageTypeName = txtCoverageTypeName.Text;
            entity.IsMarkupMarginReplaceDefault = chkIsMarkupMarginReplaceDefault.Checked;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("CoverageTypeCode = '{0}'", txtCoverageTypeCode.Text);
            List<CoverageType> lst = BusinessLayer.GetCoverageTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Coverage Type with Code " + txtCoverageTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("CoverageTypeCode = '{0}' AND CoverageTypeID != {1}", txtCoverageTypeCode.Text, ID);
            List<CoverageType> lst = BusinessLayer.GetCoverageTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Coverage Type with Code " + txtCoverageTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            CoverageTypeDao entityDao = new CoverageTypeDao(ctx);
            bool result = false;
            try
            {
                CoverageType entity = new CoverageType();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetCoverageTypeMaxID(ctx).ToString();
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
                CoverageType entity = BusinessLayer.GetCoverageType(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCoverageType(entity);
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