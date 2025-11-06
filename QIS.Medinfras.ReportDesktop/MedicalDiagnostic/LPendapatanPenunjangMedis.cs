using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPendapatanPenunjangMedis : BaseCustomDailyLandscapeRpt
    {
        public LPendapatanPenunjangMedis()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterExpression = String.Format("HealthcareServiceUnitID = {0}", Convert.ToInt32(param[2].ToString()));
            xrLabel4.Text = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression).FirstOrDefault().ServiceUnitName;
            base.InitializeReport(param);
        }
    }
}