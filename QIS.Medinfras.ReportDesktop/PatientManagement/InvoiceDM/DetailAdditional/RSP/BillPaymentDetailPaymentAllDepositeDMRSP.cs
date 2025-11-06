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
    public partial class BillPaymentDetailPaymentAllDepositeDMRSP : DevExpress.XtraReports.UI.XtraReport
    {
        public BillPaymentDetailPaymentAllDepositeDMRSP()
        {
            InitializeComponent();
        }

        protected decimal uangMuka = 0;
        protected decimal bayar = 0;
        protected decimal tempUM = 0;
        protected decimal tempBY = 0;
        protected int paramOld = 0;

        public void InitializeReport(List<vPatientPaymentHdDtRSP> lst)
        {
            //List<vPatientPaymentHd> lst = BusinessLayer.GetvPatientPaymentHdList(string.Format(
            //    "(RegistrationID = {0} AND GCTransactionStatus != '{1}' AND IsDeleted = 0)",
            //    RegistrationID, Constant.TransactionStatus.VOID));

            //uangMuka += lst.Sum(a => a.DownPaymentOut);
            //bayar += lst.Sum(a => a.NotInDownPayment);
            
          ///  List<vPatientPaymentHdDt> lst2 = lst.OrderBy(a => a.PaymentID).ToList();
            List<vPatientPaymentHdDtRSP> lstDeposit = lst.OrderBy(a => a.GCPaymentType == Constant.PaymentType.DEPOSIT_IN || a.GCPaymentType == Constant.PaymentType.DEPOSIT_OUT).ToList();

            foreach (vPatientPaymentHdDtRSP dt in lstDeposit)
            {
                tempUM += dt.DownPaymentOut;
                tempBY += dt.NotInDownPayment;
                paramOld = dt.PaymentID;
                //if (dt.PaymentID != paramOld)
                //{
                //    tempUM += dt.DownPaymentOut;
                //    tempBY += dt.NotInDownPayment;
                //    paramOld = dt.PaymentID;
                //}
            }
            //tempUM = lst2.Sum(p => p.DownPaymentOut);
            //tempBY = lst2.Sum(p => p.NotInDownPayment);

            bayar = tempBY;
            uangMuka = tempUM;

            cGTReceive.Text = (bayar - uangMuka).ToString("N2");

            this.DataSource = lst;
        }

        private void tabDetail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("GCPaymentType") != null)
            {
                if (GetCurrentColumnValue("GCPaymentType").ToString() != Constant.PaymentType.AR_PAYER)
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                }
            }
            else
            {
                e.Cancel = true;
            } 
        }
    }
}
