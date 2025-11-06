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
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLAccountReceivableEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            String[] param = Request.QueryString["id"].Split('|');
            switch (param[0])
            {
                case Constant.GCAccountReceivableType.PIUTANG_DALAM_PROSES: return Constant.MenuCode.Accounting.GL_AR_PROCESS;
                case Constant.GCAccountReceivableType.PIUTANG_INSTANSI: return Constant.MenuCode.Accounting.GL_AR_INSTANSI;
                case Constant.GCAccountReceivableType.PENYESUAIAN_PIUTANG: return Constant.MenuCode.Accounting.GL_AR_ADJUSTMENT;
                default: return Constant.MenuCode.Accounting.GL_AR_PERAWATAN;
            }
        }

        protected string OnGetMenuCaption()
        {
            string menuCode = OnGetMenuCode();
            return ((MPMain)((MPEntry)Master).Master).ListMenu.FirstOrDefault(p => p.MenuCode == menuCode).MenuCaption;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            String[] param = Request.QueryString["id"].Split('|');
            if (param.Count() > 1) 
            {
                hdnGCAccountReceivableType.Value = param[0];
                String ID = param[1];
                IsAdd = false;
                hdnID.Value = ID;
                vGLAccountReceivable entity = BusinessLayer.GetvGLAccountReceivableList(String.Format("ID = {0}", hdnID.Value))[0];

                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                hdnGCAccountReceivableType.Value = param[0];
                SetControlProperties();
                IsAdd = true;
            }
        }

        protected override void SetControlProperties()
        {
            String filterExpression = String.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1",Constant.StandardCode.GL_TRANSACTION_GROUP,Constant.StandardCode.CUSTOMER_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField(cboGCARTransactionGroup, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.GL_TRANSACTION_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCCustomerType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboGCARTransactionGroup, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboGCCustomerType, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            #region Pengaturan Perkiraan
            SetControlEntrySetting(hdnARID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtARGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblARSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnARSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtARSubLedgerName, new ControlEntrySetting(false, false, false));
            #endregion
        }

        private void EntityToControl(vGLAccountReceivable entity)
        {
            cboGCARTransactionGroup.Text = entity.GCARTransactionGroup;
            cboGCCustomerType.Text = entity.GCCustomerType;
            txtNotes.Text = entity.Remarks;

            #region Pengaturan Perkiraan
            #region AR
            hdnARID.Value = entity.GLAccount.ToString();
            txtARGLAccountNo.Text = entity.GLAccountNo;
            txtARGLAccountName.Text = entity.GLAccountName;
            hdnARSubLedgerID.Value = entity.SubLedgerID.ToString();
            hdnARSearchDialogTypeName.Value = entity.SearchDialogTypeName;
            hdnARIDFieldName.Value = entity.IDFieldName;
            hdnARCodeFieldName.Value = entity.CodeFieldName;
            hdnARDisplayFieldName.Value = entity.DisplayFieldName;
            hdnARMethodName.Value = entity.MethodName;
            hdnARFilterExpression.Value = entity.FilterExpression;
            
            hdnARSubLedger.Value = entity.SubLedger.ToString();
            txtARSubLedgerCode.Text = entity.SubLedgerCode.ToString();
            txtARSubLedgerName.Text = entity.SubLedgerName.ToString();
            #endregion
            #endregion
        }

        private void ControlToEntity(GLAccountReceivable entity)
        {
            entity.GCAccountReceivableType = hdnGCAccountReceivableType.Value;
            entity.GCARTransactionGroup = cboGCARTransactionGroup.Value.ToString();
            entity.GCCustomerType = cboGCCustomerType.Value.ToString();
            entity.Remarks = txtNotes.Text;
            
            #region Pengaturan Perkiraan
            #region AR
            if (hdnARID.Value != "" && hdnARID.Value != "0")
                entity.GLAccount = Convert.ToInt32(hdnARID.Value);
            else
                entity.GLAccount = null;
            if (hdnARSubLedger.Value != "" && hdnARSubLedger.Value != "0")
                entity.SubLedger = Convert.ToInt32(hdnARSubLedger.Value);
            else
                entity.SubLedger = null;
            #endregion
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            GLAccountReceivableDao GLAccountReceivableDao = new GLAccountReceivableDao(ctx);
            bool result = true;
            try
            {
                GLAccountReceivable entity = new GLAccountReceivable();
                ControlToEntity(entity);

                entity.LastUpdatedBy = entity.CreatedBy = AppSession.UserLogin.UserID;
                GLAccountReceivableDao.Insert(entity);
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
            GLAccountReceivableDao GLAccountReceivableDao = new GLAccountReceivableDao(ctx);
            bool result = true;
            try
            {
                GLAccountReceivable entity = GLAccountReceivableDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                GLAccountReceivableDao.Update(entity);

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