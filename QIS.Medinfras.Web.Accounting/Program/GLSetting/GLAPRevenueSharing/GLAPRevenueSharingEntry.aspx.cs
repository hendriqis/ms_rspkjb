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
    public partial class GLAPRevenueSharingEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_AP_REVENUE_SHARING;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        #region Html Getter
        protected string OnGetHealthcareID()
        {
            return AppSession.UserLogin.HealthcareID;
        }
        protected string OnGetRevenueTransactionEmergency()
        {
            return Constant.GLTransactionGroup.EMERGENCY;
        }
        protected string OnGetRevenueTransactionInpatient()
        {
            return Constant.GLTransactionGroup.INPATIENT;
        }
        protected string OnGetRevenueTransactionMedicalDiagnostic()
        {
            return Constant.GLTransactionGroup.MEDICAL_DIAGNOSTIC;
        }
        protected string OnGetRevenueTransactionMedicalCheckup()
        {
            return Constant.GLTransactionGroup.MEDICAL_CHECKUP;
        }
        protected string OnGetRevenueTransactionOutpatient()
        {
            return Constant.GLTransactionGroup.OUTPATIENT;
        }
        protected string OnGetDepartmentEmergency()
        {
            return Constant.Facility.EMERGENCY;
        }
        protected string OnGetDepartmentInpatient()
        {
            return Constant.Facility.INPATIENT;
        }
        protected string OnGetDepartmentDiagnostic()
        {
            return Constant.Facility.DIAGNOSTIC;
        }
        protected string OnGetDepartmentOutpatient()
        {
            return Constant.Facility.OUTPATIENT;
        }
        protected string OnGetDepartmentMedicalCheckup()
        {
            return Constant.Facility.MEDICAL_CHECKUP;
        }
        #endregion

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String[] param = Request.QueryString["id"].Split('|');
                String ID = param[0]; 
                hdnID.Value = ID;
                vGLAPRevenueSharing entity = BusinessLayer.GetvGLAPRevenueSharingList(String.Format("ID = {0}", hdnID.Value))[0];
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
            String filterExpression = String.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_COMPONENT, Constant.StandardCode.GL_TRANSACTION_GROUP);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboGLRevenueTransaction, lstSc.Where(p => p.ParentID == Constant.StandardCode.GL_TRANSACTION_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboSharingComponent, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_COMPONENT).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboGLRevenueTransaction, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboSharingComponent, new ControlEntrySetting(true, true, true));
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

        private void EntityToControl(vGLAPRevenueSharing entity)
        {
            cboGLRevenueTransaction.Value = entity.GCGLRevenueTransaction;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = entity.ServiceUnitCode;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            cboSharingComponent.Value = entity.GCSharingComponent;
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

        private void ControlToEntity(GLAPRevenueSharing entity)
        {
            entity.GCGLRevenueTransaction = cboGLRevenueTransaction.Value.ToString();
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entity.GCSharingComponent = cboSharingComponent.Value.ToString();
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

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                GLAPRevenueSharing entity = new GLAPRevenueSharing();
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertGLAPRevenueSharing(entity);
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
                GLAPRevenueSharing entity = BusinessLayer.GetGLAPRevenueSharing(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGLAPRevenueSharing(entity);
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