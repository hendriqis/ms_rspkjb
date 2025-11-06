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
    public partial class BNewPaymentReceipt24Dt : DevExpress.XtraReports.UI.XtraReport
    {
        public BNewPaymentReceipt24Dt()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPaymentReceiptCustomDetailTransactionDiscount> lst)
        {
            this.DataSource = lst;
        }
        
        private void tableDetail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String ChargesParamedicName = Convert.ToString(GetCurrentColumnValue("ChargesParamedicName"));
            if (ChargesParamedicName == "")
            {
                tableDetail.Visible = false;
            }
            else
            {
                tableDetail.Visible = true;
            }
        }

    }
}
