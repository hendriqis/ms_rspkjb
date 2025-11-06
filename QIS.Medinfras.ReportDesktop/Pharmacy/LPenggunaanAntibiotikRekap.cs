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
    public partial class LPenggunaanAntibiotikRekap : BaseCustomDailyLandscapeA3Rpt
    {
        public LPenggunaanAntibiotikRekap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            Healthcare _temp = BusinessLayer.GetHealthcare(appSession.HealthcareID);
            Address _AddressHealthcare = BusinessLayer.GetAddress(Convert.ToInt32(_temp.AddressID));
            List<SettingParameterDt> setPar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')", appSession.HealthcareID, Constant.SettingParameter.PHARMACIST, Constant.SettingParameter.PHARMACIST_LICENSE_NO, Constant.SettingParameter.DIREKTUR_YANMED));
            lblDirectorName.Text = setPar.Where(t => t.ParameterCode == Constant.SettingParameter.DIREKTUR_YANMED).FirstOrDefault().ParameterValue.ToString();
            lblDirectorCaption.Text += string.Format(" RS {0}", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_temp.HealthcareName.ToLower()));
            lblDatePlace.Text = string.Format("{0}, {1}", _AddressHealthcare.City, DateTime.Now.ToString("dd-MMM-yyyy"));
            SettingParameterDt pharmacistName = setPar.Where(t => t.ParameterCode == Constant.SettingParameter.PHARMACIST).FirstOrDefault();
            SettingParameterDt pharmacistSIK = setPar.Where(t => t.ParameterCode == Constant.SettingParameter.PHARMACIST_LICENSE_NO).FirstOrDefault();
            lblResponsiblePerson.Text = pharmacistName.ParameterValue;
            lblNoSIK.Text = pharmacistSIK.ParameterValue;

            base.InitializeReport(param);
        }
    }
}
