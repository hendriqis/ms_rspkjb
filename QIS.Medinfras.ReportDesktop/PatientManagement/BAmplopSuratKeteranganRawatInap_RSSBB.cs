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
    public partial class BAmplopSuratKeteranganRawatInap_RSSBB : BaseRpt
    {
        public BAmplopSuratKeteranganRawatInap_RSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            int RegistrationID = Convert.ToInt32(param[0]);
            List<vRegistrationInfo> entity = BusinessLayer.GetvRegistrationInfoList(String.Format("RegistrationID = {0}", RegistrationID));
            lblPatientName.Text = entity.FirstOrDefault().FullName;
            this.DataSource = entity;
        }
    }
}
