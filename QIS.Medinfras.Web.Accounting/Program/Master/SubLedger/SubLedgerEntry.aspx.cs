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
    public partial class SubLedgerEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.SUB_LEDGER;
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
                vSubLedgerHd entity = BusinessLayer.GetvSubLedgerHdList(string.Format("SubLedgerID = {0}", ID))[0];
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtSubLedgerCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSubLedgerCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSubLedgerName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnSubLedgerTypeID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSubLedgerTypeName, new ControlEntrySetting(false, false, true));
        }

        private void EntityToControl(vSubLedgerHd entity)
        {
            txtSubLedgerCode.Text = entity.SubLedgerCode;
            txtSubLedgerName.Text = entity.SubLedgerName;
            hdnSubLedgerTypeID.Value = entity.SubLedgerTypeID.ToString();
            txtSubLedgerTypeCode.Text = entity.SubLedgerTypeCode;
            txtSubLedgerTypeName.Text = entity.SubLedgerTypeName;
        }

        private void ControlToEntity(SubLedgerHd entity)
        {
            entity.SubLedgerCode = txtSubLedgerCode.Text;
            entity.SubLedgerName = txtSubLedgerName.Text;
            entity.SubLedgerTypeID = Convert.ToInt32(hdnSubLedgerTypeID.Value);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("SubLedgerCode = '{0}'", txtSubLedgerCode.Text);
            List<SubLedgerHd> lst = BusinessLayer.GetSubLedgerHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Sub Ledger With Code " + txtSubLedgerCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("SubLedgerCode = '{0}' AND SubLedgerID != {1}", txtSubLedgerCode.Text, hdnID.Value);
            List<SubLedgerHd> lst = BusinessLayer.GetSubLedgerHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Sub Ledger With Code " + txtSubLedgerCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            SubLedgerHdDao entityDao = new SubLedgerHdDao(ctx);
            bool result = false;
            try
            {
                SubLedgerHd entity = new SubLedgerHd();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetSubLedgerHdMaxID(ctx).ToString();
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
                SubLedgerHd entity = BusinessLayer.GetSubLedgerHd(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSubLedgerHd(entity);
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