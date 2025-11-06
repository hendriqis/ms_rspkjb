using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPenerimaanHasilLab_RSRTH : BaseDailyPortraitRpt
    {
        public BPenerimaanHasilLab_RSRTH()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            lblCityDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblPetugas.Text = appSession.UserFullName;

            base.InitializeReport(param);
        }
    }
}
