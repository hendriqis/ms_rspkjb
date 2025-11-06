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
    public partial class BKwitansiPenagihanPiutangRekapRSFM : BaseCustomDailyPotraitA5Rpt
    {
        public BKwitansiPenagihanPiutangRekapRSFM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string HealthcareID = "001";//patok dulu karna di console ga bs baca appsession
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(HealthcareID, setvar.ParameterCode);

            string ARInvoiceID = param[0]; 
            ARInvoiceHd arInvoiceHd = BusinessLayer.GetARInvoiceHdList(ARInvoiceID).FirstOrDefault();

            lblTanggal.Text = string.Format("{0}, {1}", h.City, arInvoiceHd.DocumentDate.ToString(Constant.FormatString.DATE_FORMAT));
            lblRemarks.Text = string.Format("Biaya Rawat Jalan dan Rawat Inap Pasien {0} yang berobat di {1} {2}", arInvoiceHd.PrintAsName, h.HealthcareName, h.City);

            base.InitializeReport(param);
        }

    }
}
