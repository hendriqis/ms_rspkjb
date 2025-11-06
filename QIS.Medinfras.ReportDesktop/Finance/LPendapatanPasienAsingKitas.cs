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
    public partial class LPendapatanPasienAsingKitas : BaseDailyPortraitRpt
    {
        public LPendapatanPasienAsingKitas()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriodDate.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            //string[] temp1 = param[1].Split(';');
            //lblPeriodTime.Text = string.Format("Jam : {0} s/d {1}", temp1[0].ToString(), temp1[1].ToString());
            base.InitializeReport(param);
        }
        
    }
}
