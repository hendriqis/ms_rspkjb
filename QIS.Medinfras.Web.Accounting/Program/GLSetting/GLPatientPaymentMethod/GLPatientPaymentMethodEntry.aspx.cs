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
    public partial class GLPatientPaymentMethodEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_PATIENT_PAYMENT_METHOD;
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
                vGLPatientPaymentMethod entity = BusinessLayer.GetvGLPatientPaymentMethodList(String.Format("ID = {0}", hdnID.Value))[0];
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            txtGLAccountCode.Focus();
        }

        protected override void SetControlProperties()
        {
            String filterExpression = String.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.GL_TRANSACTION_GROUP, Constant.StandardCode.PAYMENT_METHOD);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField(cboGCPaymentMethod, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.PAYMENT_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboGCPaymentMethod, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccountCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, true));            
            #endregion
        }

        private void EntityToControl(vGLPatientPaymentMethod entity)
        {
            cboGCPaymentMethod.Value = entity.GCPaymentMethod;
            txtNotes.Text = entity.Remarks;

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            hdnGLAccountID.Value = entity.GLAccount.ToString();
            txtGLAccountCode.Text = entity.GLAccountNo;
            txtGLAccountName.Text = entity.GLAccountName;
            #endregion
        }

        private void ControlToEntity(GLPatientPaymentMethod entity)
        {
            entity.GCPaymentMethod = cboGCPaymentMethod.Value.ToString();
            entity.Remarks = txtNotes.Text;

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            if (hdnGLAccountID.Value != "" && hdnGLAccountID.Value != "0")
                entity.GLAccount = Convert.ToInt32(hdnGLAccountID.Value);
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                GLPatientPaymentMethod entity = new GLPatientPaymentMethod();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertGLPatientPaymentMethod(entity);
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
                GLPatientPaymentMethod entity = BusinessLayer.GetGLPatientPaymentMethod(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGLPatientPaymentMethod(entity);
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