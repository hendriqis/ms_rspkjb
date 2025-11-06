using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
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
            tcDOB.Text = entity.DateOfBirthInString;

            if (entity.Occupation.Equals(null) || entity.Occupation.Equals(""))
            {
                tcJob.Text = "-";
            }
            else {
                tcJob.Text = entity.Occupation;            
            }
            if (entity.Sex.Equals("Female"))
            {
                tcSex.Text = "Perempuan";
            }
            else
            {
                tcSex.Text = "Laki-laki";
            }
            base.InitializeReport(param);
        }

    }
}
