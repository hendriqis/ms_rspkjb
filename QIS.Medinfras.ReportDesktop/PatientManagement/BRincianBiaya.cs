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
    public partial class BRincianBiaya : BaseDailyPortrait1Rpt
    {
        public BRincianBiaya()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblLastUpdatedDate.Text = DateTime.Now.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            lblLastUpdatedBy.Text = appSession.UserFullName;

            base.InitializeReport(param);
        }
    }
}
