using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BLembarStatusIGD : BaseDailyPortraitRpt
    {
        public BLembarStatusIGD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0]))[0];
            List<StandardCode> lstOfSC = BusinessLayer.GetStandardCodeList("");
            tcPatientName.Text = entity.PatientName;
            txtMedicalNo.Text = entity.MedicalNo;
            txtAgeInYear.Text = entity.AgeInYear.ToString();
            txtGender.Text = lstOfSC.Find(sc => sc.StandardCodeID.Equals(entity.GCGender)).StandardCodeName;
            //txtLastMedicationNote.Text = entit
            base.InitializeReport(param);
        }

        private void bs_CurrentChanged(object sender, EventArgs e)
        {

        }

    }
}
