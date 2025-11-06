using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Report
{
    public partial class BLembarStatusIGD : BaseDailyPortraitRpt
    {
        public BLembarStatusIGD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0]))[0];
            //tcPatientName.Text = entity.PatientName;
            //entity.AgeInYear
            base.InitializeReport(param);
        }

        private void bs_CurrentChanged(object sender, EventArgs e)
        {

        }

    }
}
