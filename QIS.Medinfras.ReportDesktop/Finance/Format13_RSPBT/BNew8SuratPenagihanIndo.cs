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
    public partial class BNew8SuratPenagihanIndo : BaseCustomDailyPotraitRpt
    {
        public BNew8SuratPenagihanIndo()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string ARInvoiceID = param[0];
            string type = param[2];
            string diskon = param[3];
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            List<SettingParameterDt> setvarAR = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                                    appSession.HealthcareID,
                                                                    Constant.SettingParameter.FN_EMAIL_AR,
                                                                    Constant.SettingParameter.FN_FAX_NO_AR));
            lblEmailPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_EMAIL_AR).FirstOrDefault().ParameterValue;
            lblfax.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_FAX_NO_AR).FirstOrDefault().ParameterValue;

            lblDate.Text = string.Format("{0}, {1}", h.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            ARInvoiceHd invoiceHd = BusinessLayer.GetARInvoiceHdList(ARInvoiceID).FirstOrDefault();
            if (type == "0")
            {
                lblInvoiceType.Text = "Invoice Penagihan Rawat Jalan";
            }
            else
            {
                lblInvoiceType.Text = "Invoice Penagihan Rawat Inap";
            }

            if (diskon == "0")
            {
                lblAmount.Text = string.Format("{0}", invoiceHd.TotalTransactionAmount.ToString("N2"));
            }
            else
            {
                lblAmount.Text = string.Format("{0}", invoiceHd.TotalClaimedAmount.ToString("N2"));
            }

            lblPernyataan.Text = string.Format("Bersama ini kami dari {0} mengirimkan tagihan perawatan pasien (detail rekapan pasien terlampir), sbb :", h.HealthcareName);
            lblPembayaran.Text = string.Format("Pembayaran tagihan tersebut bisa dibayarkan melalui rekening {0} :", h.HealthcareName);

            base.InitializeReport(param);
        }

    }
}
