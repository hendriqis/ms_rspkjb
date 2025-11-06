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
    public partial class RL3_10_KegiatanPelayananKhusus : BaseCustomDailyPotraitRpt
    {
        public RL3_10_KegiatanPelayananKhusus()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Healthcare entityHC = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];
            lblNamaRS.Text = entityHC.HealthcareName;
            lblTahun.Text = param[0];

            base.InitializeReport(param);
        }

    }
}
