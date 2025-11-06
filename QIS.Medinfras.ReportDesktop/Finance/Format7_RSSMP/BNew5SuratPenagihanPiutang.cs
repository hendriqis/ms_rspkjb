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
    public partial class BNew5SuratPenagihanPiutang : BaseCustomDailyPotraitRpt
    {
        public BNew5SuratPenagihanPiutang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            ARInvoiceHd entity = BusinessLayer.GetARInvoiceHdList(param[0]).FirstOrDefault();
            Healthcare healthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);
            Address address = BusinessLayer.GetAddress(Convert.ToInt32(healthcare.AddressID));
            cHealthcareEmail.Text = address.Email;

            lblTerbilang.Text = string.Format("# {0} #", entity.cfTotalClaimedAmountInStringInd);

            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            SettingParameter ttd1 = BusinessLayer.GetSettingParameter(Constant.SettingParameter.FN_BAGIAN_PENAGIHAN);
            SettingParameter ttd2 = BusinessLayer.GetSettingParameter(Constant.SettingParameter.DIREKTUR_KEUANGAN);

            lblTanggal.Text = string.Format("{0}, {1}", h.City, entity.ARInvoiceDateInString);
            lblPenagih.Text = ttd1.ParameterValue;
            ttdDirektur.Text = ttd2.ParameterValue;

            base.InitializeReport(param);
        }

    }
}
