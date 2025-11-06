using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLCustomerLineEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_CUSTOMER_LINE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            String[] param = Request.QueryString["id"].Split('|');
            if (param[0] == "edit")
            {
                IsAdd = false;
                String ID = param[1];
                hdnID.Value = ID;
                CustomerLine entity = BusinessLayer.GetCustomerLineList(String.Format("CustomerLineID = {0}", hdnID.Value))[0];
                vCustomerLineDt entityDt = BusinessLayer.GetvCustomerLineDtList(String.Format("CustomerLineID = {0}", entity.CustomerLineID))[0];

                SetControlProperties();
                EntityToControl(entity, entityDt);
                hdnGCCustomerType.Value = entity.GCCustomerType;
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
                hdnGCCustomerType.Value = param[1];
            }

            txtCustomerLineCode.Focus();
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCustomerLineCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtCustomerLineName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

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

            SetControlEntrySetting(hdnARInProcessID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARInProcessSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARInProcessSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARInProcessGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtARInProcessGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblARInProcessSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnARInProcessSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARInProcessSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtARInProcessSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnARInCareID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARInCareSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARInCareSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARInCareGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtARInCareGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblARInCareSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnARInCareSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARInCareSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtARInCareSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnARAdjustmentID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARAdjustmentSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARAdjustmentSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARAdjustmentGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtARAdjustmentGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblARAdjustmentSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnARAdjustmentSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARAdjustmentSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtARAdjustmentSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnARDiscountID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARDiscountSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnARDiscountSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtARDiscountGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtARDiscountGLAccountName, new ControlEntrySetting(false, false, false));

            #endregion
        }

        private void EntityToControl(CustomerLine entity, vCustomerLineDt entityDt)
        {
            txtCustomerLineCode.Text = entity.CustomerLineCode;
            txtCustomerLineName.Text = entity.CustomerLineName;
            txtRemarks.Text = entity.Remarks;

            #region Pengaturan Perkiraan

            #region AR
            hdnARID.Value = entityDt.AR.ToString();
            txtARGLAccountNo.Text = entityDt.ARGLAccountNo;
            txtARGLAccountName.Text = entityDt.ARGLAccountName;
            hdnARSubLedgerID.Value = entityDt.ARSubLedgerID.ToString();
            hdnARSearchDialogTypeName.Value = entityDt.ARSearchDialogTypeName;
            hdnARIDFieldName.Value = entityDt.ARIDFieldName;
            hdnARCodeFieldName.Value = entityDt.ARCodeFieldName;
            hdnARDisplayFieldName.Value = entityDt.ARDisplayFieldName;
            hdnARMethodName.Value = entityDt.ARMethodName;
            hdnARFilterExpression.Value = entityDt.ARFilterExpression;

            hdnARSubLedger.Value = entityDt.ARSubLedger.ToString();
            txtARSubLedgerCode.Text = entityDt.ARSubLedgerCode.ToString();
            txtARSubLedgerName.Text = entityDt.ARSubLedgerName.ToString();
            #endregion

            #region ARInProcess
            hdnARInProcessID.Value = entityDt.ARInProcess.ToString();
            txtARInProcessGLAccountNo.Text = entityDt.ARInProcessGLAccountNo;
            txtARInProcessGLAccountName.Text = entityDt.ARInProcessGLAccountName;
            hdnARInProcessSubLedgerID.Value = entityDt.ARInProcessSubLedgerID.ToString();
            hdnARInProcessSearchDialogTypeName.Value = entityDt.ARInProcessSearchDialogTypeName;
            hdnARInProcessIDFieldName.Value = entityDt.ARInProcessIDFieldName;
            hdnARInProcessCodeFieldName.Value = entityDt.ARInProcessCodeFieldName;
            hdnARInProcessDisplayFieldName.Value = entityDt.ARInProcessDisplayFieldName;
            hdnARInProcessMethodName.Value = entityDt.ARInProcessMethodName;
            hdnARInProcessFilterExpression.Value = entityDt.ARInProcessFilterExpression;

            hdnARInProcessSubLedger.Value = entityDt.ARInProcessSubLedger.ToString();
            txtARInProcessSubLedgerCode.Text = entityDt.ARInProcessSubLedgerCode.ToString();
            txtARInProcessSubLedgerName.Text = entityDt.ARInProcessSubLedgerName.ToString();
            #endregion

            #region ARInCare
            hdnARInCareID.Value = entityDt.ARInCare.ToString();
            txtARInCareGLAccountNo.Text = entityDt.ARInCareGLAccountNo;
            txtARInCareGLAccountName.Text = entityDt.ARInCareGLAccountName;
            hdnARInCareSubLedgerID.Value = entityDt.ARInCareSubLedgerID.ToString();
            hdnARInCareSearchDialogTypeName.Value = entityDt.ARInCareSearchDialogTypeName;
            hdnARInCareIDFieldName.Value = entityDt.ARInCareIDFieldName;
            hdnARInCareCodeFieldName.Value = entityDt.ARInCareCodeFieldName;
            hdnARInCareDisplayFieldName.Value = entityDt.ARInCareDisplayFieldName;
            hdnARInCareMethodName.Value = entityDt.ARInCareMethodName;
            hdnARInCareFilterExpression.Value = entityDt.ARInCareFilterExpression;

            hdnARInCareSubLedger.Value = entityDt.ARInCareSubLedger.ToString();
            txtARInCareSubLedgerCode.Text = entityDt.ARInCareSubLedgerCode.ToString();
            txtARInCareSubLedgerName.Text = entityDt.ARInCareSubLedgerName.ToString();
            #endregion

            #region ARAdjustment
            hdnARAdjustmentID.Value = entityDt.ARAdjustment.ToString();
            txtARAdjustmentGLAccountNo.Text = entityDt.ARAdjustmentGLAccountNo;
            txtARAdjustmentGLAccountName.Text = entityDt.ARAdjustmentGLAccountName;
            hdnARAdjustmentSubLedgerID.Value = entityDt.ARAdjustmentSubLedgerID.ToString();
            hdnARAdjustmentSearchDialogTypeName.Value = entityDt.ARAdjustmentSearchDialogTypeName;
            hdnARAdjustmentIDFieldName.Value = entityDt.ARAdjustmentIDFieldName;
            hdnARAdjustmentCodeFieldName.Value = entityDt.ARAdjustmentCodeFieldName;
            hdnARAdjustmentDisplayFieldName.Value = entityDt.ARAdjustmentDisplayFieldName;
            hdnARAdjustmentMethodName.Value = entityDt.ARAdjustmentMethodName;
            hdnARAdjustmentFilterExpression.Value = entityDt.ARAdjustmentFilterExpression;

            hdnARAdjustmentSubLedger.Value = entityDt.ARAdjustmentSubLedger.ToString();
            txtARAdjustmentSubLedgerCode.Text = entityDt.ARAdjustmentSubLedgerCode.ToString();
            txtARAdjustmentSubLedgerName.Text = entityDt.ARAdjustmentSubLedgerName.ToString();
            #endregion

            #region ARDiscount
            hdnARDiscountID.Value = entityDt.ARDiscount.ToString();
            txtARDiscountGLAccountNo.Text = entityDt.ARDiscountGLAccountNo;
            txtARDiscountGLAccountName.Text = entityDt.ARDiscountGLAccountName;
            hdnARDiscountSubLedgerID.Value = entityDt.ARDiscountSubLedgerID.ToString();
            hdnARDiscountSearchDialogTypeName.Value = entityDt.ARDiscountSearchDialogTypeName;
            hdnARDiscountIDFieldName.Value = entityDt.ARDiscountIDFieldName;
            hdnARDiscountCodeFieldName.Value = entityDt.ARDiscountCodeFieldName;
            hdnARDiscountDisplayFieldName.Value = entityDt.ARDiscountDisplayFieldName;
            hdnARDiscountMethodName.Value = entityDt.ARDiscountMethodName;
            hdnARDiscountFilterExpression.Value = entityDt.ARDiscountFilterExpression;

            hdnARDiscountSubLedger.Value = entityDt.ARDiscountSubLedger.ToString();
            txtARDiscountSubLedgerCode.Text = entityDt.ARDiscountSubLedgerCode.ToString();
            txtARDiscountSubLedgerName.Text = entityDt.ARDiscountSubLedgerName.ToString();
            #endregion

            #endregion
        }

        private void ControlToEntity(CustomerLine entity, CustomerLineDt entityDt)
        {
            entity.CustomerLineCode = txtCustomerLineCode.Text;
            entity.CustomerLineName = txtCustomerLineName.Text;
            entity.Remarks = txtRemarks.Text;

            entityDt.HealthcareID = AppSession.UserLogin.HealthcareID;

            #region Pengaturan Perkiraan

            #region AR
            if (hdnARID.Value != "" && hdnARID.Value != "0")
                entityDt.AR = Convert.ToInt32(hdnARID.Value);
            else
                entityDt.AR = null;
            if (hdnARSubLedger.Value != "" && hdnARSubLedger.Value != "0")
                entityDt.ARSubLedger = Convert.ToInt32(hdnARSubLedger.Value);
            else
                entityDt.ARSubLedger = null;
            #endregion

            #region ARInProcess
            if (hdnARInProcessID.Value != "" && hdnARInProcessID.Value != "0")
                entityDt.ARInProcess = Convert.ToInt32(hdnARInProcessID.Value);
            else
                entityDt.ARInProcess = null;
            if (hdnARInProcessSubLedger.Value != "" && hdnARInProcessSubLedger.Value != "0")
                entityDt.ARInProcessSubLedger = Convert.ToInt32(hdnARInProcessSubLedger.Value);
            else
                entityDt.ARInProcessSubLedger = null;
            #endregion

            #region ARInCare
            if (hdnARInCareID.Value != "" && hdnARInCareID.Value != "0")
                entityDt.ARInCare = Convert.ToInt32(hdnARInCareID.Value);
            else
                entityDt.ARInCare = null;
            if (hdnARInCareSubLedger.Value != "" && hdnARInCareSubLedger.Value != "0")
                entityDt.ARInCareSubLedger = Convert.ToInt32(hdnARInCareSubLedger.Value);
            else
                entityDt.ARInCareSubLedger = null;
            #endregion

            #region ARAdjustment
            if (hdnARAdjustmentID.Value != "" && hdnARAdjustmentID.Value != "0")
                entityDt.ARAdjustment = Convert.ToInt32(hdnARAdjustmentID.Value);
            else
                entityDt.ARAdjustment = null;
            if (hdnARAdjustmentSubLedger.Value != "" && hdnARAdjustmentSubLedger.Value != "0")
                entityDt.ARAdjustmentSubLedger = Convert.ToInt32(hdnARAdjustmentSubLedger.Value);
            else
                entityDt.ARAdjustmentSubLedger = null;
            #endregion

            #region ARDiscount
            if (hdnARDiscountID.Value != "" && hdnARDiscountID.Value != "0")
                entityDt.ARDiscount = Convert.ToInt32(hdnARDiscountID.Value);
            else
                entityDt.ARDiscount = null;
            if (hdnARDiscountSubLedger.Value != "" && hdnARDiscountSubLedger.Value != "0")
                entityDt.ARDiscountSubLedger = Convert.ToInt32(hdnARDiscountSubLedger.Value);
            else
                entityDt.ARDiscountSubLedger = null;
            #endregion

            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            CustomerLineDao CustomerLineDao = new CustomerLineDao(ctx);
            CustomerLineDtDao CustomerLineDtDao = new CustomerLineDtDao(ctx);
            bool result = true;
            try
            {
                CustomerLine entity = new CustomerLine();
                CustomerLineDt entityDt = new CustomerLineDt();

                ControlToEntity(entity, entityDt);

                entity.GCCustomerType = hdnGCCustomerType.Value;

                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDt.CustomerLineID = CustomerLineDao.InsertReturnPrimaryKeyID(entity);

                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                CustomerLineDtDao.Insert(entityDt);

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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            CustomerLineDao CustomerLineDao = new CustomerLineDao(ctx);
            CustomerLineDtDao CustomerLineDtDao = new CustomerLineDtDao(ctx);
            bool result = true;
            try
            {
                CustomerLine entity = CustomerLineDao.Get(Convert.ToInt32(hdnID.Value));
                CustomerLineDt entityDt = CustomerLineDtDao.Get(entity.CustomerLineID, AppSession.UserLogin.HealthcareID);

                ControlToEntity(entity, entityDt);

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                CustomerLineDao.Update(entity);

                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                CustomerLineDtDao.Update(entityDt);

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