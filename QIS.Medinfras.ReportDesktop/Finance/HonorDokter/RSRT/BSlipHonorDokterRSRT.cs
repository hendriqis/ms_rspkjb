using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSlipHonorDokterRSRT : BaseRpt
    {
        
        public BSlipHonorDokterRSRT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            String HealthcareID = appSession.HealthcareID;
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", HealthcareID)).FirstOrDefault();
            string City = oHealthcare.City;
            vTransRevenueSharingSummaryHd oData = BusinessLayer.GetvTransRevenueSharingSummaryHdList(param[0]).FirstOrDefault();
            string paramedicName ="";
            string Hal = "";
            if (oData != null) {
               paramedicName =  oData.ParamedicName;
               Hal = string.Format("REKAP HONDOK {0}", GetMonthFormat(oData.RSSummaryDate));
            }
            lblHal.Text = Hal;
            lblMenyerahkan.Text = appSession.UserFullName;
            lblTanggalTerima.Text = string.Format("{0}, {1}", City, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
            lblTTD.Text = paramedicName;
            base.InitializeReport(param);
        }
        public static String GetMonthFormat(DateTime date)
        {
            string[] bulan = { "JANUARI", "FEBUARI", "MARET", "APRIL", "MEI", "JUNI", "JULI", "AGUSTUS", "SEPTEMBER", "OKTOBER", "NOVEMBER", "DESEMBER" };
            string Month = bulan[(int)date.Month - 1];
            ////string days = date.Day.ToString();
            string years = date.Year.ToString();
            return string.Format("{0} {1}", Month, years);
        }
        
    }
}
