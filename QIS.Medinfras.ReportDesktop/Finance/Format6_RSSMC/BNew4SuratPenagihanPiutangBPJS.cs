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
    public partial class BNew4SuratPenagihanPiutangBPJS : BaseCustomDailyPotraitRpt
    {
        public BNew4SuratPenagihanPiutangBPJS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            ARInvoiceHd entity = BusinessLayer.GetARInvoiceHdList(param[0]).FirstOrDefault();
            Healthcare healthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);
            Address address = BusinessLayer.GetAddress(Convert.ToInt32(healthcare.AddressID));
            cHealthcareEmail.Text = address.Email;
            lblHeaderTTD1.Text = string.Format("{0}, {1}", address.City, entity.ARInvoiceDateInString);

            lblTerbilang.Text = string.Format("# {0} #", entity.cfTotalClaimedAmountInStringInd);

            SettingParameter ttd1 = BusinessLayer.GetSettingParameter(Constant.SettingParameter.FN_BAGIAN_PENAGIHAN);
            SettingParameter ttd2 = BusinessLayer.GetSettingParameter(Constant.SettingParameter.DIREKTUR_KEUANGAN);

            lblCaptionTTD1.Text = ttd1.ParameterName;
            lblTTD1.Text = ttd1.ParameterValue;

            lblCaptionTTD2.Text = ttd2.ParameterName;
            lblTTD2.Text = ttd2.ParameterValue;

            SettingParameter faxNo = BusinessLayer.GetSettingParameter(Constant.SettingParameter.FN_FAX_NO_AR);
            lblNoFaxPenagihan.Text = faxNo.ParameterValue;

            base.InitializeReport(param);
        }

    }
}
