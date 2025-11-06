using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class KartuPasien7Rpt : QIS.Medinfras.ReportDesktop.BaseRpt
    {
        public KartuPasien7Rpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            this.Landscape = true;
            string medicalNo = param[0];
            string filterExpression = string.Format(" MRN ='{0}'", medicalNo);
            Patient entity = BusinessLayer.GetPatientList(filterExpression)[0];
            string firstName = entity.FirstName;
            string lastName = entity.LastName;

            lblMedicalNo.Text = entity.MedicalNo;

            if (entity.Name == "" || entity.Name == null)
            {
                string FullName = entity.FullName;
                if (entity.FullName.Length > 100)
                {
                    FullName = entity.FullName.Substring(0, 100);
                }
                lblPatientName.Text = FullName;
            }
            else
            {
                lblPatientName.Text = entity.Name;
            }
        }
    }
}
