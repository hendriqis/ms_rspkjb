using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryPayReceiptDtCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Int32 paymentReceiptID = Convert.ToInt32(param);
            BindGrid(paymentReceiptID);
        }

        private void BindGrid(int paymentReceiptID)
        {
            List<vPatientPaymentHd> lst = BusinessLayer.GetvPatientPaymentHdList(string.Format("PaymentReceiptID = {0} AND IsDeleted = 0", paymentReceiptID));
            lvwView.DataSource = lst;
            lvwView.DataBind();

            //hdnTotalPayment.Value = lst.Sum(p => p.TotalPaymentAmount).ToString("N");
            hdnTotalPayment.Value = lst.Sum(p => p.ReceiptAmount).ToString("N");
        }
    }
}