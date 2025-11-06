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
    public partial class GLAPPaymentEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_AP_PAYMENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetSupplierPaymentMethodCash()
        {
            return Constant.SupplierPaymentMethod.TUNAI;
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
                vGLAPPayment entity = BusinessLayer.GetvGLAPPaymentList(String.Format("ID = {0}", hdnID.Value))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            txtGLAccount1Code.Focus();
        }

        protected override void SetControlProperties()
        {
            String filterExpression = String.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.SUPPLIER_PAYMENT_METHOD);
            List<StandardCode> lstPaymentMethod = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboSupplierPaymentMethod, lstPaymentMethod, "StandardCodeName", "StandardCodeID");

            List<Bank> lstBank = BusinessLayer.GetBankList("IsDeleted = 0");
            Methods.SetComboBoxField(cboBank, lstBank, "BankName", "BankID");

            cboSupplierPaymentMethod.Value = hdnSupplierPaymentMethod.Value;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboSupplierPaymentMethod, new ControlEntrySetting(true, false, true));
            if (hdnSupplierPaymentMethod.Value == Constant.SupplierPaymentMethod.TRANSFER || hdnSupplierPaymentMethod.Value == Constant.SupplierPaymentMethod.GIRO || hdnSupplierPaymentMethod.Value == Constant.SupplierPaymentMethod.CHEQUE)
            {
                SetControlEntrySetting(cboBank, new ControlEntrySetting(true, false, true));
            }
            else
            {
                SetControlEntrySetting(cboBank, new ControlEntrySetting(true, true, false));
            }
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            
            #region Pengaturan Perkiraan untuk Aktiva Tetap
            SetControlEntrySetting(hdnGLAccount1ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount1Code, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccount1Name, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(lblSubLedgerDt1, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt1ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt1Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt1Name, new ControlEntrySetting(false, false, false));
            #endregion
        }

        private void EntityToControl(vGLAPPayment entity)
        {
            cboSupplierPaymentMethod.Value = entity.GCSupplierPaymentMethod;
            if (entity.BankID == 0 || entity.BankID == null)
            {
                cboBank.Value = null;
            }
            else
            {
                cboBank.Value = entity.BankID.ToString();
                cboBank.Enabled = false;
            }
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

        private void ControlToEntity(GLAPPayment entity)
        {
            entity.GCSupplierPaymentMethod = cboSupplierPaymentMethod.Value.ToString();
            if (entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.TUNAI)
            {
                entity.BankID = null;
                trBank.Style.Add("display", "none");
            }
            else
            {
                trBank.Style.Remove("display");
                if (hdnSupplierPaymentMethod.Value == Constant.SupplierPaymentMethod.TRANSFER || hdnSupplierPaymentMethod.Value == Constant.SupplierPaymentMethod.GIRO || hdnSupplierPaymentMethod.Value == Constant.SupplierPaymentMethod.CHEQUE)
                {
                    entity.BankID = Convert.ToInt32(cboBank.Value);
                }
                else
                {
                    if (cboBank.Value == "0" || cboBank.Value == null)
                    {
                        entity.BankID = null;
                    }
                    else
                    {
                        entity.BankID = Convert.ToInt32(cboBank.Value);
                    }
                }
            }
            entity.Remarks = txtNotes.Text;

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            #region GL Account 1
            entity.GLAccount = Convert.ToInt32(hdnGLAccount1ID.Value);
            if (hdnSubLedgerDt1ID.Value != "" && hdnSubLedgerDt1ID.Value != "0")
                entity.SubLedger = Convert.ToInt32(hdnSubLedgerDt1ID.Value);
            else
                entity.SubLedger = null;
            #endregion
            #endregion
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            if (hdnSupplierPaymentMethod.Value == Constant.SupplierPaymentMethod.TRANSFER || hdnSupplierPaymentMethod.Value == Constant.SupplierPaymentMethod.GIRO || hdnSupplierPaymentMethod.Value == Constant.SupplierPaymentMethod.CHEQUE)
            {
                if (cboBank.Value == "0"|| cboBank.Value == null)
                {
                    errMessage = "Harap Isi Kolom Bank Terlebih Dahulu!";
                }
            }
            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                GLAPPayment entity = new GLAPPayment();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertGLAPPayment(entity);
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
                GLAPPayment entity = BusinessLayer.GetGLAPPayment(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGLAPPayment(entity);
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