using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LKontrolBatalTagihan : BaseCustomDailyPotraitRpt
    {
        public LKontrolBatalTagihan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            string deptID = param[1].ToString();

            if (String.IsNullOrEmpty(deptID)) { 
                deptID = "ALL"; 
            }

            lblPeriod.Text = String.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            xrLabel1.Text = String.Format("Department : {0}", deptID);
            base.InitializeReport(param);
        }
    }
}
