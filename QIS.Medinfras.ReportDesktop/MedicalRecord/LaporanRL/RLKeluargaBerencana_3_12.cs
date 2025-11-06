using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class RLKeluargaBerencana_3_12 : BaseCustomDailyPotraitRpt
    {
        public RLKeluargaBerencana_3_12()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            Healthcare entityHC = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];

            lblNamaRS.Text = entityHC.HealthcareName;
            base.InitializeReport(param);
        }

    }
}
