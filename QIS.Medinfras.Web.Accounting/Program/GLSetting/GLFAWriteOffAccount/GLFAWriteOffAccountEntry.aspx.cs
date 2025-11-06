using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxEditors;
using System.Reflection;
using System.Collections;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLFAWriteOffAccountEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_FA_WRITE_OFF_ACCOUNT;
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
                String[] param = Request.QueryString["id"].Split('|');
                String ID = param[0];
                IsAdd = false;
                hdnID.Value = ID;
                vGLFAWriteOffAccount entity = BusinessLayer.GetvGLFAWriteOffAccountList(String.Format("ID = {0}", hdnID.Value))[0];

                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
        }

        protected override void SetControlProperties()
        {
            String filterExpression = String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1",Constant.StandardCode.ASSET_SALES_TYPE,Constant.StandardCode.TIPE_PEMUSNAHAN);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<Bank> lstBank = BusinessLayer.GetBankList("IsDeleted = 0");
            lstBank.Insert(0, new Bank { BankID = 0, BankName = "" });
            Methods.SetComboBoxField(cboGCWriteOffType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.TIPE_PEMUSNAHAN).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCAssetSalesType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.ASSET_SALES_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboBank, lstBank, "BankName", "BankID");
            cboBank.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnFAGroupID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFAGroupCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCWriteOffType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboGCAssetSalesType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBank, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            #region Pengaturan Perkiraan
            SetControlEntrySetting(hdnWOID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnWOSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnWOSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtWOGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtWOGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblWOSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnWOSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtWOSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtWOSubLedgerName, new ControlEntrySetting(false, false, false));
            #endregion
        }

        private void EntityToControl(vGLFAWriteOffAccount entity)
        {
            hdnFAGroupID.Value = entity.FAGroupID.ToString();
            txtFAGroupCode.Text = entity.FAGroupCode;
            txtFAGroupName.Text = entity.FAGroupName;
            cboGCWriteOffType.Text = entity.GCWriteOffType;
            cboGCAssetSalesType.Text = entity.GCAssetSalesType;
            cboBank.Value = entity.BankID.ToString();
            txtNotes.Text = entity.Remarks;

            #region Pengaturan Perkiraan
            #region WO
            hdnWOID.Value = entity.GLAccount.ToString();
            txtWOGLAccountNo.Text = entity.GLAccountNo;
            txtWOGLAccountName.Text = entity.GLAccountName;
            hdnWOSubLedgerID.Value = entity.SubLedgerID.ToString();
            hdnWOSearchDialogTypeName.Value = entity.SearchDialogTypeName;
            hdnWOIDFieldName.Value = entity.IDFieldName;
            hdnWOCodeFieldName.Value = entity.CodeFieldName;
            hdnWODisplayFieldName.Value = entity.DisplayFieldName;
            hdnWOMethodName.Value = entity.MethodName;
            hdnWOFilterExpression.Value = entity.FilterExpression;

            hdnWOSubLedger.Value = entity.SubLedger.ToString();
            txtWOSubLedgerCode.Text = entity.SubLedgerCode.ToString();
            txtWOSubLedgerName.Text = entity.SubLedgerName.ToString();
            #endregion
            #endregion
        }

        private void ControlToEntity(GLFAWriteOffAccount entity)
        {
            entity.FAGroupID = Convert.ToInt32(hdnFAGroupID.Value);
            entity.GCWriteOffType = cboGCWriteOffType.Value.ToString();
            entity.GCAssetSalesType = cboGCAssetSalesType.Value.ToString();
            if (cboBank.Value != null && cboBank.Value.ToString() != "0")
                entity.BankID = Convert.ToInt32(cboBank.Value);
            else
                entity.BankID = null;
            entity.Remarks = txtNotes.Text;
            
            #region Pengaturan Perkiraan
            #region WO
            entity.GLAccount = Convert.ToInt32(hdnWOID.Value);
            if (hdnWOSubLedger.Value != "" && hdnWOSubLedger.Value != "0")
                entity.SubLedger = Convert.ToInt32(hdnWOSubLedger.Value);
            else
                entity.SubLedger = null;
            #endregion
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            GLFAWriteOffAccountDao GLFAWriteOffAccountDao = new GLFAWriteOffAccountDao(ctx);
            bool result = true;
            try
            {
                GLFAWriteOffAccount entity = new GLFAWriteOffAccount();
                ControlToEntity(entity);

                entity.LastUpdatedBy = entity.CreatedBy = AppSession.UserLogin.UserID;
                GLFAWriteOffAccountDao.Insert(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            GLFAWriteOffAccountDao GLFAWriteOffAccountDao = new GLFAWriteOffAccountDao(ctx);
            bool result = true;
            try
            {
                GLFAWriteOffAccount entity = GLFAWriteOffAccountDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                GLFAWriteOffAccountDao.Update(entity);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}