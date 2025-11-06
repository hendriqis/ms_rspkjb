using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LBukuKasDetailRSDOSKA : BaseDailyLandscapeRpt
    {
        public LBukuKasDetailRSDOSKA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
               base.InitializeReport(param);
        }
       
    }
}
