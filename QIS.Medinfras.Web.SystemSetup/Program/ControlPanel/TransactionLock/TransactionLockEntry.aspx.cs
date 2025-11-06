using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class TransactionLockEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.TRANSACTION_LOCK;
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
                vTransactionTypeLock entity = BusinessLayer.GetvTransactionTypeLockList(string.Format("TransactionCode = {0}", hdnID.Value)).FirstOrDefault();
                OnSetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtTransactionName.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void OnSetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTransactionCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtTransactionName, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtTransactionInitial, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLockingUntilDate, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
        }

        private void EntityToControl(vTransactionTypeLock entity)
        {
            txtTransactionCode.Text = entity.TransactionCode;
            txtTransactionName.Text = entity.TransactionName;
            txtTransactionInitial.Text = entity.TransactionInitial;

            if (entity.LockedUntilDate.ToString("dd-MM-yyyy") == Constant.ConstantDate.DEFAULT_NULL)
            {
                chkOpenTransaction.Checked = true;
                tdLockingDate.Style.Add("display", "none");
            }
            else
            {
                chkOpenTransaction.Checked = false;
                txtLockingUntilDate.Text = entity.LockedUntilDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                tdLockingDate.Style.Remove("display");
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                TransactionTypeLock entity = BusinessLayer.GetTransactionTypeLock(hdnID.Value);

                if (chkOpenTransaction.Checked)
                {
                    entity.LockedUntilDate = null;
                }
                else
                {
                    entity.LockedUntilDate = Helper.GetDatePickerValue(txtLockingUntilDate);
                }

                entity.LockedBy = AppSession.UserLogin.UserID;
                entity.LockedDate = DateTime.Now;
                BusinessLayer.UpdateTransactionTypeLock(entity);
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