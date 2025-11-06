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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLPaymentMethodEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_PAYMENT_METHOD;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public string OnGetHealthcareSeriveUnitFilterExpression() 
        {
            return String.Format("IsDeleted = 0 AND DepartmentID = '{0}' AND HealthcareID = '{1}'",Constant.Facility.INPATIENT,AppSession.UserLogin.HealthcareID);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String[] param = Request.QueryString["id"].Split('|');
                String ID = param[0]; 
                hdnID.Value = ID;
                vGLPaymentMethod entity = BusinessLayer.GetvGLPaymentMethodList(String.Format("ID = {0}", hdnID.Value))[0];
                SetControlProperties();
                EntityToControl(entity);
            }else
            {
                SetControlProperties();
                IsAdd = true;
            }

            txtGLAccount1Code.Focus();
        }

        protected override void SetControlProperties()
        {
            String filterExpression = String.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.GL_TRANSACTION_GROUP, Constant.StandardCode.PAYMENT_METHOD);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField(cboGLTransactionGroup, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.GL_TRANSACTION_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCPaymentMethod, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.PAYMENT_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboGLTransactionGroup, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboGCPaymentMethod, new ControlEntrySetting(true, false, true));

            SetControlEntrySetting(hdnBankID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtBankCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBankName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnEDCMachineID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtEDCMachineCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEDCMachineName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            
            #region Pengaturan Perkiraan untuk Aktiva Tetap
            SetControlEntrySetting(hdnGLAccount1ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount1Code, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccount1Name, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(lblSubLedgerDt1, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt1ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt1Code, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtSubLedgerDt1Name, new ControlEntrySetting(false, false, true));
            #endregion
        }

        private void EntityToControl(vGLPaymentMethod entity)
        {
            cboGLTransactionGroup.Value = entity.GCGLTransactionGroup;
            cboGCPaymentMethod.Value = entity.GCPaymentMethod;
            hdnBankID.Value = entity.BankID.ToString();
            txtBankCode.Text = entity.BankCode;
            txtBankName.Text = entity.BankName;
            hdnEDCMachineID.Value = entity.EDCPaymentID.ToString();
            txtEDCMachineCode.Text = entity.EDCMachineCode;
            txtEDCMachineName.Text = entity.EDCMachineName;
            txtNotes.Text = entity.Remarks;
            
            #region Pengaturan Perkiraan untuk Aktiva Tetap
            #region GL Account 1
            hdnGLAccount1ID.Value = entity.GLAccount.ToString();
            txtGLAccount1Code.Text = entity.GLAccountNo;
            txtGLAccount1Name.Text = entity.GLAccountName;

            hdnSubLedgerID1.Value = entity.SubLedgerID.ToString();
            hdnSearchDialogTypeName1.Value = entity.SearchDialogTypeName;
            hdnIDFieldName1.Value = entity.IDFieldName;
            hdnCodeFieldName1.Value = entity.CodeFieldName;
            hdnDisplayFieldName1.Value = entity.DisplayFieldName;
            hdnMethodName1.Value = entity.MethodName;
            hdnFilterExpression1.Value = entity.FilterExpression;

            hdnSubLedgerDt1ID.Value = entity.SubLedger.ToString();
            txtSubLedgerDt1Code.Text = entity.SubLedgerCode;
            txtSubLedgerDt1Name.Text = entity.SubLedgerName;
            #endregion

            #endregion
        }

        private void ControlToEntity(GLPaymentMethod entity)
        {
            entity.GCGLTransactionGroup = cboGLTransactionGroup.Value.ToString();
            entity.GCPaymentMethod = cboGCPaymentMethod.Value.ToString();
            if (hdnBankID.Value != "" && hdnBankID.Value != "0")
            {
                entity.BankID = Convert.ToInt32(hdnBankID.Value);
            }
            else
            {
                entity.BankID = null;
            }

            if (hdnEDCMachineID.Value != "" && hdnEDCMachineID.Value != "0")
            {
                entity.EDCPaymentID = Convert.ToInt32(hdnEDCMachineID.Value);
            }
            else
            {
                entity.EDCPaymentID = null;
            }
            entity.Remarks = txtNotes.Text;

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            #region GL Account 1
            if (hdnGLAccount1ID.Value != "" && hdnGLAccount1ID.Value != "0")
                entity.GLAccount = Convert.ToInt32(hdnGLAccount1ID.Value);
            
            if (hdnSubLedgerDt1ID.Value != "" && hdnSubLedgerDt1ID.Value != "0")
                entity.SubLedger = Convert.ToInt32(hdnSubLedgerDt1ID.Value);
            else
                entity.SubLedger = null;
            #endregion
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                GLPaymentMethod entity = new GLPaymentMethod();
                ControlToEntity(entity);
                entity.LastUpdatedBy = entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertGLPaymentMethod(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                GLPaymentMethod entity = BusinessLayer.GetGLPaymentMethod(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGLPaymentMethod(entity);
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