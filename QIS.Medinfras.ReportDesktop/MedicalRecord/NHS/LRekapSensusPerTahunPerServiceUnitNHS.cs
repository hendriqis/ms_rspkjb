using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LRekapSensusPerTahunPerServiceUnitNHS : BaseCustomDailyLandscapeA3Rpt
    {
        public LRekapSensusPerTahunPerServiceUnitNHS()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            int year = Convert.ToInt32(param[0]);
            int month = Convert.ToInt32(param[1]);

            #region PatientCencusOut
            subPasienPerKelas.CanGrow = true;
            pasienPerKelas.InitializeReport(year, month);
            #endregion


            base.InitializeReport(param);
        }

    }
}
