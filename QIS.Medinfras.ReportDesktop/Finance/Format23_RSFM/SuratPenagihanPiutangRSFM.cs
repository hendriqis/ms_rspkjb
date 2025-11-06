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
    public partial class SuratPenagihanPiutangRSFM : BaseCustomDailyPotraitRpt
    {
        public SuratPenagihanPiutangRSFM()
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
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                                    appSession.HealthcareID,
                                                                    Constant.SettingParameter.FN_EMAIL_AR,
                                                                    Constant.SettingParameter.FN_FAX_NO_AR,
                                                                    Constant.SettingParameter.FN_BAGIAN_PENAGIHAN,
                                                                    Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN));

            lblContactPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_FAX_NO_AR).FirstOrDefault().ParameterValue;
            lblHal.Text = string.Format("Tagihan Piutang Pasien");
            lblARInvoiceNo.Text = invoiceHd.Remarks;

            lblKeterangan2.Text = string.Format("Pertama - tama kami sampaikan terima kasih atas kepercayaan dirawat di {0}. Bersama ini kami sampaikan dan mohon untuk dilunasi tagihan piutang pelayanan pasien {1} dengan Rincian Pembayaran sebagai berikut :" , h.HealthcareName, invoiceHd.BusinessPartnerName);
            lblTTD1.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault().ParameterValue;
            lblTTD2.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_BAGIAN_PENAGIHAN).FirstOrDefault().ParameterValue;
            lblKeterangan.Text = string.Format("Demikian surat pengajuan klaim dari kami semoga bisa diterima dengan baik dan segera ditindak lanjuti.");
            lblKeterangan3.Text = string.Format("Atas perhatian dan kerjasamanya kami ucapkan terima kasih.");

            base.InitializeReport(param);
        }

    }
}
