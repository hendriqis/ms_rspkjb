using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalDocumentCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            List<String> lstParam = param.Split('|').ToList();
            hdnGLAccount.Value = lstParam[0];
            hdnSubLedger.Value = lstParam[1];
            hdnReferenceNo.Value = lstParam[2];

            string filterExpression = String.Format("GLAccount = {0} AND IsDeleted = 0", hdnGLAccount.Value);
            if (lstParam[1] != "" && lstParam[1] != "0")
            {
                filterExpression += String.Format(" AND ISNULL(SubLedger,0) = {0}", hdnSubLedger.Value);
            }
            else
            {
                filterExpression += String.Format(" AND SubLedger IS NULL");
            }
            filterExpression += String.Format(" AND ReferenceNo = '{0}'", hdnReferenceNo.Value);
            hdnFilterExpression.Value = filterExpression;
            List<vGLBalanceDtDocument> lstEntity = BusinessLayer.GetvGLBalanceDtDocumentList(filterExpression);

            if (lstEntity.Count > 0) 
            {
                txtReferenceNo.Text = hdnReferenceNo.Value;
                txtGLAccountName.Text = lstEntity.LastOrDefault().GLAccountName;
                txtSubLedgerName.Text = lstEntity.LastOrDefault().SubLedgerName;

                txtTotalDebit.Text = lstEntity.Sum(a => a.BalanceIN).ToString(Constant.FormatString.NUMERIC_2);
                txtTotalKredit.Text = lstEntity.Sum(a => a.BalanceOUT).ToString(Constant.FormatString.NUMERIC_2);
                txtTotalSelisih.Text = lstEntity.Sum(a => a.BalanceEND).ToString(Constant.FormatString.NUMERIC_2);

                BindGridView();

                divCreatedBy.InnerHtml = lstEntity.LastOrDefault().CreatedByName;
                divCreatedDate.InnerHtml = lstEntity.LastOrDefault().CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                divLastUpdatedBy.InnerHtml = lstEntity.LastOrDefault().LastUpdatedByName;
                if (lstEntity.LastOrDefault().LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
                {
                    divLastUpdatedDate.InnerHtml = lstEntity.LastOrDefault().LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
                }
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("{0} AND GCTransactionStatus = '{1}'", hdnFilterExpression.Value, Constant.TransactionStatus.APPROVED);
            List<vGLTransactionDtCustom> lstEntity = BusinessLayer.GetvGLTransactionDtCustomList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}