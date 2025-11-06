 using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BBeritaSerahTerimaBarangDanInventarisRSCK : BaseCustomDailyPotraitRpt
    {
        List<vFAAcceptanceDt> lst = new List<vFAAcceptanceDt>();

        public BBeritaSerahTerimaBarangDanInventarisRSCK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDtList(string.Format(
                "ParameterCode = '{0}'", Constant.SettingParameter.KASI_PENGELOLAAN_INVENTARIS)).FirstOrDefault();
            SettingParameter setvarhd = BusinessLayer.GetSettingParameterList(string.Format(
                "ParameterCode = '{0}'", setvardt.ParameterCode)).FirstOrDefault();

            lblTanggal.Text = string.Format("{0}, {1}", h.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblTTDApproved.Text = setvardt.ParameterValue;
            lblNameApproved.Text = setvarhd.ParameterName;


            lst = BusinessLayer.GetvFAAcceptanceDtList(param[0]);

            base.InitializeReport(param);
        }

        private void lblTerbilang_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string oFixedAssetName = Convert.ToString(GetCurrentColumnValue("FixedAssetName"));
            decimal amount = lst.Where(t => t.FixedAssetName == oFixedAssetName).Sum(x => x.ProcurementAmount);

            lblTerbilang.Text = Function.NumberWithPointInWordsInIndonesian(Convert.ToDouble(amount), true);
        }
    }
}
