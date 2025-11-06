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
    public partial class GLMDRevenueAccountEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_MD_REVENUE_ACCOUNT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public string OnGetHealthcareSeriveUnitFilterExpression() 
        {
            return String.Format("HealthcareID = '{0}' AND DepartmentID IN ('{1}','{2}') AND IsDeleted = 0 ", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, Constant.Facility.PHARMACY);
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
                vGLMDRevenueAccount entity = BusinessLayer.GetvGLMDRevenueAccountList(String.Format("ID = {0}", hdnID.Value))[0];
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
            String filterExpression = String.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.GL_REVENUE_TRANSACTION);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(String.Format("IsActive = 1"));


            Methods.SetComboBoxField(cboGLRevenueTransaction, lstStandardCode, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtHealthcareServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtHealthcareServiceUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboGLRevenueTransaction, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            SetControlEntrySetting(hdnGLAccount1ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount1Code, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccount1Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt1, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt1ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt1Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt1Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount2ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName2, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID2, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount2Code, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccount2Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt2, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt2ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt2Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt2Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount3ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName3, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID3, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount3Code, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccount3Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt3, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt3ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt3Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt3Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount4ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName4, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID4, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount4Code, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccount4Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt4, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt4ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt4Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt4Name, new ControlEntrySetting(false, false, false));
            #endregion
        }

        private void EntityToControl(vGLMDRevenueAccount entity)
        {
            txtNotes.Text = entity.Remarks;
            cboGLRevenueTransaction.Value = entity.GCGLRevenueTransaction;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            txtHealthcareServiceUnitCode.Text = entity.ServiceUnitCode;
            txtHealthcareServiceUnitName.Text = entity.ServiceUnitName;
            cboDepartment.Value = entity.DepartmentID;

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            #region GL Account 1
            hdnGLAccount1ID.Value = entity.GLAccount1.ToString();
            txtGLAccount1Code.Text = entity.GLAccount1No;
            txtGLAccount1Name.Text = entity.GLAccount1Name;

            hdnSubLedgerID1.Value = entity.SubLedgerID1.ToString();
            hdnSearchDialogTypeName1.Value = entity.SearchDialogTypeName1;
            hdnIDFieldName1.Value = entity.IDFieldName1;
            hdnCodeFieldName1.Value = entity.CodeFieldName1;
            hdnDisplayFieldName1.Value = entity.DisplayFieldName1;
            hdnMethodName1.Value = entity.MethodName1;
            hdnFilterExpression1.Value = entity.FilterExpression1;

            hdnSubLedgerDt1ID.Value = entity.SubLedger1.ToString();
            txtSubLedgerDt1Code.Text = entity.SubLedger1Code;
            txtSubLedgerDt1Name.Text = entity.SubLedger1Name;
            #endregion

            #region GL Account 2
            hdnGLAccount2ID.Value = entity.GLAccount2.ToString();
            txtGLAccount2Code.Text = entity.GLAccount2No;
            txtGLAccount2Name.Text = entity.GLAccount2Name;

            hdnSubLedgerID2.Value = entity.SubLedgerID2.ToString();
            hdnSearchDialogTypeName2.Value = entity.SearchDialogTypeName2;
            hdnIDFieldName2.Value = entity.IDFieldName2;
            hdnCodeFieldName2.Value = entity.CodeFieldName2;
            hdnDisplayFieldName2.Value = entity.DisplayFieldName2;
            hdnMethodName2.Value = entity.MethodName2;
            hdnFilterExpression2.Value = entity.FilterExpression2;

            hdnSubLedgerDt2ID.Value = entity.SubLedger2.ToString();
            txtSubLedgerDt2Code.Text = entity.SubLedger2Code;
            txtSubLedgerDt2Name.Text = entity.SubLedger2Name;
            #endregion

            #region GL Account 3
            hdnGLAccount3ID.Value = entity.GLAccount3.ToString();
            txtGLAccount3Code.Text = entity.GLAccount3No;
            txtGLAccount3Name.Text = entity.GLAccount3Name;

            hdnSubLedgerID3.Value = entity.SubLedgerID3.ToString();
            hdnSearchDialogTypeName3.Value = entity.SearchDialogTypeName3;
            hdnIDFieldName3.Value = entity.IDFieldName3;
            hdnCodeFieldName3.Value = entity.CodeFieldName3;
            hdnDisplayFieldName3.Value = entity.DisplayFieldName3;
            hdnMethodName3.Value = entity.MethodName3;
            hdnFilterExpression3.Value = entity.FilterExpression3;

            hdnSubLedgerDt3ID.Value = entity.SubLedger3.ToString();
            txtSubLedgerDt3Code.Text = entity.SubLedger3Code;
            txtSubLedgerDt3Name.Text = entity.SubLedger3Name;
            #endregion

            #region GL Account 4
            hdnGLAccount4ID.Value = entity.GLAccount4.ToString();
            txtGLAccount4Code.Text = entity.GLAccount4No;
            txtGLAccount4Name.Text = entity.GLAccount4Name;

            hdnSubLedgerID4.Value = entity.SubLedgerID4.ToString();
            hdnSearchDialogTypeName4.Value = entity.SearchDialogTypeName4;
            hdnIDFieldName4.Value = entity.IDFieldName4;
            hdnCodeFieldName4.Value = entity.CodeFieldName4;
            hdnDisplayFieldName4.Value = entity.DisplayFieldName4;
            hdnMethodName4.Value = entity.MethodName4;
            hdnFilterExpression4.Value = entity.FilterExpression4;

            hdnSubLedgerDt4ID.Value = entity.SubLedger4.ToString();
            txtSubLedgerDt4Code.Text = entity.SubLedger4Code;
            txtSubLedgerDt4Name.Text = entity.SubLedger4Name;
            #endregion
            #endregion
        }

        private void ControlToEntity(GLMDRevenueAccount entity)
        {
            entity.Remarks = txtNotes.Text;
            entity.GCGLRevenueTransaction = cboGLRevenueTransaction.Value.ToString();
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entity.DepartmentID = cboDepartment.Value.ToString();

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            #region GL Account 1
            if (hdnGLAccount1ID.Value != "" && hdnGLAccount1ID.Value != "0")
                entity.GLAccount1 = Convert.ToInt32(hdnGLAccount1ID.Value);
            else
                entity.GLAccount1 = null;
            if (hdnSubLedgerDt1ID.Value != "" && hdnSubLedgerDt1ID.Value != "0")
                entity.SubLedger1 = Convert.ToInt32(hdnSubLedgerDt1ID.Value);
            else
                entity.SubLedger1 = null;
            #endregion

            #region GL Account 2
            if (hdnGLAccount2ID.Value != "" && hdnGLAccount2ID.Value != "0")
                entity.GLAccount2 = Convert.ToInt32(hdnGLAccount2ID.Value);
            else
                entity.GLAccount2 = null;
            if (hdnSubLedgerDt2ID.Value != "" && hdnSubLedgerDt2ID.Value != "0")
                entity.SubLedger2 = Convert.ToInt32(hdnSubLedgerDt2ID.Value);
            else
                entity.SubLedger2 = null;
            #endregion

            #region GL Account 3
            if (hdnGLAccount3ID.Value != "" && hdnGLAccount3ID.Value != "0")
                entity.GLAccount3 = Convert.ToInt32(hdnGLAccount3ID.Value);
            else
                entity.GLAccount3 = null;
            if (hdnSubLedgerDt3ID.Value != "" && hdnSubLedgerDt3ID.Value != "0")
                entity.SubLedger3 = Convert.ToInt32(hdnSubLedgerDt3ID.Value);
            else
                entity.SubLedger3 = null;
            #endregion

            #region GL Account 4
            if (hdnGLAccount4ID.Value != "" && hdnGLAccount4ID.Value != "0")
                entity.GLAccount4 = Convert.ToInt32(hdnGLAccount4ID.Value);
            else
                entity.GLAccount4 = null;
            if (hdnSubLedgerDt4ID.Value != "" && hdnSubLedgerDt4ID.Value != "0")
                entity.SubLedger4 = Convert.ToInt32(hdnSubLedgerDt4ID.Value);
            else
                entity.SubLedger4 = null;
            #endregion
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                GLMDRevenueAccount entity = new GLMDRevenueAccount();
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertGLMDRevenueAccount(entity);
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
                GLMDRevenueAccount entity = BusinessLayer.GetGLMDRevenueAccount(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGLMDRevenueAccount(entity);
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