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
    public partial class LReturResep : BaseCustomDailyLandscapeRpt
    {
        public LReturResep()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriode.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
            vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(String.Format("DepartmentID LIKE '{0}'", param[1])).FirstOrDefault();
            lblDept.Text = string.Format("Departemen : {0}", vsu.DepartmentName);
        }
    }
}
