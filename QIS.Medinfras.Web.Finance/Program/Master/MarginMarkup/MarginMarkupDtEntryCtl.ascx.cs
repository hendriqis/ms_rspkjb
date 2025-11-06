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
    public partial class MarginMarkupDtEntryCtl : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnMarkupIDCtl.Value = paramInfo[0];
            hdnSequenceNoCtl.Value = paramInfo[1];

            MarginMarkupHd entityHd = BusinessLayer.GetMarginMarkupHd(Convert.ToInt32(hdnMarkupIDCtl.Value));
            txtMarkupCodeCtl.Text = entityHd.MarkupCode;
            txtMarkupNameCtl.Text = entityHd.MarkupName;

            if (hdnSequenceNoCtl.Value != "0")
            {
                IsAdd = false;
                MarginMarkupDt entity = BusinessLayer.GetMarginMarkupDt(Convert.ToInt32(hdnMarkupIDCtl.Value), Convert.ToInt16(hdnSequenceNoCtl.Value));
                txtSequenceNoCtl.Text = entity.SequenceNo.ToString();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
                txtStartingValue.Text = "0";
                txtEndingValue.Text = "0";
                txtMarkupAmount.Text = "0";
                txtMarkupAmount2.Text = "0";
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtStartingValue, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtEndingValue, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMarkupAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMarkupAmount2, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(MarginMarkupDt entity)
        {
            hdnStartingValueCtl.Value = entity.StartingValue.ToString();
            txtStartingValue.Text = entity.cfStartingValueInString;
            hdnEndingValueCtl.Value = entity.EndingValue.ToString();
            txtEndingValue.Text = entity.cfEndingValueInString;
            txtMarkupAmount.Text = entity.cfMarkupAmountInString;
            txtMarkupAmount2.Text = entity.cfMarkupAmount2InString;
        }

        private void ControlToEntity(MarginMarkupDt entity)
        {
            entity.StartingValue = Convert.ToDecimal(txtStartingValue.Text);
            entity.EndingValue = Convert.ToDecimal(txtEndingValue.Text);
            entity.MarkupAmount = Convert.ToDecimal(txtMarkupAmount.Text);
            entity.MarkupAmount2 = Convert.ToDecimal(txtMarkupAmount2.Text);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MarginMarkupDtDao entityDao = new MarginMarkupDtDao(ctx);
            try
            {
                MarginMarkupDt entity = new MarginMarkupDt();
                entity.MarkupID = Convert.ToInt32(hdnMarkupIDCtl.Value);
                entity.SequenceNo = Convert.ToInt16(BusinessLayer.GetMarginMarkupDtMaxSequenceNo(string.Format("MarkupID = {0}", hdnMarkupIDCtl.Value)) + 1);
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
            MarginMarkupDtDao entityDao = new MarginMarkupDtDao(ctx);
            try
            {
                MarginMarkupDt entity = BusinessLayer.GetMarginMarkupDt(Convert.ToInt32(hdnMarkupIDCtl.Value), Convert.ToInt16(hdnSequenceNoCtl.Value));
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