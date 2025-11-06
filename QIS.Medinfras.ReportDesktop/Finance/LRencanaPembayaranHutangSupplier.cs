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
    public partial class LRencanaPembayaranHutangSupplier : BaseCustomDailyPotraitRpt 
    {
        public LRencanaPembayaranHutangSupplier()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriode.Text = string.Format("Periode Rencana Bayar : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                     "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                     oHealthcare.HealthcareID,
                                                                     Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE));

            var isUsedProductLine = setvar.Where(a => a.ParameterCode == Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE).FirstOrDefault().ParameterValue;

            if (isUsedProductLine == "0")
            {
                xrPageBreak1.Visible = false;
            }

            base.InitializeReport(param);
        }
    }
}
