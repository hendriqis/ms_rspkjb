using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class RLPengunjung_5_1_RSSBB : BaseCustomDailyPotraitRpt
    {
        public RLPengunjung_5_1_RSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHC = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            lblNamaRS.Text = entityHC.HealthcareName;
            lblKotaRS.Text = entityHC.City;
            string[] temp = param[0].Split(';');

            base.InitializeReport(param);

        }
    }
}
