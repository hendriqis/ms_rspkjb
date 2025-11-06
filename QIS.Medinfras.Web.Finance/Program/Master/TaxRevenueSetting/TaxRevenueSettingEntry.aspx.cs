using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TaxRevenueSettingEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.TAX_REVENUE_SETTING;
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
                TaxRevenueSetting entity = BusinessLayer.GetTaxRevenueSetting(Convert.ToInt32(ID));
                EntityToControl(entity);
                txtTaxRevenueSettingName.Focus();
            }
            else
            {
                IsAdd = true;
                txtTaxRevenueSettingCode.Focus();
            }

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TAX_REVENUE_FORMULA_TYPE));
            Methods.SetRadioButtonListField<StandardCode>(rblFormulaType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.TAX_REVENUE_FORMULA_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            rblFormulaType.SelectedIndex = 0;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTaxRevenueSettingCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTaxRevenueSettingName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(rblFormulaType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(TaxRevenueSetting entity)
        {
            txtTaxRevenueSettingCode.Text = entity.TaxRevenueSettingCode;
            txtTaxRevenueSettingName.Text = entity.TaxRevenueSettingName;
            rblFormulaType.SelectedValue = entity.GCTaxFormulaType;
            txtNotes.Text = entity.Remarks;
        }

        private void ControlToEntity(TaxRevenueSetting entity)
        {
            entity.TaxRevenueSettingCode = txtTaxRevenueSettingCode.Text;
            entity.TaxRevenueSettingName = txtTaxRevenueSettingName.Text;
            entity.GCTaxFormulaType = rblFormulaType.SelectedValue;
            entity.Remarks = txtNotes.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("TaxRevenueSettingCode = '{0}'", txtTaxRevenueSettingCode.Text);
            List<TaxRevenueSetting> lst = BusinessLayer.GetTaxRevenueSettingList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Setting With Code " + txtTaxRevenueSettingCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("TaxRevenueSettingCode = '{0}' AND TaxRevenueSettingID != {1}", txtTaxRevenueSettingCode.Text, hdnID.Value);
            List<TaxRevenueSetting> lst = BusinessLayer.GetTaxRevenueSettingList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Setting With Code " + txtTaxRevenueSettingCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TaxRevenueSettingDao entityDao = new TaxRevenueSettingDao(ctx);
            try
            {
                TaxRevenueSetting entity = new TaxRevenueSetting();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetTaxRevenueSettingMaxID(ctx).ToString();

                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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
            TaxRevenueSettingDao entityDao = new TaxRevenueSettingDao(ctx);
            try
            {
                TaxRevenueSetting entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
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