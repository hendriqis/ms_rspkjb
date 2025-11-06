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
    public partial class LKunjunganPasien : BaseCustomDailyLandscapeA3Rpt
    {
        public LKunjunganPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            xrLabel2.Text = String.Format("Department : {0}", BusinessLayer.GetvHealthcareServiceUnitList(String.Format("DepartmentID = '{0}'", param[1].ToString())).FirstOrDefault().DepartmentName.ToString());
            base.InitializeReport(param);
        }
    }
}