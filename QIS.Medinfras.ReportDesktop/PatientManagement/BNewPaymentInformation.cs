using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewPaymentInformation : BaseCustomDailyPotraitRpt
    {
        public BNewPaymentInformation()
        {
            InitializeComponent();
        }

        protected decimal uangMuka = 0;
        protected decimal bayar = 0;
        protected decimal tempUM = 0;
        protected decimal tempBY = 0;
        protected int paramOld = 0;

        protected decimal gt = 0;


        public override void InitializeReport(string[] param)
        {
        //public void InitializeReport(string[] param)
        //{
           List<vPatientPaymentHdDt> lst = BusinessLayer.GetvPatientPaymentHdDtList(string.Format(
                "{0} AND GCTransactionStatus != '{1}'", param[0].ToString() , Constant.TransactionStatus.VOID));

            //uangMuka += lst.Sum(a => a.DownPaymentOut);
            //bayar += lst.Sum(a => a.NotInDownPayment);
                //List<vPatientPaymentHdDt> lst = BusinessLayer.GetvPatientPaymentHdDtList(string.Format())
            foreach (vPatientPaymentHdDt dt in lst)
            {
                if (dt.PaymentID != paramOld)
                {
                    tempUM += dt.DownPaymentOut;
                    tempBY += dt.NotInDownPayment;
                    paramOld = dt.PaymentID;
                }
            }

            bayar = tempBY;
            uangMuka = tempUM;

            cGT.Text = (bayar - uangMuka).ToString("N2");

            ////cGT.Text = gt.ToString("N2");

            //this.DataSource = lst;
            base.InitializeReport(param);
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
