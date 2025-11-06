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
    public partial class BNew13SuratPenagihanPiutang : BaseCustomDailyPotraitRpt
    {
        public BNew13SuratPenagihanPiutang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            vARInvoiceHd3 invoiceHd = BusinessLayer.GetvARInvoiceHd3List(param[0]).FirstOrDefault();

            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN)).FirstOrDefault();

            List<SettingParameterDt> setvarAR = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                    appSession.HealthcareID,
                                                                    Constant.SettingParameter.FN_EMAIL_AR,
                                                                    Constant.SettingParameter.FN_FAX_NO_AR,
                                                                    Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN));
            lblEmailPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_EMAIL_AR).FirstOrDefault().ParameterValue;
            lblContactPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_FAX_NO_AR).FirstOrDefault().ParameterValue;

            lblARInvoiceNo.Text = invoiceHd.Remarks;

            if (invoiceHd.BusinessPartnerID == 175)
            {
                lblHal.Text = string.Format("Klaim Jasa Medis Bulan {0} {1}", invoiceHd.ARInvoiceMonth , invoiceHd.ARInvoiceDate.ToString("yyyy"));
                lblKeterangan2.Text = string.Format("Bersama ini kami kirimkan Klaim Jasa Medis Bulan {0} {1} atas nama", invoiceHd.ARInvoiceMonth, invoiceHd.ARInvoiceDate.ToString("yyyy"));
                lblBussinessPartnerName.Text = invoiceHd.BusinessPartnerName;
            }
            else
            {
                lblHal.Text = string.Format("Tagihan Piutang Pasien");
                lblKeterangan2.Text = string.Format("Bersama ini kami sampaikan dan mohon untuk dilunasi tagihan piutang pelayanan pasien {0} dengan rincian sebagai berikut :", invoiceHd.BusinessPartnerName);
                lblBussinessPartnerName.Visible = false;
            }


            lblTTD1.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault().ParameterValue;
            lblKeterangan.Text = string.Format("Kami sangat mengharapkan pelunasan kwitansi ini dalam waktu dekat, dan selambat - lambatnya 14 (Empat Belas) hari setelah tagihan ini diterima. Dan mohon kiranya dikirimkan kepada kami bukti pembayarannya bersama dengan rincian nama - nama yang telah dilunasi.");

            base.InitializeReport(param);
        }

    }
}
