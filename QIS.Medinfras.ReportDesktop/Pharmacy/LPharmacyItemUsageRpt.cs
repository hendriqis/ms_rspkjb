using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPharmacyItemUsageRpt : BaseDailyPortraitRpt
    {
        public LPharmacyItemUsageRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterExpression = string.Format("HealthcareServiceUnitID = {0}", param[2]);
            string[] temp = param[0].Split(';');
            string[] temp1 = param[1].Split(';');
            vHealthcareServiceUnit entity = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression)[0];
            lblPenunjangMedis.Text = string.Format("Farmasi : {0}", entity.ServiceUnitName);
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            lblTimePeriod.Text = string.Format("Jam : {0} s/d {1}", temp1[0].ToString(), temp1[1].ToString());
            base.InitializeReport(param);
        }
    }
}
