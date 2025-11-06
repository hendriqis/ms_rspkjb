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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TaxRevenueDtEntryCtl : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTaxRevenueIDCtl.Value = paramInfo[0];
            hdnSequenceNoCtl.Value = paramInfo[1];

            TaxRevenueHd entityHd = BusinessLayer.GetTaxRevenueHd(Convert.ToInt32(hdnTaxRevenueIDCtl.Value));
            txtTaxRevenueCodeCtl.Text = entityHd.TaxRevenueCode;
            txtTaxRevenueNameCtl.Text = entityHd.TaxRevenueName;

            if (hdnSequenceNoCtl.Value != "0")
            {
                IsAdd = false;
                TaxRevenueDt entity = BusinessLayer.GetTaxRevenueDt(Convert.ToInt32(hdnTaxRevenueIDCtl.Value), Convert.ToInt16(hdnSequenceNoCtl.Value));
                txtSequenceNoCtl.Text = entity.SequenceNo.ToString();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
                txtStartingValue.Text = "0";
                txtEndingValue.Text = "0";
                txtTaxRevenueAmount.Text = "0";
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtStartingValue, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEndingValue, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTaxRevenueAmount, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(TaxRevenueDt entity)
        {
            hdnStartingValueCtl.Value = entity.StartingValue.ToString();
            txtStartingValue.Text = entity.cfStartingValueInString;
            hdnEndingValueCtl.Value = entity.EndingValue.ToString();
            txtEndingValue.Text = entity.cfEndingValueInString;
            txtTaxRevenueAmount.Text = entity.cfTaxAmountInString;
        }

        private void ControlToEntity(TaxRevenueDt entity)
        {
            entity.StartingValue = Convert.ToDecimal(txtStartingValue.Text);
            entity.EndingValue = Convert.ToDecimal(txtEndingValue.Text);
            entity.TaxAmount = Convert.ToDecimal(txtTaxRevenueAmount.Text);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TaxRevenueDtDao entityDao = new TaxRevenueDtDao(ctx);
            try
            {
                TaxRevenueDt entity = new TaxRevenueDt();
                entity.TaxRevenueID = Convert.ToInt32(hdnTaxRevenueIDCtl.Value);
                entity.SequenceNo = Convert.ToInt16(BusinessLayer.GetTaxRevenueDtMaxSequenceNo(string.Format("TaxRevenueID = {0}", hdnTaxRevenueIDCtl.Value)) + 1);
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TaxRevenueDtDao entityDao = new TaxRevenueDtDao(ctx);
            try
            {
                TaxRevenueDt entity = BusinessLayer.GetTaxRevenueDt(Convert.ToInt32(hdnTaxRevenueIDCtl.Value), Convert.ToInt16(hdnSequenceNoCtl.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                errMessage = ex.Message;
                result = false;
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