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
    public partial class LRekapitulasiPemotonganPPH21 : BaseCustom2DailyPotraitRpt
    {
        public LRekapitulasiPemotonganPPH21()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
             vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
             lblPeriod.Text = string.Format("Periode Tahun : {0}", param[0]);

             List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                     "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                     appSession.HealthcareID,
                                                                     Constant.SettingParameter.FN_HEALTHCARE_NAME_RECAPITULATION_PPH21_REPORT,
                                                                     Constant.SettingParameter.FN_NPWP_RECAPITULATION_PPH21_REPORT,
                                                                     Constant.SettingParameter.FN_USER_NAME_RECAPITULATION_PPH21_REPORT));
             lblHeaderTTD.Text = setvar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_HEALTHCARE_NAME_RECAPITULATION_PPH21_REPORT).FirstOrDefault().ParameterValue;
             var NPWP = setvar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_NPWP_RECAPITULATION_PPH21_REPORT).FirstOrDefault().ParameterValue;
             lblTTD.Text = setvar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_USER_NAME_RECAPITULATION_PPH21_REPORT).FirstOrDefault().ParameterValue;

             if (!String.IsNullOrEmpty(NPWP))
             {
                 lblNPWP.Text = string.Format("NPWP : {0}", NPWP);
             }
             else
             {
                 lblNPWP.Visible = false;
             }

             if (string.IsNullOrEmpty(lblHeaderTTD.Text) || string.IsNullOrEmpty(lblNPWP.Text) || string.IsNullOrEmpty(lblTTD.Text))
             {
                 lblTanggal.Visible = false;

             }
             else
             {
                 lblTanggal.Text = string.Format("{0}, {1} {2}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.MONTH_FORMAT_2), DateTime.Now.ToString(Constant.FormatString.YEAR_FORMAT));
             }
            
            base.InitializeReport(param);
        }

    }
}
