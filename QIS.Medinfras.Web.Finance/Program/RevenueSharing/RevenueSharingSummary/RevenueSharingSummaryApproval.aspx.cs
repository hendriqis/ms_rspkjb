using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingSummaryApproval : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_SUMMARY_APPROVAL;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetRevenueSharingFilterExpression()
        {
            return string.Format("ParamedicID = {0}", AppSession.ParamedicID);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            txtApprovalDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnParamedicID.Value = AppSession.ParamedicID.ToString();

            txtPeriodFrom.Text = DateTime.Now.AddMonths(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();

        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = OnGetRevenueSharingFilterExpression();
            filterExpression += string.Format(" AND RSSummaryDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtPeriodFrom), Helper.GetDatePickerValue(txtPeriodTo));
            filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.OPEN);
            List<vTransRevenueSharingSummaryHd> lstEntity = BusinessLayer.GetvTransRevenueSharingSummaryHdList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingSummaryHdDao entitySummaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);

            try
            {
                string lstRSSummaryID = hdnSelectedMember.Value.Substring(1);

                string filterExpression = string.Format("RSSummaryID IN ({0})", lstRSSummaryID);

                List<TransRevenueSharingSummaryHd> lst = BusinessLayer.GetTransRevenueSharingSummaryHdList(filterExpression, ctx);
                foreach (TransRevenueSharingSummaryHd entity in lst)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entity.ApprovedBy = AppSession.UserLogin.UserID;
                    entity.ApprovedDate = Helper.GetDatePickerValue(txtApprovalDate.Text);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entitySummaryHdDao.Update(entity);
                }

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