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
    public partial class LPenerimaanPiutang : BaseCustomDailyPotraitRpt
    {
        public LPenerimaanPiutang()
        {
            InitializeComponent();
        }

        private int detailID = 0, oldDetailID = 0;
        private decimal totalAmountCD = 0, totalAmount = 0;

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriode.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

        private void xrTableCell18_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("GCTransactionStatus") != null)
            {
                String GCTransactionStatus = GetCurrentColumnValue("GCTransactionStatus").ToString();
                if (GCTransactionStatus == Constant.TransactionStatus.VOID)
                {
                    detailID = Convert.ToInt32(GetCurrentColumnValue("ARReceivingID"));

                    if (detailID != oldDetailID)
                    {
                        decimal receivingamount = Convert.ToDecimal(GetCurrentColumnValue("ReceivingAmount")) * -1;
                        String receivingamount1 = Convert.ToString(receivingamount);
                        xrTableCell18.Text = receivingamount.ToString("N2");
                    }
                }
                else
                {
                    detailID = Convert.ToInt32(GetCurrentColumnValue("ARReceivingID"));

                    if (detailID != oldDetailID)
                    {
                        totalAmount += Convert.ToDecimal(GetCurrentColumnValue("ReceivingAmount"));
                        totalAmountCD += Convert.ToDecimal(GetCurrentColumnValue("ReceivingAmount"));
                    }
                }
            }
        }

        private void cReceivingCreatedDate_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cReceivingCreatedDate.Text = totalAmountCD.ToString("N2");
        }

        private void cReceivingGT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cReceivingGT.Text = totalAmount.ToString("N2");
        }

        private void GroupFooter3_AfterPrint(object sender, EventArgs e)
        {
            totalAmountCD = 0;
        }

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if ((GetCurrentColumnValue("GCTransactionStatus") != null))
            {
                String GCTransactionStatus = GetCurrentColumnValue("GCTransactionStatus").ToString();
                if (GCTransactionStatus == Constant.TransactionStatus.VOID)
                {
                    xrTable2.ForeColor = Color.Red;
                    xrTable7.Visible = false;
                }
                else
                {
                    xrTable2.ForeColor = Color.Black;
                    xrTable7.Visible = true;
                }
            }
        }

    }
}
