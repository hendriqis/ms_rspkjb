using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LLaluLintasGiroKeBank : BaseCustomDailyPotraitRpt
    {
        Decimal totalAmount = 0;

        public LLaluLintasGiroKeBank()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriode.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            lblSignDate.Text = string.Format("{0}, {1}", h.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            SettingParameter svPresDir = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PRESIDEN_DIREKTUR);
            lblPresDirCaption.Text = svPresDir.ParameterName;
            lblPresDirName.Text = svPresDir.ParameterValue;

            SettingParameter svDirektur = BusinessLayer.GetSettingParameter(Constant.SettingParameter.DIREKTUR_YANMED);
            lblDirCaption.Text = svDirektur.ParameterName;
            lblDirName.Text = svDirektur.ParameterValue;

            SettingParameter svManager = BusinessLayer.GetSettingParameter(Constant.SettingParameter.MANAGER_KEUANGAN);
            lblManagerCaption.Text = svManager.ParameterName;
            lblManagerName.Text = svManager.ParameterValue;
            
            base.InitializeReport(param);
        }

        private void lblSummaryPage_SummaryReset(object sender, EventArgs e)
        {
            //totalAmount = 0;
        }

        private void lblSummaryPage_SummaryRowChanged(object sender, EventArgs e)
        {
            //totalAmount += Convert.ToDecimal(GetCurrentColumnValue("PaymentAmount"));
        }

        private void lblSummaryPage_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            //e.Result = totalAmount;
            //e.Handled = true;
        }

    }
}
