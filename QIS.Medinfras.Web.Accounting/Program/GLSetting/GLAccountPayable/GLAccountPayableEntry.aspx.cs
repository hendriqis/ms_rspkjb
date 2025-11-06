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
    public partial class GLAccountPayableEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_ACCOUNT_PAYABLE;
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
                vGLAccountPayable entity = BusinessLayer.GetvGLAccountPayableList(String.Format("ID = {0}", hdnID.Value))[0];

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
            String filterExpression = String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1",Constant.StandardCode.ITEM_TYPE,Constant.StandardCode.GL_ACCOUNT_PAYABLE_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField(cboGCAccountPayableType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.GL_ACCOUNT_PAYABLE_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCItemType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.ITEM_TYPE && (x.StandardCodeID == Constant.ItemType.OBAT_OBATAN || x.StandardCodeID == Constant.ItemType.BARANG_MEDIS || x.StandardCodeID == Constant.ItemType.BARANG_UMUM)).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboGCAccountPayableType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboGCItemType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            #region Pengaturan Perkiraan
            SetControlEntrySetting(hdnAPID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAPSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAPGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAPSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAPSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAPSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAPSubLedgerName, new ControlEntrySetting(false, false, false));
            #endregion
        }

        private void EntityToControl(vGLAccountPayable entity)
        {
            cboGCAccountPayableType.Text = entity.GCAccountPayableType;
            cboGCItemType.Text = entity.GCItemType;
            txtNotes.Text = entity.Remarks;

            #region Pengaturan Perkiraan
            #region AP
            hdnAPID.Value = entity.GLAccount.ToString();
            txtAPGLAccountNo.Text = entity.GLAccountNo;
            txtAPGLAccountName.Text = entity.GLAccountName;
            hdnAPSubLedgerID.Value = entity.SubLedgerID.ToString();
            hdnAPSearchDialogTypeName.Value = entity.SearchDialogTypeName;
            hdnAPIDFieldName.Value = entity.IDFieldName;
            hdnAPCodeFieldName.Value = entity.CodeFieldName;
            hdnAPDisplayFieldName.Value = entity.DisplayFieldName;
            hdnAPMethodName.Value = entity.MethodName;
            hdnAPFilterExpression.Value = entity.FilterExpression;

            hdnAPSubLedger.Value = entity.SubLedger.ToString();
            txtAPSubLedgerCode.Text = entity.SubLedgerCode.ToString();
            txtAPSubLedgerName.Text = entity.SubLedgerName.ToString();
            #endregion
            #endregion
        }

        private void ControlToEntity(GLAccountPayable entity)
        {
            entity.GCAccountPayableType = cboGCAccountPayableType.Value.ToString();
            entity.GCItemType = cboGCItemType.Value.ToString();
            entity.Remarks = txtNotes.Text;
            
            #region Pengaturan Perkiraan
            #region AP
            if (hdnAPID.Value != "" && hdnAPID.Value != "0")
                entity.GLAccount = Convert.ToInt32(hdnAPID.Value);
            else
                entity.GLAccount = null;
            if (hdnAPSubLedger.Value != "" && hdnAPSubLedger.Value != "0")
                entity.SubLedger = Convert.ToInt32(hdnAPSubLedger.Value);
            else
                entity.SubLedger = null;
            #endregion
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            GLAccountPayableDao GLAccountPayableDao = new GLAccountPayableDao(ctx);
            bool result = true;
            try
            {
                GLAccountPayable entity = new GLAccountPayable();
                ControlToEntity(entity);

                entity.LastUpdatedBy = entity.CreatedBy = AppSession.UserLogin.UserID;
                GLAccountPayableDao.Insert(entity);
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
            GLAccountPayableDao GLAccountPayableDao = new GLAccountPayableDao(ctx);
            bool result = true;
            try
            {
                GLAccountPayable entity = GLAccountPayableDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                GLAccountPayableDao.Update(entity);

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