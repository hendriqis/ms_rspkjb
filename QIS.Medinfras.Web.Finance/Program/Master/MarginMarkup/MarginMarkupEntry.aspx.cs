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
    public partial class MarginMarkupEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.MARKUP_MARGIN;
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
                MarginMarkupHd entity = BusinessLayer.GetMarginMarkupHd(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtMarkupCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMarkupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMarkupName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsMarkupInPercentage, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(MarginMarkupHd entity)
        {
            txtMarkupCode.Text = entity.MarkupCode;
            txtMarkupName.Text = entity.MarkupName;
            chkIsMarkupInPercentage.Checked = entity.IsMarkupInPercentage;   
        }

        private void ControlToEntity(MarginMarkupHd entity)
        {
            entity.MarkupCode = txtMarkupCode.Text;
            entity.MarkupName = txtMarkupName.Text;
            entity.IsMarkupInPercentage = chkIsMarkupInPercentage.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("MarkupCode = '{0}'", txtMarkupCode.Text);
            List<MarginMarkupHd> lst = BusinessLayer.GetMarginMarkupHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Margin Markup With Code " + txtMarkupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("MarkupCode = '{0}' AND MarkupID != {1}", txtMarkupCode.Text, hdnID.Value);
            List<MarginMarkupHd> lst = BusinessLayer.GetMarginMarkupHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Margin Markup With Code " + txtMarkupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            MarginMarkupHdDao entityDao = new MarginMarkupHdDao(ctx);
            bool result = false;
            try
            {
                MarginMarkupHd entity = new MarginMarkupHd();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetMarginMarkupHdMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                MarginMarkupHd entity = BusinessLayer.GetMarginMarkupHd(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMarginMarkupHd(entity);
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