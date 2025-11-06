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
    public partial class BPendaftaranMCURSFM : BaseDailyPortrait2Rpt
    {
        public BPendaftaranMCURSFM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            txtEmail.Text = entityHealthcare.Email;
                     
            base.InitializeReport(param);
        }
    }
}
