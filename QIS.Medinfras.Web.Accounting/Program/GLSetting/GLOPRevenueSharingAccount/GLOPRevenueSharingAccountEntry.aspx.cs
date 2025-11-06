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
    public partial class GLOPRevenueSharingAccountEntry : BasePageEntry
    {
        String page;

        public override string OnGetMenuCode()
        {
            switch (Page.Request.QueryString["ID"])
            {
                case "ER": return Constant.MenuCode.Accounting.GL_ER_REVENUE_SHARING_ACCOUNT;
                default: return Constant.MenuCode.Accounting.GL_OP_REVENUE_SHARING_ACCOUNT;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public string OnGetHealthcareSeriveUnitFilterExpression() 
        {
            return String.Format("IsDeleted = 0 AND DepartmentID = '{0}' AND HealthcareID = '{1}'",Constant.Facility.OUTPATIENT,AppSession.UserLogin.HealthcareID);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Request.QueryString.Count > 0)
            {
                String[] param = Request.QueryString["id"].Split('|');
                String ID = "";

                page = param[0];
                if (param.Count() > 1) ID = param[1];

                if (ID != "")
                {
                    IsAdd = false;
                    hdnID.Value = ID;
                    vGLOPRevenueSharingAccount entity = BusinessLayer.GetvGLOPRevenueSharingAccountList(String.Format("ID = {0}", hdnID.Value))[0];
                    SetControlProperties();
                    EntityToControl(entity);
                }
                else
                {
                    if (page != "OP")
                    {
                        hdnHealthcareServiceUnitID.Value = (BusinessLayer.GetvHealthcareServiceUnitList(String.Format("DepartmentID = '{0}'", Constant.Facility.EMERGENCY))[0]).HealthcareServiceUnitID.ToString();
                        trHealthcareServiceUnit.Style.Add("Display", "none");
                    }
                    SetControlProperties();
                    IsAdd = true;
                }
            }

            txtGLAccount1Code.Focus();
        }

        protected override void SetControlProperties()
        {
            String filterExpression = String.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_COMPONENT);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(String.Format("IsActive = 1"));
            
            //Methods.SetComboBoxField(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            Methods.SetComboBoxField(cboGCSharingComponent, lstStandardCode, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtHealthcareServiceUnitCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtHealthcareServiceUnitName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboGCSharingComponent, new ControlEntrySetting(true, false, true));
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
            #endregion
        }

        private void EntityToControl(vGLOPRevenueSharingAccount entity)
        {
            txtNotes.Text = entity.Remarks;
            //cboDepartment.Value = entity.DepartmentID;
            cboGCSharingComponent.Value = entity.GCSharingComponent;
            txtHealthcareServiceUnitCode.Text = entity.ServiceUnitCode;
            txtHealthcareServiceUnitName.Text = entity.ServiceUnitName;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

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

        private void ControlToEntity(GLOPRevenueSharingAccount entity)
        {
            entity.Remarks = txtNotes.Text;
            String[] param = Request.QueryString["id"].Split('|');

            entity.DepartmentID = param[0] == "ER" ? Constant.Facility.EMERGENCY : Constant.Facility.OUTPATIENT;
            entity.GCSharingComponent = cboGCSharingComponent.Value.ToString();
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            #region GL Account 1
            if (hdnGLAccount1ID.Value != "" && hdnGLAccount1ID.Value != "0")
                entity.GLAccount = Convert.ToInt32(hdnGLAccount1ID.Value);
            else
                entity.GLAccount = null;
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
                GLOPRevenueSharingAccount entity = new GLOPRevenueSharingAccount();
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertGLOPRevenueSharingAccount(entity);
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
                GLOPRevenueSharingAccount entity = BusinessLayer.GetGLOPRevenueSharingAccount(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGLOPRevenueSharingAccount(entity);
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