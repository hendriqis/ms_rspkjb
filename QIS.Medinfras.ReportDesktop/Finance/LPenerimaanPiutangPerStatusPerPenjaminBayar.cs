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
    public partial class LPenerimaanPiutangPerStatusPerPenjaminBayar : BaseCustomDailyPotraitRpt
    {
        public LPenerimaanPiutangPerStatusPerPenjaminBayar()
        {
            InitializeComponent();
        }

        private int detailID = 0, oldDetailID = 0;
        private decimal totalAmountBP = 0, totalAmountCD = 0, totalAmountPO = 0, totalAmount = 0;

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriode.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
                        
            base.InitializeReport(param);
        }

        private void xrTableCell18_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            detailID = Convert.ToInt32(GetCurrentColumnValue("ARReceivingID"));

            if (detailID != oldDetailID)
            {
                totalAmount += Convert.ToDecimal(GetCurrentColumnValue("ReceivingAmount"));
                totalAmountBP += Convert.ToDecimal(GetCurrentColumnValue("ReceivingAmount"));
                totalAmountCD += Convert.ToDecimal(GetCurrentColumnValue("ReceivingAmount"));
                totalAmountPO += Convert.ToDecimal(GetCurrentColumnValue("ReceivingAmount"));
            }
        }

        private void cReceivingBusinessPartner_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cReceivingBusinessPartner.Text = totalAmountBP.ToString("N2");
        }

        private void cReceivingCreatedDate_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cReceivingCreatedDate.Text = totalAmountCD.ToString("N2");
        }

        private void cReceivingPrintOrder_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cReceivingPrintOrder.Text = totalAmountPO.ToString("N2");
        }

        private void cReceivingGT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cReceivingGT.Text = totalAmount.ToString("N2");
        }

        private void GroupFooter2_AfterPrint(object sender, EventArgs e)
        {
            totalAmountBP = 0;
        }

        private void GroupFooter3_AfterPrint(object sender, EventArgs e)
        {
            totalAmountCD = 0;
        }

        private void GroupFooter4_AfterPrint(object sender, EventArgs e)
        {
            totalAmountPO = 0;
        }

        private void xrTable7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if ((GetCurrentColumnValue("GCTransactionStatus") != null))
            {
                String GCTransactionStatus = GetCurrentColumnValue("GCTransactionStatus").ToString();
                if (GCTransactionStatus == Constant.TransactionStatus.VOID)
                {
                    xrTable7.Visible = false;
                }
                else
                {
                    xrTable7.Visible = true;
                }
            }
        }
    }
}
