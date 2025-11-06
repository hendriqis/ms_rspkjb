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
    public partial class BFormVerifikasi : BaseCustomDailyPotraitRpt
    {
        public BFormVerifikasi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
            cHealthcareEmail.Text = healthcare.Email;

            lblHealthcareName.Text = healthcare.HealthcareName;
            lblHealthcareClass.Text = healthcare.HealthcareClass;
            lblHealthcareCode.Text = healthcare.BPJSCode;

            base.InitializeReport(param);
        }

    }
}
