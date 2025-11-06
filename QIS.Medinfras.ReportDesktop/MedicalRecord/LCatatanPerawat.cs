using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LCatatanPerawat : BaseCustomDailyPotraitRpt
    {
        public LCatatanPerawat()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(String.Format(param[0].ToString())).FirstOrDefault();
            lblPatientInformation.Text = String.Format("Registrasi : {0} | {1} ({2})" , entity.RegistrationNo, entity.PatientName, entity.MedicalNo);
            base.InitializeReport(param);
        }

    }
}
