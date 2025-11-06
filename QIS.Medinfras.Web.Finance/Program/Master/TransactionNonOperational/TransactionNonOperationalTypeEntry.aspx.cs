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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TransactionNonOperationalTypeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.TRANSACTION_NON_OPERATIONAL_TYPE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;

               vTransactionNonOperationalType entity = BusinessLayer.GetvTransactionNonOperationalTypeList(string.Format("TransactionNonOperationalTypeID = {0}", ID)).FirstOrDefault();

                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtTransactionNonOperationalTypeCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTransactionNonOperationalTypeCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtTransactionNonOperationalTypeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTransactionCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTransactionName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnGLAccountID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccountNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtGLAccountName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnGLAccountIDDiscount, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountNoDiscount, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountNameDiscount, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnGLAccountIDAmortization, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountNoAmortization, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountNameAmortization, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRevenueCostCenterCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRevenueCostCenterName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vTransactionNonOperationalType entity)
        {
            txtTransactionNonOperationalTypeCode.Text = entity.TransactionNonOperationalTypeCode;
            txtTransactionNonOperationalTypeName.Text = entity.TransactionNonOperationalTypeName;

            hdnTransactionCode.Value = entity.TransactionCode;
            txtTransactionCode.Text = entity.TransactionCode;
            txtTransactionName.Text = entity.TransactionName;

            if (entity.GLAccountID != null && entity.GLAccountID != 0)
            {
                hdnGLAccountID.Value = entity.GLAccountID.ToString();
                txtGLAccountNo.Text = entity.GLAccountNo;
                txtGLAccountName.Text = entity.GLAccountName;
            }

            if (entity.GLAccountIDDiscount != null && entity.GLAccountIDDiscount != 0)
            {
                hdnGLAccountIDDiscount.Value = entity.GLAccountIDDiscount.ToString();
                txtGLAccountNoDiscount.Text = entity.GLAccountNoDiscount;
                txtGLAccountNameDiscount.Text = entity.GLAccountNameDiscount;
            }

            if (entity.GLAccountIDAmortization != null && entity.GLAccountIDAmortization != 0)
            {
                hdnGLAccountIDAmortization.Value = entity.GLAccountIDAmortization.ToString();
                txtGLAccountNoAmortization.Text = entity.GLAccountNoAmortization;
                txtGLAccountNameAmortization.Text = entity.GLAccountNameAmortization;
            }

            if (entity.RevenueCostCenterID != null && entity.RevenueCostCenterID != 0)
            {
                hdnRevenueCostCenterID.Value = entity.RevenueCostCenterID.ToString();
                txtRevenueCostCenterCode.Text = entity.RevenueCostCenterCode;
                txtRevenueCostCenterName.Text = entity.RevenueCostCenterName;
            }

            if (entity.BusinessPartnerID != null && entity.BusinessPartnerID != 0)
            {
                hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
                txtBusinessPartnerCode.Text = entity.BusinessPartnerCode;
                txtBusinessPartnerName.Text = entity.BusinessPartnerName;
            }

            txtRemarks.Text = entity.Remarks;

            chkIsUsedPPN.Checked = entity.IsUsedPPN;
            chkIsUsedPPH.Checked = entity.IsUsedPPH;
        }

        private void ControlToEntity(TransactionNonOperationalType entity)
        {
            entity.TransactionNonOperationalTypeCode = txtTransactionNonOperationalTypeCode.Text;
            entity.TransactionNonOperationalTypeName = txtTransactionNonOperationalTypeName.Text;

            entity.TransactionCode = hdnTransactionCode.Value;

            entity.GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);

            if (hdnGLAccountIDDiscount.Value != "")
            {
                entity.GLAccountIDDiscount = Convert.ToInt32(hdnGLAccountIDDiscount.Value);
            }

            if (hdnGLAccountIDAmortization.Value != "")
            {
                entity.GLAccountIDAmortization = Convert.ToInt32(hdnGLAccountIDAmortization.Value);
            }

            if (hdnRevenueCostCenterID.Value != "")
            {
                entity.RevenueCostCenterID = Convert.ToInt32(hdnRevenueCostCenterID.Value);
            }

            if (hdnBusinessPartnerID.Value != "")
            {
                entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);
            }
            
            entity.Remarks = txtRemarks.Text;

            entity.IsUsedPPN = chkIsUsedPPN.Checked;
            entity.IsUsedPPH = chkIsUsedPPH.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("TransactionNonOperationalTypeCode = '{0}'", txtTransactionNonOperationalTypeCode.Text);
            List<TransactionNonOperationalType> lst = BusinessLayer.GetTransactionNonOperationalTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Transaction Non Operational Type With Code " + txtTransactionNonOperationalTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransactionNonOperationalTypeDao entityDao = new TransactionNonOperationalTypeDao(ctx);
            try
            {
                TransactionNonOperationalType entity = new TransactionNonOperationalType();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransactionNonOperationalTypeDao entityDao = new TransactionNonOperationalTypeDao(ctx);
            try
            {
                int TransactionNonOperationalTypeID = Convert.ToInt32(hdnID.Value);
                TransactionNonOperationalType entity = entityDao.Get(TransactionNonOperationalTypeID);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
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