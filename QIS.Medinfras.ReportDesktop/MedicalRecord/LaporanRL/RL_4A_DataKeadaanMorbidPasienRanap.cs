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
    public partial class RL_4A_DataKeadaanMorbidPasienRanap : BaseCustomDailyLandscapeA3Rpt
    {
        public RL_4A_DataKeadaanMorbidPasienRanap()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            Healthcare entityHC = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            lblNamaRS.Text = entityHC.HealthcareName;
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);

        }
    }
}
