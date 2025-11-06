using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAGroupEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_GROUP;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                        "ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",
                                                        Constant.StandardCode.PURCHASING_BUDGET_CATEGORY
                                                 ));

            Methods.SetComboBoxField<StandardCode>(cboBudgetCategory, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.PURCHASING_BUDGET_CATEGORY).ToList(), "StandardCodeName", "StandardCodeID");
            cboBudgetCategory.SelectedIndex = 0;

            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                List<vFAGroup> listGroup = BusinessLayer.GetvFAGroupList(String.Format("FAGroupID = {0}", Convert.ToInt32(ID)));
                if (listGroup.Count>0)
                {
                    vFAGroup entity = BusinessLayer.GetvFAGroupList(String.Format("FAGroupID = {0}", Convert.ToInt32(ID)))[0];
                    List<vFAGroupCOA> listCOA = BusinessLayer.GetvFAGroupCOAList(String.Format("HealthcareID = '{0}' AND FAGroupID = {1}", AppSession.UserLogin.HealthcareID, entity.FAGroupID));
                    vFAGroupCOA entityGCoa;
                    if (listCOA.Count > 0)
                        entityGCoa = BusinessLayer.GetvFAGroupCOAList(String.Format("HealthcareID = '{0}' AND FAGroupID = {1}", AppSession.UserLogin.HealthcareID, entity.FAGroupID))[0];
                    else
                        entityGCoa = null;
                    EntityToControl(entity, entityGCoa); 
                }
            }
            else
            {
                IsAdd = true;
            }

            txtFAGroupCode.Focus();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtFAGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFAGroupName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnMethodID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtMethodCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMethodName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboBudgetCategory, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnSourceProductLineID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSourceProductLineCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSourceProductLineName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

            #region Pengaturan Perkiraan untuk Kelompok Aktiva
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

            SetControlEntrySetting(hdnGLAccount5ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName5, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID5, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount5Code, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccount5Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt5, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt5ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt5Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt5Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount6ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName6, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID6, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount6Code, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccount6Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt6, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt6ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt6Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt6Name, new ControlEntrySetting(false, false, false));
            #endregion
        }

        private void EntityToControl(vFAGroup entity, vFAGroupCOA entityGCoa)
        {
            txtFAGroupCode.Text = entity.FAGroupCode;
            txtFAGroupName.Text = entity.FAGroupName;
            hdnMethodID.Value = entity.MethodID.ToString();
            txtMethodCode.Text = entity.MethodCode;
            txtMethodName.Text = entity.MethodName;
            cboBudgetCategory.Value = entity.GCBudgetCategory;
            hdnSourceProductLineID.Value = entity.SourceProductLineID.ToString();
            txtSourceProductLineCode.Text = entity.SourceProductLineCode;
            txtSourceProductLineName.Text = entity.SourceProductLineName;
            txtRemarks.Text = entity.Remarks;

            if (entityGCoa != null)
            {
                #region Pengaturan Perkiraan untuk Aktiva Tetap
                #region GL Account 1
                hdnGLAccount1ID.Value = entityGCoa.GLAccount1.ToString();
                txtGLAccount1Code.Text = entityGCoa.GLAccount1No.ToString();
                txtGLAccount1Name.Text = entityGCoa.GLAccount1Name.ToString();

                hdnSubLedgerID1.Value = entityGCoa.SubLedgerID1.ToString();
                hdnSearchDialogTypeName1.Value = entityGCoa.SearchDialogTypeName1;
                hdnFilterExpression1.Value = entityGCoa.FilterExpression1;
                hdnIDFieldName1.Value = entityGCoa.IDFieldName1;
                hdnCodeFieldName1.Value = entityGCoa.CodeFieldName1;
                hdnDisplayFieldName1.Value = entityGCoa.DisplayFieldName1;
                hdnMethodName1.Value = entityGCoa.MethodName1;

                hdnSubLedgerDt1ID.Value = entityGCoa.SubLedger1.ToString();
                txtSubLedgerDt1Code.Text = entityGCoa.SubLedger1Code.ToString();
                txtSubLedgerDt1Name.Text = entityGCoa.SubLedger1Name.ToString();
                #endregion

                #region GL Account 2
                hdnGLAccount2ID.Value = entityGCoa.GLAccount2.ToString();
                txtGLAccount2Code.Text = entityGCoa.GLAccount2No.ToString();
                txtGLAccount2Name.Text = entityGCoa.GLAccount2Name.ToString();

                hdnSubLedgerID2.Value = entityGCoa.SubLedgerID2.ToString();
                hdnSearchDialogTypeName2.Value = entityGCoa.SearchDialogTypeName2;
                hdnFilterExpression2.Value = entityGCoa.FilterExpression2;
                hdnIDFieldName2.Value = entityGCoa.IDFieldName2;
                hdnCodeFieldName2.Value = entityGCoa.CodeFieldName2;
                hdnDisplayFieldName2.Value = entityGCoa.DisplayFieldName2;
                hdnMethodName2.Value = entityGCoa.MethodName2;

                hdnSubLedgerDt2ID.Value = entityGCoa.SubLedger2.ToString();
                txtSubLedgerDt2Code.Text = entityGCoa.SubLedger2Code.ToString();
                txtSubLedgerDt2Name.Text = entityGCoa.SubLedger2Name.ToString();
                #endregion

                #region GL Account 3
                hdnGLAccount3ID.Value = entityGCoa.GLAccount3.ToString();
                txtGLAccount3Code.Text = entityGCoa.GLAccount3No.ToString();
                txtGLAccount3Name.Text = entityGCoa.GLAccount3Name.ToString();

                hdnSubLedgerID3.Value = entityGCoa.SubLedgerID3.ToString();
                hdnSearchDialogTypeName3.Value = entityGCoa.SearchDialogTypeName3;
                hdnFilterExpression3.Value = entityGCoa.FilterExpression3;
                hdnIDFieldName3.Value = entityGCoa.IDFieldName3;
                hdnCodeFieldName3.Value = entityGCoa.CodeFieldName3;
                hdnDisplayFieldName3.Value = entityGCoa.DisplayFieldName3;
                hdnMethodName3.Value = entityGCoa.MethodName3;

                hdnSubLedgerDt3ID.Value = entityGCoa.SubLedger3.ToString();
                txtSubLedgerDt3Code.Text = entityGCoa.SubLedger3Code.ToString();
                txtSubLedgerDt3Name.Text = entityGCoa.SubLedger3Name.ToString();
                #endregion

                #region GL Account 4
                hdnGLAccount4ID.Value = entityGCoa.GLAccount4.ToString();
                txtGLAccount4Code.Text = entityGCoa.GLAccount4No.ToString();
                txtGLAccount4Name.Text = entityGCoa.GLAccount4Name.ToString();

                hdnSubLedgerID4.Value = entityGCoa.SubLedgerID4.ToString();
                hdnSearchDialogTypeName4.Value = entityGCoa.SearchDialogTypeName4;
                hdnFilterExpression4.Value = entityGCoa.FilterExpression4;
                hdnIDFieldName4.Value = entityGCoa.IDFieldName4;
                hdnCodeFieldName4.Value = entityGCoa.CodeFieldName4;
                hdnDisplayFieldName4.Value = entityGCoa.DisplayFieldName4;
                hdnMethodName4.Value = entityGCoa.MethodName4;

                hdnSubLedgerDt4ID.Value = entityGCoa.SubLedger4.ToString();
                txtSubLedgerDt4Code.Text = entityGCoa.SubLedger4Code.ToString();
                txtSubLedgerDt4Name.Text = entityGCoa.SubLedger4Name.ToString();
                #endregion

                #region GL Account 5
                hdnGLAccount5ID.Value = entityGCoa.GLAccount5.ToString();
                txtGLAccount5Code.Text = entityGCoa.GLAccount5No.ToString();
                txtGLAccount5Name.Text = entityGCoa.GLAccount5Name.ToString();

                hdnSubLedgerID5.Value = entityGCoa.SubLedgerID5.ToString();
                hdnSearchDialogTypeName5.Value = entityGCoa.SearchDialogTypeName5;
                hdnFilterExpression5.Value = entityGCoa.FilterExpression5;
                hdnIDFieldName5.Value = entityGCoa.IDFieldName5;
                hdnCodeFieldName5.Value = entityGCoa.CodeFieldName5;
                hdnDisplayFieldName5.Value = entityGCoa.DisplayFieldName5;
                hdnMethodName5.Value = entityGCoa.MethodName5;

                hdnSubLedgerDt5ID.Value = entityGCoa.SubLedger5.ToString();
                txtSubLedgerDt5Code.Text = entityGCoa.SubLedger5Code.ToString();
                txtSubLedgerDt5Name.Text = entityGCoa.SubLedger5Name.ToString();
                #endregion

                #region GL Account 6
                hdnGLAccount6ID.Value = entityGCoa.GLAccount6.ToString();
                txtGLAccount6Code.Text = entityGCoa.GLAccount6No.ToString();
                txtGLAccount6Name.Text = entityGCoa.GLAccount6Name.ToString();

                hdnSubLedgerID6.Value = entityGCoa.SubLedgerID6.ToString();
                hdnSearchDialogTypeName6.Value = entityGCoa.SearchDialogTypeName6;
                hdnFilterExpression6.Value = entityGCoa.FilterExpression6;
                hdnIDFieldName6.Value = entityGCoa.IDFieldName6;
                hdnCodeFieldName6.Value = entityGCoa.CodeFieldName6;
                hdnDisplayFieldName6.Value = entityGCoa.DisplayFieldName6;
                hdnMethodName6.Value = entityGCoa.MethodName6;

                hdnSubLedgerDt6ID.Value = entityGCoa.SubLedger6.ToString();
                txtSubLedgerDt6Code.Text = entityGCoa.SubLedger6Code.ToString();
                txtSubLedgerDt6Name.Text = entityGCoa.SubLedger6Name.ToString();
                #endregion
                #endregion 
            }
        }

        private void ControlToEntity(FAGroup entity, FAGroupCOA entityGCoa)
        {
            entity.FAGroupCode = txtFAGroupCode.Text;
            entity.FAGroupName = txtFAGroupName.Text;
            entity.MethodID = Convert.ToInt32(hdnMethodID.Value);
            entity.GCBudgetCategory = cboBudgetCategory.Value.ToString();
            if (hdnSourceProductLineID.Value != "" && hdnSourceProductLineID.Value != "0" && hdnSourceProductLineID.Value != null)
            {
                entity.SourceProductLineID = Convert.ToInt32(hdnSourceProductLineID.Value);
            }
            entity.Remarks = txtRemarks.Text;

            #region FAGroupCOA
            if (hdnGLAccount1ID.Value != "" && hdnGLAccount1ID.Value != "0") entityGCoa.GLAccount1 = Convert.ToInt32(hdnGLAccount1ID.Value);
            else entityGCoa.GLAccount1 = null;
            if (hdnSubLedgerDt1ID.Value != "" && hdnSubLedgerDt1ID.Value != "0") entityGCoa.SubLedger1 = Convert.ToInt32(hdnSubLedgerDt1ID.Value);
            else entityGCoa.SubLedger1 = null;
            
            if (hdnGLAccount2ID.Value != "" && hdnGLAccount2ID.Value != "0") entityGCoa.GLAccount2 = Convert.ToInt32(hdnGLAccount2ID.Value);
            else entityGCoa.GLAccount2 = null;
            if (hdnSubLedgerDt2ID.Value != "" && hdnSubLedgerDt2ID.Value != "0") entityGCoa.SubLedger2 = Convert.ToInt32(hdnSubLedgerDt2ID.Value);
            else entityGCoa.SubLedger2 = null;
            
            if (hdnGLAccount3ID.Value != "" && hdnGLAccount3ID.Value != "0") entityGCoa.GLAccount3 = Convert.ToInt32(hdnGLAccount3ID.Value);
            else entityGCoa.GLAccount3 = null;
            if (hdnSubLedgerDt3ID.Value != "" && hdnSubLedgerDt3ID.Value != "0") entityGCoa.SubLedger3 = Convert.ToInt32(hdnSubLedgerDt3ID.Value);
            else entityGCoa.SubLedger3 = null;
            
            if (hdnGLAccount4ID.Value != "" && hdnGLAccount4ID.Value != "0") entityGCoa.GLAccount4 = Convert.ToInt32(hdnGLAccount4ID.Value);
            else entityGCoa.GLAccount4 = null;
            if (hdnSubLedgerDt4ID.Value != "" && hdnSubLedgerDt4ID.Value != "0") entityGCoa.SubLedger4 = Convert.ToInt32(hdnSubLedgerDt4ID.Value);
            else entityGCoa.SubLedger4 = null;
            
            if (hdnGLAccount5ID.Value != "" && hdnGLAccount5ID.Value != "0") entityGCoa.GLAccount5 = Convert.ToInt32(hdnGLAccount5ID.Value);
            else entityGCoa.GLAccount5 = null;
            if (hdnSubLedgerDt5ID.Value != "" && hdnSubLedgerDt5ID.Value != "0") entityGCoa.SubLedger5 = Convert.ToInt32(hdnSubLedgerDt5ID.Value);
            else entityGCoa.SubLedger5 = null;
            
            if (hdnGLAccount6ID.Value != "" && hdnGLAccount6ID.Value != "0") entityGCoa.GLAccount6 = Convert.ToInt32(hdnGLAccount6ID.Value);
            else entityGCoa.GLAccount6 = null;
            if (hdnSubLedgerDt6ID.Value != "" && hdnSubLedgerDt6ID.Value != "0") entityGCoa.SubLedger6 = Convert.ToInt32(hdnSubLedgerDt6ID.Value);
            else entityGCoa.SubLedger6 = null;
            #endregion
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("FAGroupCode = '{0}' AND IsDeleted = 0", txtFAGroupCode.Text);
            List<FAGroup> lst = BusinessLayer.GetFAGroupList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Group With Code " + txtFAGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("FAGroupCode = '{0}' AND FAGroupID != {1} AND IsDeleted = 0", txtFAGroupCode.Text, hdnID.Value);
            List<FAGroup> lst = BusinessLayer.GetFAGroupList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Group With Code " + txtFAGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            FAGroupDao entityDao = new FAGroupDao(ctx);
            FAGroupCOADao entityGCoaDao = new FAGroupCOADao(ctx);
            bool result = false;
            try
            {
                FAGroup entity = new FAGroup();
                FAGroupCOA entityGCoa = new FAGroupCOA();
                ControlToEntity(entity, entityGCoa);

                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                entityGCoa.FAGroupID = BusinessLayer.GetFAGroupMaxID(ctx);

                entityGCoa.HealthcareID = AppSession.UserLogin.HealthcareID;
                entityGCoa.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityGCoa.FAGroupID.ToString();

                entityGCoaDao.Insert(entityGCoa);

                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
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
            FAGroupDao entityDao = new FAGroupDao(ctx);
            FAGroupCOADao entityGCoaDao = new FAGroupCOADao(ctx);
            bool result = true;
            try
            {
                FAGroup entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                FAGroupCOA entityGCoa = entityGCoaDao.Get(AppSession.UserLogin.HealthcareID, entity.FAGroupID);
                ControlToEntity(entity, entityGCoa);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                entityGCoa.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityGCoaDao.Update(entityGCoa);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally 
            {
                ctx.Close();
            }

            return result;
        }
    }
}