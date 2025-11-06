using System.Collections.Generic;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Linq;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OutstandingARInvoicePatient : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] lstParam = param.Split('|');
            hdnMRNCtl.Value = lstParam[0];

            txtPatient.Text = string.Format("({0}) {1}", lstParam[1], lstParam[2]);
            BindGridView(param);
        }

        private void BindGridView(string param)
        {
            string filterExpression = string.Format("MRN = '{0}' AND TransactionCode = '{1}' AND TotalPaymentAmount < TotalClaimedAmount ORDER BY ARInvoiceNo", hdnMRNCtl.Value, Constant.TransactionCode.AR_INVOICE_PATIENT);
            List<vARInvoiceHd> lstEntity = BusinessLayer.GetvARInvoiceHdList(filterExpression);

            txtTotalClaimedAmount.Text = lstEntity.Sum(a => a.TotalClaimedAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPaymentAmount.Text = lstEntity.Sum(a => a.TotalPaymentAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalSaldoAmount.Text = lstEntity.Sum(a => a.TotalSaldoAmount).ToString(Constant.FormatString.NUMERIC_2);

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

    }
}