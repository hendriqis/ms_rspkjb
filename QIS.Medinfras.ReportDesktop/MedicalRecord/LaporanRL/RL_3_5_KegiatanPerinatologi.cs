using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class RL_3_5_KegiatanPerinatologi : BaseCustomDailyLandscapeRpt
    {
        public RL_3_5_KegiatanPerinatologi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Healthcare entityHC = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            txtName.Text = entityHC.HealthcareName;
            //txtCode.Text = entityHC.Initial;
            //txtYear.Text = param[1];
            base.InitializeReport(param);
        }

    }
}
