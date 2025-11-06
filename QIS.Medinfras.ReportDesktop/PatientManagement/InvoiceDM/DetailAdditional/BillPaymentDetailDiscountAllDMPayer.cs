using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BillPaymentDetailDiscountAllDMPayer : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailDiscountAllDMPayer()
        {
            InitializeComponent();
        }

        public void InitializeReport(int RegistrationID)
        {
            List<vPatientBillDiscount> lst = BusinessLayer.GetvPatientBillDiscountList(string.Format(
                "RegistrationID = {0} AND GCTransactionStatus != '{1}' AND IsDeleted = 0 AND PayerDiscountAmount != 0",
                RegistrationID, Constant.TransactionStatus.VOID));

            this.DataSource = lst;
        }

    }
}
