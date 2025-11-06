using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Report
{
    public partial class LLembarKartuIndeksPenderita : BaseDailyPortraitRpt
    {
        public LLembarKartuIndeksPenderita()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPatient entity = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", param[0]))[0];
            tcName.Text = entity.PatientName;
            tcAge.Text = entity.PatientAge;
            tcReligion.Text = entity.Religion;
            tcAddr.Text = entity.HomeAddress;
            tcSex.Text = entity.Sex;
            tcDOB.Text = entity.DateOfBirthInString;
            tcJob.Text = entity.Occupation;
            base.InitializeReport(param);
        }

    }
}
