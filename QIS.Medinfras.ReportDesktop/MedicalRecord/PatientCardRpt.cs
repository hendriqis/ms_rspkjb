using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class PatientCardRpt : QIS.Medinfras.ReportDesktop.BaseRpt
    {
        public PatientCardRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string medicalNo = param[0];
            string filterExpression = string.Format(" MRN ='{0}'", medicalNo);
            vPatient entity = BusinessLayer.GetvPatientList(filterExpression)[0];
            string firstName = entity.FirstName;
            string lastName = entity.LastName;

            lblPatientName.Text = entity.PatientName;
            xrBarCode1.Text = entity.MedicalNo;
            lblMedicalNo.Text = entity.MedicalNo;
        }
    }
}
