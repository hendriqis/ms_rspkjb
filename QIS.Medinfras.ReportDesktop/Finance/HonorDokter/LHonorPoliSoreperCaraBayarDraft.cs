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
    public partial class LHonorPoliSoreperCaraBayarDraft : BaseCustom2DailyPotraitRpt
    {
        public LHonorPoliSoreperCaraBayarDraft()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
         {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            String HealthcareID = AppSession.UserLogin.HealthcareID;

            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            List<SettingParameter> lstSetvar = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')",
                                                                                        Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN,
                                                                                        Constant.SettingParameter.FN_KASIE_PENGELOLAAN_UTANG));

            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}')",
                                                                                                Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN,
                                                                                                Constant.SettingParameter.FN_KASIE_PENGELOLAAN_UTANG));

            lblTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            lblTTD1.Text = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_KASIE_PENGELOLAAN_UTANG).FirstOrDefault().ParameterValue;
            lblTTD2.Text = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_KASIE_PENGELOLAAN_UTANG).FirstOrDefault().ParameterName;

            lblMenyetujuiTTD1.Text = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault().ParameterValue;
            lblMenyetujuiTTD2.Text = lstSetvar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault().ParameterName;

            base.InitializeReport(param);
        }

    }
}
