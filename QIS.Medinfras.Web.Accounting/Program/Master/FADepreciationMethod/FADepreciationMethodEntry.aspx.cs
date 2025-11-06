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
    public partial class FADepreciationMethodEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_DEPRECIATION_METHOD;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                FADepreciationMethod entity = BusinessLayer.GetFADepreciationMethod(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtMethodCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMethodCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMethodName, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(FADepreciationMethod entity)
        {
            txtMethodCode.Text = entity.MethodCode;
            txtMethodName.Text = entity.MethodName;
        }

        private void ControlToEntity(FADepreciationMethod entity)
        {
            entity.MethodCode = txtMethodCode.Text;
            entity.MethodName = txtMethodName.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("MethodCode = '{0}'", txtMethodCode.Text);
            List<FADepreciationMethod> lst = BusinessLayer.GetFADepreciationMethodList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Method With Code " + txtMethodCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("MethodCode = '{0}' AND MethodID != {1}", txtMethodCode.Text, hdnID.Value);
            List<FADepreciationMethod> lst = BusinessLayer.GetFADepreciationMethodList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Method With Code " + txtMethodCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            FADepreciationMethodDao entityDao = new FADepreciationMethodDao(ctx);
            bool result = false;
            try
            {
                FADepreciationMethod entity = new FADepreciationMethod();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetFADepreciationMethodMaxID(ctx).ToString();
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
                FADepreciationMethod entity = BusinessLayer.GetFADepreciationMethod(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateFADepreciationMethod(entity);
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