using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Globalization;
using System.Linq;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenggunaanMorphinPethidin : BaseDailyLandscapeRpt
    {
        public LPenggunaanMorphinPethidin()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            Healthcare _temp = BusinessLayer.GetHealthcare(appSession.HealthcareID);
            lblDirectorName.Text = _temp.DirectorName;
            lblDirectorCaption.Text += string.Format(" RS {0}", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_temp.HealthcareName.ToLower()));
            List<SettingParameterDt> setPar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", appSession.HealthcareID, Constant.SettingParameter.PHARMACIST, Constant.SettingParameter.PHARMACIST_LICENSE_NO));
            lblDatePlace.Text = string.Format("Belitang, {0}", DateTime.Now.ToString("dd-MMM-yyyy"));
            SettingParameterDt pharmacistName = setPar.Where(t => t.ParameterCode == Constant.SettingParameter.PHARMACIST).FirstOrDefault();
            SettingParameterDt pharmacistSIK = setPar.Where(t => t.ParameterCode == Constant.SettingParameter.PHARMACIST_LICENSE_NO).FirstOrDefault();
            lblResponsiblePerson.Text = pharmacistName.ParameterValue;
            lblNoSIK.Text = pharmacistSIK.ParameterValue;

            base.InitializeReport(param);
        }

        Int16 RecordNo = 0;

        private void xrTableCell14_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            RecordNo += 1;
            ((XRLabel)sender).Text = RecordNo.ToString();
        }

    }
}
