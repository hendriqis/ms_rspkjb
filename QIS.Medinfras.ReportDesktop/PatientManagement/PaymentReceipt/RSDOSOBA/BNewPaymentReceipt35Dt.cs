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
    public partial class BNewPaymentReceipt35Dt : DevExpress.XtraReports.UI.XtraReport
    {

        public BNewPaymentReceipt35Dt()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetPaymentReceiptCustomDetailTransaction1> lst)
        {
            decimal discountAmount = 0;
            decimal discountFinal = 0;
            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].DiscountAmount != 0)
                    {
                        discountAmount += lst[i].DiscountAmount;
                    }
                }

                discountFinal = discountAmount + lst.FirstOrDefault().BillDiscountAmount;
            }

            if (discountFinal != 0 || discountFinal != null)
            {
                lblDiscount.Text = discountFinal.ToString("N2");
            }
            else
            {
                string.Format("0.00");
            }
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
