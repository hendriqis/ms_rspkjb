using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLPatientPaymentMethodEDCBankEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_PATIENT_PAYMENT_METHOD_EDC_BANK;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public string OnGetHealthcareSeriveUnitFilterExpression()
        {
            return String.Format("IsDeleted = 0 AND HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String[] param = Request.QueryString["id"].Split('|');
                hdnID.Value = param[0];
                vGLPatientPaymentMethodEDCBank entity = BusinessLayer.GetvGLPatientPaymentMethodEDCBankList(String.Format("ID = {0}", hdnID.Value))[0];
                SetControlProperties();
                EntityToControl(entity);
                txtNotes.Focus();
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
                txtGLAccountCode.Focus();
            }

        }

        protected override void SetControlProperties()
        {
            String filterExpression = String.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                                                            Constant.StandardCode.PAYMENT_METHOD,
                                                            Constant.StandardCode.CASHIER_GROUP
                                                        );
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField(cboGCPaymentMethod, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.PAYMENT_METHOD).ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField(cboCashierGroup, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.CASHIER_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtGLAccountCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(cboGCPaymentMethod, new ControlEntrySetting(true, false, true));

            SetControlEntrySetting(hdnEDCMachineID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtEDCMachineCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEDCMachineName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnBankID, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtBankCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBankName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(cboCashierGroup, new ControlEntrySetting(true, false, true));

            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

        }

        private void EntityToControl(vGLPatientPaymentMethodEDCBank entity)
        {
            hdnGLAccountID.Value = entity.GLAccount.ToString();
            txtGLAccountCode.Text = entity.GLAccountNo;
            txtGLAccountName.Text = entity.GLAccountName;

            cboGCPaymentMethod.Value = entity.GCPaymentMethod;

            hdnEDCMachineID.Value = entity.EDCMachineID.ToString();
            txtEDCMachineCode.Text = entity.EDCMachineCode;
            txtEDCMachineName.Text = entity.EDCMachineName;

            hdnBankID.Value = entity.BankID.ToString();
            txtBankCode.Text = entity.BankCode;
            txtBankName.Text = entity.BankName;

            cboCashierGroup.Value = entity.GCCashierGroup;

            txtNotes.Text = entity.Remarks;

        }

        private void ControlToEntity(GLPatientPaymentMethodEDCBank entity)
        {
            entity.GLAccount = Convert.ToInt32(hdnGLAccountID.Value);
            entity.GCPaymentMethod = cboGCPaymentMethod.Value.ToString();

            if (hdnEDCMachineID.Value != null && hdnEDCMachineID.Value != "" && hdnEDCMachineID.Value != "0")
            {
                entity.EDCMachineID = Convert.ToInt32(hdnEDCMachineID.Value);
            }
            else
            {
                entity.EDCMachineID = null;
            }

            if (hdnBankID.Value != null && hdnBankID.Value != "" && hdnBankID.Value != "0")
            {
                entity.BankID = Convert.ToInt32(hdnBankID.Value);
            }
            else
            {
                entity.BankID = null;
            }

            entity.GCCashierGroup = cboCashierGroup.Value.ToString();

            entity.Remarks = txtNotes.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                GLPatientPaymentMethodEDCBank entity = new GLPatientPaymentMethodEDCBank();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertGLPatientPaymentMethodEDCBank(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                GLPatientPaymentMethodEDCBank entity = BusinessLayer.GetGLPatientPaymentMethodEDCBank(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGLPatientPaymentMethodEDCBank(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}