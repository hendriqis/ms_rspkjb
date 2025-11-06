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
    public partial class BillPaymentDetailDiscountByBillingRSP : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailDiscountByBillingRSP()
        {
            InitializeComponent();
        }

        public void InitializeReport(int RegistrationID, String PatientBillingID)
        {
            List<vPatientBillDiscount> lst = BusinessLayer.GetvPatientBillDiscountList(string.Format(
                "(RegistrationID = {0} AND PatientBillingID IN ({1}) AND GCTransactionStatus != '{2}' AND IsDeleted = 0)",
                RegistrationID, PatientBillingID.Substring(1), Constant.TransactionStatus.VOID));

            this.DataSource = lst;
        }

    }
}
