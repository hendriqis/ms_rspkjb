using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratIzinPulang : BaseCustomDailyPotraitRpt
    {
        public BSuratIzinPulang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Registration entity = BusinessLayer.GetRegistrationList(param[0])[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];

            lblNamaPasien.Text = entityP.PatientName;
            lblNoRM.Text = entityP.MedicalNo;
            lblNoReg.Text = entity.RegistrationNo;

            lblPenanggungJawab.Text = appSession.UserFullName;

            base.InitializeReport(param);
        }

    }
}
