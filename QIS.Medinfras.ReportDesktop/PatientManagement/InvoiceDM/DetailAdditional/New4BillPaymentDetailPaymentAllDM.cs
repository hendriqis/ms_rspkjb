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
    public partial class New4BillPaymentDetailPaymentAllDM : DevExpress.XtraReports.UI.XtraReport
    {
        public New4BillPaymentDetailPaymentAllDM()
        {
            InitializeComponent();
        }
        protected decimal uangMuka = 0;
        protected decimal bayar = 0;
        protected decimal tempUM = 0;
        protected decimal tempBY = 0;
        protected int paramOld = 0;
        protected bool Isbpjs; 

        public void InitializeReport(List<vPatientPaymentHdDt> lst, bool IsBpjsPayer)
        {
            Isbpjs = IsBpjsPayer;
            //List<vPatientPaymentHd> lst = BusinessLayer.GetvPatientPaymentHdList(string.Format(
            //    "(RegistrationID = {0} AND GCTransactionStatus != '{1}' AND IsDeleted = 0)",
            //    RegistrationID, Constant.TransactionStatus.VOID));

            //uangMuka += lst.Sum(a => a.DownPaymentOut);
            //bayar += lst.Sum(a => a.NotInDownPayment);

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

            cGTReceive.Text = (bayar - uangMuka).ToString("N2");

            if (!Isbpjs)
            {
                this.DataSource = lst;
            }
            else {
                List<vPatientPaymentHdDt> lstTemp = new List<vPatientPaymentHdDt>();
                lstTemp.Add(lst.FirstOrDefault());

                this.DataSource = lstTemp;
            }
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

                    if (Isbpjs == true)
                    {
                        string filter = String.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN, Constant.SettingParameter.FN_PENJAMIN_BPJS_KETENAGAKERJAAN);
                        List<SettingParameterDt> lstsetpar = BusinessLayer.GetSettingParameterDtList(filter);

                        string bpjskesehatan = lstsetpar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
                        string bpjsketenagakerjaan = lstsetpar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KETENAGAKERJAAN).FirstOrDefault().ParameterValue;

                        List<vPatientPaymentHdDt> lstEntityPayment = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("RegistrationID = {0} AND BusinessPartnerID IN ('{1}','{2}') AND GCTransactionStatus != '{3}'" , GetCurrentColumnValue("RegistrationID"), bpjskesehatan, bpjsketenagakerjaan, Constant.TransactionStatus.VOID));

                        cPayerName.Text = lstEntityPayment.FirstOrDefault().BusinessPartnerName;
                        cPayerAmount.Text = lstEntityPayment.FirstOrDefault().TotalPaymentAmount.ToString("N2");
                        //cReceiveAmount.Text = lstEntityPayment.Sum(t => t.TotalPaymentAmount).ToString("N2");

                    }
                    else
                    {
                        List<vPatientPaymentHdDt> lstEntityPayment = BusinessLayer.GetvPatientPaymentHdDtList(string.Format("RegistrationID = {0} AND BusinessPartnerID = {1} AND GCTransactionStatus != '{2}'", GetCurrentColumnValue("RegistrationID"), GetCurrentColumnValue("BusinessPartnerID"), Constant.TransactionStatus.VOID));
                        cPayerName.Text = lstEntityPayment.FirstOrDefault().BusinessPartnerName;
                        cPayerAmount.Text = lstEntityPayment.FirstOrDefault().PaymentAmount.ToString("N2");
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
            
        }

    }
}
