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
    public partial class TaxRevenueEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.TAX_REVENUE_RANGE;
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
                TaxRevenueHd entity = BusinessLayer.GetTaxRevenueHd(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtTaxRevenueCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTaxRevenueCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTaxRevenueName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsTaxRevenueInPercentage, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(TaxRevenueHd entity)
        {
            txtTaxRevenueCode.Text = entity.TaxRevenueCode;
            txtTaxRevenueName.Text = entity.TaxRevenueName;
            chkIsTaxRevenueInPercentage.Checked = entity.IsInPercentage;   
        }

        private void ControlToEntity(TaxRevenueHd entity)
        {
            entity.TaxRevenueCode = txtTaxRevenueCode.Text;
            entity.TaxRevenueName = txtTaxRevenueName.Text;
            entity.IsInPercentage = chkIsTaxRevenueInPercentage.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("TaxRevenueCode = '{0}'", txtTaxRevenueCode.Text);
            List<TaxRevenueHd> lst = BusinessLayer.GetTaxRevenueHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Tax Revenue With Code " + txtTaxRevenueCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("TaxRevenueCode = '{0}' AND TaxRevenueID != {1}", txtTaxRevenueCode.Text, hdnID.Value);
            List<TaxRevenueHd> lst = BusinessLayer.GetTaxRevenueHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Tax Revenue With Code " + txtTaxRevenueCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TaxRevenueHdDao entityDao = new TaxRevenueHdDao(ctx);
            try
            {
                TaxRevenueHd entity = new TaxRevenueHd();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetTaxRevenueHdMaxID(ctx).ToString();

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
            TaxRevenueHdDao entityDao = new TaxRevenueHdDao(ctx);
            try
            {
                TaxRevenueHd entity = BusinessLayer.GetTaxRevenueHd(Convert.ToInt32(hdnID.Value));
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