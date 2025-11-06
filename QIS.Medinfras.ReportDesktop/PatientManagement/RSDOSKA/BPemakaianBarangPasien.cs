using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPemakaianBarangPasien : BaseDailyLandscapeRpt
    {
        public BPemakaianBarangPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0} ", param[0]))[0];
            txtPatient.Text = entityReg.PatientName;
            lblRegNo.Text = entityReg.RegistrationNo;
            base.InitializeReport(param);
        }
    }
}
